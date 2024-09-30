using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel
{
   

    class BoardControllerViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        private UserModel user;
        private BoardControllerModel controllerBoard;
        public BoardControllerModel ControllerBoard { get; private set; }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(user.Email.Contains("achiya") ? Colors.Blue : Colors.Black);
            }
        }
        public string Title { get; private set; }


        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public BoardControllerViewModel(UserModel u)
        {
            this.controller = u.Controller;
            this.user = u;
            Title = "Boards for " + user.Email;
            try { ControllerBoard = user.GetBoards(); }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
            }
            ControllerBoard =  user.GetBoards();
            //controller.ListUserBoards(user.Email);

        }


        
        public bool Logout()
        {
            try
            {
                controller.Logout(user.Email);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("cannot logOut" + ex.Message);
                return false;
            }
        }
        public BoardModel SelectBoard()
        {
            return SelectedBoard;
        }
        public void RemoveBoard()
        {
            BoardModel board = SelectBoard(); //correct!
            //Debug.WriteLine("board id: " + board.ID);
            try {
                //ols : controller.DeleteBoard(user.Email, board.ID.ToString());
                controller.DeleteBoard(user.Email, board.Name);
                //not sure!!
                //user.remove(board);
                ControllerBoard = user.GetBoards();
                RaisePropertyChanged("ControllerBoard");
            }
            catch (Exception e)
            {
                //Message = e.Message; 
                MessageBox.Show("cannot remove"+e.Message);
            }

           
        }
        public void AddBoard(string boardName)
        {
            try {
                controller.CreateBoard(user.Email, boardName);
                //not sure!!
                ControllerBoard = user.GetBoards();
                RaisePropertyChanged("ControllerBoard");
            }
            catch (Exception e)
            {
                MessageBox.Show("cannot add board" + e.Message);
            }

        }
    }
}
