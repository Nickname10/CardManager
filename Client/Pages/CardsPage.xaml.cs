using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.ViewModels;

namespace Client.Pages
{
    /// <summary>
    /// Логика взаимодействия для CardsPage.xaml
    /// </summary>
    public partial class CardsPage : Page
    {
        public CardsPageViewModel ViewModel { get; }
        public CardsPage(ClientWindowViewModel parentWindowViewModel)
        {
            InitializeComponent();
            ViewModel = new CardsPageViewModel(parentWindowViewModel);
            DataContext = ViewModel;
        }
    }
}
