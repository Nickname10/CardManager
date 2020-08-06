using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Client.Annotations;
using Client.Models;
using Client.Pages;

namespace Client.ViewModels
{
    public class ClientWindowViewModel:ViewModelBase
    {

        private readonly Page _loadCardPage;
        private readonly CardsPage _cardsPage;

        private Page _currentPage;

        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage == value) return;
                _currentPage = value;
                OnPropertyChanged();
            }
        }
        public ClientWindowViewModel()
        {
            _loadCardPage = new LoadCardPage(this);
            _cardsPage = new CardsPage(this);
            ShowCardsPage();
        }

        public void ShowLoadCardPage()
        {
            CurrentPage = _loadCardPage;
        }
        public void ShowCardsPage()
        {
            _cardsPage.ViewModel.RefreshCommand.Execute(null);
            CurrentPage = _cardsPage;
            
        }

        public void ShowReplacePage(Card selectedCard)
        {
            CurrentPage = new ReplacePage(this, selectedCard);
        }
    }
}