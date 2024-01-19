using Models.DTOs;

namespace Bll.Interfaces
{
    public interface IPushNotificationService
    {
        Task<(string Message, bool Success)> SendNotification(PushNotificationDto notificationModel);
    }
}