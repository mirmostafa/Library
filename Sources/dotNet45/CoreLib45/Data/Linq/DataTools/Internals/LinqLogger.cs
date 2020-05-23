#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.IO;
using System.Text;

namespace Mohammad.Data.Linq.DataTools.Internals
{
    internal class LinqLogger : TextWriter
    {
        // Methods
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(object value)
        {
            base.Write(value);
        }

        public override void Write(string value)
        {
            base.Write(value);
        }

        public override void Write(string format, params object[] arg)
        {
            base.Write(format, arg);
        }

        public override void Write(string format, object arg0)
        {
            base.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            base.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            base.Write(format, arg0, arg1, arg2);
        }
    }
}