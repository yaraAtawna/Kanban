using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{    /// <summary>
     /// Provides test methods for BoardService functionalities.
     /// </summary>
    class TaskTests
    {

        private readonly TaskService taskService;

        public TaskTests(TaskService taskService)
        {
            this.taskService = taskService;

        }


        /// <summary>
        /// Tests a successful task title update.
        /// </summary>
        public void updateTitlesuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskTitle("user@gmail.com", "ImportantBoard", 0, 0, "workBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("title for task updated successfully");
            }
        }
        /// <summary>
        /// Tests failed task title updates.
        /// </summary>
        public void updateTitleFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskTitle("user@gmail.com", "importantBoard", 0, 0, "aaaaaaaaaaaaaaaaaaaazzzzzzzzzzzzzzzzzzzzzdddddmggggkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkddddddffffgggggggg"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("title for task updated successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskTitle("user@gmail.com", "importantBoard", 2, 333, "tittle"));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("title for task updated successfully");
            }
        }



        /// <summary>
        /// Tests a successful task due date update.
        /// </summary>
        public void updateDueDateSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskDueDate("user@gmail.com", "importantBoard", 0, 333, new DateTime(2024, 6, 14)));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("due date for task updated successfully");
            }
        }
        /// <summary>
        /// Tests failed task due date updates.
        /// </summary>
        public void updateDueDateFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskDueDate("user@gmail.com", "importantBoard", 0, 0, new DateTime(1, 1, 1, 0, 0, 0)));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("due date for task updated successfully");
            }
        }


        /// <summary>
        /// Tests a successful task description update.
        /// </summary>
        public void updateDescriptionSuccessfullyTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskDescription("user@gmail.com", "Board", 0, 0, "this task is about the work"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("  description for task  updated successfully");
            }
        }
        /// <summary>
        /// Tests failed task description updates.
        /// </summary>
        public void updateDescriptionFailedTest()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskDescription("user@gmail.com", "importamtBoard",1 ,0, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"));
           if (res1.ErrorOccurd)
            {
                Console.WriteLine(res1.ErrorMessage);
            }

            else
            {
                Console.WriteLine(" description for task  updated successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(taskService.UpdateTaskDescription("user@gmail.com", "Board", 0, 0, null));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("  description for task updated successfully");
            }
        }





    }
}