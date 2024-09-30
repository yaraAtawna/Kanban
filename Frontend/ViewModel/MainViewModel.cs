using Frontend.Model;
using System;
using System.Windows;

namespace Frontend.ViewModel
{
    class MainViewModel : NotifiableObject
    {
        public MainViewModel()
        {
            this.Controller = new BackendController();
            this.Email = "mail@mail.com";
            this.Password = "Password1";
            // Console.WriteLine("test MainViewModel ");
        }

        public BackendController Controller { get; private set; }

        private string _Email;
        public string Email
        {
            get => _Email;
            set
            {
                this._Email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
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

        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Email, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                MessageBox.Show("cannot logIn" + e.Message);
                return null;
            }
        }
        public UserModel Register()
        {
            Message = "";
            try
            {
                //Controller.Register(Email, Password);
                Message = "Registered successfully";
                return Controller.Register(Email, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                MessageBox.Show("cannot " + e.Message);
                return null;
            }
        }

        
    }
}


