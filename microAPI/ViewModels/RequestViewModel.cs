using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace microAPI.ViewModels
{
    public partial class RequestViewModel : ObservableObject
    {
        private readonly Action? _saveCallback;
        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _method = "GET";
        [ObservableProperty] private string _url = string.Empty;
        [ObservableProperty] private string _requestBody = string.Empty;
        [ObservableProperty] private string _responseBody = string.Empty;
        [ObservableProperty] private string _statusCode = string.Empty;
        [ObservableProperty] private bool _isEditingName = true;

        public List<string> HttpMethods { get; } = ["GET", "PUT", "POST", "DELETE"];

        public RequestViewModel() { }

        public RequestViewModel(Action saveCallback)
        {
            _saveCallback = saveCallback;
        }

        [RelayCommand]
        private void EndEdit()
        {
            IsEditingName = false;
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = "Requisição sem Nova";
            }
            _saveCallback?.Invoke();
        }

        [RelayCommand]
        private void StartEdit()
        {
            IsEditingName = true;
        }
    }
}