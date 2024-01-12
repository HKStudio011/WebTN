using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTN.Models;

namespace WebTN.Admin.Role
{
    public class RolePageModel : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly AppDBContext _myBlogContext;

        [TempData]
        public string StatusMessage { get; set; }
        public RolePageModel(RoleManager<IdentityRole> roleManager, AppDBContext myBlogContext)
        {
            _roleManager = roleManager;
            _myBlogContext = myBlogContext;
        }
    }
}
