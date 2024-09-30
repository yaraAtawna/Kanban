using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccesLayer.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccesLayer.DAO
{
    internal class BoardDAO
    {
        private int boardId;
        public int BoardId
        {
            get
            {
                return boardId;
            }
            set
            {
                this.boardId = value;
            }
        }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        private string owner;

        public string Owner
        {
            get
            {
                return owner;
            }
            set
            {
                this.owner = value;
            }
        }

        public const string nameColumnName = "Name";
        public const string idColumnName = "Id";
        public const string ownerColumnName = "Owner";

        private BoardController boardController;
        public BoardDAO(int boardId,string boardName,string owner) 
        {
            this.boardController = new BoardController();
            this.boardId = boardId;
            this.owner= owner;
            this.name = boardName;
        }
        public void persist()
        {
            this.boardController.insert(this);
        }
    }
}
