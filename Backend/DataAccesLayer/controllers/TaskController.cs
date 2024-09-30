using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Diagnostics;


namespace IntroSE.Kanban.Backend.DataAccesLayer.controllers
{
    internal class TaskController
    {
        private const string TableName = "Task";
        private readonly string connectionString;
        private readonly string tableName;

        public TaskController() {
            //string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Backend\", "kanban.db"));
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this.connectionString = $"Data Source={path}; Version=3";
            this.tableName = TableName;
            //test
            //Debug.WriteLine("the path in Task controller  is :");
            //Debug.WriteLine(path);
        }
        public bool insert(TaskDAO task)
        {
            //Console.WriteLine("insert in task controller");
            int res = -1;
            using (var connection = new SQLiteConnection(this.connectionString))
            {
                try
                {

                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    connection.Open();

                    //string insert = $"INSERT INTO {TableName}({TaskDAO.idColumn},{TaskDAO.titleColumn}, {TaskDAO.descriptionColumn} {TaskDAO.dueColumn}, {TaskDAO.createColumn} {TaskDAO.assigneeColumn}, {TaskDAO.colidColumn}, {TaskDAO.boardIdColumn}) VALUES(@id, @title, @description, @dueDate, @creationDate, @assignee, @columnId, @boardId)";
                    string insert = $"INSERT INTO {TableName}({TaskDAO.idColumn}, {TaskDAO.titleColumn}, {TaskDAO.descriptionColumn}, {TaskDAO.dueColumn}, {TaskDAO.createColumn}, {TaskDAO.assigneeColumn}, {TaskDAO.colidColumn}, {TaskDAO.boardIdColumn}) VALUES(@id, @title, @description, @dueDate, @creationDate, @assignee, @columnId, @boardId)";
                    SQLiteParameter idParam = new SQLiteParameter(@"id", task.Id);
                    SQLiteParameter titleParam = new SQLiteParameter(@"tittle", task.Title);
                    SQLiteParameter descriptionlParam = new SQLiteParameter(@"description", task.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDate", task.DueDate);
                    SQLiteParameter creatParam = new SQLiteParameter(@"creationDate", task.CreationDate);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assignee", task.Assignee);
                    SQLiteParameter colIdParam = new SQLiteParameter(@"columnId", task.ColumnId);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", task.BoardId);
                    command.CommandText = insert;

                    //command.Parameters.Add(idParam);
                    //command.Parameters.Add(titleParam);
                    //command.Parameters.Add(descriptionlParam);
                    //command.Parameters.Add(dueDateParam);
                    //command.Parameters.Add(creatParam);
                    //command.Parameters.Add(assigneeParam);
                    //command.Parameters.Add(colIdParam);
                    //command.Parameters.Add(boardIdParam);
                    command.Parameters.AddWithValue("@id", task.Id);
                    command.Parameters.AddWithValue("@title", task.Title);
                    command.Parameters.AddWithValue("@description", task.Description);
                    command.Parameters.AddWithValue("@dueDate", task.DueDate);
                    command.Parameters.AddWithValue("@creationDate", task.CreationDate);
                    command.Parameters.AddWithValue("@assignee", task.Assignee);
                    command.Parameters.AddWithValue("@columnId", task.ColumnId);
                    command.Parameters.AddWithValue("@boardId", task.BoardId);
                    //connection.Open();
                    //Console.WriteLine("insert in task controller2");
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    //Console.WriteLine(res);

                }

                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    connection.Close();
                }

                return res > -1;
            }
        }



        public bool delete(TaskDAO task)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where ID={task.Id}"

                };
               // command.Parameters.Add(new SQLiteParameter("@id", task.id));

                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally { connection.Close(); }
            }

            return res > 0;
        }


        //delete all task with given boardId -- new
        public bool deleteByBoard(int boardId) {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {TableName} WHERE BoardId={boardId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }

            return res > 0;
        }

        public List<TaskDAO> getAllTasks(int BoardId)
        {
            //test
            //Debug.WriteLine("getAllTasks in task controller for board : " + BoardId);
            //Debug.WriteLine(tasks.Count);

            List<TaskDAO> ans = new List<TaskDAO>();

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {TableName}";
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //old :  ans.Add(ConvertReaderToObject(reader));
                        //new !! mileStone3
                        TaskDAO task = ConvertReaderToObject(reader);
                        if (task.BoardId == BoardId)
                        {
                            ans.Add(ConvertReaderToObject(reader)); 
                        }
                    }
                }
                catch (Exception ex) //new
                {
                   // Debug.WriteLine($"Exception in task controller: {ex.Message}");
                    //throw; // Optionally rethrow if you want the exception to propagate
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                       // Debug.WriteLine("getAllTasks0");

                    }
                   // Debug.WriteLine("getAllTasks1");
                }
               // Debug.WriteLine("getAllTasks2");

            }
            //test
           // Debug.WriteLine("getAllTasks");
            return ans;
        }
        public TaskDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
           
            return new TaskDAO(Convert.ToInt32(reader.GetInt64(0)), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3), reader.GetDateTime(4), reader.GetString(5), Convert.ToInt32(reader.GetInt64(6)), Convert.ToInt32(reader.GetInt64(7)));
        }
    
    public void deleteAll()
        {
            int res = -1;
            int count = 0;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} "
                };
                SQLiteCommand commandCount = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"SELECT COUNT(*) FROM {tableName}"
                };
                try
                {

                    connection.Open();
                    count = Convert.ToInt32(commandCount.ExecuteScalar());
                    res = command.ExecuteNonQuery();
                }
                finally { connection.Close(); }
            }

            if (res <= 0 && count != 0)
                throw new Exception("Failed to delete Boards!");
        }

        

        public bool update(int taskId, string attributeName, object attributeValue)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {tableName} SET {attributeName}=@Val WHERE Id={taskId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@Val", attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally { connection.Close(); }
            }
            return res > 0;
        }
    }
}
