using System.Collections.Generic;

namespace TicketManagement.ViewModels
{
    class TextMessageViewModel
    {
    }

    public class AllTextMessagesViewModel
    {
        public List<Models.Entities.SentTextMessage> SentMessages { get; set; }

        public List<Models.Entities.ReceivedTextMessage> ReceivedMessages { get; set; }
    }
}
