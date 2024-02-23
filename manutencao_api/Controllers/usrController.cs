using gestaomanutencao.Classes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


[ApiController]
[Route("api/usrController")]
[ApiExplorerSettings(GroupName = "v1")]
[SwaggerTag("Operações de Usuários")]
public class usrController : ControllerBase
{
    private readonly gestaomanutencao.Services.UsuarioService _usuarioService;

    public usrController(gestaomanutencao.Services.UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

   [HttpPost("cadastrar")]
[SwaggerOperation(Summary = "Cadastrar Usuário", Tags = new[] { "usrController" })]
public IActionResult CadastrarUsuario([FromBody] Usuario usuarioInput)
{
   
    _usuarioService.CadastrarUsuario(new Usuario
    {
        Nome = usuarioInput.Nome,
        Email = usuarioInput.Email,
        Senha = usuarioInput.Senha
    });

    return Ok("Usuário cadastrado com sucesso!");
}
}
