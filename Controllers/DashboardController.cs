using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Demo.Mvc.Models;

namespace WebService.Controllers
{


    [ApiVersionNeutral]
    public class DashboardController : Controller
    {
        #region setup
        private readonly ILogger<DashboardController> _logger;


        public DashboardController(
            ILogger<DashboardController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Actions
        [HttpGet]
        [Route("[controller]/{tenantId}")]
        // I expect redirect to tenant-specific help page here
        // to the action "HttpGet Same(tenantId)"
        // but get "too many redirect" because of incorrect route
        public IActionResult Index(string tenantId)
        {
            return RedirectToAction("Same", new { tenantId = tenantId});
        }


        [HttpGet]
        [Route("[controller]/{tenantId}/same")]
        // This action has HttpPost with the same name, so it will produce incorrect route
        // with 'tenantId' in query string (as like as HttpPost route supposes)

        public IActionResult Same(string tenantId)
        {
            return View("Help", tenantId);
        }


        [HttpPost]
        [Route("[controller]/same")]
        // if we use the same action name  as Get action does, then
        // .RedirectToAction() will incorrectly make the link to Post(!) method
        // the issue occurs for the route patterns such as [controller]/{tenantId}/action_name
        public IActionResult Same(DummyViewModel vm)
        {
            try
            {
                return Content($"Hello, this is HttpPost action");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Content( $"Error : {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[controller]/{tenantId}/unique")]

        // This action has not HttpPost with the same name, so it will produce correct route
        public IActionResult Unique(string tenantId)
        {
            return Content($"Congrats, {tenantId} !");
        }

        [HttpGet]
        [Route("[controller]/actionfirst/{tenantId}")]

        // This action has parameter at the end, so it will produce correct route
        // regardless of POST action
        public IActionResult ActionFirst(string tenantId)
        {
            return Content($"Congrats, {tenantId}, this route pattern produces the correct link !");
        }

        [HttpPost]
        [Route("[controller]/actionfirst")]

        // This action has parameter at the end, so it will produce correct route
        // regardless of POST action
        public IActionResult ActionFirst(DummyViewModel vm)
        {
            return Content($"This is POST action, but it is not an obstacle when the route pattern does not contain parameter at the middle");
        }



        #endregion



    }
}
