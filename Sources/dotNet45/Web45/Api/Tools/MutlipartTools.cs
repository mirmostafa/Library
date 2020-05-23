#region Code Identifications

// Created on     2018/01/03
// Last update on 2018/01/31 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mohammad.Helpers;
using Newtonsoft.Json;

namespace Mohammad.Web.Api.Tools
{
    public sealed class MutlipartTools
    {
        public static IEnumerable<WebPostedFile> Get() => GetKeys().Select(Get);

        public static WebPostedFile Get(string key)
        {
            var postedFile = HttpContext.Current?.Request.Files[key];
            return postedFile?.ContentLength > 0 ? new WebPostedFile(postedFile, key) : null;
        }

        public static WebPostedFile Get(int index)
        {
            var postedFile = CodeHelper.CatchFunc(() => HttpContext.Current?.Request.Files[index]);
            return postedFile?.ContentLength > 0 ? new WebPostedFile(postedFile, index.ToString()) : null;
        }

        public static IEnumerable<WebPostedFile> Get(params string[] keys)
        {
            return keys.Select(key => new {key, postedFile = HttpContext.Current?.Request.Files[key]})
                .Where(data => data.postedFile?.ContentLength > 0)
                .Select(data => new WebPostedFile(data.postedFile, data.key));
        }

        public static IEnumerable<string> GetKeys()
        {
            var httpFiles = HttpContext.Current?.Request.Files;

            if (httpFiles == null)
            {
                yield break;
            }

            foreach (string fileKey in httpFiles)
            {
                yield return fileKey;
            }
        }

        public static IEnumerable<WebPostedFile> GetByContentType(params string[] contentTypes)
        {
            return Get().Where(f => f.ContentType.ToLower().IsInRange(contentTypes));
        }

        public static IEnumerable<WebPostedFile> GetFiles() => Get(GetKeys().ToArray());

        public static (bool IsFound, int Index) FindMultipartKeyIndex(string key)
        {
            if (HttpContext.Current?.Request == null)
            {
                return (false, -1);
            }

            var result = -1;
            var found = false;
            foreach (string formKey in HttpContext.Current?.Request.Form.Keys)
            {
                result++;
                if (formKey != key)
                {
                    continue;
                }

                found = true;
                break;
            }

            return (found, result);
        }

        public static string GetMultipartValue(string key)
        {
            var (isFound, index) = FindMultipartKeyIndex(key);
            return !isFound ? null : HttpContext.Current?.Request.Form[index];
        }

        public static TModel FillModel<TModel>()
            where TModel : new()
        {
            var result = new TModel();
            foreach (var property in typeof(TModel).GetProperties())
            {
                if (ObjectHelper.GetAttribute<JsonIgnoreAttribute>(property) != null)
                {
                    continue;
                }

                property.SetValue(result, GetMultipartValue(property.Name));
            }

            return result;
        }
    }
}