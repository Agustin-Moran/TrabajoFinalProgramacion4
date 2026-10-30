namespace GymTurnos.Application.Interfaces.Services;

public interface IEmailService
{
    Task<bool> EnviarEmailAsync(
        string destinatario,
        string asunto,
        string html,
        string? idempotencyKey = null
    );
}