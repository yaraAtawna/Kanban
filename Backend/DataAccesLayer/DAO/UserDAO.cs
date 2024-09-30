using IntroSE.Kanban.Backend.DataAccesLayer.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccesLayer.DAO
{
    internal class UserDAO
    {
        public string Email { get; set; }
        public  string password { get; set; }
        public UserController userController { get; set; }

        public const string userEmailColumnName = "Email";
        public const string userPasswordColumnName = "Password";


        public UserDAO(string Email, string password) 
        {
            //Console.WriteLine("new DAO");
            this.Email = Email;
            this.password = password;
            this.userController = new UserController();
        }

        public void persist()
        {
            try
            {
                this.userController.insert(this);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
