using Microsoft.AspNetCore.Authorization;

namespace WebTN.Security.Requirements
{
    public class ArticleUpdateRequirement : IAuthorizationRequirement
    {
        public ArticleUpdateRequirement(int year = 2023, int month = 6, int date = 30)
        {
            Year = year;
            Month = month;
            Date = date;
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Date { get; set; }
    }
}