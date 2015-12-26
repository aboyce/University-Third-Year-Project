using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Management
{
    public enum NotificationCategory
    {
        Role = 0,
        User = 1
    }

    /// <summary>
    /// When adding types here also add how to handle them in the NotificationHelper and NotificationController.
    /// </summary>
    public enum RoleNotificationType
    {
        PendingApproval = 0,
        PendingInternalApproval = 1
    }

    /// <summary>
    /// When adding types here also add how to handle them in the NotificationHelper and NotificationController.
    /// </summary>
    public enum UserNotificationType
    {
        PendingApproval = 0,
        PendingInternalApproval = 1
    }

    // ----------------------------------------------------------------------------------------------------------------

    public enum TextResult
    {
        SendSuccess,
        SendFailure
    }

    // ----------------------------------------------------------------------------------------------------------------

    public enum TicketLogType
    {
        Message,
        File
    }

    // ----------------------------------------------------------------------------------------------------------------

    public enum TicketSort
    {
        Open = 0,
        Closed = 1,
        Unanswered = 2,
        PendingApproval = 3,
        Mine = 4,
        All = 5
    }

    // ----------------------------------------------------------------------------------------------------------------

    public enum ViewMessage
    {
        ProfileUpdated,

        PendingApproval,

        AppliedRoleFromNotification,
        FailedToApplyRoleFromNotification,

        RoleAdded,
        RoleNotAdded,
        AlreadyInRole,

        RoleRemoved,
        RoleNotRemoved,
        NotInRole,

        OrganisationAdded,
        OrganisationUpdated,
        OrganisationDeleted,

        ProjectAdded,
        ProjectUpdated,
        ProjectDeleted,

        TeamAdded,
        TeamUpdated,
        TeamDeleted,

        TicketMessageAdded,

        TicketCategoryAdded,
        TicketCategoryUpdated,
        TicketCategoryDeleted,

        TicketPriorityAdded,
        TicketPriorityUpdated,
        TicketPriorityDeleted,

        TicketStateAdded,
        TicketStateUpdated,
        TicketStateDeleted,
    }


    // ----------------------------------------------------------------------------------------------------------------

    public enum ManageMessageId
    {
        AddPhoneSuccess,
        SetPasswordSuccess,
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        LoggedOff,
        Error
    }
}
