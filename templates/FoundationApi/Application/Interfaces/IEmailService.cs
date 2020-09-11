namespace Application.Interfaces
{
    using Application.Dtos.Shared;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
