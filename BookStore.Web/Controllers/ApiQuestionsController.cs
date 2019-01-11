using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiQuestionsController : ApiController
    {
        private readonly IQuestionService questionService;

        public ApiQuestionsController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        [HttpGet("notificationCount")]
        public ActionResult<int> GetNotificationCount()
        {
            return this.questionService.NotVisitYetQuestionCount();
        }

        [HttpPut("updateState/{id}")]
        public ActionResult<bool> PutUpdateStateVisit([FromRoute]int id)
        {
            var IsHaveIdToUpdate = this.questionService.UpdateStateOnSeen(id);

            if (!IsHaveIdToUpdate)
            {
                return NotFound(false);
            }

            return Ok(true);
        }

        [HttpDelete("deleteQuestion/{id}")]
        public ActionResult<bool> DeleteItem(int id)
        {
            var IsHaveIdToDelete = this.questionService.DeleteItem(id);

            if (!IsHaveIdToDelete)
            {
                return NotFound(false);
            }

            return Ok(true);
        }
    }
}
