using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Responses
{
    public class SuccessResponse:Response
    {
        public SuccessResponse(dynamic data)
        {
            this.data = data;
        }

        public dynamic data { get; set; }
    }
}
