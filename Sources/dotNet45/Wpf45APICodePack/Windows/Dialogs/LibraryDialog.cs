using System;
using System.Windows;
using Mohammad.Wpf.Windows.Controls;
using Mohammad.Wpf.Windows.Media;

namespace Mohammad.Wpf.Windows.Dialogs
{
    public static class LibraryDialog
    {
        public static bool AddEffetcts { get; set; }

        public static bool? ShowDialog<TLibraryCommonPage>(Func<TLibraryCommonPage> creator,
            out TLibraryCommonPage page,
            Window owner = null,
            bool? addEffetcts = null) where TLibraryCommonPage : LibraryCommonPage
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            page = creator();
            return ShowDialog(page, owner, addEffetcts);
        }

        public static bool? ShowDialog<TLibraryCommonPage>(out TLibraryCommonPage page, Window owner = null, bool? addEffetcts = null)
            where TLibraryCommonPage : LibraryCommonPage, new() => ShowDialog(page = new TLibraryCommonPage(), owner, addEffetcts);

        public static void Show<TLibraryCommonPage>(out TLibraryCommonPage page, out LibraryCommonDialog dialog, Window owner = null)
            where TLibraryCommonPage : LibraryCommonPage, new()
        {
            page = new TLibraryCommonPage();
            dialog = GetDialog(page, owner);
            dialog.Show();
        }

        public static void Show<TLibraryCommonPage>(out LibraryCommonDialog dialog, Window owner = null) where TLibraryCommonPage : LibraryCommonPage, new()
        {
            TLibraryCommonPage page;
            Show(out page, out dialog, owner);
        }

        public static void Show<TLibraryCommonPage>(out TLibraryCommonPage page, Window owner = null) where TLibraryCommonPage : LibraryCommonPage, new()
        {
            LibraryCommonDialog dialog;
            Show(out page, out dialog, owner);
        }

        public static void Show<TLibraryCommonPage>(Window owner = null) where TLibraryCommonPage : LibraryCommonPage, new()
        {
            TLibraryCommonPage page;
            LibraryCommonDialog dialog;
            Show(out page, out dialog, owner);
        }

        public static bool? ShowDialog<TLibraryCommonPage>(Window owner = null, bool? addEffetcts = null) where TLibraryCommonPage : LibraryCommonPage, new()
        {
            var page = new TLibraryCommonPage();
            return ShowDialog(page, owner, addEffetcts);
        }

        public static bool? ShowDialog<TLibraryCommonPage>(Func<TLibraryCommonPage> creator, Window owner = null, bool? addEffetcts = null)
            where TLibraryCommonPage : LibraryCommonPage
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            var page = creator();
            return ShowDialog(page, owner, addEffetcts);
        }

        public static bool? ShowDialog(LibraryCommonPage page, Window owner = null, bool? addEffetcts = null)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            double opacity = 1;
            if (addEffetcts == null)
            {
                addEffetcts = AddEffetcts;
            }

            try
            {
                var dialog = GetDialog(page, owner);
                if (owner == null)
                {
                    return dialog.ShowDialog();
                }

                opacity = owner.Opacity;
                if (addEffetcts.Value)
                {
                    Animations.FadeOut(owner, .75);
                }
                else
                {
                    owner.Opacity = .75;
                }

                return dialog.ShowDialog();
            }
            finally
            {
                if (owner != null)
                {
                    if (addEffetcts.Value)
                    {
                        Animations.FadeIn(owner, opacity);
                    }

                    else
                    {
                        owner.Opacity = opacity;
                    }
                }
            }
        }

        public static LibraryCommonDialog GetDialog<TLibraryCommonPage>(Func<TLibraryCommonPage> creator, Window owner = null)
            where TLibraryCommonPage : LibraryCommonPage
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            return GetDialog(creator(), owner);
        }

        public static LibraryCommonDialog GetDialog<TLibraryCommonPage>(TLibraryCommonPage page, Window owner = null) where TLibraryCommonPage : LibraryCommonPage
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            if (owner == null)
            {
                owner = Application.Current.MainWindow;
            }

            var lcd = new LibraryCommonDialog {Page = page, Owner = owner};
            return lcd;
        }

        public static LibraryCommonDialog GetDialog<TLibraryCommonPage>(Window owner = null) where TLibraryCommonPage : LibraryCommonPage, new()
            => GetDialog(new TLibraryCommonPage(), owner);
    }
}