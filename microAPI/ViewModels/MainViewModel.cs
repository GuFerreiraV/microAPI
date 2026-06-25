using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;

namespace microAPI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly CancellationTokenSource _cts = new();

    // Propriedades observáveis (O Toolkit gera o código boilerplate INotifyPropertyChanged automaticamente)
    [ObservableProperty] private string _url = "https://jsonplaceholder.typicode.com/posts/1";
    [ObservableProperty] private string _requestBody = "{\n  \"title\": \"foo\",\n  \"body\": \"bar\",\n  \"userId\": 1\n}";
    [ObservableProperty] private string _responseBody = string.Empty;
    [ObservableProperty] private string _statusCode = string.Empty;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _isOnline = true;
    [ObservableProperty] private bool _isSidebarOpen = true;

    // Lista de verbos HTTP para o ComboBox
    public List<string> HttpMethods { get; } = ["GET", "POST", "PUT", "DELETE"];

    [ObservableProperty] private string _selectedMethod = "GET";

    public MainViewModel (IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        // inicia a verificação periódica de internet em segundo plano
        _ = StartInternetCheckLoopAsync(_cts.Token);
    }

    [RelayCommand]
    private async Task ExecuteRequestAsync ()
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
    [RelayCommand]
    private void AddNewCollection()
    {
        System.Diagnostics.Debug.WriteLine("New Collection Clicked!");
    }
    private string TryFormatJson (string json)
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

        while(await timer.WaitForNextTickAsync(cancellationToken))
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