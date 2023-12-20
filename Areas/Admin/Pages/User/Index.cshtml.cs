using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Admin.User
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public class UserAddRole : AppUser
        {
            public string RoleNames { get; set; }
        }
        public List<UserAddRole> Users { get; private set; }
        private readonly UserManager<AppUser> _userManager;

        public const int ITEMS_PER_PAGE = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true, Name = "SearchString")]
        public string? SearchString { get; set; }
        public int CountPages { get; set; }

        public int TotalUser { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            //Users = await _userManager.Users.OrderBy(u => u.UserName).ToListAsync();
            var qr = from u in _userManager.Users
                     orderby u.UserName 
                     select new UserAddRole()
                     {
                        Id = u.Id,
                        UserName = u.UserName, 
                     };


            if (SearchString != null)
            {
                CountPages = (int)Math.Ceiling((double)await qr.Where(u => u.UserName.Contains(SearchString)).CountAsync() / ITEMS_PER_PAGE);
                TotalUser = await qr.Where(u => u.UserName.Contains(SearchString)).CountAsync();
                Users = await qr.Where(u => u.UserName.Contains(SearchString)).Skip((CurrentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync();
            }
            else
            {
                CountPages = (int)Math.Ceiling((double)await qr.CountAsync() / ITEMS_PER_PAGE);
                TotalUser = await qr.CountAsync();
                Users = await qr.Skip((CurrentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync();
            }

            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames = string.Join(",",roles);
            }
        }

        public void OnPost() => RedirectToPage();
    }
}
