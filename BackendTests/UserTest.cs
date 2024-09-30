using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTests
{
    class UserTest
    {
        private readonly UserService userService;

        public UserTest(UserService userService)
        {
            this.userService = userService;
        }
        public void register(string email, string password)
        {
            // ---------------------Try to Register------------------------
            string returnValue = userService.Register(email, password);
            if (returnValue != null | returnValue != "")
            {
                Console.WriteLine(returnValue);
            }
            else
            {
                Console.WriteLine("User created successfuly :)");
            }
        }
        public void logIn(string email, string password)
        {
            string returnValue;
            //-------------------Try_to_login--------------------
            returnValue = userService.Login(email, password); // Login with correct email and password
            if (returnValue == email)
            {
                Console.WriteLine("User login successfly :)");
            }
            else
            {
                Console.WriteLine(returnValue);
            }
        }
        public void logout(string email)
        {
            string returnValue;
            // ------------------Try_to_logout-----------------------
            returnValue = userService.Logout(email);
            if (returnValue != null | returnValue != "")
            {
                Console.WriteLine(returnValue);
            }
            else
            {
                Console.WriteLine("User Logout successfuly :)");
            }
        }
        public void runTestWithConditions(string email, string password)
        {
            Console.WriteLine("run withConditions funcit!!!!");
            string returnValue;
            Console.WriteLine("---------------------------------------------------------------------------------");
            //-------------------First_we_have_to_login----------------------------------------------
            returnValue = userService.Login(email, password);
            Console.WriteLine("First_we_have_to_login" + returnValue);
            Console.WriteLine("---------------------------------------------------------------------------------");
            //-------------------Try_to_login_with_non-correct_conditions----------------------------
            returnValue = userService.Login(email, password); // Try to login when user already loggedIn
            Console.WriteLine("login already :" + returnValue);
            Console.WriteLine("---------------------------------------------------------------------------------");
            //---------------------------------------------------------------------------------------
            returnValue = userService.Logout(email); // Logout to check if the other conditions work 
            Console.WriteLine("logout: " + returnValue);
            Console.WriteLine("---------------------------------------------------------------------------------");
            //---------------------------------------------------------------------------------------
            string email1 = "mo@gmail.com";
            returnValue = userService.Login(email1, password); // Login with not existed email 
            Console.WriteLine("login with not-existed email :" + returnValue);
            Console.WriteLine("---------------------------------------------------------------------------------");
            //---------------------------------------------------------------------------------------
            returnValue = userService.Logout(email); // Logout to check if the other condition work 
            Console.WriteLine("logout: " + returnValue);
            Console.WriteLine("---------------------------------------------------------------------------------");
            //---------------------------------------------------------------------------------------
            string password1 = "bgu123";
            returnValue = userService.Login(email, password1); // Login with incorrect password
            Console.WriteLine("incorrect password : " + returnValue);


        }

    }
}
