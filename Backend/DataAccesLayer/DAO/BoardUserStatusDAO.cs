using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccesLayer.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.DataAccesLayer.DAO
{
    internal class BoardUserStatusDAO
    {
        internal string userEmail;
        public string UserEmail
        {
            get
            {
                return userEmail;
            }
            set
            {
                this.userEmail = value;
            }
        }
        internal int boardID;
        public int BoardID
        {
            get
            {
                return boardID;
            }
            set
            {
                this.boardID = value;
            }
        }

        internal string status; //Owner or Member
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                this.status = value;
            }
        }

        public const string idColumnName = "BoardId";
        public const string emailColumnName= "UserEmail";
        public const string statusColumnName = "Status";

        private BoardUserStatusController boardUserStatusController;
        public BoardUserStatusDAO(string userEmail, int boardID , string status)
        {
            boardUserStatusController = new BoardUserStatusController();
            this.userEmail =userEmail;
            this.boardID =boardID;
            this.status =status;
        }
        public void persist()
        {
            
            boardUserStatusController.insert(this);
        }
    }
}
