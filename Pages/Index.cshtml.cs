using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTN.Models;

namespace WebTN.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MyBlogContext _myBlogContext1;

    public IndexModel(ILogger<IndexModel> logger, MyBlogContext myBlogContext)
    {
        _logger = logger;
        _myBlogContext1 = myBlogContext;
    }
    
    public void OnGet()
    {
        var posts = (from a in _myBlogContext1.Articles
                        orderby a.Created descending
                        select a).ToList();
        ViewData["posts"]=posts;
    }
}
