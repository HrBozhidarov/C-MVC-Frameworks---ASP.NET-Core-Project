using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IImgService
    {
        void CreateImg(IFormFile image, string path, string imgUrl);

        string ReturnPath(IHostingEnvironment environment, string currentFoledName);

        string ReturnUrl(IHostingEnvironment environment, string currentFoledName, string extention);

        string ReturnExtension(string fileName);

        bool MingLengthOnImg(IFormFile img);

        bool CheckImgExtention(IFormFile img);
    }
}
