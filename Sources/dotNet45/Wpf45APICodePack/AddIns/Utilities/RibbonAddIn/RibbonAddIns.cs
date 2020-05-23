using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls.Ribbon;
using Mohammad.AddIns;
using Mohammad.AddIns.Contacts;
using Mohammad.Primitives;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Controls;

namespace Mohammad.Wpf.AddIns.Utilities.RibbonAddIn
{

    #region Data Structures

    public interface IAddInRibbonAppContext : IAddInAppContext
    {
        IEnumerable<TRibbonAction> GetRibbonActions<TRibbonAction>() where TRibbonAction : IRibbonAction;
    }

    public interface IRibbonAction : IAddInAction
    {
        string GroupHeader { get; }
        string ButtonLabel { get; }
    }

    public class RibbonAction : RibbonAction<LibraryPage> {}

    public class RibbonAction<TPage> : IRibbonAction
        where TPage : LibraryPage
    {
        public TPage Page { get; set; }
        public string GroupHeader { get; set; }
        public string ButtonLabel { get; set; }
        public string Id { get; set; }
    }

    public class RetieveResult<TAddInAction, TAddInAppContext>
    {
        public TAddInAppContext App { get; }
        public IEnumerable<TAddInAction> Actions { get; }

        public RetieveResult(TAddInAppContext app, IEnumerable<TAddInAction> actions)
        {
            this.App = app;
            this.Actions = actions;
        }
    }

    #endregion

    public static class RibbonAddInLoader
    {
        public static IEnumerable<RetieveResult<RibbonAction, IAddInRibbonAppContext>> RetrieveAndInitializeAddIns(RibbonApplicationMenuItem addInMenuItem,
            RibbonTab addInTab, LibFrame contentFrame, IApplicationInjector injector, params string[] files)
            =>
                RetrieveAndInitializeAddIns(addInMenuItem,
                    addInTab,
                    action => contentFrame.Navigate(action.Page, action),
                    addIn => { addIn.Initialize(injector); },
                    files);

        public static IEnumerable<RetieveResult<TAddInAction, TAddInAppContext>> RetrieveAndInitializeAddIns<TAddInAction, TAddInAppContext, TPage>(
            RibbonApplicationMenuItem addInMenuItem, RibbonTab addInTab, LibFrame contentFrame, IApplicationInjector injector, params string[] files)
            where TAddInAction : RibbonAction<TPage> where TAddInAppContext : class, IAddInRibbonAppContext where TPage : LibraryPage
            =>
                RetrieveAndInitializeAddIns<TAddInAction, TAddInAppContext>(addInMenuItem,
                    addInTab,
                    action => contentFrame.Navigate(action.Page, action),
                    addIn => { addIn.Initialize(injector); },
                    files);

        public static IEnumerable<RetieveResult<TRibbonAction, IAddInRibbonAppContext>> RetrieveAndInitializeAddIns<TRibbonAction, TPage>(
            RibbonApplicationMenuItem addInMenuItem, RibbonTab addInTab, LibFrame contentFrame, IApplicationInjector injector, params string[] files)
            where TRibbonAction : RibbonAction<TPage> where TPage : LibraryPage
            =>
                RetrieveAndInitializeAddIns<TRibbonAction>(addInMenuItem,
                    addInTab,
                    action => contentFrame.Navigate(action.Page, action),
                    addIn => { addIn.Initialize(injector); },
                    files);

        public static IEnumerable<RetieveResult<RibbonAction, IAddInRibbonAppContext>> RetrieveAndInitializeAddIns(RibbonApplicationMenuItem addInMenuItem,
            RibbonTab addInTab, Action<RibbonAction> onButtonClick, Action<IAddInRibbonAppContext> onAddInInitialize, params string[] files)
            => RetrieveAndInitializeAddIns<RibbonAction, IAddInRibbonAppContext>(addInMenuItem, addInTab, onButtonClick, onAddInInitialize, files);

        public static IEnumerable<RetieveResult<TRibbonAction, IAddInRibbonAppContext>> RetrieveAndInitializeAddIns<TRibbonAction>(
            RibbonApplicationMenuItem addInMenuItem, RibbonTab addInTab, Action<TRibbonAction> onButtonClick, Action<IAddInRibbonAppContext> onAddInInitialize,
            params string[] files) where TRibbonAction : IRibbonAction
            => RetrieveAndInitializeAddIns<TRibbonAction, IAddInRibbonAppContext>(addInMenuItem, addInTab, onButtonClick, onAddInInitialize, files);

        public static IEnumerable<RetieveResult<TAddInAction, TAddInAppContext>> RetrieveAndInitializeAddIns<TAddInAction, TAddInAppContext>(
            RibbonApplicationMenuItem addInMenuItem, RibbonTab addInTab, Action<TAddInAction> onButtonClick, Action<TAddInAppContext> onAddInInitialize,
            params string[] files) where TAddInAction : IRibbonAction where TAddInAppContext : class, IAddInRibbonAppContext
        {
            var result = new List<RetieveResult<TAddInAction, TAddInAppContext>>();
            Func<string, RibbonGroup> getGroup = header =>
            {
                var ribbonGroup = addInTab.Items.Cast<RibbonGroup>().FirstOrDefault(g => g.Header.ToString() == header);
                if (ribbonGroup != null)
                    return ribbonGroup;
                ribbonGroup = new RibbonGroup {Header = header};
                addInTab.Items.Add(ribbonGroup);
                return ribbonGroup;
            };

            Func<string, RibbonApplicationMenuItem> getGroupMenuItem = header =>
            {
                var addInMenu = addInMenuItem.Items.Cast<RibbonApplicationMenuItem>().FirstOrDefault(g => g.Header.ToString() == header);
                if (addInMenu != null)
                    return addInMenu;
                addInMenu = new RibbonApplicationMenuItem {Header = header};
                addInMenuItem.Items.Add(addInMenu);
                return addInMenu;
            };

            if (!files.Any())
                files = Directory.GetFiles(".", "*.dll");
            var addIns = Composer.Compose<TAddInAppContext, AddInAttribute>(files).ToList();
            addIns.ForEach(addIn =>
            {
                onAddInInitialize?.Invoke(addIn);
                var actions = addIn.GetRibbonActions<TAddInAction>().ToList();
                actions.ForEach(action =>
                {
                    var btn = new RibbonButton {Label = action.ButtonLabel};
                    btn.Click += (_, __) => onButtonClick(action);
                    var grp = getGroup(action.GroupHeader);
                    grp.Items.Add(btn);

                    var menu = new RibbonApplicationMenuItem {Header = action.ButtonLabel};
                    menu.Click += (_, __) => onButtonClick(action);
                    var addinMenu = getGroupMenuItem(action.GroupHeader);
                    addinMenu.Items.Add(menu);
                });
                result.Add(new RetieveResult<TAddInAction, TAddInAppContext>(addIn.To<TAddInAppContext>(), actions));
            });
            return result;
        }
    }
}