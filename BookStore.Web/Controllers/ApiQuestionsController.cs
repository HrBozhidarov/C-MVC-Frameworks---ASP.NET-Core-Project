using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiQuestionsController : ApiController
    {
        private const string GetNotificationCountName = "notificationCount";
        private const string PutUpdateStateName = "updateState";
        private const string DeleteDeleteQuestionName = "deleteQuestion";

        private readonly IQuestionService questionService;

        public ApiQuestionsController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(GetNotificationCountName)]
        public ActionResult<int> GetNotificationCount()
        {
            return this.questionService.NotVisitYetQuestionCount();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(PutUpdateStateName)]
        public ActionResult<bool> PutUpdateStateVisit([FromBody]int id)
        {
            var IsHaveIdToUpdate = this.questionService.UpdateStateOnSeen(id);

            if (!IsHaveIdToUpdate)
            {
                return NotFound(false);
            }

            return true;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete(DeleteDeleteQuestionName)]
        public ActionResult<bool> DeleteItem([FromBody]int id)
        {
            var IsHaveIdToDelete = this.questionService.DeleteItem(id);

            if (!IsHaveIdToDelete)
            {
                return NotFound(false);
            }

            return true;
        }
    }
}
