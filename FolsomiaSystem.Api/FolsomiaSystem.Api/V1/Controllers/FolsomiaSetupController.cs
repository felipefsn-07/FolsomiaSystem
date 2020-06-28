using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FolsomiaSystem.Api.Controllers;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Models;
using Microsoft.AspNetCore.Mvc;


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
        /// Get Setup.
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if usign URL versioning.</param>
        /// <returns>Get Setup</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> GetList(ApiVersion apiVersion)
        {
            var result = await _folsomiaSetupUseCase.GetAsync();
            Response.Headers.Add("X-Total-Count", result.ToString());
            return Ok(result);
        }
    }
}