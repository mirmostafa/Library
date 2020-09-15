// Created on     2018/01/03
// Last update on 2018/01/03 by Mohammad Mir mostafa 

using System;
using System.IO;
using System.Web;
using Mohammad.Helpers;

namespace Mohammad.Web.Api.Tools
{
    public sealed class WebPostedFile
    {
        private readonly HttpPostedFile _File;

        public WebPostedFile(HttpPostedFile file, string key)
        {
            this._File = file ?? throw new ArgumentNullException(nameof(file));
            this.Key = key;
        }

        public string Key { get; }
        public string ContentType => this._File.ContentType;
        public string FileName => this._File.FileName;
        public int ContentLength => this._File.ContentLength;
        public Stream InputStream => this._File.InputStream;

        public void Save(string fileName = null)
        {
            this._File.SaveAs(fileName ?? this.FileName);
        }

        public string SaveIn(string absolutePath, string fileName = null)
        {
            var filePath = FileSystemHelper.CombineFilePath(absolutePath,
                fileName ?? $"{this.Key}_{this.FileName}.{Path.GetExtension(this.FileName)}");
            this.Save(filePath);
            return filePath;
        }
    }
}