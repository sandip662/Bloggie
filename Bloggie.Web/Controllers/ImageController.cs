using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        public ImageController(IImageRepository imageRepository)
        {
                this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("UploadAsync")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
          var imageURL =  await imageRepository.UploadAsync(file);
            if (imageURL == null)
            {
                return Problem("Somthing went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new {link = imageURL});    
        }


    }
}
