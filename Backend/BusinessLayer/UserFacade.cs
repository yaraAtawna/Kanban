using IntroSE.Kanban.Backend.DataAccesLayer.controllers;
using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserFacade
    {
        private readonly Dictionary<string, User> users ;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserController userController;

        public UserFacade() 
        {
            users = new Dictionary<string, User>();
            userController = new UserController();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("new UserFacade log!");
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            try
            {
                if (email == null || password == null)
                {
                    throw new Exception("email or password is illegal");
                }
                if (!ValidEmail(email))
                {
                    //Console.WriteLine(email);
                    log.Warn("check the email");
                    throw new Exception("check the email");
                }
                email = ConvertEmailToLower(email);
                if (isExist(email))
                {
                    throw new Exception("The email already exists");
                }
                //A valid password must be between 6 to 20 characters and must include at least one 
                //uppercase letter, one lowercase character, and a number.
                if (password.Length > 20 || password.Length < 6 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
                {
                    throw new Exception("password is illegal");
                }
                //Console.WriteLine("new user");
                User u = new User(email, password,false);
                users.Add(email, u);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private bool ValidEmail(string email)
        {
            if (email == null)
                return false;
            else email = ConvertEmailToLower(email);
            //
            //Console.WriteLine(1);

           
            
            //Console.WriteLine(email);
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {

                return false;
            }
            catch (ArgumentException e)
            {

                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static string ConvertEmailToLower(string email)
        {
            return email.ToLower();
        }



        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LogIn(string email, string password)
        {
            try
            {
                if (email == null || password == null)
                {
                    throw new Exception("email or password is illegal");
                }
                if (!(isExist(email)))
                {
                    log.Warn("The user email is not exist. Check the email if you have registered.\n OTHERWISE, PLEASE REGISTER FIRST!");

                    throw new Exception("The user email doesnt exist");

                }
                else if (isLoggedIn(email))
                {
                    log.Warn("You are already logged in !!");

                    throw new Exception("You are already logged in !!");

                }
                if (!users[email].LogIn(password))
                {
                    log.Warn("WRONG PASSWORD!!!");

                    throw new Exception("WRONG PASSWORD!!!!!");
                }
                users[email].LoggedIn = true;
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
    }

        public string LogOut(string email) 
        {
            try
            {
                if (email == null)
                {
                    throw new Exception("email or password is illegal");
                }
                if (!(isExist(email)))
                {
                    log.Warn("The user email is not exist. Check the email if you have registered.\n OTHERWISE, PLEASE REGISTER FIRST!");

                    throw new Exception("The user email doesnt exist");

                }
                else if (!isLoggedIn(email))
                {
                    log.Warn("You're not logged in !!");

                    throw new Exception("You're not logged in !!");

                }
                users[email].LogOut();
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
        /// <summary>
        /// This method check if the user is exist.
        /// </summary>
        /// <param name="email"></param>
        /// <returns> Boolean value 'true' if he exist else 'false' --</returns>
        public bool isExist(string email)
        {
            return users.ContainsKey(email);
        }
        /// <summary>
        /// This method check if the user is logged in.
        /// </summary>
        /// <param name="email"></param>
        /// <returns> Boolean value 'true' if he is logged in else 'false' --</returns>
        public bool isLoggedIn(string email) 
        {

            return users[email].LoggedIn ;
        }
        /// <summary>
        ///  This is a getter for user
        /// </summary>
        /// <param name="email"></param>
        /// <returns> A User object /returns>
        public User getUser(string email)
        {
            if (!isExist(email))
            {
               log.Error("User not found");
                throw new Exception("User is not exist !!");
            }
            return users[email];
        }

        ///<summary>This method loads all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public bool LoadData()
        {
            log.Info("Attempting to load data from database");
            try
            {
                //Console.WriteLine("load data userFacade");
                bool ret = false;
                List<UserDAO> list = userController.getAllUsers();
                //Debug.WriteLine("test load data for users,number os useres are:");
                //Debug.WriteLine(list.Count);

                if (list.Count != 0)
                {
                    foreach (UserDAO user in list)
                    {
                        User us = new User(user.Email, user.password,true);
                        us.LoggedIn=false;
                        users.Add(us.Email, us);
                    }
                    ret = true;
                }
                else ret = true;
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        ///<summary>This method deletes all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b>		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public bool DeleteData()
        {
            //Console.WriteLine(" ret1");
            try
            {
                //Console.WriteLine(" ret2");

                string ret = userController.deleteAll();
                //Console.WriteLine(" ret3");
                if (ret == "null")
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}