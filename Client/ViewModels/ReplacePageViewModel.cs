using System;
using System.Runtime.Serialization.Formatters;
using System.Windows;
using System.Windows.Media.Imaging;
using Client.Models;
using Client.Services.DialogService;
using Client.Services.HttpService;

namespace Client.ViewModels
{
    public class ReplacePageViewModel : ViewModelBase
    {
        private Card _currentCard;

        public Card CurrentCard
        {
            get => _currentCard;
            set
            {
                if (_currentCard == value) return;
                _currentCard = value;
                OnPropertyChanged(nameof(CurrentCard));
            }
        }

        public Card NewCard { get; set; }

        private readonly ClientWindowViewModel _parentWindow;

        private readonly HttpCardManager _httpCardManager;

        public RelayCommand ShowCardsPageCommand { get; }
        public RelayCommand ReplaceCommand { get; }
        public RelayCommand SelectCommand { get; }
        private string _selectedImagePath;

        public string SelectedImagePath
        {
            get => _selectedImagePath;
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged();
            }
        }

        public ReplacePageViewModel(ClientWindowViewModel parentViewModel, Card currentCard)
        {
            _parentWindow = parentViewModel;
            CurrentCard = currentCard;
            NewCard = new Card() {Id = currentCard.Id};
            _httpCardManager = new HttpCardManager();
            ShowCardsPageCommand = new RelayCommand(obj => _parentWindow.ShowCardsPage());
            ReplaceCommand = new RelayCommand(ReplaceAction);
            SelectCommand = new RelayCommand(SelectAction);
        }

        private void ReplaceAction(object obj)
        {
            _httpCardManager.Replace(NewCard);

            MessageBox.Show("Data has been replaced");
            _parentWindow.ShowCardsPage();
        }

        private void SelectAction(object obj)
        {
            var imageDialog = new SelectImageDialogService();
            if (imageDialog.OpenFileDialog())
            {
                SelectedImagePath = imageDialog.FilePath;
                NewCard.Image = new BitmapImage(new Uri(SelectedImagePath));
                OnPropertyChanged(nameof(NewCard));
            }
            else
            {
                SelectedImagePath = "Please, select a path";
            }
        }
    }
}