
using Microsoft.AspNetCore.Authorization;

namespace WebTN.Security.Requirements
{
    public class GenZRequirement : IAuthorizationRequirement
    {

        public GenZRequirement(int formYear = 1997, int toYear = 2012)
        {
            FormYear = formYear;
            ToYear = toYear;
        }

        public int FormYear { get; set; }
        public int ToYear { get; set; }


    }
}