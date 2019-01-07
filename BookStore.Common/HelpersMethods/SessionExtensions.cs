using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common.HelpersMethods
{
    public static class SessionExtensions
    {
        private const string ShopingCartKey = "Shopping_Cart_Key";

        public static string GetShopingCartKey(this ISession session)
        {
            var shopingCartKey = session.GetString(ShopingCartKey);

            if (shopingCartKey == null)
            {
                shopingCartKey = Guid.NewGuid().ToString();

                session.SetString(ShopingCartKey, shopingCartKey);
            }

            return shopingCartKey;
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T)
                    : JsonConvert.DeserializeObject<T>(value);
        }
    }

}
