using System;
using MongoDB.Bson;

namespace gestaomanutencao.Classes
{
    public class Usuario
    {
    public ObjectId Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }
    }
}
