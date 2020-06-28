using FolsomiaSystem.Api.Controllers;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Models;
using FolsomiaSystem.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;

namespace FolsomiaSystem.Api.V1.Controllers
{
    public class FolsomiaCountController : BaseController
    {
        private readonly IConfiguration Configuration;
        private readonly IFolsomiaCountUseCase _folsomiaCountUseCase;
        private readonly IAuditLogUseCase _auditLogUseCase;
        private readonly IMapper _mapper;
        private readonly string _fileShared;
        private readonly string _folsomiaJob;

        public FolsomiaCountController(IFolsomiaCountUseCase folsomiaCountUseCase, 
                                        IConfiguration configuration,
                                        IAuditLogUseCase auditLogUseCase,
                                        IMapper mapper
                                        )
        {
            Configuration = configuration;

            _folsomiaCountUseCase = folsomiaCountUseCase;
            _fileShared = Configuration["FileShared"];
            _folsomiaJob = Configuration["FolsomiaCountJob"];
            _auditLogUseCase = auditLogUseCase;
            _mapper = mapper;
        }

        /// <summary>
        /// Count Folsomia Candida
        /// </summary>
        /// <param name="model">Folsomia to be calculated.</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(FolsomiaSetupInvalidException), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]

        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CountFolsomiaCandida([FromBody] FolsomiaCountInput model)
        {

                if (model == null || !ModelState.IsValid) return BadRequest();
                var imageOut = string.Format(@"{0}.jpg", Guid.NewGuid());
                model.ImageFolsomiaOutlinedURL = Path.Combine(_fileShared, imageOut);
                model.ImageFolsomiaURL = Path.Combine(_fileShared, model.ImageFolsomiaURL);
                model.ImageFolsomiaOutlinedURL = Path.Combine(_fileShared, imageOut);
                var item = await _folsomiaCountUseCase.CountFolsomiaCandidaAsync(model, _folsomiaJob);
                var auditLog = _mapper.Map<AuditLogInput>(item);
                await _auditLogUseCase.AddNewLogAuditAsync(auditLog);
                if (item?.TotalCountFolsomia == 0) return BadRequest(item);
                var urlString = $"{HttpContext.Request.Path}/{item.ImageFolsomiaOutlinedURL}";
                return Created(urlString, item);

        }
    }
}
