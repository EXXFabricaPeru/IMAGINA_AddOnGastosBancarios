using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Domain.Entities
{
    public class ErrorSLModel
    {
        public class Error
        {
            public int code { get; set; }
            public Message message { get; set; }
        }

        public class Message
        {
            public string lang { get; set; }
            public string value { get; set; }
        }


        public Error error
        {
            get; set;
        }
    }
}
