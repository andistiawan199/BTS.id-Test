using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_API.dto;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Authorize]
    [Route("api/checklist/")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {
        private readonly DBContext _dbContext;
        public ChecklistController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CreateChecklist createChecklist)
        {
            try
            {
                ChecklistModel model = new()
                {
                    Id = 1,
                    Name = createChecklist.name,
                };
                await _dbContext.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                ResponseDto<ChecklistModel> response = new()
                {
                    statusCode = 201,
                    data = model,
                    message = "Checklist created"
                };
                return new ObjectResult(response) { StatusCode = 201 };
            }
            catch (Exception ex)
            {
                ResponseDto<ChecklistModel> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetChecklists()
        {
            try
            {
                var checklists = _dbContext.Checklist.ToList();
                ResponseDto<List<ChecklistModel>> response = new()
                {
                    statusCode = 200,
                    data = checklists,
                    message = "Showing all checklists"
                };
                return new ObjectResult(response) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                ResponseDto<List<ChecklistModel>> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpDelete("{checklistId}")]
        public async Task<IActionResult> DeleteChecklist(int checklistId)
        {
            try
            {
                _dbContext.Checklist.Remove(new()
                {
                    Id = checklistId
                });
                ResponseDto<ChecklistModel> response = new()
                {
                    statusCode = 200,
                    data = null,
                    message = "Checklist deleted"
                };
                _dbContext.SaveChanges();
                return new ObjectResult(response) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                ResponseDto<ChecklistModel> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }
    }
}
