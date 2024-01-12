using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : RolePageModel
    {
        public class RoleModel : IdentityRole
        {
            public string[] Claims { get; set; }

        }

        public List<RoleModel> IdentityRoles { get; set; }

        public IndexModel(RoleManager<IdentityRole> roleManager, AppDBContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public async Task OnGet()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            IdentityRoles = new List<RoleModel>();

            foreach (var role in roles)
            {

                var rm = new RoleModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                };
                rm.Claims = (await _roleManager.GetClaimsAsync(role))
                            .Select(c => c.Type + "=" + c.Value)
                            .ToArray();
                IdentityRoles.Add(rm);
            }
        }

        public void OnPost() => RedirectToPage();
    }
}
