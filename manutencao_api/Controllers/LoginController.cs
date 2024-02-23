using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using gestaomanutencao.Classes;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.IdentityModel.Tokens; // Importe a biblioteca

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly gestaomanutencao.Services.UsuarioService _usuarioService;
    private readonly IConfiguration _configuration;

    public LoginController(gestaomanutencao.Services.UsuarioService usuarioService, IConfiguration configuration)
    {
        _usuarioService = usuarioService;
        _configuration = configuration;
    }

    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Autenticar Usuário",
        Description = "Autenticação do usuário e geração de token JWT",
        Tags = new[] { "LoginController" })]
    [SwaggerResponse(200, "Token JWT gerado com sucesso")]
    [SwaggerResponse(401, "Credenciais inválidas")]
    public IActionResult Login([FromBody] Usuario usuarioInput)
    {
        var usuarioAutenticado = AutenticarUsuario(usuarioInput.Nome, usuarioInput.Senha);

        if (usuarioAutenticado == null)
        {
            return Unauthorized(new { Mensagem = "Credenciais inválidas" });
        }

        var token = GerarTokenJwt(usuarioAutenticado.Nome);
        return Ok(new { Token = token });
    }

    private Usuario AutenticarUsuario(string nomeUsuario, string senha)
    {
        // Lógica de autenticação, por exemplo, consultar no banco de dados
        // Este é um exemplo simples; não use em produção.
        return _usuarioService.ObterUsuarioAutenticado(nomeUsuario, senha);
    }

    private string GerarTokenJwt(string nomeUsuario)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key"));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, nomeUsuario)
            // Adicione outras claims conforme necessário
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("ExpirationInMinutes")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
