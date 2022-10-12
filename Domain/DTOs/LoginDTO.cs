using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email é um campo obrigatório para Login")]
        [EmailAddress(ErrorMessage ="Email inválido")]
        [StringLength(100,ErrorMessage ="Email deve ter no máximo {1} caracteres")]
        public String? Email { get; set; }
    }
}
