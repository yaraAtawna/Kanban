using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response<T> 
    {
        [JsonInclude]

        public T ReturnValue { get; set; }

        [JsonInclude]

        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool ErrorOccurd { get => ErrorMessage != null; }

        public Response() { }

        public Response(string message)
        {
            ErrorMessage = message;
        }


        public Response(T obj)
        {
            ReturnValue = obj;
        }


        public Response(string message, T value)
        {
            ErrorMessage = message;
            this.ReturnValue = value;

        }
        
    }
    
}
