using BookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IModelKendoService
    {
        EditModel GetModel(
          string smallTitle,
          string collectionName,
          string textFieldAndValue,
          string url,
          string eventFunction,
          string id);
    }
}
