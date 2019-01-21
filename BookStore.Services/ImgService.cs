using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using ImageMagick;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BookStore.Services
{
    public class ImgService : IImgService
    {
        private const string ExtentionJpg = ".jpg";
        private const string ExtentionPng = ".png";
        private const string ParentFolder = "images";
        private const int MinAllowableSize = 5000;
        private const int WidthImg = 154;
        private const int HeightImg = 230;

        public bool CheckImgExtention(IFormFile img)
        {
            var fileName = img.FileName;

            var extention = ReturnExtension(fileName);

            if (extention != ExtentionJpg && extention != ExtentionPng)
            {
                return false;
            }

            return true;
        }

        public bool MingLengthOnImg(IFormFile img)
        {
            if (img.Length < MinAllowableSize)
            {
                return false;
            }

            return true;
        }

        public string ReturnExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public string ReturnUrl(IHostingEnvironment environment, string currentFoledName, string extention)
        {
            var newFileName = Guid.NewGuid().ToString() + extention;
            var path = this.ReturnPath(environment, currentFoledName);
            var imgUrl = path + $@"\{newFileName}";

            return imgUrl;
        }

        public string ReturnPath(IHostingEnvironment environment, string currentFoledName)
        {
            return Path.Combine(environment.WebRootPath + $@"\{ParentFolder}", currentFoledName);
        }

        public void CreateImg(IFormFile image, string path, string imgUrl)
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
                MagickGeometry size = new MagickGeometry(WidthImg, HeightImg);

                magicImage.Resize(size);

                magicImage.Write(imgUrl);
            }
        }
    }
}
