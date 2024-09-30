using IntroSE.Kanban.Backend.BusinessLayer;
using log4net.Config;
using log4net;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private BoardFacade boardFacade;
        //log field
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal BoardService(BoardFacade boardFacade)
        {
            this.boardFacade = boardFacade;
            //intilize log
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("new BoardService log!");
        }

        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string CreateBoard(string email, string name)
        {
            try
            {
                boardFacade.createBoard(email, name);
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);

            }
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteBoard(string email, string name)
        {
            try
            {
                //Debug.WriteLine("delete board in board Service");
                //Debug.WriteLine("board name is "+name); 
                boardFacade.deleteBoard(email, name);
                //check success
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
                // return $"{{ErrorMessage:\"{ex.Message}\", ReturnValue:\"}}";
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {

                boardFacade.limitColumn(email, boardName, columnOrdinal, limit);
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
                //return $"{{ErrorMessage:\"{ex.Message}\", ReturnValue:\"}}";
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                int lim = boardFacade.getColumnLimit(email, boardName, columnOrdinal);
                //Response response = new Response(lim.ToString());
                Response response = new Response();
                response.ReturnValue = lim;
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                String ans = boardFacade.getColumnName(email, boardName, columnOrdinal);
                Response response = new Response();
                response.ReturnValue = ans;

                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {

                boardFacade.addTask(email, boardName, title, description, dueDate);
                //check for succes
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                boardFacade.advanceTask(email, boardName, columnOrdinal, taskId);
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                LinkedList<BusinessLayer.Task> ans = boardFacade.getColumn(email, boardName, columnOrdinal);
                Response response = new Response();
                response.ReturnValue = ans;
                return JsonSerializer.Serialize(response);

            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize<Response>(response);
            }
        }//GetColumnIds

        public Response<List<int>> GetColumnIds(string email, string boardName, int columnOrdinal)
        {
            try
            {
                LinkedList<BusinessLayer.Task> ans = boardFacade.getColumn(email, boardName, columnOrdinal);
                List<int> tasks = new List<int>();
                foreach (BusinessLayer.Task t in ans)
                {
                    tasks.Add(t.Id);
                }

                //Response response = new Response();
                //response.ReturnValue = ans;
                //return JsonSerializer.Serialize(response);
                return new Response<List<int>>(tasks);

            }
            catch (Exception ex)
            {
                //Response response = new Response(ex.Message);
                //return JsonSerializer.Serialize<Response>(response);
                return new Response<List<int>>(ex.Message);
            }
        }


        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            try
            {
                //String ans = boardFacade.inProgressTasks(email);

                LinkedList<BusinessLayer.Task> ans = boardFacade.inProgressTasks(email);
                Response response = new Response();
                response.ReturnValue = ans;
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize<Response>(response);
            }
        }


        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------



        /// <summary>		 
        /// This method returns a list of IDs of all user's boards.		 
        /// </summary>		 
        /// <param name="email">Email of the user. Must be logged in</param>		 
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string GetUserBoards(string email)
        {
            try
            {
                //String ans = boardFacade.inProgressTasks(email);

                LinkedList<int> ans = boardFacade.GetUserBoards(email);
                Response response = new Response();
                response.ReturnValue = ans;
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize<Response>(response);
            }
        }
        public LinkedList<int> GetUserBoards1(string email)
        {
            LinkedList<int> ans = boardFacade.GetUserBoards(email);
            return ans;
        }


            /// <summary>		 
            /// This method adds a user as member to an existing board.		 
            /// </summary>		 
            /// <param name="email">The email of the user that joins the board. Must be logged in</param>		 
            /// <param name="boardID">The board's ID</param>		 
            /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
            public string JoinBoard(string email, int boardID)
        {
            try
            {
                boardFacade.JoinBoard(email, boardID);
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        /// <summary>		 
        /// This method removes a user from the members list of a board.		 
        /// </summary>		 
        /// <param name="email">The email of the user. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string LeaveBoard(string email, int boardID)
        {
            try
            {

                boardFacade.LeaveBoard(email, boardID);
                //Console.WriteLine("hh");
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        /// <summary>		 
        /// This method assigns a task to a user		 
        /// </summary>		 
        /// <param name="email">Email of the user. Must be logged in</param>		 
        /// <param name="boardName">The name of the board</param>		 
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>		 
        /// <param name="taskID">The task to be updated identified a task ID</param>        		 
        /// <param name="emailAssignee">Email of the asignee user</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                boardFacade.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                Response response = new Response();

                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        /// <summary>		 
        /// This method returns a board's name		 
        /// </summary>		 
        /// <param name="boardId">The board's ID</param>		 
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string GetBoardName(int boardId)
        {
            try
            {
                //boardFacade.GetBoardName(boardId);
                Response response = new Response();
                response.ReturnValue = boardFacade.GetBoardName(boardId);

                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }
        public string GetBoardName1(int boardId)
        {
            return boardFacade.GetBoardName(boardId);
        }

            /// <summary>		 
            /// This method transfers a board ownership.		 
            /// </summary>		 
            /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>		 
            /// <param name="newOwnerEmail">Email of the new owner</param>		 
            /// <param name="boardName">The name of the board</param>		 
            /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
            public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                boardFacade.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        ///<summary>This method loads all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string LoadData()
        {
            //Console.WriteLine("load");
            try
            {
                boardFacade.LoadData();
                Response response = new Response();

                return JsonSerializer.Serialize(response);
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
                //Console.WriteLine("DeleteData in board");
                boardFacade.DeleteData();
                Response response = new Response();

                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }
        public string GetBoardOwner(int id)
        {
            try
            {
                string ans = boardFacade.GetOwner(id);
                Response response = new Response();
                response.ReturnValue = ans;
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }
        public string GetBoardOwner1(int id)
        {
            return boardFacade.GetOwner(id);
        }
        public string GetTask(string email, int boardId, int column, int taskid)
        {
            try
            {
                BusinessLayer.Task ans = boardFacade.GetTask(boardId, column, taskid);
                Response response = new Response();
                response.ReturnValue = ans;
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }
        public List<string> GetMembers1(int id)
        {
            return boardFacade.GetMembers(id);
        }
       public string GetMembers(int id)
       {
            try
            {
                List<string> ans = boardFacade.GetMembers(id);
                Response response = new Response();
                response.ReturnValue = ans;
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
       }


        //updated
        public Response<int> GetBoardId(string email , string name)
        {
            try
            {
                //int ans = boardFacade.getIdByEmail(email, name);
                //Response response = new Response();
                //response.ReturnValue = ans;
                //return JsonSerializer.Serialize(response);
                return new Response<int>(boardFacade.getIdByEmail(email, name));

            }
            catch (Exception ex)
            {
                //Response response = new Response(ex.Message);
                //return JsonSerializer.Serialize(response);
                return new Response<int>(ex.Message);

            }
           
        }
        //new 
        //public Response<Board> GetBoard(string email, string name)
        //{
        //     Board b=boardFacade.getBoardByEmail(email,name);
        //    if(b == null)
        //    {
        //        return new Response<BusinessLayer.Board>("Board not found");
        //    }
        //    else
        //    {
        //        return new Response<BusinessLayer.Board>(b);
        //    }
        //}
    }
}

