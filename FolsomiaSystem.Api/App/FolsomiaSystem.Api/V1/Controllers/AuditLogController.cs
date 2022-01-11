using FolsomiaSystem.Api.Controllers;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using Microsoft.AspNetCore.Authorization;

namespace FolsomiaSystem.Api.V1.Controllers
{
    public class AuditLogController: BaseController
    {

        private readonly IAuditLogExternalService _auditLogExternalService;

        public AuditLogController(IAuditLogExternalService auditLogExternalService)
        {
            _auditLogExternalService = auditLogExternalService;
        }

        /// <summary>
        /// Busca lista dos logs de auditoria salvos no banco
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if usign URL versioning.</param>
        /// <param name="model">Paging information</param>
        /// <returns>A collection of LogAudit</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
 
        public virtual async Task<IActionResult> GetList(ApiVersion apiVersion, [FromQuery] PagedListIntput model)
        {
            var result = await _auditLogExternalService.GetAllAsync(model);
            Response.Headers.Add("X-Total-Count", result.TotalItemCount.ToString());
            return Ok(result);
        }


        /// <summary>
        /// Lista de auditoria por filtro
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if usign URL versioning.</param>
        /// <param name="filter">Filters</param>
        /// <param name="model">Paging information</param>
        /// <returns>A collection of LogAudit</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
     
        public virtual async Task<IActionResult> GetList(ApiVersion apiVersion, [FromQuery] PagedListIntput model, [FromBody] AuditLogInput filter)
        {
            var result = await _auditLogExternalService.GetAllAsync(filter, model);
            Response.Headers.Add("X-Total-Count", result.TotalItemCount.ToString());
            return Ok(result);
        }

    }
}
