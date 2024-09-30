using IntroSE.Kanban.Backend.DataAccesLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;
using System.Data.Common;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Data.SQLite;
using System.Diagnostics;

namespace IntroSE.Kanban.Backend.DataAccesLayer.controllers
{
    internal class UserController
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string connectionString;
        private readonly string tableName;
        private const string TableName = "User";
        public UserController()
        {
            // Directory.GetCurrentDirectory();
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kanban.db"));
            ////test
            //Debug.WriteLine("the path in user controller  is :");
            //Debug.WriteLine(path);



            //string path = Path.GetFullPath(Path.Combine( Directory.GetCurrentDirectory(), "kanban.db"));

            //string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Backend\", "kanban.db"));
            //Console.WriteLine(path);
            //Console.WriteLine("path");
            this.connectionString = $"Data Source={path}; Version=3";

            this.tableName = TableName;
        }
        
        public string deleteAll()
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
                return "null";
                //throw new Exception("Failed to delete Boards!");
            return "true";
        
        }

        public bool delete(UserDAO user)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string str = $"DELETE FROM {tableName} WHERE Email={user.Email}";
                    command.CommandText = str;                   
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
            }

            return res > 0;
        }
        public UserDAO getUser(string email)
        {
            UserDAO ans;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {TableName} WHERE Email={email}";
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


        public bool insert(UserDAO user)
        {
           // Console.WriteLine("enter insert");
           //test
           //Debug.WriteLine("enter insert in user controller");
            int res = -1;
            using (var connection = new SQLiteConnection(this.connectionString))
            {
               // Console.WriteLine("enter connection");

                try
                {
                    SQLiteCommand command = new SQLiteCommand(connection);


                    string insert = $"INSERT INTO {TableName}({UserDAO.userEmailColumnName}, {UserDAO.userPasswordColumnName}) VALUES (@Email, @Password)";

                    //Console.WriteLine("the path in inseert:");
                    //Console.WriteLine(insert);



                    SQLiteParameter emailParam = new SQLiteParameter(@"Email", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"Password", user.password);

                    command.CommandText = insert;

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    connection.Open();

                    res = command.ExecuteNonQuery();
                    //Console.WriteLine(res);

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    //command.Dispose();
                    connection.Close();
                }

                if( res > -1)
                {
                    Console.WriteLine("Success to insert User");
                    return true;
                }
                Console.WriteLine("fail to insert User");
                return false;
            }

        }
        public bool update(string userEmail, string attributeName, object attributeValue)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {tableName} SET {attributeName} = @Val WHERE Email={userEmail}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@Val", attributeValue));
                    connection.Open();
                    res=command.ExecuteNonQuery();
                }
                finally { connection.Close(); }
            }

            return res > 0;
        }


        public List<UserDAO> getAllUsers()
        {
            List<UserDAO> ans = new List<UserDAO>();

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
                        //Debug.WriteLine("getAllUsers in user controller dal");
                        //Debug.WriteLine("user enail  "+ConvertReaderToObject(reader).Email);
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

        public UserDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new UserDAO(reader.GetString(0), reader.GetString(1));
        }
    }
}
