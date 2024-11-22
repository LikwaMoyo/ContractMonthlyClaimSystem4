using ContractMonthlyClaimSystem4.Models;
using ContractMonthlyClaimSystem4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem4.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Coordinator,Manager")]
    public class ClaimsApiController : ControllerBase
    {
        private readonly ApprovalWorkflowService _workflowService;

        public ClaimsApiController(ApprovalWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        // POST: api/ClaimsApi/Approve/5
        [HttpPost("Approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _workflowService.ProcessClaimApproval(id);

            if (result != null)
            {
                if (result.IsValid)
                {
                    return Ok(new { message = "Claim approved successfully." });
                }
                else
                {
                    return BadRequest(new { errors = result.Errors.Select(e => e.ErrorMessage).ToList() });
                }
            }
            return NotFound();
        }
    }
}
