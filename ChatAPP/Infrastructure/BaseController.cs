using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatAPP.Infrastructure
{
    public class BaseController:Controller
    {
        protected string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
