using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class FactoryService
    {
        internal UserFacade userFacade { get; }
        public UserService userService;
        internal BoardFacade boardFacade;
        public BoardService boardService;
        public TaskService taskService;
        
        //new
        private bool first;

        public FactoryService()
        {
            this.userFacade = new UserFacade();
            this.userService = new UserService(userFacade);
            this.boardFacade = new BoardFacade(userFacade);
            this.boardService = new BoardService(boardFacade);
            this.taskService = new TaskService(boardFacade);
            first = true;
            //test
            //Debug.WriteLine("in factory");
        }

        public string LoadData()
        {
            if (first==false) //new
            {
                throw new Exception("data is updated no need for load");
            }
            try
            {
               
                first = false;
                //string str = boardService.LoadData();
                string str= userService.LoadData();
                Response response = JsonSerializer.Deserialize<Response>(str);
               // Console.WriteLine(str);
                if (!response.ErrorOccurd)
                {
                    //Console.WriteLine("call load data userService");
                    str = boardService.LoadData();
                    //str = userService.LoadData();

                }
                Console.WriteLine(str);
                return str;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public string DeleteData()
        {
            try
            {
                //Console.WriteLine("delete in factory");
                string str = boardService.DeleteData();
                Response response = JsonSerializer.Deserialize<Response>(str);
                //Console.WriteLine(response.ReturnValue);
                //Console.WriteLine(response.ErrorMessage);

                if (!response.ErrorOccurd)
                {
                    str = userService.DeleteData();
                    Console.WriteLine("succes to delete Data!!");
                }
                return str;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail to delete data!" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        
            
        
    }
}

