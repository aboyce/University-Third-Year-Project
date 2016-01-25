using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    class FacebookViewModels
    {
    }

    public class FacebookIndexViewModel
    {
        public bool IsLoggedIn { get; set; } = false;

        public FacebookProfileSummaryViewModel FacebookProfileSummaryViewModel { get; set; }

    }

    public class FacebookProfileSummaryViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public string Locale { get; set; }

        public string Location { get; set; }

        public string Birthday { get; set; }

        public string Bio { get; set; }

        public string ExternalLink { get; set; }
    }
}
