using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace microAPI.ViewModels
{
    public partial class CollectionViewModel : ObservableObject
    {
        private readonly Action? _saveCallback;

        [ObservableProperty] private string _name = string.Empty;
        public string Arrow => IsExpanded ? "▼" : "▶";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Arrow))]
        private bool _isExpanded = true;

        [ObservableProperty]
        private bool _isEditingName = true;

        public ObservableCollection<RequestViewModel> Requests { get; } = new();

        public CollectionViewModel() { }
        public CollectionViewModel(Action? saveCallback)
        {
            _saveCallback = saveCallback;
        }

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
            _saveCallback?.Invoke();
        }

        [RelayCommand]
        private void StartEdit()
        {
            IsEditingName = true;
        }

        [RelayCommand]
        private void AddRequest()
        {
            if(_saveCallback is null) return;
            var newRequest = new RequestViewModel(_saveCallback);
            Requests.Add(newRequest);
            IsExpanded = true;
            _saveCallback?.Invoke();
        }
    }
}