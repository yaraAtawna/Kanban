using IntroSE.Kanban.Backend.DataAccesLayer.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccesLayer.DAO
{
    internal class TaskDAO
    {
        private int id ; //{ get; set; }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string title ;//{ get; set; }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string description ;// { get; set; }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private DateTime dueDate ;// { get; set; }
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        private DateTime creationDate;
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        private string assignee; // user
        public string Assignee
        {
            get { return assignee; }
            set { assignee = value; }
        }
        public bool isAssigned;
        //public int columnId;
        //public int boardId;

        private int columnId;
        public int ColumnId
        {
            get { return columnId; }
            set { columnId = value; }
        }

        private int boardId;
        public int BoardId
        {
            get { return boardId; }
            set { boardId = value; }
        }

        public const string idColumn = "Id";
        public const string titleColumn = "Title";
        public const string descriptionColumn = "Descreption";
        public const string dueColumn = "DueDate";
        public const string createColumn = "CreationDate";
        public const string assigneeColumn = "Assignee";
        public const string colidColumn = "ColumnId";
        public const string boardIdColumn = "BoardId";

        private TaskController taskController;


        public TaskDAO(int id, string title, string description, DateTime dueDate, DateTime creationDate, string assignee, int columnId, int boardId)
        {

            this.taskController = new TaskController();
            this.id = id;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.creationDate = creationDate;
            this.assignee = assignee;
            this.columnId = columnId;
            this.boardId = boardId;
        }
        public void persist()
        {
            //Console.WriteLine("persist taskDao");
            taskController.insert(this);
        }
    }
}
