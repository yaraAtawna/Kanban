
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _descreption;
        public string Descreption
        {
            get => _descreption;
            set
            {
                this._descreption = value;
                RaisePropertyChanged("Descreption");
            }
        }
        private string _creationtime;
        public string CreationTime
        {
            get => _creationtime;
            set
            {
                this._creationtime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        private string _todo;
        public string Todo
        {
            get => _todo;
            set
            {
                this._todo = value;
                RaisePropertyChanged("Todo");
            }
        }
        public TaskModel(BackendController controller, int id, string title, string descreption, string creationtime, string todo) : base(controller)
        {
            _id = id;
            _title = title;
            _descreption = descreption;
            _creationtime = creationtime;
            _todo = todo;
                
        }
        public TaskModel(BackendController controller, (int Id, string Title, string description, string creationtime, string todo) task) : this(controller, task.Id, task.Title, task.description, task.creationtime, task.todo) { }

     
    public string TitleView => "Title: " + Title; 
        public string DescriptionView => "Description: " + Descreption;
        public string CreationTimeView => "Creation Time: " + CreationTime;
        public string TodoView => "Due date: " + Todo;
    }
}