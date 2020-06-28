using FolsomiaSystem.Api.Controllers;
using FolsomiaSystem.Application.Interfaces;
using FolsomiaSystem.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FolsomiaSystem.Api.V1.Controllers
{
    public class AuditLogController: BaseController
    {

        private readonly IAuditLogUseCase _auditLogUseCase;

        public AuditLogController(IAuditLogUseCase auditLogUseCase)
        {
            _auditLogUseCase = auditLogUseCase;
        }

        /// <summary>
        /// Gets the list of Audit Log
        /// </summary>
        /// <param name="apiVersion">API version. It`s automatically populated if usign URL versioning.</param>
        /// <param name="model">Paging information</param>
        /// <returns>A collection of LogAudit</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<IActionResult> GetList(ApiVersion apiVersion, [FromQuery] PagedListIntput model)
        {
            var result = await _auditLogUseCase.GetAllAsync(model);
            Response.Headers.Add("X-Total-Count", result.TotalItemCount.ToString());
            return Ok(result);
        }
    }
}
