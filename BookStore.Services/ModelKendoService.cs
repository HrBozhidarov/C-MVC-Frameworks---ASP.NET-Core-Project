using BookStore.Models.ViewModels;
using BookStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services
{
    public class ModelKendoService : IModelKendoService
    {
        public EditModel GetModel(string smallTitle, string collectionName, string textFieldAndValue, string url, string eventFunction, string id)
        {
            return new EditModel
            {
                SmallTitle = smallTitle,
                CollectionName = collectionName,
                TextFieldAndValue = textFieldAndValue,
                Url = url,
                EventFunction = eventFunction,
                Id = id,
            };
        }
    }
}