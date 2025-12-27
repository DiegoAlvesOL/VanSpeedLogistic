using Microsoft.AspNetCore.Mvc;

namespace VanSpeedLogistics.Controllers;


// Nota: O atributo [Authorize] não será implementado temporariamente para 
// que consiga acessar em produção e resetar o Peter Parker.
public class AdminController : Controller
{
    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View();
    }
    
    
    
}