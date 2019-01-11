using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class QuestionsController : BaseController
    {
        private readonly IQuestionService questionService;

        public QuestionsController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All()
        {
            var model = this.questionService.GetAll();

            return View(model);
        }

        [HttpPost]
        public PartialViewResult VisualizeSendEmail(int id)
        {
            var model = this.questionService.GetSenderContactDetailsById(id);

            return PartialView("_QestionDetailsPartial", model);
        }
    }
}
