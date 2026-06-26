using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using microAPI.Models;
using microAPI.Interfaces;
using microAPI.Repositories;
namespace microAPI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICollectionRepository _collectionRepository;
    private readonly CancellationTokenSource _cts = new();

    // Propriedades observáveis (O Toolkit gera o código boilerplate INotifyPropertyChanged automaticamente)
    [ObservableProperty] private string _url = "https://jsonplaceholder.typicode.com/posts/1";
    [ObservableProperty] private string _requestBody = "{\n  \"title\": \"foo\",\n  \"body\": \"bar\",\n  \"userId\": 1\n}";
    [ObservableProperty] private string _responseBody = string.Empty;
    [ObservableProperty] private string _statusCode = string.Empty;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _isOnline = true;
    [ObservableProperty] private bool _isSidebarOpen = true;
    [ObservableProperty] private string _selectedMethod = "GET";

    // Lista de verbos HTTP para o ComboBox
    public List<string> HttpMethods { get; } = ["GET", "POST", "PUT", "DELETE"];

    public ObservableCollection<CollectionViewModel> Collections { get; } = new();

    public MainViewModel(IHttpClientFactory httpClientFactory, ICollectionRepository collectionRepository)
    {
        _httpClientFactory = httpClientFactory;
        _collectionRepository = collectionRepository;
        // inicia a verificação periódica de internet em segundo plano
        _ = StartInternetCheckLoopAsync(_cts.Token);

        // carrega as coleções salvas anteriormente
        _ = LoadCollectionsAsync();
    }



    [RelayCommand]
    private async Task ExecuteRequestAsync()
    {
        // 1. Validação Básica
        if (string.IsNullOrWhiteSpace(Url) || !Uri.TryCreate(Url, UriKind.Absolute, out _))
        {
            ResponseBody = "Erro: URL inválida.";
            return;
        }

        IsLoading = true;
        ResponseBody = "Processando...";
        StatusCode = "";

        try
        {
            // 2. Criação do Cliente via Factory (Best Practice)
            using var client = _httpClientFactory.CreateClient();

            var requestMessage = new HttpRequestMessage(new HttpMethod(SelectedMethod), Url);

            // 3. Configuração do Body (apenas se não for GET/DELETE geralmente, mas permitido aqui)
            if ((SelectedMethod == "POST" || SelectedMethod == "PUT") && !string.IsNullOrWhiteSpace(RequestBody))
            {
                requestMessage.Content = new StringContent(RequestBody, Encoding.UTF8, "application/json");
            }

            // Exemplo de Header fixo (em app real, seria uma lista dinâmica)
            requestMessage.Headers.Add("User-Agent", "microAPI-WPF");

            // 4. Execução
            var response = await client.SendAsync(requestMessage);

            // 5. Tratamento da Resposta
            StatusCode = $"{(int)response.StatusCode} - {response.ReasonPhrase}";
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // 6. Formatação do JSON (Pretty Print)
            ResponseBody = TryFormatJson(jsonResponse);
        }
        catch (HttpRequestException ex)
        {
            ResponseBody = $"Erro de Rede: {ex.Message}";
        }
        catch (Exception ex)
        {
            ResponseBody = $"Erro Crítico: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    // SIDEBAR
    [RelayCommand]
    private void ToggleSidebar()
    {
        IsSidebarOpen = !IsSidebarOpen;
    }
    private async Task LoadCollectionsAsync()
    {
        var savedCollections = await _collectionRepository.LoadCollectionsAsync();

        foreach (var colModel in savedCollections)
        {
            var colVm = new CollectionViewModel(SaveCollectionsCallback)
            {
                Name = colModel.Name,
                IsEditingName = false,
                IsExpanded = false
            };

            foreach (var reqModel in colModel.Requests)
            {
                var reqVm = new RequestViewModel(SaveCollectionsCallback)
                {
                    Name = reqModel.Name,
                    Method = reqModel.Method,
                    Url = reqModel.Url,
                    RequestBody = reqModel.RequestBody,
                    ResponseBody = reqModel.ResponseBody,
                    StatusCode = reqModel.StatusCode,
                    IsEditingName = false
                };
                colVm.Requests.Add(reqVm);
            }

            Collections.Add(colVm);
        }
    }
    private void SaveCollectionsCallback()
    {
        _ = SaveCollectionsAsync();
    }
    // Traduz a estrutura da UI de volta em Models puros e salva no JSON
    private async Task SaveCollectionsAsync()
    {
        var list = new List<CollectionModel>();

        foreach (var colVm in Collections)
        {
            var colModel = new CollectionModel
            {
                Name = colVm.Name
            };

            foreach (var reqVm in colVm.Requests)
            {
                colModel.Requests.Add(new RequestModel
                {
                    Name = reqVm.Name,
                    Method = reqVm.Method,
                    Url = reqVm.Url,
                    RequestBody = reqVm.RequestBody,
                    ResponseBody = reqVm.ResponseBody,
                    StatusCode = reqVm.StatusCode,
                });
            }

            list.Add(colModel);
        }
        await _collectionRepository.SaveCollectionsAsync(list);
    }

    [RelayCommand]
    private void AddNewCollection()
    {
        var newCollection = new CollectionViewModel(SaveCollectionsCallback);
        Collections.Add(newCollection);
        // Salva que uma nova coleção foi adicionada (embora vazia/em edição)
        SaveCollectionsCallback();
    }
    private string TryFormatJson(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return string.Empty;
            using var doc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
        }
        catch
        {
            return json; // Retorna texto puro se não for JSON válido
        }
    }
    private async Task StartInternetCheckLoopAsync(CancellationToken cancellationToken)
    {
        // ping a cada 5 segundos
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        // faz checagem inicial imediata
        IsOnline = await CheckInternetConnectionAsync();

        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            IsOnline = await CheckInternetConnectionAsync();
        }
    }
    private async Task<bool> CheckInternetConnectionAsync()
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(3);

            // request head para um servidor estável
            var request = new HttpRequestMessage(HttpMethod.Head, "https://www.google.com");
            using var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}