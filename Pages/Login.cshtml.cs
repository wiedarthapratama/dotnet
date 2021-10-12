using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using absensionline.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace absensionline.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration configuration;
        public LoginModel(IConfiguration configuration){
            this.configuration=configuration;
        }
        [BindProperty]
        public string UserEmail{get;set;}
        [BindProperty, DataType(DataType.Password)]
        public string Password{get;set;}
        public string Message{get;set;}
        public async Task<IActionResult> onPost(){
            var user = configuration.GetSection("User").Get<User>();
            if(UserEmail==user.Email){
                if(Password==user.Password){
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, UserEmail),
                        new Claim(ClaimTypes.Role, "admin")
                    };
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    return RedirectToPage("/Index"); 
                }
                Message="Password Salah";
                return Page();
            }
            Message="User tidak terdaftar";
            return Page();
        }
        public void OnGet()
        {
        }
    }
}
