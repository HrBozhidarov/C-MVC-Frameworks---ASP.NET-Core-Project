using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public abstract class BaseController : Controller
    {
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
                Id = id
            };
        }
    }
}
