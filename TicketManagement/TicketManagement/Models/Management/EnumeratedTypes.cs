using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Management
{
    public enum NotificationCategory
    {
        Role,
        User
    }

    public enum NotificationType
    {
        PendingApproval
    }


    public enum TextResult
    {
        SendSuccess,
        SendFailure
    }

    public enum ManageMessageId
    {
        AddPhoneSuccess,

        RoleAdded,
        RoleNotAdded,
        AlreadyInRole,

        RoleRemoved,
        RoleNotRemoved,
        NotInRole,

        ProfileUpdated,
        PendingApproval,
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        LoggedOff,
        Error
    }
}
