// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Admin.User
{
    //[Authorize(Policy = "AllowEditRole")]
    [Authorize(Roles = "Admin")]
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _rolenManager;
        private readonly MyBlogContext _myBlogContext;



        public AddRoleModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> rolenManager,
            MyBlogContext myBlogContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _rolenManager = rolenManager;
            _myBlogContext = myBlogContext;
        }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        public AppUser user { get; set; }
        public SelectList selectList { get; set; }

        public List<IdentityRoleClaim<string>> RoleClaims { get; set; }
        public List<IdentityUserClaim<string>> UserClaims { get; set; }


        [BindProperty]
        [DisplayName("Các role của user")]
        public string[] RoleNames { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không có user có id '{_userManager.GetUserId(User)}'.");
            }

            RoleNames = (await _userManager.GetRolesAsync(user)).ToArray<string>();
            selectList = new SelectList(_rolenManager.Roles.Select(r => r.Name));

            await GetClaim(id);

            return Page();
        }

        private async Task GetClaim(string id)
        {
            var listROle = from r in _myBlogContext.Roles
                           join ur in _myBlogContext.UserRoles on r.Id equals ur.RoleId
                           where ur.UserId == id
                           select r;

            var claimInRole = from c in _myBlogContext.RoleClaims
                              join r in listROle on c.RoleId equals r.Id
                              select c;

            RoleClaims = await claimInRole.ToListAsync();

            UserClaims = await (from c in _myBlogContext.UserClaims
                                where c.UserId == id
                                select c).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Không có user có id '{id}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oldRoleS = (await _userManager.GetRolesAsync(user)).ToArray();
            var deleteRoles = oldRoleS.Where(r => !RoleNames.Contains(r));
            var addRoles = RoleNames.Where(r => !oldRoleS.Contains(r));

            selectList = new SelectList(_rolenManager.Roles.Select(r => r.Name));
            await GetClaim(id);

            var resultDel = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            if (!resultDel.Succeeded)
            {
                resultDel.Errors.ToList().ForEach(er =>
                {
                    ModelState.AddModelError(string.Empty, er.Description);
                });
                return Page();
            }

            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(er =>
                {
                    ModelState.AddModelError(string.Empty, er.Description);
                });
                return Page();
            }

            StatusMessage = $"Thiết lập role thành công cho user {user.UserName}.";

            return RedirectToPage("./Index");
        }
    }
}
