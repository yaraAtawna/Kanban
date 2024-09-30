using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTests
{
     class program
    {
        public static void Main(string[] args)
        {
            UserService userService = new UserService();
            UserTest us = new UserTest(userService);
            Console.WriteLine("Let's start the test :) \n Insert a valid EMAIL and PASSWORD!");
            Console.Write("EMAIL : ");
      //      string email = Console.ReadLine().ToString();
            Console.Write("PASSWORD : ");
          //  string password = Console.ReadLine().ToString();
            us.register(email, password);
            us.logIn(email, password);
            us.logout(email);
            us.runTestWithConditions(email, password);
            Console.ReadKey();
        }
    }
}
