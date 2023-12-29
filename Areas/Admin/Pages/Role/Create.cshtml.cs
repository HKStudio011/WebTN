using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTN.Models;

namespace WebTN.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : RolePageModel
    {
        public class InputModel
        {
            [Display(Name = "Tên role")]
            [Required(ErrorMessage = "{0} phải nhập.")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "[0] phải dài từ {2} đén {1} kí tự.")]
            public string RoleName { get; set; }
        }

        [BindProperty]
        public InputModel inputModel { get; set; }
        public CreateModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var role = new IdentityRole(inputModel.RoleName);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo một role mới: {inputModel.RoleName}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(er => ModelState.AddModelError(string.Empty, er.Code + ": " + er.Description));
            }
            return Page();
        }
    }
}
