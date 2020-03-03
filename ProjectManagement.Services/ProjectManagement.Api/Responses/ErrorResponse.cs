using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Responses
{
    public class ErrorResponse:Response
    {
        public ErrorResponse(string error)
        {
            this.error = error;
        }

        public string error { get; set; }
    }
}
