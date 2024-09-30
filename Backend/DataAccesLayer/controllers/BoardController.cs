using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace IntroSE.Kanban.Backend.DataAccesLayer.controllers
{
    internal class BoardController
    {
        private const string TableName = "Board";

        private readonly string connectionString;
        private readonly string tableName;
    
        public BoardController()
        {
            //string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Backend\", "kanban.db"));
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this.connectionString = $"Data Source={path}; Version=3";
            this.tableName = TableName;
            //test
            //Debug.WriteLine("the path in board controller  is :");
            //Debug.WriteLine(path);
        }

       
        /// <summary>
        /// insert a board to dataBase 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool insert(BoardDAO board)
        {
            //Console.WriteLine("insert board controller1");

            int res = -1;
            using (var connection = new SQLiteConnection(this.connectionString))
            {
                try
                {
                    //Console.WriteLine("insert board controller");
                    SQLiteCommand command = new SQLiteCommand(null, connection);


                    string insert = $"INSERT INTO {TableName}( {BoardDAO.idColumnName} ,{BoardDAO.nameColumnName} , {BoardDAO.ownerColumnName}) VALUES(@id ,@name, @owner)";

                    SQLiteParameter namelParam = new SQLiteParameter(@"name", board.Name);
                    SQLiteParameter idParam = new SQLiteParameter(@"id", board.BoardId);
                    SQLiteParameter ownerParam = new SQLiteParameter(@"owner", board.Owner);

                    command.CommandText = insert;
                    command.Parameters.Add(namelParam);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(ownerParam);
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

                return res > -1;
            }
        }



        public BoardDAO getBoardDAO(int boardId)
        {
            BoardDAO ans;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {TableName} WHERE Id={boardId}";
                SQLiteDataReader reader = null;
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    ans = ConvertReaderToObject(reader);

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

        public void deleteAll()
        {
            int count=0;
            
            int res = -1;
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
           
            if (res <= 0 && count!=0)
                throw new Exception("Failed to delete Board!");
        }

        public bool deleteBoard(int id)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {tableName} WHERE Id={id}"
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

            return res > 0;
        }


        public List<BoardDAO> GetAllBoards()
        {
            List<BoardDAO> ans = new List<BoardDAO>();

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName}";

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
        
        
        public bool update(int boardId, string attributeName, object attributeValue)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {tableName} SET {attributeName}=@Val WHERE Id={boardId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@Val", attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally { connection.Close(); }
            }
            return res > 0;
        }

        public BoardDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            // int text text
            return new BoardDAO(Convert.ToInt32(reader.GetInt64(0)), reader.GetString(1), reader.GetString(2));
        }
    }
}
