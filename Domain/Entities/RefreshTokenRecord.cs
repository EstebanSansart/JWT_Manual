using System;
using System.Collections.Generic;
using Domain.Entities.Generics;

namespace Domain.Entities;

public partial class RefreshTokenRecord : BaseEntityA
{

    public int UserId { get; set; }

    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public bool IsActive { get; set; }

    public virtual User User { get; set; }
}