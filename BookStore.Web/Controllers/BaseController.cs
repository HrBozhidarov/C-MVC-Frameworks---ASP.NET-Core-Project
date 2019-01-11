using BookStore.Models.ViewModels;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private const string ExtentionJpg = ".jpg";
        private const string ExtentionPng = ".png";
        private const string ErrorMessageExtention = "The file have to be with extention png or jpg.";
        private const string ErrorMessageSizeImg = "Picture is too large or too small, it can be between 15000 and 64000 bytes.";
        protected const string FolderName = "Img";
        private const string ParentFolder = "images";
        protected const int NumberOnFolder = 20;
        private const int MaxAllowableSize = 65000;
        private const int MinAllowableSize = 5000;

        protected EditModel GetModel(
            string smallTitle,
            string collectionName,
            string textFieldAndValue,
            string action,
            string controller,
            string eventFunction,
            string id)
        {
            return new EditModel
            {
                SmallTitle = smallTitle,
                CollectionName = collectionName,
                TextFieldAndValue = textFieldAndValue,
                Action = action,
                Controller = controller,
                EventFunction = eventFunction,
                Id = id,
            };
        }

        protected bool CheckServerSideValidationImgCreate(IFormFile img)
        {
            var fileName = img.FileName;

            var extention = ReturnExtension(fileName);

            if (extention != ExtentionJpg && extention != ExtentionPng)
            {
                this.TempData["error"] = ErrorMessageExtention;
                //ModelState.AddModelError("", ErrorMessageExtention);
                return false;
            }

            if (MaxAllowableSize < img.Length || img.Length < MinAllowableSize)
            {
                this.TempData["error"] = ErrorMessageSizeImg;
                //ModelState.AddModelError("", ErrorMessageSizeImg);

                return false;
            }

            return true;
        }

        protected string ReturnExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        protected string ReturnUrl(IHostingEnvironment environment, string currentFoledName, string extention)
        {
            var newFileName = Guid.NewGuid().ToString() + extention;
            var path = this.ReturnPath(environment, currentFoledName);
            var imgUrl = path + $@"\{newFileName}";

            return imgUrl;
        }

        protected string ReturnPath(IHostingEnvironment environment, string currentFoledName)
        {
            return Path.Combine(environment.WebRootPath + $@"\{ParentFolder}", currentFoledName);
        }

        protected void CreateImg(IFormFile image, string path, string imgUrl)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var imgBytesArray = new byte[0];

            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                imgBytesArray = memoryStream.ToArray();
            }

            using (MagickImage magicImage = new MagickImage(imgBytesArray))
            {
                MagickGeometry size = new MagickGeometry(154, 230);

                magicImage.Resize(size);

                magicImage.Write(imgUrl);
            }
        }
    }
}
