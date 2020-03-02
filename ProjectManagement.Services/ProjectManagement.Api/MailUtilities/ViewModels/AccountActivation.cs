using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.MailUtilities.ViewModels
{
    public class AccountActivationViewModel
    {
        public string UserName { get; set; }
        public string ActivateLink { get; set; }
    }
}
