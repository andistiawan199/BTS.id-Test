using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API.dto;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Authorize]
    [Route("api/checklist/")]
    [ApiController]
    public class ChecklistItemController : ControllerBase
    {
        private readonly DBContext _dbContext;
        public ChecklistItemController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpPost("{checklistId}/item")]
        public async Task<IActionResult> Save(int checklistId, [FromBody] CreateChecklistItem createChecklistItem)
        {
            try
            {
                ChecklistItemModel model = new()
                {
                    Id = 1,
                    ChecklistId = checklistId,
                    Name = createChecklistItem.itemName,
                };
                await _dbContext.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                ResponseDto<ChecklistItemModel> response = new()
                {
                    statusCode = 201,
                    data = model,
                    message = "Checklist created"
                };
                return new ObjectResult(response) { StatusCode = 201 };
            }
            catch (Exception ex)
            {
                ResponseDto<ChecklistItemModel> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpGet("{checklistId}/item")]
        public async Task<IActionResult> GetChecklistItems(int checklistId)
        {
            try
            {
                var checklistItems = _dbContext.ChecklistItem.Where(x => x.ChecklistId == checklistId).ToList();
                ResponseDto<List<ChecklistItemModel>> response = new()
                {
                    statusCode = 200,
                    data = checklistItems,
                    message = "Showing all checklist items"
                };
                return new ObjectResult(response) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                ResponseDto<List<ChecklistItemModel>> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpGet("{checklistId}/item/{checklistItemId}")]
        public async Task<IActionResult> GetChecklistItem(int checklistId, int checklistItemId)
        {
            try
            {
                ChecklistItemModel checklistItem = (ChecklistItemModel)_dbContext.ChecklistItem
                    .Where(x => x.ChecklistId == checklistId)
                    .Where(x => x.Id == checklistItemId);
                ResponseDto<ChecklistItemModel> response = new()
                {
                    statusCode = 200,
                    data = checklistItem,
                    message = "Showing checklist item"
                };
                return new ObjectResult(response) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                ResponseDto<ChecklistItemModel> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpDelete("{checklistId}/item/{checklistItemId}")]
        public async Task<IActionResult> DeleteChecklistItem(int checklistId, int checklistItemId)
        {
            try
            {
                _dbContext.ChecklistItem.Remove(new()
                {
                    Id = checklistItemId,
                    ChecklistId = checklistId
                });
                _dbContext.SaveChanges();
                ResponseDto<ChecklistItemModel> response = new()
                {
                    statusCode = 200,
                    data = null,
                    message = "Checklist Item deleted"
                };
                return new ObjectResult(response) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                ResponseDto<ChecklistItemModel> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpPut("{checklistId}/item/rename/{checklistItemId}")]
        public async Task<IActionResult> RenameChecklistItem(int checklistId, int checklistItemId, [FromBody] RenameChecklistItem renameChecklistItem)
        {
            try
            {
                ResponseDto<ChecklistItemModel> response = new ResponseDto<ChecklistItemModel>();
                var checklistItem = _dbContext.ChecklistItem.First(x => x.ChecklistId == checklistId && x.Id == checklistItemId);
                if (checklistItem == null)
                {

                    response = new()
                    {
                        statusCode = 404,
                        data = null,
                        message = "item not found"
                    };
                    return new ObjectResult(response) { StatusCode = 404 };
                }
                checklistItem.Name = renameChecklistItem.itemName;
                _dbContext.SaveChanges();
                response = new()
                {
                    statusCode = 200,
                    data = null,
                    message = "Checklist Item renamed"
                };
                return new ObjectResult(response) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                ResponseDto<ChecklistItemModel> response = new()
                {
                    statusCode = 500,
                    data = null,
                    message = ex.Message.ToString()
                };
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpPut("{checklistId}/item/{checklistItemId}")]
        public async Task<IActionResult> UpdateChecklistItem(int checklistId, int checklistItemId)
        {
            try
            {
                ResponseDto<ChecklistItemModel> response = new ResponseDto<ChecklistItemModel>();
                var checklistItem = _dbContext.ChecklistItem.First(x => x.ChecklistId == checklistId && x.Id == checklistItemId);
                if (checklistItem == null)
                {

                    response = new()
                    {
                        statusCode = 404,
                        data = null,
                        message = "item not found"
                    };
                    return new ObjectResult(response) { StatusCode = 404 };
                }
                checklistItem.status = checklistItem.status == 0 ? 1 : 0;
                _dbContext.SaveChanges();
                response = new()
                {
                    statusCode = 200,
                    data = null,
                    message = "Checklist Item status changed"
                };
                return new ObjectResult(response) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                ResponseDto<ChecklistItemModel> response = new()
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
