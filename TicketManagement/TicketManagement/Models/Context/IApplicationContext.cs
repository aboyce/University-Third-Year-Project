using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketManagement.Management;
using TicketManagement.Models.Entities;

namespace TicketManagement.Models.Context
{
    public interface IApplicationContext : IDisposable
    {
        void MarkAsModified(User item);
        void MarkAsModified(Organisation item);
        void MarkAsModified(Team item);
        void MarkAsModified(Project item);
        void MarkAsModified(Ticket item);
        void MarkAsModified(TicketCategory item);
        void MarkAsModified(TicketLog item);
        void MarkAsModified(File item);
        void MarkAsModified(TicketPriority item);
        void MarkAsModified(TicketState item);
        void MarkAsModified(SentTextMessage item);
        void MarkAsModified(ReceivedTextMessage item);
        void MarkAsModified(RoleNotification item);
        void MarkAsModified(UserNotification item);

        UserManager<User> UserManager { get; }

        DbSet<Organisation> Organisations { get; }
        DbSet<Team> Teams { get; }
        DbSet<Project> Projects { get; }
        DbSet<Ticket> Tickets { get; }
        DbSet<TicketCategory> TicketCategories { get; }
        DbSet<TicketLog> TicketLogs { get; }
        DbSet<File> Files { get; }
        DbSet<TicketPriority> TicketPriorities { get; }
        DbSet<TicketState> TicketStates { get; }
        DbSet<SentTextMessage> TextMessagesSent { get; }
        DbSet<ReceivedTextMessage> TextMessagesReceived { get; }

        DbSet<RoleNotification> RoleNotifications { get; }
        DbSet<UserNotification> UserNotifications { get; }
    }
}
