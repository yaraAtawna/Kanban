using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Column
    {
        private int max;
        public int Max
        {
            get
            {
                return max;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (tasks.Count > value)
                {
                    throw new Exception("the new limit is smaller than the tasks number ");
                }
                max = value;

                

            }
        }
        private LinkedList<Task> tasks;
        public LinkedList<Task> Tasks
        {
            get
            {
                return tasks;
            }
        }

        private int id;
        private int boardId;
        private ColumnDAO columnDAO;
        public Column(int id,bool load ,int board)

        {
            this.tasks = new LinkedList<Task>();
            max = -1;
            this.id = id;
            this.boardId = board;
            if(!load)
            {
                this.columnDAO = new ColumnDAO(id, boardId, max);

                this.columnDAO.persist();
            }
            
            //Console.WriteLine(" column2");


        }
        public bool addTask(Task task)
        {
            //no limit
            if (max == -1)
            {
                tasks.AddLast(task);
                return true;
            }
            else // there is limit
            {
                if (this.tasks.Count < this.max)
                {
                    tasks.AddLast(task);
                    return true;
                }
                else if (this.tasks.Count == this.max - 1)
                {
                    tasks.AddLast(task);
                    return true;
                }
                else
                {
                    //error reach max
                    return false;
                }
            }

        }
        //public void setMax(int max)
        //{
        //    //new
        //    if (tasks.Count > max)
        //    {
        //        throw new Exception( "the new limit is smaller than the tasks number ");
        //    }
        //    this.max = max;
        //}
        //public LinkedList<Task> getTasks()
        //{
        //    return this.tasks;
        //}

        public Task getTask(int taskId)
        {

            foreach (Task s in this.tasks)
            {
                if (s.Id == taskId) { return s; }
            }
            //error
            return null;
        }
        //public int getMax()
        //{
        //    return this.max;
        //}
        public bool removeTask(Task task)
        {
            return this.tasks.Remove(task);
        }

        public void toUnassigned(string email)
        {
            foreach (Task task in tasks)
            {
                if (task.Assignee.Equals(email) && task.IsAssigned)
                {
                    task.Assignee = null;
                    task.IsAssigned = false;
                }
            }

        }

    }
}
