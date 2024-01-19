using System.ComponentModel;

namespace Models.Enums
{
    public enum NotificationPriority
    {
        [Description("bg-danger")]
        Urgent,

        [Description("bg-warning")]
        High,

        [Description("bg-orange")]
        Medium,

        [Description("bg-info-800")]
        Low
    }
}