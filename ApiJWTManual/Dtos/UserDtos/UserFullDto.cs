using System.ComponentModel.DataAnnotations;

namespace ApiJWTManual.Dtos;
public class UserFullDto : UserDto{

    [Required]
    public int Id { get; set; }
}