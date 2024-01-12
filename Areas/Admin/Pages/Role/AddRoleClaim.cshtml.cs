using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTN.Models;

namespace WebTN.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class AddRoleClaimModel : RolePageModel
    {
        public class InputModel
        {
            [Display(Name = "Kiểu Claim")]
            [Required(ErrorMessage = "{0} phải nhập.")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "[0] phải dài từ {2} đén {1} kí tự.")]
            public string ClaimType { get; set; }
            [Display(Name = "Giá trị")]
            [Required(ErrorMessage = "{0} phải nhập.")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "[0] phải dài từ {2} đén {1} kí tự.")]
            public string ClaimValue { get; set; }
        }

        [BindProperty]
        public InputModel inputModel { get; set; }

        public IdentityRole role { get; private set; }

        public AddRoleClaimModel(RoleManager<IdentityRole> roleManager, AppDBContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public async Task<IActionResult> OnGet(string roleid)
        {
            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tồn tại role.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tồn tại role.");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if ((await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == inputModel.ClaimType && c.Value == inputModel.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Claim đã tồn tại.");
                return Page();
            }
            else
            {
                var newClaim = new Claim(inputModel.ClaimType, inputModel.ClaimValue);
                var result = await _roleManager.AddClaimAsync(role, newClaim);
                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
                    return Page();
                }

                StatusMessage = "Thêm claim mới thành công.";
                return RedirectToPage("./Edit", new { roleid = role.Id });
            }


        }
    }
}
