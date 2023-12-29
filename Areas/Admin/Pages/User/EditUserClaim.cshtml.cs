using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Admin.User
{
    public class EditUserClaimModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly MyBlogContext _myBlogContext;

        [TempData]
        public string StatusMessage { get; set; }

        public AppUser user { get; private set; }

        public IdentityUserClaim<string> userClaim { get; private set; }

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


        public EditUserClaimModel(MyBlogContext myBlogContext, UserManager<AppUser> userManager)
        {
            _myBlogContext = myBlogContext;
            _userManager = userManager;
        }
        public NotFoundObjectResult OnGet() => NotFound("Không được truy cập.");

        public async Task<ActionResult> OnGetAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound("Không tìm thấy user.");
            }
            return Page();
        }

        public async Task<ActionResult> OnPostAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound("Không tìm thấy user.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var claims = _myBlogContext.UserClaims.Where(c => c.UserId == user.Id);

            if (claims.Any(c => c.ClaimType == inputModel.ClaimType && c.ClaimValue == inputModel.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Claim đã tồn tại.");
                return Page();
            }

            var result = await _userManager.AddClaimAsync(user, new Claim(inputModel.ClaimType, inputModel.ClaimValue));
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Thêm claim không thành công.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thêm claim thành công.");
            }
            return RedirectToPage("./AddRole", new { id = user.Id });
        }

        public async Task<ActionResult> OnGetEditClaimAsync(int? claimid)
        {
            if (claimid == null)
            {
                return NotFound("Không tìm thấy claim.");
            }

            userClaim = await _myBlogContext.UserClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();
            user = await _userManager.FindByIdAsync(userClaim.UserId);
            if (user == null)
            {
                return NotFound("Không tìm thấy user.");
            }

            inputModel = new InputModel()
            {
                ClaimType = userClaim.ClaimType,
                ClaimValue = userClaim.ClaimValue,
            };

            return Page();
        }

        public async Task<ActionResult> OnPostEditClaimAsync(int? claimid)
        {
            if (claimid == null)
            {
                return NotFound("Không tìm thấy claim.");
            }

            userClaim = await _myBlogContext.UserClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();
            user = await _userManager.FindByIdAsync(userClaim.UserId);
            if (user == null)
            {
                return NotFound("Không tìm thấy user.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_myBlogContext.UserClaims.Any(c => c.UserId == user.Id &&
                                    c.ClaimType == inputModel.ClaimType &&
                                    c.ClaimValue == inputModel.ClaimValue &&
                                    c.Id != userClaim.Id))
            {
                ModelState.AddModelError(string.Empty, "Claim đã tồn tại.");
                return Page();
            }

            userClaim.ClaimType = inputModel.ClaimType;
            userClaim.ClaimValue = inputModel.ClaimValue;

            await _myBlogContext.SaveChangesAsync();

            StatusMessage = "Cập nhật claim thành công.";
            return RedirectToPage("./AddRole", new { id = user.Id });
        }

        public async Task<ActionResult> OnPostDeleteAsync(int? claimid)
        {
            if (claimid == null)
            {
                return NotFound("Không tìm thấy claim.");
            }

            userClaim = await _myBlogContext.UserClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();
            user = await _userManager.FindByIdAsync(userClaim.UserId);
            if (user == null)
            {
                return NotFound("Không tìm thấy user.");
            }

            var result = await _userManager.RemoveClaimAsync(user, new Claim(userClaim.ClaimType, userClaim.ClaimValue));
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Claim chưa được xoá.");
                return Page();
            }
            else
            {
                StatusMessage = "Xoá claim thành công.";
                return RedirectToPage("./AddRole", new { id = user.Id });
            }

        }
    }
}
