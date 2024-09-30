
using Frontend.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    internal class BoardViewModel : NotifiableObject
    {
        //private Model.BackendController controller;
        //private UserModel user;

        private BoardModel _board;
        public BoardModel Board { get { return _board; } set { _board = value; } }

        public string Title { get; private set; }
        public string Owner { get; private set; }

     

        public BoardViewModel(BoardModel board)
        {
            if (board != null)
            {
                Title = "Tasks for: " + board.Name;
                Owner = "Owner of this board: " + board.Owner;
                Board = board;
            }
            else
            {
                Title = "There are no boards for this user yet!";
            }
       
        }

       

    }
}