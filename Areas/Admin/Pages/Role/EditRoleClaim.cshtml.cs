using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class EditRoleClaimModel : RolePageModel
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

        IdentityRoleClaim<string> claim;

        public EditRoleClaimModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public async Task<IActionResult> OnGet(int? claimid)
        {
            if (claimid == null)
            {
                return NotFound("Không tồn tại claim.");
            }

            claim = await _myBlogContext.RoleClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();

            if (claim == null)
            {
                return NotFound("Không tồn tại claim.");
            }

            role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null)
            {
                return NotFound("Không tồn tại role.");
            }

            inputModel = new InputModel()
            {
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? claimid)
        {
            if (claimid == null)
            {
                return NotFound("Không tồn tại claim.");
            }

            claim = await _myBlogContext.RoleClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();

            if (claim == null)
            {
                return NotFound("Không tồn tại claim.");
            }

            role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null)
            {
                return NotFound("Không tồn tại role.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_myBlogContext.RoleClaims.Any(c => c.RoleId == role.Id &&
                                                c.ClaimType == inputModel.ClaimType &&
                                                c.ClaimValue == inputModel.ClaimValue &&
                                                c.Id != claim.Id))
            {
                ModelState.AddModelError(string.Empty, "Claim đã tồn tại.");
                return Page();
            }
            else
            {
                claim.ClaimType = inputModel.ClaimType;
                claim.ClaimValue = inputModel.ClaimValue;

                await _myBlogContext.SaveChangesAsync();

                StatusMessage = "Cập nhật claim thành công.";
                return RedirectToPage("./Edit", new { roleid = role.Id });
            }


        }


        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if (claimid == null)
            {
                return NotFound("Không tồn tại claim.");
            }

            claim = await _myBlogContext.RoleClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();

            if (claim == null)
            {
                return NotFound("Không tồn tại claim.");
            }

            role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null)
            {
                return NotFound("Không tồn tại role.");
            }

            var result = await _roleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue));
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Claim chưa được xoá.");
                return Page();
            }
            else
            {
                StatusMessage = "Xoá claim thành công.";
                return RedirectToPage("./Edit", new { roleid = role.Id });
            }

        }
    }
}
