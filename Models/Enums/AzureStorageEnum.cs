using System.ComponentModel;

namespace Models.Enums
{
    public enum AzureStorageEnum
    {
        [Description("users")]
        users = 1,

        [Description("qrs")]
        qrs = 2,

        [Description("departments")]
        departments = 3,
    }
}