using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly JwtService jwtservice;

        public BaseController(JwtService jwtservice)
        {
            this.jwtservice = jwtservice;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        public long GetUserIdFromToken()
        {
            var authorization = Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorization))
            {
                var token = authorization.ToString().Replace("Bearer ", "");
                var jwt = jwtservice.Verify(token);

                var userId = jwt.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Sid);

                if (userId != null)
                {
                    return long.Parse(userId.Value);
                }
            }

            if (User != null)
            {
                var userId = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Sid);

                if (userId != null)
                {
                    return long.Parse(userId.Value);
                }

            }            

            return 0;
        }
    }
}
