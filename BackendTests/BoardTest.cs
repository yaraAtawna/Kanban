using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{/// <summary>
 /// Provides test methods for BoardService functionalities.
 /// </summary>
    public class BoardTest
    {
        private readonly BoardService boardService;

        public BoardTest(BoardService boardService)
        {
            this.boardService = boardService;


        }


        /// <summary>
        /// Tests a successful board creation.
        /// </summary>
        public void createBoardSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("botha@gmail.com", "ImportantBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("board createcd successfully");
            }



        }
        /// <summary>
        /// Tests failed board creation.
        /// </summary>

        public void createBoardFailedTest()
        {

            Response res2 = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("botha@gmail.com", ""));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("board createcd successfully");
            }
        }


        /// <summary>
        /// Tests a successful board deletion.
        /// </summary>
        public void deleteBoardSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard("yara@gmail.com", "ImportantBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine(" board deleted successfully");
            }
        }
        /// <summary>
        /// Tests failed board deletion.
        /// </summary>
        public void deleteBoardFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard("yara@gmail.com", ""));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine(" board deleted successfully");
            }
        }



        /// <summary>
        /// Tests a successful task addition.
        /// </summary>
        public void addTaskSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AddTask("botha@gmail.com", "importantBoard", "work1", "description to the task", new DateTime(2024, 6, 14)));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }



        }
        /// <summary>
        /// Tests failed task additions.
        /// </summary>
        public void addTaskFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AddTask("yara@gmail.com", "importantBoard", "", "description to the task", new DateTime(2024, 6, 14)));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.AddTask(null, "importantBoard", "work1", "description to the task", new DateTime(2024, 6, 14)));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }
        }


        /// <summary>
        /// Tests successfully getting a column name.
        /// </summary>
        public void GetColumnNameSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnName("yara@gmail.com", "importantBoard", 2));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column name successed");
            }
        }
        /// <summary>
        /// Tests failed attempt to get column name
        /// </summary>
        public void GetColumnNameFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnName("bothaina@gmail.com", "Board", 4));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column name successed");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumnName("bothaina@gmail.com", null, 1));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column name successed");
            }
        }


        /// <summary>
        /// Tests successfully getting column limit
        /// </summary>
        public void GetColumnLimitSSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("bothaina@gmail.com", "ImportantBoard", 1));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column limit successed");
            }
        }
        /// <summary>
        /// Tests failed attempt to get column limit
        /// </summary>
        public void GetColumnLimitFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("bothaina@gmail.com", "ImportantBoard", 4));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column limit successed");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("bothaina@gmail.com", "", 0));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column limit successed");
            }
        }

        /// <summary>
        /// Tests successfully getting a column
        /// </summary>
        public void GetColumnSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("yara@gmail.com", "ImportantBoard", 2));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column successed");
            }
        }
        /// <summary>
        /// Tests failed attempt to get column
        /// </summary>
        public void GetColumnFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("bothaina@gmail.com", "ImportantBoard", 5));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column successed");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("", "ImportantBoard", 5));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column  successed");
            }
            Response res3 = JsonSerializer.Deserialize<Response>(boardService.GetColumn("bothaina@gmail.com", null, 1));
            if (res3.ErrorOccurd)
            {
                Console.WriteLine(res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting column successed");
            }
        }


        /// <summary>
        /// Tests successfully updeting a column limit
        /// </summary>
        public void LimitColumnSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.LimitColumn("bothaina@gmail.com", "importantBoard", 1, 9));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("updateing limit column successed");
            }
        }
        /// <summary>
        /// Tests failed attempt to set column limit
        /// </summary>
        public void LimitColumnFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.LimitColumn("yara@gmail.com", "importantBoard", 3, 6));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("updateing limit  column successed");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.LimitColumn("bothaina@gmail.com", "", 0, 6));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("updateing limit  column successed");
            }
        }



        /// <summary>
        /// Tests successfully advancing a task.
        /// </summary>
        public void advanceTaskSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("bothaina@gmail.com", "importantBoard", 0, 11));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine(" Task Advanced successfully");
            }
        } /// <summary>
          /// Tests failed attempts to advance tasks.
          /// </summary>
        public void advanceTaskFailedTest()
        {

            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("yara@gmail.com", "importantBoard", 1,-1 ));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine(" Task Advanced successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("bothaina@gmail.com", "importantBoard", 3, 11));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task Advanceed successfully");
            }

        }
        /// <summary>
        /// Tests successfully retrieving in-progress tasks.
        /// </summary>
        public void InProgressTasksSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.InProgressTasks("bothaina@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("In - progress tasks completed successfully");
            }
        }
        /// <summary>
        /// Tests failed retrieval of in-progress tasks.
        /// </summary>
        public void InProgressTasksFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.InProgressTasks(null));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine(" In-progress tasks completed successfully");
            }
        }


    }
}