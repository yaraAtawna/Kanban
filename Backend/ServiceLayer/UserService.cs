using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.Specialized;



namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class UserService
    {
        private UserFacade userFacade;
        public UserService(){ 
            this.userFacade = new UserFacade();
        }

        internal UserService(UserFacade userFacade) { 
            this.userFacade = userFacade;
        }
        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            Response response;
            try
            {
                string str = userFacade.Register(email, password);
                if (str == "")
                {
                    response = new Response();
                    Console.WriteLine("Success to Resgister");
                    return JsonSerializer.Serialize(response);
                }
                else
                {
                    response = new Response(str);
                    Console.WriteLine(str);
                    return JsonSerializer.Serialize(response);
                }
            }
            catch (Exception ex)
            {
                response = new Response(ex.Message);
                Console.WriteLine(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }
        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            try
            {
                Response response;
                string str = this.userFacade.LogIn(email, password);
                if (str == "")
                {
                    response = new Response();
                    response.ReturnValue = email;
                    return JsonSerializer.Serialize(response);
                }
                else
                {
                    response = new Response(str);
                    return JsonSerializer.Serialize(response);
                }
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }

        }
        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Logout(string email) 
        {
            try
            {
                string str = this.userFacade.LogOut(email);
                if (str == "")
                {
                    Response response = new Response();
                    return JsonSerializer.Serialize(response);
                }
                else
                {
                    Response response = new Response(str);
                    return JsonSerializer.Serialize(response);
                }
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }
        //---------------------------------------------------------------------------------
        ///<summary>This method loads all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string LoadData()
        {
            try
            {
                Response response = new Response();
               bool ret = userFacade.LoadData();
                if (ret)
                {
                    return JsonSerializer.Serialize(response);
                } 
                else
                {
                    response.ErrorMessage = "Failed to load data from database";
                   return JsonSerializer.Serialize(response);
                }
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        ///<summary>This method deletes all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b>		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string DeleteData()
        {
            try
            {
                //Console.WriteLine("enter delete");
                Response response = new Response();
                bool ret = userFacade.DeleteData();
                //Console.WriteLine(ret);
                if (ret)
                {
                    return JsonSerializer.Serialize(response);

                }
                else
                {
                    response.ErrorMessage = "Failed to Delete";
                    return JsonSerializer.Serialize(response);
                }
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }
    }
}
