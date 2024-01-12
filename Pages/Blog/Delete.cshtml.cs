using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Pages_Blog
{
    public class DeleteModel : PageModel
    {
        private readonly WebTN.Models.AppDBContext _context;

        public DeleteModel(WebTN.Models.AppDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return Content("Không tìm thấy bài viết"); ;
            }

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.ID == id);

            if (article == null)
            {
                return Content("Không tìm thấy bài viết"); ;
            }
            else
            {
                Article = article;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return Content("Không tìm thấy bài viết"); ;
            }
            var article = await _context.Articles.FindAsync(id);

            if (article != null)
            {
                Article = article;
                _context.Articles.Remove(Article);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
