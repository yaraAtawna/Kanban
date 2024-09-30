﻿
namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        /// <summary>
        /// Returns User BoardControllerModel
        /// <returns>BoardControllerModel</returns>
        public BoardControllerModel GetBoards()
        {
            return new BoardControllerModel(Controller, this);
        }

        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
        }
    }
}