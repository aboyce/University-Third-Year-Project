using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Management
{
    public enum NotificationType
    {
        PendingApproval,
        PendingInternalApproval
    }

    public enum TextResult
    {
        SendSuccess,
        SendFailure
    }

    public enum ManageMessageId
    {
        PendingApproval,

        RoleAdded,
        RoleNotAdded,
        AlreadyInRole,

        RoleRemoved,
        RoleNotRemoved,
        NotInRole,

        ProfileUpdated,
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
