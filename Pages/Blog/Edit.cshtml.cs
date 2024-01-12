using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Pages_Blog
{
    public class EditModel : PageModel
    {
        private readonly WebTN.Models.AppDBContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(WebTN.Models.AppDBContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return Content("Không tìm thấy bài viết");
            }

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return Content("Không tìm thấy bài viết");
            }
            Article = article;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                var result = await _authorizationService.AuthorizeAsync(this.User, Article, "CanUpdateArticle");
                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không được phép edit.");
                    return Page();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.ID))
                {
                    return Content("Không tìm thấy bài viết");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
            return (_context.Articles?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
