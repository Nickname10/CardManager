using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Client.Models;
using Client.Services.DialogService;
using Client.Services.HttpService;

namespace Client.ViewModels
{
    public class LoadCardPageViewModel : ViewModelBase
    {
        private readonly ClientWindowViewModel _parentWindowViewModel;
        private BitmapImage _selectedImageBitmap;

        public BitmapImage SelectedImageBitmap
        {
            get => _selectedImageBitmap;
            set
            {
                _selectedImageBitmap = value;
                OnPropertyChanged();
            }
        }

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

        public string CardTitle { get; set; }
        public RelayCommand LoadCardCommand { get; }
        public RelayCommand ShowCardsPageCommand { get; }
        public RelayCommand SendToServerCommand { get; }

        public LoadCardPageViewModel(ClientWindowViewModel parentWindowViewModel)
        {
            _parentWindowViewModel = parentWindowViewModel;
            LoadCardCommand = new RelayCommand(LoadCardAction);
            SendToServerCommand = new RelayCommand(SendToServerAction, o =>
                !string.IsNullOrEmpty(SelectedImagePath) && SelectedImageBitmap != null);
            ShowCardsPageCommand = new RelayCommand((obj) => _parentWindowViewModel.ShowCardsPage());
        }

        private void SendToServerAction(object obj)
        {
            var sender = new HttpCardManager();
            sender.SendCardToServer(new Card()
            {
                Image = _selectedImageBitmap,
                Title = string.IsNullOrEmpty(CardTitle) ? string.Empty : CardTitle
            });
            MessageBox.Show("Data has been sent");
            _parentWindowViewModel.ShowCardsPage();
        }

        private void LoadCardAction(object obj)
        {
            var imageDialog = new SelectImageDialogService();
            if (imageDialog.OpenFileDialog())
            {
                SelectedImagePath = imageDialog.FilePath;
                SelectedImageBitmap = new BitmapImage(new Uri(SelectedImagePath));
            }
            else
            {
                SelectedImagePath = "Please, select a path";
            }
        }
    }
}