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
