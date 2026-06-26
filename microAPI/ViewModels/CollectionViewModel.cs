using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace microAPI.ViewModels
{
    public partial class CollectionViewModel : ObservableObject
    {
        [ObservableProperty] private string _name = string.Empty;
        public string Arrow => IsExpanded ? "▼" : "▶";

        [ObservableProperty] 
        [NotifyPropertyChangedFor(nameof(Arrow))] 
        private bool _isExpanded = true;
        [ObservableProperty] private bool _isEditingName = true;

        public ObservableCollection<RequestViewModel> Requests {get; } = new();

        [RelayCommand]
        private void ToggleExpand()
        {
            IsExpanded = !IsExpanded;
        }

        [RelayCommand]
        private void EndEdit()
        {
            IsEditingName = false;
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = "Coleção sem nome";
            }
        }

        [RelayCommand]
        private void StartEdit()
        {
            IsEditingName = true;
        }

        [RelayCommand]
        private void AddRequest()
        {
            var newRequest = new RequestViewModel();
            Requests.Add(newRequest);
            IsExpanded = true;
        }
    }
}