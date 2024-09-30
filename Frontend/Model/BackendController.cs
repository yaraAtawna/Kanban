using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Diagnostics;
namespace Frontend.Model
{
    public class BackendController
    {
        private FactoryService Service { get; set; }
        public BackendController(FactoryService service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new FactoryService();
            Service.LoadData();
            
        }
        internal UserModel Register(string username, string password)
        {
            string res2 = Service.userService.Register(username, password);
            Response res= JsonSerializer.Deserialize<Response>(res2);
            if (res.ErrorOccurd)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(this, username);
        }
        public UserModel Login(string username, string password)
        {
            string res2 = Service.userService.Login(username, password);
            Response response = JsonSerializer.Deserialize<Response>(res2);
            if (response.ErrorOccurd)
            {
                throw new Exception(response.ErrorMessage);
            }
            return new UserModel(this, username);
        }
        public void Logout(string email)
        {
            string res2 = Service.userService.Logout(email);
            Response res = JsonSerializer.Deserialize<Response>(res2);
            if (res.ErrorOccurd)
            {
                throw new Exception(res.ErrorMessage);
            }
        }


        
        internal LinkedList<int> GetAllBoardsIds(string email)
        {
            string jsonBoardsIds = Service.boardService.GetUserBoards(email);
            Response res1 = JsonSerializer.Deserialize<Response>(jsonBoardsIds);
            if (res1.ErrorOccurd)
            {
                throw new Exception(res1.ErrorMessage);
            }

            LinkedList<int> boardsIds = Service.boardService.GetUserBoards1(email);
            //test
            //Debug.WriteLine("testing GetAllBoardsIds");
            //Debug.WriteLine("boards number is "+boardsIds.Count +"for use :" + email);
            return boardsIds;
        }
        internal (string Name, int id) GetBoard(int boardId)
        {
            string ans = Service.boardService.GetBoardName(boardId);
            
            //check this
            Response res1 = JsonSerializer.Deserialize<Response>(ans);
            if (res1.ErrorOccurd)
            {
                throw new Exception(res1.ErrorMessage);
            }
          
            string Name = Service.boardService.GetBoardName1(boardId);
            //test
            //Debug.WriteLine("testing GetBoard");
            //Debug.WriteLine("board Id is " + boardId + " board name is :" + Name);

            return (Name, boardId);
        }

        //fixed vesrion
        internal List<int> GetColumnIds(string email, string boardName, int columnOrdinal)
        {
            //like tirgul
            List<int>  ans= Service.boardService.GetColumnIds(email, boardName, columnOrdinal).ReturnValue;
            return ans;
            
        }
        /*
         * old version:
         * internal List<int> GetColumnIds(string email, string boardName, int columnOrdinal)
        {
            string jsonTasksId = Service.boardService.GetColumn(email, boardName, columnOrdinal);
            Response res0 = JsonSerializer.Deserialize<Response>(jsonTasksId);


            if (res0.ErrorOccurd)
            {
                throw new Exception(res0.ErrorMessage);
            }
            List<int> taskIds = new List<int>();

            var tasks = JsonSerializer.Deserialize<LinkedList<Task>>(res0.ReturnValue.ToString());
            foreach (var t in tasks)
            {
                taskIds.Add(t.Id);

            }

            return taskIds;
        }

         */

        internal (int Id, string Title, string Descreption,string creationtime,string todo) GetTask(string email, string _boardName, int columnOrdinal, int taskId)
         {

            
            int boardid = Service.boardService.GetBoardId(email, _boardName).ReturnValue;
            string taskTitle = Service.taskService.GetTitle(boardid, columnOrdinal, taskId);
            //Debug.WriteLine(taskTitle);

            string taskDesc = Service.taskService.GetDescription(boardid, columnOrdinal, taskId);
            string doto = Service.taskService.GetDueDate(boardid, columnOrdinal, taskId);
            string creation = Service.taskService.GetCreationTime(boardid, columnOrdinal, taskId);
            

           
            int taskId1 = taskId;
               
                return (taskId1, taskTitle, taskDesc, creation, doto);

            }
          
        

        internal (int id, string name ,List<string> member,string owner) GetBoard(string email, int boardid)
        {
            string res = Service.boardService.GetMembers(boardid);
            Response response = JsonSerializer.Deserialize<Response>(res);
            List<string> ans = Service.boardService.GetMembers1(boardid);
            string name = Service.boardService.GetBoardName(boardid);
            Response response1 = JsonSerializer.Deserialize<Response>(name);
            name  = response1.ReturnValue.ToString();
            string owner = Service.boardService.GetBoardOwner1(boardid);
          //  Response response2 = JsonSerializer.Deserialize<Response>(owner);
            //owner = (string)response1.ReturnValue;
            return (boardid,name,ans,owner);
        }
        internal void CreateBoard(string email,string name)
        {
            string res = Service.boardService.CreateBoard(email,name);
            Response response = JsonSerializer.Deserialize<Response>(res);
            if (response.ErrorOccurd)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        internal void DeleteBoard(string email, string boardid) {
            //Debug.WriteLine("DeleteBoard in backEnd");
            //Debug.WriteLine("boardid");
            //change this to name!!!
            string res = Service.boardService.DeleteBoard(email, boardid);
            Response response = JsonSerializer.Deserialize<Response>(res);
            if (response.ErrorOccurd)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        //new
        internal string GetBoardOwner(int boardId)
        {
            string ans = Service.boardService.GetBoardOwner(boardId);
           

            Response res1 = JsonSerializer.Deserialize<Response>(ans);
            if (res1.ErrorOccurd)
            {
                throw new Exception(res1.ErrorMessage);
            }

             ans =  res1.ReturnValue.ToString();

            // Assuming the Response object has a property called `Owner` or similar
            return ans;
        }

    }   
}
