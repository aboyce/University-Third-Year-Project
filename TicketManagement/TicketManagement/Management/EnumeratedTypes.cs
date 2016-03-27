using System.Collections.Generic;

namespace TicketManagement.Management
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

    public static class RoleNotificationTypeString
    {
        private static readonly Dictionary<RoleNotificationType, string> RoleNotificationTypeValues = new Dictionary<RoleNotificationType, string>()
        {
            {RoleNotificationType.PendingApproval, "Pending Approval"},
            {RoleNotificationType.PendingInternalApproval, "Pending Internal Approval"},
        };

        public static string GetStringForType(RoleNotificationType type)
        {
            return RoleNotificationTypeValues[type];
        }
    }

    // ----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// When adding types here also add how to handle them in the NotificationHelper and NotificationController.
    /// </summary>
    public enum UserNotificationType
    {
        PendingApproval = 0,
        PendingInternalApproval = 1,
        NewTicketLog = 2
    }

    public static class UserNotificationTypeString
    {
        private static readonly Dictionary<UserNotificationType, string> UserNotificationTypeValues = new Dictionary<UserNotificationType, string>()
        {
            {UserNotificationType.PendingApproval, "Pending Approval"},
            {UserNotificationType.PendingInternalApproval, "Pending Internal Approval"},
            {UserNotificationType.NewTicketLog, "New Ticket Log"}
        };

        public static string GetStringForType(UserNotificationType type)
        {
            return UserNotificationTypeValues[type];
        }
    }

    // ----------------------------------------------------------------------------------------------------------------

    public enum TextResult
    {
        SendSuccess,
        SendFailure
    }

    // ----------------------------------------------------------------------------------------------------------------

    public enum ErrorType
    {
        Info = 0,
        Warning = 1,
        Error = 2,
        Exception = 3
    }

    // ----------------------------------------------------------------------------------------------------------------

    public enum FileType
    {
        Image = 0,
        Text = 1,
        PDF = 2
    }

    // ----------------------------------------------------------------------------------------------------------------

    public enum FacebookPostType
    {
        Link = 0,
        Status = 1,
        Photo = 2,
        Video = 3,
        Offer = 4
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

        DataPopulated,
        DataNotPopulated,

        TwitterReplyAdded,
        TwitterReplyFailed,

        UserTokenGenerated,
        UserTokenGenerationFailed,
        UserTokenConfirmed,
        UserTokenConfirmationFailed,
        UserTokenDeactivated,
        UserTokenDeactivationFailed,
        UserTokenSentViaText,
        UserTokenSentViaTextFailed,

        AppliedRoleFromNotification,
        FailedToApplyRoleFromNotification,

        DismissedNotification,
        FailedToDismissNotification,

        DeclinedRoleFromNotification,
        FailedToDeclineRoleFromNotification,

        RoleAdded,
        RoleNotAdded,
        AlreadyInRole,
        NotInternal,

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
        TicketMessageNotAdded,

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
        ClearedExternalLoginInformation,
        ErrorWithTwitterAuthentication,
        LoggedOff,
        Error
    }
}
