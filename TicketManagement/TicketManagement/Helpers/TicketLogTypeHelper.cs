using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Helpers
{
    public enum TicketLogType
    {
        MessageFromExternalUser = 0,
        MessageFromInternalUser = 1,
        FileFromExternalUser = 2,
        FileFromInternalUser = 3
    }

    public static class TicketLogTypeHelper
    {
        public static Dictionary<TicketLogType, string> TicketLogTypeDisplay = new Dictionary<TicketLogType, string>
        {
            { TicketLogType.FileFromExternalUser, "File"},
            { TicketLogType.FileFromInternalUser, "File"},
            { TicketLogType.MessageFromExternalUser, "Message"},
            { TicketLogType.MessageFromInternalUser, "Message"}
        };

        public static bool FromInternal(TicketLog log)
        {
            switch (log.TicketLogType)
            {
                case (TicketLogType.MessageFromExternalUser):
                    return false;
                case (TicketLogType.MessageFromInternalUser):
                    return true;
                case (TicketLogType.FileFromExternalUser):
                    return false;
                case (TicketLogType.FileFromInternalUser):
                    return true;
            }

            return false;
        }

        public static async Task<bool> NewTicketLogAsync(string userId, int ticketId, TicketLogType type, bool isInternal, ApplicationContext db, string message = null, File file = null)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            Ticket ticket = db.Tickets.FirstOrDefault(t => t.Id == ticketId);

            TicketLog ticketLog = new TicketLog
            {
                Ticket = ticket,
                TicketId = ticket?.Id ?? 0,
                TicketLogType = type,
                SubmittedByUserId = user.Id,
                SubmittedByUser = user,
                Message = message,
                File = file,
                FileId = file?.Id,
                IsInternal = isInternal,
                TimeOfLog = DateTime.Now
            };

            if (ticket != null)
            {
                if (FromInternal(ticketLog))
                {
                    ticket.LastResponse = DateTime.Now;
                }

                else
                {
                    ticket.LastMessage = DateTime.Now;
                }
            }

            db.Entry(ticket).State = EntityState.Modified;
            db.TicketLogs.Add(ticketLog);
            await db.SaveChangesAsync();

            return false;
        }
    }
}
