using System.Net.Http.Headers;
using System.Net.Http.Json;
using GymTurnos.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace GymTurnos.Infrastructure.ExternalServices;

public class ResendEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ResendEmailService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> EnviarEmailAsync(
        string destinatario,
        string asunto,
        string html,
        string? idempotencyKey = null)
    {
        var apiKey = _configuration["Resend:ApiKey"];
        var from = _configuration["Resend:From"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "No se encontró la API Key de Resend."
            );
        }

        if (string.IsNullOrWhiteSpace(from))
        {
            throw new InvalidOperationException(
                "No se encontró el remitente de Resend."
            );
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "emails"
        );

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                apiKey
            );

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            request.Headers.TryAddWithoutValidation(
                "Idempotency-Key",
                idempotencyKey
            );
        }

        request.Content = JsonContent.Create(new
        {
            from,
            to = new[] { destinatario },
            subject = asunto,
            html
        });

        var response =
            await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
}