using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class QuestionsController : AdminController
    {
        private readonly IQuestionService questionService;

        public QuestionsController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        public IActionResult All()
        {
            var model = this.questionService.GetAll();

            return View(model);
        }
    }
}
