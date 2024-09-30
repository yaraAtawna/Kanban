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
    /// Interaction logic for BoardControllerView.xaml
    /// </summary>
    public partial class BoardControllerView : Window
    {
        private BoardControllerViewModel viewModel;
        private UserModel user;
        public BoardControllerView(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardControllerViewModel(u);
            this.DataContext = viewModel;
            this.user = u;
        }

        //select board
        private void Choose_Button(object sender, RoutedEventArgs e)
        {
            BoardModel board = viewModel.SelectBoard();
            if (board != null)
            {
                BoardView boardView = new BoardView(user, board);
                boardView.Show();
                this.Close();
            }

        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {

            bool isLoggedOut = viewModel.Logout();
            if (isLoggedOut)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            AddBoardPanel.Visibility = Visibility.Visible;
        }
        //delete board
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            viewModel.RemoveBoard();
        }

        private void AddBoard_Click(object sender, RoutedEventArgs e)
        {
            string newBoardName = NewBoardNameTextBox.Text;
            if (!string.IsNullOrEmpty(newBoardName))
            {
                viewModel.AddBoard(newBoardName);
                AddBoardPanel.Visibility = Visibility.Hidden;
            }
            //error message !!
            // Hide the AddBoardPanel again

        }
    }
}










