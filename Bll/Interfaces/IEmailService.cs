using Bll.commons;

namespace Bll.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}