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
        private const string QuestuibDetailsPartialName = "_QuestionDetailsPartial";

        private readonly IQuestionService questionService;

        public QuestionsController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        [HttpPost]
        public PartialViewResult VisualizeSendEmail(int id)
        {
            var model = this.questionService.GetSenderContactDetailsById(id);

            return PartialView(QuestuibDetailsPartialName, model);
        }
    }
}
