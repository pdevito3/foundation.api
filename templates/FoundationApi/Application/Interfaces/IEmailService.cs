namespace Application.Interfaces
{
    using Application.Dtos.Auth;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
