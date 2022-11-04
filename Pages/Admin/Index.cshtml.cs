using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace MoonriseMovies.Pages.Admin
{
    [Authorize(Roles="Admin")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
