using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccesLayer.controllers
{
    internal class BoardUserStatusController
    {
        private const string TableName = "UserBoard";
        private readonly string connectionString;
        private readonly string tableName;
        
        public BoardUserStatusController()
        {
            //string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Backend\", "kanban.db"));

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this.connectionString = $"Data Source={path}; Version=3";
            this.tableName = TableName;
            //test
            //Debug.WriteLine("the path in Boarduser controller  is :");
            //Debug.WriteLine(path);
        }
        //join
        public void insert(BoardUserStatusDAO boardUserStatusDAO)
        {
            //Console.WriteLine("insert boardUser controller1");
            int res = -1;
            using (var connection = new SQLiteConnection(this.connectionString))
            {
                try
                {
                    

                    SQLiteCommand command = new SQLiteCommand(null, connection);

                    //string insert = $"INSERT INTO {TableName}( {BoardUserStatusDAO.idColumnName} ,{BoardUserStatusDAO.emailColumnName} , {BoardUserStatusDAO.statusColumnName})" + $"VALUES(@id ,@email, @status)";
                    string insert = $"INSERT INTO {TableName} ({BoardUserStatusDAO.emailColumnName}, {BoardUserStatusDAO.idColumnName}, {BoardUserStatusDAO.statusColumnName}) VALUES (@userEmail, @boardID, @status)";


                    SQLiteParameter emailParam = new SQLiteParameter("@userEmail", boardUserStatusDAO.UserEmail);
                    SQLiteParameter idParam = new SQLiteParameter("@boardID", boardUserStatusDAO.BoardID);
                    SQLiteParameter statusParam = new SQLiteParameter("@status", boardUserStatusDAO.Status);



                    command.CommandText = insert;
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(statusParam);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    connection.Close();
                }

                if (res == -1)
                    throw new Exception("Failed to insert!");
            }
        }

        //User leave board
        public void delete(BoardUserStatusDAO boardUserStatusDAO)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                //Console.WriteLine("hi1");
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {tableName} WHERE {BoardUserStatusDAO.emailColumnName} = @Email AND {BoardUserStatusDAO.idColumnName} = @BoardId"
                };
                //{BoardUserStatusDAO.emailColumnName} = @Email AND {BoardUserStatusDAO.idColumnName} = @BoardId"
                command.Parameters.AddWithValue("@Email", boardUserStatusDAO.UserEmail);


                command.Parameters.AddWithValue("@BoardId", boardUserStatusDAO.boardID);
               // Console.WriteLine("hi2");

                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    //Console.WriteLine("hi3");

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally { connection.Close(); }
            }

            if (res <= 0)
                throw new Exception("Failed to delete"); 
        }

        //delete board from table --new
        public void deleteBoard(int Boardid)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {tableName} WHERE BoardId={Boardid}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally { connection.Close(); }
            }

            if (res <= 0)
                throw new Exception("Failed to delete Board!");
        }
     
        // Return all the board with all the user in the boards
        public List<BoardUserStatusDAO> getAll()
        {
            List<BoardUserStatusDAO> ans = new List<BoardUserStatusDAO>();

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
                        ans.Add(ConvertReaderToObject(reader));
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            return ans;
        }

        // Delete all boards
        public void deleteAll()
        {
            int res = -1;
            int count = 0;
            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {tableName} "
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
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally { connection.Close(); }
            }
            
            if (res <= 0 && count != 0)
                throw new Exception("Failed to delete Boards!");
        }
        

        public BoardUserStatusDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            // int text text
            return new BoardUserStatusDAO(reader.GetString(0), Convert.ToInt32(reader.GetInt64(1)), reader.GetString(2));
        }

        //update 
        public bool updateStatus(string email ,int boardId ,string newStatus)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,

                    CommandText = $"UPDATE {tableName} SET {BoardUserStatusDAO.statusColumnName} = @Val WHERE {BoardUserStatusDAO.emailColumnName} = @Email AND {BoardUserStatusDAO.idColumnName} = @BoardId"
                };

                command.Parameters.AddWithValue("@Val", newStatus);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@BoardId", boardId);

                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally { connection.Close(); }
            }
            return res > 0;

        }

    }
}
