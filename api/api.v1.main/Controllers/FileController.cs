using api.v1.main.DTOs.File;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using service.v1.minio;

namespace api.v1.main.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/files")]
    public sealed class FileController : APIController
    {
        private readonly IMinioService _minioService;

        public FileController(IMinioService minioService) => _minioService = minioService;



        [HttpPost("push")]
        public IActionResult PushFile([FromBody] FilePushDTO body)
        {
            //body.File.ContentType = "application/json";
            var userID = GetUserIDFromAccessToken();
            //_minioService.PushFile(userID, )
            return Ok("Файл был успешно загружен");
        }
    }
}