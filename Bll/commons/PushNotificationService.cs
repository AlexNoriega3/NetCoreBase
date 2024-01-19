using Bll.FactoryServices.UOW.Interfaces;
using Bll.Interfaces;
using CorePush.Google;
using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
using Models.Globals;
using Serilog;
using System.Net.Http.Headers;

namespace Bll.commons
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly Iuow _UOW;
        private readonly ILogger _logger;

        public PushNotificationService(
            IOptions<FcmNotificationSetting> notSettings,
            Iuow UOW,
            ILogger logger)
        {
            this._fcmNotificationSetting = notSettings.Value;
            this._UOW = UOW;
            this._logger = logger;
        }

        public async Task<(string Message, bool Success)> SendNotification(PushNotificationDto notificationModel)
        {
            (string Message, bool Success) response = (Message: string.Empty, Success: false);

            try
            {
                if (string.IsNullOrWhiteSpace(notificationModel.Token))
                {
                    notificationModel.Token = "strTokens";
                    //string.Join(",",
                    //this._UOW.DeviceRepository
                    //.FindBy(u => u.UserId == notificationModel.UserId && u.Active && !string.IsNullOrWhiteSpace(u.TokenDevice))
                    //.Select(t => t.TokenDevice)
                    //.ToList()
                    //);
                }

                if (notificationModel.IsSingleAndroiodDevice)
                {
                    bool dispatched = true;
                    List<string> errs = new List<string>();

                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = _fcmNotificationSetting.SenderId,
                        ServerKey = _fcmNotificationSetting.ServerKey
                    };

                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    PushNotification notification = new PushNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);

                    string[] tokens = notificationModel.Token.Split(",");

                    for (int i = 0; i < tokens.Count(); i++)
                    {
                        string token = tokens[i];

                        var fcmSendResponse = await fcm.SendAsync(token, notification);

                        if (!fcmSendResponse.IsSuccess())
                        {
                            dispatched = false;
                            errs.Add(fcmSendResponse.Results[0].Error);
                        }
                    }

                    response.Message = (dispatched ? "Notification sent successfully" : string.Join(",", errs));
                    response.Success = dispatched;

                    return response;
                }
                if (!notificationModel.IsSingleAndroiodDevice)
                {
                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = _fcmNotificationSetting.SenderId,
                        ServerKey = _fcmNotificationSetting.ServerKey
                    };
                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                    string deviceToken = "/topics/" + notificationModel.Token;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    PushNotification notification = new PushNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    response.Message = (fcmSendResponse.IsSuccess() ? "Notification sent successfully" : "Notification not sent");
                    response.Success = fcmSendResponse.IsSuccess();

                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Something went wrong " + ex;
                this._logger.Error(ex, ex.Message);

                return response;
            }
        }
    }
}