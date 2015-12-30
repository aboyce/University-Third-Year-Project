using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    class TextMessageViewModel
    {
    }

    public class AllTextMessagesViewModel
    {
        public IEnumerable<Models.Entities.SentTextMessage> SentMessages { get; set; }

        public IEnumerable<Models.Entities.ReceivedTextMessage> ReceivedMessages { get; set; }
    }
}
