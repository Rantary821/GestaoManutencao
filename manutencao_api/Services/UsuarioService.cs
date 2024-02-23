using System;
using gestaomanutencao.Classes;
using MongoDB.Driver;

namespace gestaomanutencao.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Classes.Usuario> _usuarios;

    public UsuarioService(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("gestao");
        _usuarios = database.GetCollection<Classes.Usuario>("Usuarios");
    }

     public void CadastrarUsuario(Classes.Usuario usuario)
    {
        _usuarios.InsertOne(usuario);
    }

    public Usuario ObterUsuarioAutenticado(string nomeUsuario, string senha)
{
    // Simulação de autenticação (substitua pela lógica real)
    var usuario = _usuarios.Find(u => u.Nome == nomeUsuario && u.Senha == senha).FirstOrDefault();

    // Adicione logs para depuração
    if (usuario == null)
    {
        Console.WriteLine($"Usuário não encontrado para {nomeUsuario} e {senha}");
    }
    else
    {
        Console.WriteLine($"Usuário autenticado: {usuario.Nome}");
    }

    return usuario;
}



    }
}
