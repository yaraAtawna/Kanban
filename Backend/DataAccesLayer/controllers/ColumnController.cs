using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccesLayer.controllers
{
    internal class ColumnController
    {
        private const string TableName = "Column";

        private readonly string connectionString;
        private readonly string tableName;

        public ColumnController() 
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            //string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Backend\", "kanban.db"));
            //Console.WriteLine(path);
            //Console.WriteLine("path");

            this.connectionString = $"Data Source={path}; Version=3";
            this.tableName = TableName;
        }
        public bool insert(ColumnDAO column)
        {
            //Console.WriteLine("insert column");
            int res = -1;
            using (var connection = new SQLiteConnection(this.connectionString))
            {
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string insert = $"INSERT INTO {TableName}( {ColumnDAO.idColumnName} ,{ColumnDAO.maxEmailColumnName} , {ColumnDAO.boardIdColumnName})  VALUES(@id ,@max, @boardId)";

                    SQLiteParameter idlParam = new SQLiteParameter(@"id", column.Id);
                    SQLiteParameter MaxParam = new SQLiteParameter(@"max", column.Max);
                    SQLiteParameter boardIdapram = new SQLiteParameter(@"boardId", column.BoardId);

                    command.CommandText = insert;
                    command.Parameters.Add(idlParam);
                    command.Parameters.Add(MaxParam);
                    command.Parameters.Add(boardIdapram);
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

        public void deleteCol(ColumnDAO column)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {tableName} WHERE Id={column.Id}"
                };
                // command.Parameters.Add(new SQLiteParameter("@id", column.id));

                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally { connection.Close(); }
            }

            if (res <= 0)
                throw new Exception("Failed to delete");
        }

        //delete all columns with given boardId -- new
        public bool deleteByBoard(int boardId)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {TableName} WHERE boardId={boardId}"
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


        // what is the input??
        public List<ColumnDAO> getAllcolumns()
        {
            List<ColumnDAO> ans = new List<ColumnDAO>();

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
        public bool update(int boardId, int id, string attributeName, object attributeValue)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {tableName} SET {attributeName} =@Val WHERE boardId={boardId} AND Id={id}"
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
        public ColumnDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            // int text text
            //Convert.ToInt32(reader.GetInt64(0))
            return new ColumnDAO(Convert.ToInt32(reader.GetInt64(0)), Convert.ToInt32(reader.GetInt64(1)), Convert.ToInt32(reader.GetInt64(2)));
        }
    }
    
}
