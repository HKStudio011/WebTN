using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebTN.Models;

namespace WebTN.Security.Requirements
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        private ILogger<AppAuthorizationHandler> _logger;
        private UserManager<AppUser> _userManager;

        public AppAuthorizationHandler(ILogger<AppAuthorizationHandler> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        Task IAuthorizationHandler.HandleAsync(AuthorizationHandlerContext context)
        {
            var requirements = context.PendingRequirements.ToList();
            _logger.LogInformation($"context.Resource :{context.Resource?.GetType().Name}");
            foreach (var requirement in requirements)
            {
                if (requirement is GenZRequirement)
                {
                    if (IsGenZ(context.User, (GenZRequirement)requirement))
                    {
                        context.Succeed(requirement);
                    }
                }

                if (requirement is ArticleUpdateRequirement)
                {
                    if (CanUpdateArticle(context.User, context.Resource, (ArticleUpdateRequirement)requirement))
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            return Task.CompletedTask;
        }

        private bool CanUpdateArticle(ClaimsPrincipal user, object? resource, ArticleUpdateRequirement requirement)
        {
            if (user.IsInRole("Admin"))
            {
                _logger.LogInformation($"Admin update.");
                return true;
            }

            var article = resource as Article;

            if (article != null)
            {
                var dateCreated = article.Created;
                var dateCanUpdate = new DateTime(requirement.Year, requirement.Month, requirement.Date);

                if (dateCreated >= dateCanUpdate)
                {
                    return true;
                }
            }

            _logger.LogInformation($"Từ chối cập nhập.");

            return false;
        }

        private bool IsGenZ(ClaimsPrincipal user, GenZRequirement requirement)
        {
            var task = _userManager.GetUserAsync(user);
            Task.WaitAll(task);
            var appUser = task.Result;

            if (appUser.BirthDate == null)
            {
                _logger.LogInformation($"khong co ngay sinh, khong thoa man GenZRequirement");
                return false;
            }

            int year = appUser.BirthDate.Value.Year;

            var success = (year >= requirement.FormYear && year <= requirement.ToYear);

            if (success)
            {
                _logger.LogInformation($"Thoa man GenZRequirement");
            }
            else
            {
                _logger.LogInformation($"Khong thoa man GenZRequirement");
            }
            return success;
        }
    }
}