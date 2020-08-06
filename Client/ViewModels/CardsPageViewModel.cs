using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Client.Models;
using Client.Services.HttpService;
using Newtonsoft.Json;

namespace Client.ViewModels
{
    public class CardsPageViewModel : ViewModelBase
    {
        public RelayCommand ShowLoadCardPageCommand { get; }
        public RelayCommand RefreshCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ReplaceCommand { get; }
        public RelayCommand SortCommand { get; }
        public ObservableCollection<Card> CardsView { get; set; }
        private Card _selectedCard;

        public Card SelectedCard
        {
            get => _selectedCard;
            set
            {
                if (SelectedCard != value)
                {
                    _selectedCard = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly HttpCardManager _httpCardManager;

        public CardsPageViewModel(ClientWindowViewModel parentWindowViewModel)
        {
            _httpCardManager = new HttpCardManager();
            RefreshCommand = new RelayCommand(RefreshCardsFromServerAsync);
            ShowLoadCardPageCommand = new RelayCommand((obj) => parentWindowViewModel.ShowLoadCardPage());
            DeleteCommand = new RelayCommand(obj =>
                {
                    _httpCardManager.DeleteRequest(SelectedCard);
                    RefreshCardsFromServerAsync(null);
                },
                obj => SelectedCard != null);
            ReplaceCommand = new RelayCommand(obj => parentWindowViewModel.ShowReplacePage(SelectedCard),
                obj => SelectedCard != null);
            SortCommand = new RelayCommand(SortAction);
            CardsView = new ObservableCollection<Card>();
            CardsView.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(CardsView));
        }

        private async void RefreshCardsFromServerAsync(object obj)
        {
            var cardsTask = _httpCardManager.GetAllCards();
            await Task.WhenAll(cardsTask);
            var result = cardsTask.Result;
            CardsView.Clear();
            foreach (var netCard in result)
            {
                CardsView.Add(netCard.ToCard());
            }
        }

        private void SortAction(object obj)
        {
            var sortedCardsView = CardsView.OrderBy(t => t.Title);
            CardsView = new ObservableCollection<Card>(sortedCardsView);
            CardsView.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(CardsView));
            OnPropertyChanged(nameof(CardsView));
        }
    }
}