using IntroSE.Kanban.Backend.DataAccesLayer.controllers;
using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Config;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardFacade
    {
        private Dictionary<int, Board> boards;
        private int idCounter;
        private int taskCounter;
        private UserFacade userFacade;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly BoardController boardController;
        private readonly ColumnController columnController;
        private readonly TaskController taskController;
        private readonly BoardUserStatusController boardUserStatusController;

        //new
        private int maxTitle=50;
       private int maxDec=300;
        private const int colBack = 0;
        private const int colProgress = 1;
        private const int colDone = 2;

        public BoardFacade(UserFacade userFacade)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("new BoardFacade log!");
            this.userFacade = userFacade;
            this.boards = new Dictionary<int, Board>();
            this.idCounter = 0;
            this.taskCounter = 0;
            this.boardController = new BoardController();
            this.taskController = new TaskController();
            this.columnController = new ColumnController();
            this.boardUserStatusController= new BoardUserStatusController();
           // Debug.WriteLine("boards.Count " + boards.Count);
        }

        /// <summary>
        /// check if the user email and board Name are legal and user is logged in
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="boardName">The name of the board</param>
        /// <exception cref="Exception"></exception>
        private void check(string userEmail, string boardName)
        {
            //Console.WriteLine("enterd check");
            //before : boardName == null || boardName == ""  now : if(string.IsNullOrEmpty(boardName))
            if (string.IsNullOrEmpty(boardName))
            {
                //Console.WriteLine("boardName is null");
                log.Warn("Board name is null or empty");

                throw new Exception("Board name is null or empty");

            }
            //before : userEmail == null || userEmail == ""
            if (string.IsNullOrEmpty(userEmail))
            {
                //Console.WriteLine("email is null");
                log.Warn("user email is null or empty");

                throw new Exception("user email is null or empty");

            }
            if (userFacade.isExist(userEmail) == false)
            {
                //
                //Console.WriteLine("user not exist");
                log.Warn("user not exist");
                throw new Exception("user not exist");


            }
            if (isLogIn(userEmail) == false)
            {
                //Console.WriteLine("user not loggeIn");
                log.Warn("user is not logged IN");
                throw new Exception("user is not logged IN");
            }

        }

        public bool boardExist(string boardName, string userEmail)
        {
           // Console.WriteLine("hi");
            foreach(int key in boards.Keys)
            {
                //Console.WriteLine(key);

                if (boards[key].isUserExist(userEmail) && boards[key].getBoardName() == boardName)
                    return true;
            }
            return false;
        }

        private bool isOwner(string boardName, string userEmail)
        {
            foreach (int key in boards.Keys)
            {
                if (boards[key].getOwner().Equals(userEmail) && boards[key].getBoardName() == boardName)
                       return true;         
            }
            return false;
        }


        /// <summary>
        /// This method creates a board for the given user
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="boardName">The name of the board</param>
        /// <exception cref="Exception"></exception>
        /// 
        public void createBoard(string userEmail, string boardName) // just the owner
        {
            

            //updated mileStone2
            check(userEmail, boardName); //7/7
            //Console.WriteLine("new2 id");

            if (boardExist(boardName , userEmail))
            { 
                throw new Exception("board name is taken");
            }
            //Console.WriteLine("new1 id");
            //Console.WriteLine(idCounter );

            Board b = new Board(idCounter, boardName, userEmail,false);
          
            boards.Add(idCounter, b);
            idCounter++;

            //data mileStone2
            //Console.WriteLine("new id");
            //Console.WriteLine(idCounter - 1);
            BoardDAO dao=new BoardDAO(idCounter-1,b.getBoardName(),b.getOwner());
            dao.persist();

            //needed?
           

            BoardUserStatusDAO boardUser = new BoardUserStatusDAO(userEmail, b.getBoardId(),"Owner");
            boardUser.persist();

            log.Info(" successfully created board");
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="boardName"></param>
        /// <exception cref="Exception"></exception>
        public void deleteBoard(string userEmail, string boardName)
        {
            //Debug.WriteLine("deleteBoard in board facade");
            //Debug.WriteLine(userEmail);
            //Debug.WriteLine("board name is"+boardName);

            check(userEmail, boardName);

            User user = userFacade.getUser(userEmail);

            if (boardExist(boardName, userEmail) == false)
            {

                throw new Exception("board does not exist ");

            }
            //updated mileStone2
            //check the owner
            if (isOwner(boardName, userEmail)==false)
            {
                throw new Exception("the user is not the owner of the board,cant delete! ");
            }

            int id = getIdByEmail(userEmail, boardName);
            //added
            if (id == -1)
            {
                throw new Exception("board does no exist");
            }
            boards.Remove(id);

            //data
            this.boardController.deleteBoard(id);
            this.boardUserStatusController.deleteBoard(id);
            this.taskController.deleteByBoard(id);
            this.columnController.deleteByBoard(id);

            log.Info("deleted board");

        }


        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>returns a task</returns>
        public Task addTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
          

            check(email, boardName);
            //check he is a member or owner of the board! new  to mileStone2
            if (boardExist(boardName, email)==false)
            {
                //Console.WriteLine("the user in not a member or owner of the board");

                throw new Exception("the user in not a member or owner of the board");  
            }
          

            if (DateTime.Now > dueDate)
            {
                log.Warn("The due date should not be in the past");
                throw new Exception("Due date is in the past");
            }
            //7/7
            //private int maxTitle=50;
            //private int maxDec=300;
            if (string.IsNullOrEmpty(title) || title.Length > maxTitle)
            {

                log.Warn("illegal title ");
                throw new Exception("illegal title");
            }
            

            if (description == null || description.Length > maxDec)
            {

                log.Warn("illegal description ");
                throw new Exception("illegal description ");
            }
            

            ////added
            /////updated mileStone2
            int id = getIdByEmail(email, boardName);
            if (id == -1)
            {
                throw new Exception("board does no exist");
            }

            //Console.WriteLine("new task");
            //Console.WriteLine("taskCounter");
           
            //Console.WriteLine(taskCounter);

            Task task = new Task(taskCounter, title, description, dueDate, DateTime.Now,email, colBack, id,false); // add assignee (email),  boardId , columndId
            boards[id].addTask(task);
            taskCounter++;

            ////data
            //TaskDAO dao = new TaskDAO(taskCounter, title, description, dueDate, DateTime.Now, email, 0, id);
            //this.taskController.insert(dao);


            log.Info("added task");
            return task;
        }

        /// <summary>
        /// get the id of a board 
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>return id of the board</returns>
        public int getIdByEmail(string email, string boardName)
        {
            //if(getBoardByEmail(email, boardName)==null)
            //{
            //    throw new Exception("board does not exist");
            //}

            //return getBoardByEmail(email, boardName).getBoardId();

            //updated mileStone2
            foreach (int key in boards.Keys)
            {
                if (boards[key].isUserExist(email) && boards[key].getBoardName().Equals(boardName))
                    return key;
            }
            return -1;
        }

        /// <summary>
        /// return a board with the name boardName
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>return a Board with the name boardName </returns>
        /// <exception cref="Exception"></exception>
        public Board getBoardByEmail(string email, string boardName)
        {
            //for owner and member
            //updated mileStone2
            try
            {
                check(email, boardName);

                foreach (int key in boards.Keys)
                {
                    if (boards[key].isUserExist(email) && boards[key].getBoardName().Equals(boardName))
                        return boards[key];
                }
                return null;
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// check if the user is looged in
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <returns>true if the user is logged in ,otherwise false</returns>
        private bool isLogIn(string email)
        {
            //no need to test email

            return userFacade.isLoggedIn(email);
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        public void limitColumn(string userEmail, string boardName, int columnOrdinal, int max)
        {
            if(max<=0 & max!=-1)
            {
                throw new Exception("illegal limit");
            }
            check(userEmail, boardName);
            //new -  member can limit column??
            //updated mileStone2
            if ( !isOwner(boardName, userEmail))
            {
                throw new Exception("user is not the owner cant limit");
            }

            if (columnOrdinal == colBack || columnOrdinal == colProgress || columnOrdinal == colDone)
            {

                Board b = getBoardByEmail(userEmail, boardName);
                
                b.setLimit(columnOrdinal, max);

                //data
                int id=b.getBoardId();
                columnController.update(id, columnOrdinal, "Max", max);

                log.Info("successfully limit column");
            }
            else
            {
                log.Warn("illegal columnOrdinal");
                throw new Exception("illegal columnOrdinal");
            }

        }

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <returns>list of tasks</returns>
        public LinkedList<BusinessLayer.Task> inProgressTasks(string userEmail)
        {
            //before : userEmail == null || userEmail == ""
            if (string.IsNullOrEmpty(userEmail))
            {
                log.Warn("user email is null or empty");
                throw new Exception("error in progress tasks");
            }
            if (userFacade.isExist(userEmail) == false)
            {
                //Console.WriteLine("user not exist");
                log.Warn("user not exist");
                throw new Exception("user not exist");


            }
            if (isLogIn(userEmail) == false)
            {
                //Console.WriteLine("user not loggeIn");
                log.Warn("user is not logged IN");
                throw new Exception("user is not logged IN");
            }

            LinkedList<Task> res = new LinkedList<Task>(); ;

            //updated mileStone2
            //Dictionary<string, Board> boards = userFacade.getUser(userEmail).getAllBoards();
            Dictionary<string, Board> boards= getUserBoardsDict(userEmail);

            if (boards == null)
            {
                throw new Exception("boards dict is null");
            }
            foreach (Board board in boards.Values)
            {
                if (board != null)
                {
                    Column col = board.getInProcess();
                    if (col != null)
                    {
                        LinkedList<Task> col1 = col.Tasks;
                        if (col1 != null)
                        {
                            foreach (Task task in col1)
                            {
                                if (task != null)
                                {
                                    //updated mileStone2
                                    if(task.Assignee.Equals(userEmail))
                                    {
                                        res.AddFirst(task);
                                    }
                                 } } }}  }  }
            return res;
        }

        //return all board the user is member in them or owner
        private Dictionary<string, Board> getUserBoardsDict(string userEmail)
        {
            Dictionary<string, Board> ans= new Dictionary<string, Board>();
            foreach (int key in boards.Keys)
            {
                if (boards[key].isUserExist(userEmail) )
                    ans.Add(boards[key].getBoardName(), boards[key]);
            }
            return ans;
        }

        /// <summary>
        /// change the task title
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">board title</param>
        /// <param name="columnOrdinal">column number</param>
        /// <param name="taskId">task id</param>
        /// <param name="title">new task title</param>
        /// <exception cref="Exception"></exception>
        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            check(email, boardName);

            if (columnOrdinal<colBack || columnOrdinal>colProgress )
            {
                log.Info("illegal columnOrdinal");
                throw new Exception("illegal columnOrdinal");

            }
            if (string.IsNullOrEmpty(title) || title.Length > maxTitle)
            {
                log.Warn("title is not legal");
                throw new Exception("title is not legal");

            }
            int id = getIdByEmail(email, boardName);
            //updated mileStone2

            if (id == -1)
            {
                throw new Exception("board does no exist");
            }
            Task s = boards[id].getTask(columnOrdinal, taskId);
            if(!s.Assignee.Equals(email))
            {
                throw new Exception("user not assigned to the task");
            }
            s.Title = title;

            //data
            int sId=s.Id;
            this.taskController.update(sId, "Title", title);
        }

        /// <summary>
        /// change the task Description
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">board title</param>
        /// <param name="columnOrdinal">column number</param>
        /// <param name="taskId">task id</param>
        /// <param name="description">new task description</param>
        /// <exception cref="Exception"></exception>
        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            if (columnOrdinal == colDone)
            {
                log.Info("the task is done  cant be changed");
                throw new Exception("the task is done  cant be changed");

            }
            if (description == null || description.Length > maxDec)
            {
                log.Warn("description is not legal");
                throw new Exception("description is not legal");
            }
            //updated mileStone2
            int id = getIdByEmail(email, boardName);
            if (id == -1)
            {
                throw new Exception("board does no exist");
            }
            Task s = boards[id].getTask(columnOrdinal, taskId);
            if (!s.Assignee.Equals(email))
            {
                throw new Exception("user not assigned to the task");
            }
            s.Description = description;

            //data
            int sId = s.Id;
            this.taskController.update(sId, "Descreption", description);
        }

        /// <summary>
        /// change the task dueDate
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">board title</param>
        /// <param name="columnOrdinal">column number</param>
        /// <param name="taskId">task id</param>
        /// <param name="dueDate">new task description</param>
        /// <exception cref="Exception"></exception>
        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {

            if (columnOrdinal == 2)
            {
                log.Info("the task is done  cant be changed");
                throw new Exception("the task is done  cant be changed");

            }

            if (DateTime.Now > dueDate)
            {
                log.Warn("The due date should not be in the past");
                throw new Exception("Due date is in the past");
            }
            int id = getIdByEmail(email, boardName);
            //updated mileStone2
            if (id == -1)
            {
                throw new Exception("board does no exist");
            }
            Task s = boards[id].getTask(columnOrdinal, taskId);

            if (!s.Assignee.Equals(email))
            {
                throw new Exception("user not assigned to the task");
            }
            s.DueDate = dueDate;

            //data
            int sId = s.Id;
            this.taskController.update(sId, "DueDate", dueDate);
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> the column's limit, unless an error occurs </returns>
        public int getColumnLimit(string email, string boardName, int columnOrdinal)
        {
            check(email, boardName);
            //updated mileStone2
            if (boardExist(boardName,email) == false)
            {
                throw new Exception("user is not a member or owner");
            }
            if (columnOrdinal == colDone || columnOrdinal == colBack || columnOrdinal == colProgress)
            {
                int lim = getBoardByEmail(email, boardName).getLimit(columnOrdinal);

                return lim;
            }
            else
            {
                throw new Exception("illegal columnOrdinal");
            }
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> column's name, unless an error occurs </returns>
        public string getColumnName(string email, string boardName, int columnOrdinal)
        {
            //Console.WriteLine($"col in facad: {columnOrdinal} ");
            check(email, boardName);

            //updated mileStone2
            if (boardExist(boardName, email) == false)
            {
                throw new Exception("user is not a member or owner");
            }

            if (columnOrdinal == colBack)
            {
                return "backlog";
            }
            if (columnOrdinal == colProgress)
            {
                return "in progress";
            }
            if (columnOrdinal == colDone)
            {
                return "done";
            }
            throw new Exception("illegal  columnOrdinal");
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>"true" unless an error occurs</returns>
        public string advanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {

            if (taskId < 0)
            {
                throw new Exception("error advanceTask");
            }
            check(email, boardName);
            if (columnOrdinal < 0 || columnOrdinal > 1)
            {
                throw new Exception("illegal columnOrdinal");
            }
            //updated mileStone2
            Board b = getBoardByEmail(email, boardName);
            Task s = b.getTask(columnOrdinal, taskId);
            if (!s.Assignee.Equals(email))
            {
                throw new Exception("user not assigned to the task");
            }
            if (columnOrdinal == colBack)
            {     
                if (b.moveToColumnOne(taskId))
                {
                    //data
                    int sId = s.Id;
                    this.taskController.update(sId, "ColumnId", columnOrdinal+1);
                    //remove from col
                    //add to col
                    //data
                    //TaskDAO dao = new TaskDAO(taskCounter, s.title, s.description, s.dueDate, s.creationDate, email, columnOrdinal+1, s.id);
                    //this.columnController.advanceTask(taskId);



                    return "true";
                }
                else
                {
                    throw new Exception("error advanceTask to in progross");
                }

            }
            else if (columnOrdinal == colProgress)
            {
                //move to done
                if (b.moveToColumnTwo(taskId))
                {
                    //data
                    int sId = s.Id;
                    this.taskController.update(sId, "ColumnId", columnOrdinal + 1);
                    return "true";
                }
                else
                {
                    //error
                    log.Error("cant advanceTask to done");
                    throw new Exception("error advanceTask to done");
                }
            }
            else
            {
                log.Warn("cant andvance ");
                throw new Exception("cant andvance ");

            }


        }


        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>a list of the column's tasks, unless an error occurs </returns>
        public LinkedList<Task> getColumn(string email, string boardName, int columnOrdinal)
        {
            //updated 2
            check(email, boardName);
            if (userFacade.getUser(email) == null)
            {
                log.Warn("user doesnt exist");
                throw new Exception("user doesnt exist");
            }
            //GetUserBoards userFacade.getUser(email).getAllBoards()
            //if (getUserBoardsDict(email)[boardName] == null)
            //{
            //    log.Warn("board doesnt exist for this username ");
            //    throw new Exception("board doesnt exist for this username ");
            //}
            
            //new mileStone2
            if (boardExist(boardName, email) == false)
            {
                throw new Exception("the user in not a member or owner of the board");
            }

            if (!(columnOrdinal==colBack || columnOrdinal ==colProgress || columnOrdinal == colDone ))
            {
                log.Warn("illegal columnOrdinal ");
                throw new Exception("illegal columnOrdinal ");
            }
            Board board = getUserBoardsDict(email)[boardName];
            if (board == null)
            {
                throw new Exception("boards dict is null");
            }
            try
            {
                Column col;
                if (columnOrdinal == 0)
                {
                    col = board.getbacklog();
                }
                else if (columnOrdinal == 1)
                {
                    col = board.getInProcess();
                }
                else
                {
                    col = board.getDone();
                }
                if (col != null)
                {
                  
                    return col.Tasks;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return null;
        }


        //mileStone2 methods!!!!




        /// <summary>		 
        /// This method returns a list of IDs of all user's boards.		 
        /// </summary>		 
        /// <param name="email">Email of the user. Must be logged in</param>		 
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public LinkedList<int> GetUserBoards(string email)
        {
            LinkedList < int > ans =new LinkedList < int >();
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }
            if (userFacade.isExist(email) == false)
            {
                //Console.WriteLine("user not exist");
                log.Warn("user not exist");
                throw new Exception("user not exist");


            }
            if (isLogIn(email) == false)
            {
                //Console.WriteLine("user not loggeIn");
                log.Warn("user is not logged IN");
                throw new Exception("user is not logged IN");
            }
            
            foreach (int key in boards.Keys)
            {
                if (boards[key].isUserExist(email))
                    ans.AddLast(key);
            }
            return ans;
        }

        /// <summary>		 
        /// This method adds a user as member to an existing board.		 
        /// </summary>		 
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public void JoinBoard(string email, int boardID)
        {
            if(!boards.ContainsKey(boardID))
            {
                throw new Exception("illegal board id");
            }
            if (boards[boardID] ==null)
            {
                throw new Exception("board does not exist");
            }
            check(email, boards[boardID].getBoardName());
            
            //add user to members list
            User user = userFacade.getUser(email);
            if(boards[boardID].addMember( email,user)==false)
            {
                throw new Exception("cant join");
            }

            //data
            BoardUserStatusDAO bu = new BoardUserStatusDAO(email,boardID,"Member");
            bu.persist();

            
        }
        


        /// <summary>		 
        /// This method removes a user from the members list of a board.		 
        /// </summary>		 
        /// <param name="email">The email of the user. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public void LeaveBoard(string email, int boardID)
        {

            if (!boards.ContainsKey(boardID))
            {
                throw new Exception("illegal board id");
            }
            if (boards[boardID] == null)
            {
                throw new Exception("board does not exist");
            }

            string boardName = boards[boardID].getBoardName();
            check(email, boardName);
            //Console.WriteLine("del00");

            //owner cant leave 
            if (boards[boardID].getOwner().Equals(email) )
            {
                throw new Exception("owner cant leave board");
            }
                
            //remove the user from board
            User user = userFacade.getUser(email);
            if (boards[boardID].removeMember(email) == false)
            {
                throw new Exception("cant remove");
            }

            //data + mileStone2
            //Console.WriteLine("del0");

            BoardUserStatusDAO bu = new BoardUserStatusDAO(email, boardID, "Member");
            //Console.WriteLine("del1");
            this.boardUserStatusController.delete(bu);
            //Console.WriteLine("del");
            unAssignTaskData(boardID,colBack);
            unAssignTaskData( boardID, colProgress);
            unAssignTaskData( boardID, colDone);
            //this.taskController.update()


        }

        private void unAssignTaskData( int boardID ,int col) 
        {
            LinkedList<Task> lst1=boards[boardID].GetColumn(col).Tasks;
            foreach (Task t in lst1)
            {
                if(!t.IsAssigned)
                {
                    this.taskController.update(t.Id,"Assignee","unassign");
                }
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
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            check(email, boardName);
            check(emailAssignee, boardName);
            if (columnOrdinal < colBack || columnOrdinal > colDone) 
            {
                throw new Exception("illegal column ordinal");
            }
            //check if board exist
            Board b= getBoard(boardName, emailAssignee);
            if(b == null)
            {
                throw new Exception("board does no exist");
            }
            Column col=b.GetColumn(columnOrdinal);
            if (col == null)
            {
                throw new Exception("illegal column");
            }
            Task task=col.getTask(taskID);
            if (task == null)
            {
                throw new Exception("task does no exist");
            }
            if (!task.IsAssigned || (task.IsAssigned && task.Assignee.Equals(email)))
            {
                task.Assignee = emailAssignee;
                task.IsAssigned = true;
            }
            else
            {
                throw new Exception("unassigned user can assign an assigned task ");
            }
            //C:\Users\yaraa\source\repos\BGU-SE-Courses\kanban-2024-2024-29\Backend\kanban.db
            //C:\Users\yaraa\source\repos\BGU-SE-Courses\kanban-2024-2024-29\Backend\kanban.db

            //data
            this.taskController.update(task.Id, "Assignee", emailAssignee);

        }

        private Board getBoard(string name,string email)
        {
            foreach (int key in boards.Keys)
            {
                if (boards[key].getBoardName().Equals(name) && (boards[key].isUserExist(email)))
                    return boards[key];
            }
            return null;
        }

        /// <summary>		 
        /// This method returns a board's name		 
        /// </summary>		 
        /// <param name="boardId">The board's ID</param>		 
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string GetBoardName(int boardId)
        {
            if (!boards.ContainsKey(boardId))
            {
                throw new Exception("illegal board id");
            }
            if (boards[boardId] == null)
            {
                throw new Exception("board does not exist");
            }
            return boards[boardId].getBoardName();
        }


        /// <summary>		 
        /// This method transfers a board ownership.		 
        /// </summary>		 
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>		 
        /// <param name="newOwnerEmail">Email of the new owner</param>		 
        /// <param name="boardName">The name of the board</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            check(currentOwnerEmail, boardName);
            check(newOwnerEmail, boardName);
            if (!boardExist(boardName, newOwnerEmail)  )
            {
                throw new Exception("the new owner is not a member");
            }
            if (! isOwner(boardName, currentOwnerEmail))
            {
                throw new Exception("the currentOwnerEmail is not the owner");
            }
            Board b=getBoardByEmail(currentOwnerEmail, boardName);
            b.setNweOwner(currentOwnerEmail, newOwnerEmail, userFacade.getUser(currentOwnerEmail));
            int id=b.getBoardId();
            //data
            
            this.boardUserStatusController.updateStatus(newOwnerEmail, id, "Owner");
            this.boardUserStatusController.updateStatus(currentOwnerEmail,id, "Member");
            this.boardController.update(b.getBoardId(), "Owner",newOwnerEmail);
            //Console.WriteLine("here 1");


        }



        ///<summary>This method loads all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public void LoadData()
        {
            //load boards
           

            List<BoardDAO> list = boardController.GetAllBoards();
           

            foreach (BoardDAO b in  list)
            {
               

                Board board = new Board(b.BoardId, b.Name, b.Owner ,true);
               
                boards.Add(b.BoardId, board);
                

                loadCloumn(board);
                
                loadTasks(board);

               // Console.WriteLine("1");
                loadBoardToUser(board);
                //Console.WriteLine("2");

                //not sure
                this.idCounter= b.BoardId+1;
                //or
                //this.idCounter++;
                //
            }

        }

        private void loadCloumn(Board board)
        {
            //load column
            List<ColumnDAO> columns = this.columnController.getAllcolumns();
            foreach (ColumnDAO column in columns)
            {
                if(column.BoardId==board.getBoardId())
                {
                    board.setLimit(column.Id, column.Max);
                }
            }
        }
        private void loadTasks(Board board)
        {
            List < TaskDAO> tasks = this.taskController.getAllTasks(board.getBoardId());
            //test
            //Debug.WriteLine("loadTasks for board : " + board.getBoardId());
            //Debug.WriteLine(tasks.Count);


            foreach (TaskDAO task in tasks)
            {
                //Console.WriteLine("task id ");
                //Console.WriteLine(taskCounter);
                Task s=new Task(task.Id,task.Title,task.Description,task.DueDate,task.CreationDate,task.Assignee,task.ColumnId,task.BoardId,true);
                //taskCounter++;
                taskCounter = task.Id+1;
                switch (task.ColumnId)
                {
                    case 0:
                        board.Backlog.addTask(s);
                        break;

                    case 1:
                        board.InProgress.addTask(s);
                        break;
                    case 2:
                        board.Done.addTask(s);
                        break;

                }
            }

        }
        private void loadBoardToUser(Board board)
        {
            List<BoardUserStatusDAO> boards = this.boardUserStatusController.getAll();
            foreach(BoardUserStatusDAO b in boards  )
            {
                User us= userFacade.getUser(b.UserEmail);
                if(b.status=="Member")
                { board.addMember(b.UserEmail, us); }

            }
        }

        

        ///<summary>This method deletes all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b>		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public void DeleteData()
        {
            //Console.WriteLine("delete in facade");
            this.boardController.deleteAll();
            this.taskController.deleteAll();
            this.columnController.deleteAll();
            this.boardUserStatusController.deleteAll();
        }
        public string GetOwner(int  boardid)
        {
            if (!boards.ContainsKey(boardid))
            {
                throw new Exception("illegal board id");
            }
            if (boards[boardid] == null)
            {
                throw new Exception("board does not exist");
            }
            return boards[boardid].getOwner();
        }

        public Task GetTask( int boardId, int columnorOrdinal,int taskid)
        {
           return boards[boardId].getTask(columnorOrdinal, taskid);
        }
        public string GetTitle(int boardId, int columnorOrdinal, int taskid)
        {
            Task s = GetTask(boardId, columnorOrdinal, taskid);
            return s.Title;
        }
        public string GetDescription(int boardId, int columnorOrdinal, int taskid)
        {
            Task s = GetTask(boardId, columnorOrdinal, taskid);

            return s.Description;
        }

        public string GetDueDate(int boardId, int columnorOrdinal, int taskid)
        {
            Task s = GetTask(boardId, columnorOrdinal, taskid);

            return s.DueDate.ToString();
        }
        public string GetCreationTime(int boardId, int columnorOrdinal, int taskid)
        {
            Task s = GetTask(boardId, columnorOrdinal, taskid);

            return s.CreationDate.ToString();
        }
        public List<string> GetMembers(int boardId)
        {
            Dictionary<string, User> members = boards[boardId].GetMembers();
            List<string> ret = new List<string>();
            foreach(string key in members.Keys)
            {
                ret.Add(key);
            }
            return ret;
        }
    } 
}


