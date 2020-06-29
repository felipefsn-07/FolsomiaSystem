using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FolsomiaSystem.Api.Controllers;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FolsomiaSystem.Api.V1.Controllers
{
    public class FolsomiaSetupController : BaseController
    {
        private readonly IFolsomiaSetupUseCase _folsomiaSetupUseCase;

        public FolsomiaSetupController(IFolsomiaSetupUseCase folsomiaSetupAppService)
        {
            _folsomiaSetupUseCase = folsomiaSetupAppService;
        }


        /// <summary>
        /// Get Setup
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if usign URL versioning.</param>
        /// <returns>Get Setup</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetList(ApiVersion apiVersion)
        {
            var result = await _folsomiaSetupUseCase.GetAsync();
            if (result != null) result.AuditLog = null;
            Response.Headers.Add("X-Total-Count", result.ToString());
            return Ok(result);
        }

        /// <summary>
        /// Update Setup
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if usign URL versioning.</param>
        /// <param name="folsomiaSetupInput">Setup</param>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        public virtual async Task<IActionResult> UpdateToVip(ApiVersion apiVersion, [FromRoute] FolsomiaSetupInput folsomiaSetupInput)
        {
            await _folsomiaSetupUseCase.UpdateSetup(folsomiaSetupInput);
            return Ok();
        }

    }
}