using GymTurnos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymTurnos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("prueba")]
    public async Task<IActionResult> EnviarPrueba(
        string destinatario)
    {
        var enviado = await _emailService.EnviarEmailAsync(
            destinatario,
            "Prueba GymTurnos",
            """
            <h2>GymTurnos</h2>
            <p>La integración con Resend funciona correctamente.</p>
            """
        );

        if (!enviado)
        {
            return BadRequest(new
            {
                mensaje = "No se pudo enviar el email."
            });
        }

        return Ok(new
        {
            mensaje = "Email enviado correctamente."
        });
    }
}