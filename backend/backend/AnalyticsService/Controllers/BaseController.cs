using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnalyticsService.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("ID de usuario no encontrado en el token JWT.");
            }
            return userId;
        }
    }
}
