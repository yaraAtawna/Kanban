    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        [JsonInclude]
        
        public object ReturnValue { get; set; }

        [JsonInclude]
       
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool ErrorOccurd { get => ErrorMessage != null; }

        public Response() { }

        public Response(string message)
        {
            ErrorMessage = message;
        }


        public Response(object obj)
        {
            ReturnValue = obj;
        }


        public Response(string message, object value)
        {
            ErrorMessage = message;
            this.ReturnValue = value;

        }
    }
}
