using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mohammad.Win.Helpers
{
    public static class ClipboardHelper
    {
        public static void CopyFiles(IEnumerable<string> files)
        {
            var paths = new StringCollection();
            paths.AddRange(files.ToArray());
            Clipboard.SetFileDropList(paths);
        }

        public static void CutFiles(IEnumerable<string> files)
        {
            var moveEffect = new byte[] {2, 0, 0, 0};
            var dropEffect = new MemoryStream();
            dropEffect.Write(moveEffect, 0, moveEffect.Length);

            var data = new DataObject();
            var paths = new StringCollection();
            paths.AddRange(files.ToArray());
            data.SetFileDropList(paths);
            data.SetData("Preferred DropEffect", dropEffect);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);
        }

        public static IEnumerable<string> Paste()
        {
            bool cut;
            return Paste(out cut);
        }

        public static IEnumerable<string> Paste(out bool cut)
        {
            cut = false;
            var data = Clipboard.GetDataObject();
            if (data == null)
                return Enumerable.Empty<string>();
            if (!data.GetDataPresent(DataFormats.FileDrop))
                return Enumerable.Empty<string>();

            var files = (string[]) data.GetData(DataFormats.FileDrop);
            var stream = (MemoryStream) data.GetData("Preferred DropEffect", true);
            var flag = stream.ReadByte();
            if (flag != 2 && flag != 5)
                return Enumerable.Empty<string>();
            cut = flag == 2;
            return files;
        }
    }
}