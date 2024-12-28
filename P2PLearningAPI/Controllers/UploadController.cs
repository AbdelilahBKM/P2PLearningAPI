using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.Interfaces;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class UploadController : ControllerBase
{
    private readonly IUploadInterface _uploadRepository;

    public UploadController(IUploadInterface uploadRepository)
    {
        _uploadRepository = uploadRepository;
    }
    [Authorize]
    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
    {
        try
        {
            var fileName = await _uploadRepository.UploadFileAsync(file); // this probably already has uploads/ in the path?
            var filePath = fileName.Replace("\\", "/");
            return Ok(new { filePath = filePath, Message = "File uploaded successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    [Authorize]
    [HttpDelete("DeleteImage")]
    public async Task<IActionResult> DeleteImage([FromQuery] string filePath)
    {
        try
        {
            var result = await _uploadRepository.DeleteFileAsync(filePath);
            if (result)
            {
                return Ok(new { Message = "File deleted successfully." });
            }

            return NotFound(new { Message = "File not found." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
