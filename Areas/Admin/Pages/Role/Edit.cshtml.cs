using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Admin.Role
{
    [Authorize(Policy = "AllowEditRole")]
    //[Authorize(Roles ="Admin")]
    public class EditModel : RolePageModel
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

        public List<IdentityRoleClaim<string>> Claims { get; set; }
        public IdentityRole role { get; private set; }
        public EditModel(RoleManager<IdentityRole> roleManager, AppDBContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role.");

            role = await _roleManager.FindByIdAsync(roleid);

            if (role != null)
            {
                inputModel = new InputModel()
                {
                    RoleName = role.Name,
                };
                Claims = await _myBlogContext.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
                return Page();
            }

            return NotFound("Không tìm thấy role.");
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role.");
            role = await _roleManager.FindByIdAsync(roleid);

            if (role != null)
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                role.Name = inputModel.RoleName;

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    StatusMessage = $"Bạn vừa cập nhật một role: {inputModel.RoleName}";
                    return RedirectToPage("./Index");
                }
                else
                {
                    result.Errors.ToList().ForEach(er => ModelState.AddModelError(string.Empty, er.Code + ": " + er.Description));
                }
                return Page();
            }

            return NotFound("Không tìm thấy role.");
        }
    }
}
