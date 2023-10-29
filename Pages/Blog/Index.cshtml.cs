using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;

namespace WebTN.Pages_Blog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly WebTN.Models.MyBlogContext _context;

        public const int ITEMS_PER_PAGE =  10;

        [BindProperty(SupportsGet =true,Name ="p")] 
        public int CurrentPage { get; set; }=1;

        [BindProperty(SupportsGet =true,Name ="SearchString")] 
        public string? SearchString { get; set; }
        public int CountPages {get; set;}

        public IndexModel(WebTN.Models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Articles != null)
            {
                // Article = await _context.Articles.ToListAsync();
                var qr = from a in _context.Articles
                                orderby a.Created descending
                                select a;
                if(SearchString != null)
                {
                    CountPages =(int)Math.Ceiling( (double) await qr.Where( a => a.Title.Contains(SearchString)).CountAsync()/ITEMS_PER_PAGE);
                    Article = await  qr.Where( a => a.Title.Contains(SearchString)).Skip((CurrentPage-1)*ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync();
                }
                else
                {
                    CountPages =(int)Math.Ceiling( (double) await qr.CountAsync()/ITEMS_PER_PAGE);
                    Article = await qr.Skip((CurrentPage-1)*ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync();
                }

                
            }
        }
    }
}
