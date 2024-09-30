using Frontend.Model;
using Frontend.ViewModel;
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
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;
        private UserModel user;
        private BoardControllerViewModel controller;
        public BoardView(UserModel user, BoardModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(u);
            this.DataContext = viewModel;
            this.controller = new BoardControllerViewModel(user);
            
            this.user = user;
        }

        
        //logout button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isLoggedOut = controller.Logout();
            if (isLoggedOut)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

       

        private void InProgress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Return(object sender, RoutedEventArgs e)
        {
            BoardControllerView Window = new BoardControllerView(user);
            Window.Show();
            this.Close();
        }

        
    }
}
