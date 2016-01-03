using System.Collections.Generic;

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
