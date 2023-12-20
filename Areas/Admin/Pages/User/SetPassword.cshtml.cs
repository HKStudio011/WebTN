// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTN.Models;

namespace WebTN.Admin.User
{
    [Authorize(Roles ="Admin")]
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SetPasswordModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "{0} phải nhập.")]
            [StringLength(100, ErrorMessage = "{0} phải dài từ {2} đén {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu mới")]
            public string NewPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("NewPassword", ErrorMessage = "Mật khẩu không chính xác.")]
            public string ConfirmPassword { get; set; }
        }

        public AppUser user {get;set;}

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không có user có id '{id}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
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

            await _userManager.RemovePasswordAsync(user);

            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            StatusMessage = $"Thiết lập mật khâu thành công cho user {user.UserName}.";

            return RedirectToPage("./Index");
        }
    }
}
