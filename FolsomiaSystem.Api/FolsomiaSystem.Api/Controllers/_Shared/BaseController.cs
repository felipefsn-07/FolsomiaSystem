namespace FolsomiaSystem.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Base controller.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]

    public abstract class BaseController : ControllerBase { }
}
