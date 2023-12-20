using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Admin.Role
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : RolePageModel
    {
        public List<IdentityRole> IdentityRoles {get; set;}

        public IndexModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public async Task OnGet()
        {
           IdentityRoles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
        }

        public void OnPost() => RedirectToPage();
    }
}
