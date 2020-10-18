using FolsomiaSystem.Api.Controllers;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace FolsomiaSystem.Api.V1.Controllers
{
    public class FolsomiaCountController : BaseController
    {
        private readonly IConfiguration Configuration;
        private readonly IFolsomiaCountUseCase _folsomiaCountUseCase;
        private readonly string _fileShared;
        private readonly string _folsomiaJob;

        public FolsomiaCountController(IFolsomiaCountUseCase folsomiaCountUseCase, 
                                        IConfiguration configuration
                                        )
        {
            Configuration = configuration;

            _folsomiaCountUseCase = folsomiaCountUseCase;
            _fileShared = Configuration["FileShared"];
            _folsomiaJob = Configuration["FolsomiaCountJob"];
        }

        /// <summary>
        /// Count Folsomia Candida
        /// </summary>
        /// <param name="model">Folsomia to be calculated.</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> CountFolsomiaCandida([FromBody] FolsomiaCountInput model)
        {

                if (model == null || !ModelState.IsValid) return BadRequest();
                var item = await _folsomiaCountUseCase.CountFolsomiaCandidaAsync(model, _folsomiaJob, _fileShared);
                if (item?.TotalCountFolsomia == 0) return BadRequest(item.AuditLog.MessageLog);

                var urlString = $"{HttpContext.Request.Path}/{item.IdTest}";
                return Created(urlString, item);
        }
    }
}
