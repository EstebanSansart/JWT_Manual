using Domain.Entities.Generics;

namespace Domain.Entities;
public class User: BaseEntityA{

    public string Username { get; set; } 
    public string Password { get; set; }
}