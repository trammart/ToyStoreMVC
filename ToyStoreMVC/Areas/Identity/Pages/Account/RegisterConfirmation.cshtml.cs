using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ToyStoreMVC.Models;

namespace ToyStoreMVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            //_sender = sender;
        }

        public string Email { get; set; }
		public string UrlContinue { set; get; }

		//public bool DisplayConfirmAccountLink { get; set; }

        //public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Không có người dùng với email '{email}'.");
            }

            Email = email;
			if (returnUrl != null)
			{
				UrlContinue = Url.Page("RegisterConfirmation", new { email = Email, returnUrl = returnUrl });
			}
			else
				UrlContinue = Url.Page("RegisterConfirmation", new { email = Email });
			return Page();
        }
    }
}
