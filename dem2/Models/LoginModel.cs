using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dem2.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

    
    }
}
