using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class User
    {
        private string password;
        private string email;
        private bool loggedIn;

        //mileStone 2
        UserDAO userDAO;

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                email = value;

                //mileStone 2
                //userDAO.Email = email;
                
            }
        }
     
     public bool LoggedIn
        {
            get
            {
                return loggedIn;
            }
            set
            {
                this.loggedIn = value;
            }
        }
        public User(string email, string password,bool load)
        {
            this.userDAO = new UserDAO(email, password);
            this.Email = email;
            this.password = password;
            this.loggedIn = true;
            //new
            if (!load)
            {
                //mileStone2
                userDAO.persist();
            }
            

        }



        /// <summary>
        /// This method check if the passwrd is right
        /// </summary>
        /// <param name="password"></param>
        /// <returns> Boolean value 'true' if it's right else 'false' </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool LogIn(string password)
        {
            if(password == null)    throw new ArgumentNullException("Password");
            return this.password == password;
        }
        /// <summary>
        /// This method update the field 'loggedIn' to be 'false' . In other words: (The user have logged out)
        /// </summary>
        public void LogOut()
        {
            LoggedIn = false;
        }

        public UserDAO retUserDAO()
        {
            return this.userDAO;
        }
    }
}