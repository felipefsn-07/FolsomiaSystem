using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FolsomiaSystem.Api.Controllers;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FolsomiaSystem.Api.V1.Controllers
{

    public class AdminUserController : BaseController
    {
        private readonly ICredentialSafeExternalService _credentialSafeExternalService;

        public AdminUserController(ICredentialSafeExternalService credentialSafeExternalService)
        {
            _credentialSafeExternalService = credentialSafeExternalService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if usign URL versioning.</param>
        /// <param name="user">User</param>
        /// <returns>login</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetList(ApiVersion apiVersion, [FromBody] AdminUserInputs user)
        {
            var result = await _credentialSafeExternalService.Login(user);
            //Response.Headers.Add("X-Total-Count", "1");
            if (result?.AuditLog != null) return BadRequest(result.AuditLog);
            return Ok(result);
        }

    }
}