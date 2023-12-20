using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTN.Models;

namespace WebTN.Admin.Role
{
    [Authorize(Roles ="Admin")]
    public class DeleteModel : RolePageModel
    {

        public IdentityRole role { get; private set; }
        public DeleteModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role.");

            role = await _roleManager.FindByIdAsync(roleid);

            if (role != null)
            {
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

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    StatusMessage = $"Bạn vừa xoá một role: {role.Name}";
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
