using Bll.FactoryServices.UOW.Interfaces;
using Bll.Interfaces;
using Models.DTOs;

namespace Bll.HangFire
{
    public class HangFireBll : IHangFireBll
    {
        private readonly Iuow _UOW;
        private readonly IPushNotificationService _pushNotificationService;

        public HangFireBll(
            Iuow iUOW,
            IPushNotificationService pushNotificationService)
        {
            this._UOW = iUOW;
            this._pushNotificationService = pushNotificationService;
        }

        public void DailyService()
        {
            DateTime dateToday = DateTime.Now;

            var notificationsToken = "";
            //string.Join(",", this._UOW.VisitRepository
            //.FindBy(v => v.VisitDate == dateToday.Date && v.Active)
            //.SelectMany(d => d.User.Devices)
            //.Select(t => t.TokenDevice)
            //.Distinct()
            //.ToList());

            if (!string.IsNullOrWhiteSpace(notificationsToken))
            {
                this._pushNotificationService.SendNotification(new PushNotificationDto()
                {
                    Token = notificationsToken,
                    Title = "Recordatorio de Cita",
                    Body = $"Usted tiene una cita programada para el día de hoy!",
                    IsSingleAndroiodDevice = true,
                });
            }
        }

        public void HourlyService()
        {
            DateTime dateToday = DateTime.Now;

            var notificationsToken = "";

            //string.Join(",", this._UOW.VisitRepository
            //.FindBy(v => v.VisitDate == dateToday.Date &&
            //(v.StartHour.Hours >= dateToday.Hour && v.StartHour.Hours <= dateToday.Hour + 1) && v.Active)
            //.SelectMany(d => d.User.Devices)
            //.Select(t => t.TokenDevice)
            //.Distinct()
            //.ToList());

            if (!string.IsNullOrWhiteSpace(notificationsToken))
            {
                this._pushNotificationService.SendNotification(new PushNotificationDto()
                {
                    Token = notificationsToken,
                    Title = "Recordatorio de Cita",
                    Body = $"Usted tiene una cita próxima!",
                    IsSingleAndroiodDevice = true,
                });
            }
        }

        public void fifteenMinutesService()
        {
            DateTime dateToday = DateTime.Now;

            var notificationsToken = "";

            //string.Join(",", this._UOW.VisitRepository
            //    .FindBy(v =>
            //    v.VisitDate == dateToday.Date &&
            //    (v.StartHour.Hours >= dateToday.Hour && v.StartHour.Hours <= dateToday.Hour + 1)
            //    && (dateToday.Minute >= v.StartHour.Minutes)
            //    && dateToday.Minute <= (v.StartHour.Minutes + 14)
            //    && v.Active)
            //    .SelectMany(d => d.User.Devices)
            //    .Select(t => t.TokenDevice)
            //    .Distinct()
            //    .ToList());

            if (!string.IsNullOrWhiteSpace(notificationsToken))
            {
                this._pushNotificationService.SendNotification(new PushNotificationDto()
                {
                    Token = notificationsToken,
                    Title = "Recordatorio de Cita",
                    Body = $"Usted tiene una cita próxima!",
                    IsSingleAndroiodDevice = true,
                });
            }
        }

        public async Task DailyVisitStatusService()
        {
            DateTime dateToday = DateTime.Now;
            string todayFormat = dateToday.ToString("yyyy-MM-dd");
            string query = string.Empty;

            var statusObject = new { Id = 1, Name = "" };
            //this._UOW.VisitStatusRepository
            //    .FindBy(v => v.Code == "04")
            //    .Select(vs => new { Id = vs.VisitStatusId, vs.Name })
            //    .FirstOrDefault();

            query = "UPDATE Visit AS V " +
                "JOIN VisitStatus AS VS ON VS.VisitStatusId = V.VisitStatusId " +
                $" SET V.VisitStatusId = '{statusObject.Id}', Cancelled = '{statusObject.Name}', V.active = 0 " +
                $"WHERE VS.Code = '01' AND V.visitDate <= CONVERT('{todayFormat}', DATE)";

            //await this._UOW.VisitRepository
            //   .ExecuteSqlRawAsync(query);

            //await this._UOW.SaveAsync();
        }
    }
}