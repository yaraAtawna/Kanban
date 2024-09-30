using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;
using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Task
    {
        private int id;
        public int Id 
        { get { return id; }  
            set {
                if (value == null)
                    throw new ArgumentNullException("value");
                id = value;
            } 
        }

        private string title ;
        public string Title{ get { return title; } 
            set 
            {
                if (checktitle(value))
                      { title = value; }
            } }

        private string description ;
        public string Description
        { get { return description; } set 
            { 
                if(checkdesc(value))
                { description = value; }    

            } 
        }
        private DateTime dueDate ;
        public DateTime DueDate 
        { 
            get { return dueDate; } 
            set { 
            if(checkdate(value))
                {  dueDate = value; }
            } }

        private DateTime creationDate;
        public DateTime CreationDate { get { return creationDate; } }

        private string assignee; // user
        public string Assignee // user
        { get { return assignee; } set 
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                assignee = value;
            } }

        private bool isAssigned;
        public bool IsAssigned {  get { return isAssigned; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                isAssigned = value;
            }
        }


        private int columnId;
        public int ColumnId
        {
            get { return columnId; }
            set
            {
                if(value==null)
                {
                    throw new ArgumentNullException("value");
                }
                columnId = value;
            }
        }
        private int boardId;
        public int BoardId
        {
            get { return boardId; }
            set
            {
                if(value==null)
                { throw new ArgumentNullException("value"); }
            boardId = value;
            }
            
        }

        private TaskDAO taskDAO;
        public TaskDAO TaskDAO { get { return taskDAO; } set {
                if (value == null)
                { throw new ArgumentNullException("value"); }
                taskDAO = value;
            } }

        public readonly int maxTitle = 50;
        public readonly int maxDesc = 300;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       // [JsonConstructor]
        public Task(int id, string title, string description, DateTime dueDate, DateTime creationDate ,string assignee, int columnId , int boardId,bool load)
        {
            
            this.id = id;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.creationDate = creationDate;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            this.assignee = assignee;
            this.columnId = columnId;
            this.boardId = boardId;
            this.isAssigned=true;
            if (!load)
            {
                taskDAO = new TaskDAO(id, title, description, dueDate, creationDate, assignee, columnId, boardId);
                taskDAO.persist();
            }
            log.Info("new Task log!");
           
        }
        //public Task()
        //{
        //    this.id = 0;
        //    this.title = "''";
        //    this.description = "''";
        //    this.dueDate = new DateTime(1, 1, 1, 0, 0, 0); 
        //    this.creationDate = new DateTime(1, 1, 1, 0, 0, 0); 
        //    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        //    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

        //    log.Info("new Task log!");
        //}

        public override string ToString()
        {
            return $"Task ID: {id}, Title: {title}, Description: {description}, Due Date: {dueDate}, Creation Time: {creationDate}";
        }
        //public long getId()
        //{
        //    log.Info($"getId called. Returning taskId: {this.id}");
        //    return (long)this.id;
        //}
        //public void setTitle(string title)
        //{
        //    if (checktitle(title))
        //        this.title = title;
        //}
        

        public bool checktitle(string title)
        {
            if (title == null)
            {
                log.Warn("title is null");
                throw new Exception("title is null");
            }
            if (title.Length == 0)
            {
                log.Warn("title sould not be empty");
                throw new Exception("title is empty");
            }
            if (title.Length > maxTitle)
            {
                log.Warn("title sould not be more than 50 in length");
                throw new Exception("title length more than 50");
            }
            return true;
        }
        //public void setDescription(string description)
        //{
        //    if (checkdesc(description))
        //    {
        //        this.description = description;

        //    }
        //}
        public bool checkdesc(string desc)
        {
            if (desc == null)
            {
                log.Warn("Description is null");
                throw new Exception("Description is null");
            }
            if (desc.Length > maxDesc)
            {
                log.Warn("Description should not be more than 300 characters in length");
                throw new Exception("Description length exceeds 300 characters");
            }
            return true;
        }
        //public void setDueDate(DateTime dueDate)
        //{
        //    if (checkdate(dueDate))
        //    {
        //        this.dueDate = dueDate;

        //    }
        //}
        public bool checkdate(DateTime date)
        {
            if (date.Equals(null))
            {
                log.Warn("Date is null");
                throw new Exception("Date is null");
            }
            if (DateTime.Now > date)
            {
                log.Warn("The due date should not be in the past");
                throw new Exception("Due date is in the past");
            }
            return true;
        }
        //public string getTitle()
        //{
        //    return title;
        //}
        //public string getDescription()
        //{
        //    return description;
        //}
        //public string getDueDate()
        //{
        //    return dueDate.ToString();
        //}
        //public string getCreationTime()
        //{
        //    return creationDate.ToString();
        //}
    }
}
