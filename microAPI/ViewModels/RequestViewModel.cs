using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace microAPI.ViewModels
{
    public partial class RequestViewModel : ObservableObject
    {
        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _method = "GET";
        [ObservableProperty] private string _url = string.Empty;
        [ObservableProperty] private bool _isEditingName = true;

        public List<string> HttpMethods {get; } = ["GET", "PUT", "POST", "DELETE"];

        [RelayCommand]
        private void EndEdit()
        {
            IsEditingName = false;
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = "Requisição sem Nova";
            }            
        }

        [RelayCommand]
        private void StartEdit()
        {
            IsEditingName = true;
        }
    }
}