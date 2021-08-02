using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class AuthCredentials
    {
        [Required] public string ClientId { get; set; }

        [Required] public string ClientSecret { get; set; }
    }
}