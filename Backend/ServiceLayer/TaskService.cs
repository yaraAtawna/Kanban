using IntroSE.Kanban.Backend.BusinessLayer;
using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{ 
    public class TaskService

{
    private BoardFacade boardFacade;
    //log field
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

      internal TaskService(BoardFacade boardFacade) {
      this.boardFacade = boardFacade;
            //intilize log
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("new TaskFacade log!");
        }
        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
           {


        try
        {
            boardFacade.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
            Response response = new Response();
            return JsonSerializer.Serialize<Response>(response);
            }
            catch (Exception ex)
        {
            Response response = new Response(ex.Message);
            return JsonSerializer.Serialize<Response>(response);
        }
    }
        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {


        try
        {
            boardFacade.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
            Response response = new Response();
            return JsonSerializer.Serialize<Response>(response);
            }
        catch (Exception ex)
        {
            Response response = new Response(ex.Message);
            return JsonSerializer.Serialize<Response>(response);
        }
    }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
        try
        {
            boardFacade.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
                Response response = new Response();
                return JsonSerializer.Serialize<Response>(response);
            }
        catch (Exception ex)
        {
            Response response = new Response(ex.Message);
            return JsonSerializer.Serialize<Response>(response);
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
            throw new NotImplementedException();

            //  WE DID THE LOAD DATA IN BOARD SERVICE !!!!
            //  WE DID THE LOAD DATA IN BOARD SERVICE !!!!
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
            throw new NotImplementedException();
            //  WE DID THE DELETE DATA IN BOARD SERVICE !!!!
            //  WE DID THE DELETE DATA IN BOARD SERVICE !!!!

        }
        public string GetTitle(int boardId, int columnorOrdinal, int taskid)
        {
            return boardFacade.GetTitle(boardId,columnorOrdinal,taskid);
        }
        public string GetDescription(int boardId, int columnorOrdinal, int taskid)
        {
            return boardFacade.GetDescription(boardId,columnorOrdinal,taskid);
        }

        public string GetDueDate(int boardId, int columnorOrdinal, int taskid)
        {
            return boardFacade.GetDueDate(boardId,columnorOrdinal,taskid);
        }
        public string GetCreationTime(int boardId, int columnorOrdinal, int taskid)
        {
            return boardFacade.GetCreationTime( boardId, columnorOrdinal, taskid);
        }

    }
}
