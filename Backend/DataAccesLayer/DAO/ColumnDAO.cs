using IntroSE.Kanban.Backend.DataAccesLayer.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccesLayer.DAO
{
    internal class ColumnDAO
    {
        public ColumnController columnController;
        private int max;
        public int Max
        {
            get
            {
                return max;
            }
            set
            {
                this.max = value;
            }
        }

        private int id;
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }

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

        public const string maxEmailColumnName = "Max";
        public const string idColumnName = "Id";
        public const string boardIdColumnName = "boardId";
        public ColumnDAO(int id,int boardId,int max) 
        {
            this.id = id;
            this.boardId = boardId;
            this.max = max;
            this.columnController = new ColumnController();
        }
        public void persist()
        {
            columnController.insert(this);
        }
    }
}
