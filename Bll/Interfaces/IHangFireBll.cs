namespace Bll.Interfaces
{
    public interface IHangFireBll
    {
        void DailyService();

        void HourlyService();

        void fifteenMinutesService();

        Task DailyVisitStatusService();
    }
}