using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Board
    {
        private int boardId;
        private string name;
        private Column backlog; //0
        private Column inProgress;//1
        private Column done; //2
        private Dictionary<string, User> members;
        private string owner;

        private const int colBack=0;
        private const int colProgress = 1;
        private const int colDone=2;



        public Column Backlog
        {
            get
            {
                return backlog;
            }
            set
            {
                this.backlog = value;
            }
        }
        public Column InProgress
        {
            get
            {
                return inProgress;
            }
            set
            {
                this.inProgress = value;
            }
        }
        public Column Done
        {
            get
            {
                return done;
            }
            set
            {
                this.done = value;
            }
        }
        public Board(int id, string name , string ownerEmail ,bool load) 
        {
            this.boardId = id;
            this.name = name;

            
            backlog = new Column(colBack,load, boardId);
            //Console.WriteLine("test");
            inProgress = new Column(colProgress, load, boardId);
            done = new Column(colDone, load, boardId);

            this.owner = ownerEmail; 
            //added 
            members = new Dictionary<string, User>();
           

        }

        public void addTask(Task task ) 
        { 
            this.backlog.addTask( task );
        }
        
        public Dictionary<string,User> GetMembers()
        {
          

            return members;
        }

        public void removeTask(Task task, int columnOrdinal)
        {
            if (columnOrdinal == colBack) 
            {
                backlog.removeTask(task);
            }
            else if (columnOrdinal == colProgress) { inProgress.removeTask(task); }
            else if (columnOrdinal == colDone)
            { 
                done.removeTask(task);
            }
            
        }
        public int getBoardId() { return boardId; }
        public void setLimit(int columnOrdinal, int max) 
        {
            if (columnOrdinal == colBack) 
            {
                backlog.Max=max;
                //Console.WriteLine($"new limit is: {backlog.getMax()} ");
            }
            else if (columnOrdinal == colProgress)
            { 
                inProgress.Max=max;
                //Console.WriteLine($"new limit is: {inProgress.getMax()} ");

            }
            else if (columnOrdinal == colDone) 
            { 
                done.Max = max;
                //Console.WriteLine($"new limit is: {done.getMax()} ");

            }
            else { //error;
                
                throw new Exception(" illegal columnOrdinal  ");
            }
            
        }

        public Column getInProcess()
        {
            return this.inProgress;
        }
        
        public Column getDone() { return this.done; }
        public Column getbacklog() { return this.backlog; }

        public Task getTask(int columnOrdinal, int taskId) 
        {
            if (columnOrdinal == colBack)
            {
                Task s= backlog.getTask(taskId);
                if (s != null)
                {
                    return s;
                }
                throw new Exception("task does not exist");
               
            }
            else if (columnOrdinal == colProgress) 
            { 
                Task s = inProgress.getTask(taskId);
                if (s != null)
                {
                    return s;
                }
                throw new Exception("task does not exist");


            }
            else if (columnOrdinal == colDone) 
            {
                Task s = done.getTask(taskId);
                if (s != null)
                {
                    return s;
                }
                throw new Exception("task does not exist");
            }
            throw new Exception("illegal columnOrdinal");
        }

        public int getLimit(int columnOrdinal) 
        {
            if (columnOrdinal == colBack) { return backlog.Max; }
            else if (columnOrdinal == colProgress) { return inProgress.Max; }
            else if (columnOrdinal == colDone) { return done.Max; }
            return -1;
             //error;
            
        }
        public string getColumnName(int columnOrdinal)
        {
            if (columnOrdinal == colBack) { return "backlog";  }
            else if (columnOrdinal == colProgress) { return "inProgress"; }
             return "done "; 
          
        }
        public bool moveToColumnOne(int taskId)
        {
            Task s = backlog.getTask(taskId);
            if (s == null)
            {
                throw new Exception("task does not exist"); ;
            }
            if (inProgress.addTask(s))
            {
                removeTask(s, colBack);
                return true;
            }
            return false;
          
            
        }
        public bool moveToColumnTwo(int taskId)
        {
            Task s = inProgress.getTask(taskId);
            if (s == null)
            {
                throw new Exception("task does not exist"); ;
            }
            if (done.addTask(s))
            {
                removeTask(s, colProgress);
                return true;
            }
            return false;


        }
        public string getBoardName()
        {
            return this.name;
        }
        public Column GetColumn(int columnOrdinal)
        {
            if (columnOrdinal == colBack)
            {
                return backlog;
            }
            else if (columnOrdinal == colProgress)
            {
                return inProgress;
            }
            return done;
        }

        public string getOwner()
        {
            return owner;
        }
        public void setNweOwner(string old,string newOwner,User user)
        {
            this.owner = newOwner;
            //add old to members and delete new from members
            members.Remove(newOwner);
            members.Add(old, user);
        }
           

        public bool isUserExist(string email)
        {
            if(owner == email || members.ContainsKey(email))
                return true;
            return false;
        }

        public bool addMember(string email,User user)
        {
            if(isUserExist(email))
            {
                return false;
            }
            else
            {
                members.Add(email, user);
                return true;
            }
        }

        public bool removeMember(string email)
        {
            if (!isUserExist(email))
            {
                return false;
            }
            //convert task to unassigned
            // toUnassigned
            this.backlog.toUnassigned(email);
            this.inProgress.toUnassigned(email);
            this.done.toUnassigned(email);
            members.Remove(email);  
            return true;

        }


    }




}