#region Code Identifications

// Created on     2017/12/17
// Last update on 2017/12/17 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

// ReSharper disable once CheckNamespace
namespace Mohammad.Helpers
{
    public static class ControlHelper
    {
        public static IEnumerable<TControl> GetControls<TControl>(this Control control)
            where TControl : class
        {
            foreach (Control childControl in control.Controls)
            {
                TControl buffer;
                if ((buffer = childControl as TControl) != null)
                    yield return buffer;

                if (childControl.Controls.Count > 0)
                    foreach (var t in GetControls<TControl>(childControl))
                        yield return t;
            }
        }

        public static void Bind<T>(this BaseDataBoundControl listView, IEnumerable<T> datasource)
        {
            listView.DataSource = null;
            listView.DataBind();
            listView.DataSource = datasource.ToList();
            listView.DataBind();
        }

        public static void Bind(this BaseDataBoundControl grid, object datasource)
        {
            grid.DataSource = null;
            grid.DataBind();
            grid.DataSource = datasource;
            grid.DataBind();
        }

        public static IEnumerable<string> FindCheckedStringIds(this DataGrid grid, string checkBoxName = "DeleteCheckBox") => grid
            .FindCheckedCells(checkBoxName).ToStringId();

        public static IEnumerable<TableCellCollection> FindCheckedCells(this DataGrid grid, string checkBoxName = "DeleteCheckBox")
        {
            return grid.Items.Cast<DataGridItem>().Select(item => new
                                                                  {
                                                                      item,
                                                                      isChecked = ((CheckBox) item.FindControl(checkBoxName)).Checked
                                                                  }).Where(data => data.isChecked).Select(data => data.item.Cells);
        }

        public static IEnumerable<long> FindCheckedIds(this DataGrid grid, string checkBoxName = "DeleteCheckBox") => grid
            .FindCheckedCells(checkBoxName).ToId();

        public static IEnumerable<long> ToId(this IEnumerable<TableCellCollection> cells, int index = 1) => cells.Select(
            cell => cell[index].Text.ToLong());

        public static IEnumerable<string> ToStringId(this IEnumerable<TableCellCollection> cells, int index = 1) =>
            cells.Select(cell => cell[index].Text);

        public static void SetVisibility(this HtmlControl control, bool isVisible)
        {
            control.Visible = isVisible;
            var cssClass = control.Attributes["class"].Replace("invisible", "");
            control.Attributes.Remove("class");

            if (!isVisible)
                cssClass = cssClass.Insert(0, "invisible ");

            control.Attributes.Add("class", cssClass);
        }

        public static void SetEnability(this HtmlControl control, bool isEnabled)
        {
            control.Disabled = !isEnabled;
            var cssClass = control.Attributes["class"].Replace("disabled", "");
            control.Attributes.Remove("class");
            if (!isEnabled)
                cssClass = cssClass.Insert(0, "disabled ");
            control.Attributes.Add("class", cssClass);
        }

        public static void SetVisibility(this WebControl control, bool isVisible)
        {
            control.Visible = isVisible;
            var cssClass = control.CssClass.Replace("invisible", "");

            if (!isVisible)
                control.CssClass = string.Concat("invisible ", cssClass);
        }

        public static void SetVisibility(this Control control, bool isVisible) { control.Visible = isVisible; }

        public static void SetReadOnly(this HtmlInputText control, bool isReadOnly)
        {
            control.Attributes.Remove("readonly");
            if (isReadOnly)
                control.Attributes.Add("readonly", "true");
        }

        public static void SetReadOnly(this HtmlTextArea control, bool isReadOnly)
        {
            control.Attributes.Remove("readonly");
            if (isReadOnly)
                control.Attributes.Add("readonly", "true");
        }

        private static IEnumerable<T> Compact<T>(this IEnumerable<T> items) => items.Where(item => item != null);

        public static IEnumerable<Control> FindAllControls(this Control control, Func<Control, bool> predicate = null)
        {
            if (predicate == null)
                yield return control;
            else if (predicate(control))
                yield return control;

            foreach (var child in control.Controls.CastOrNull<Control>().Compact())
                if (predicate == null)
                    yield return child;
                else if (predicate(child))
                    yield return control;

            foreach (var child in control.Controls.CastOrNull<Control>().Compact())
            foreach (var match in FindAllControls(child, predicate))
                yield return match;
        }

        public static IEnumerable<HtmlControl> FindAllControls(this HtmlControl control, Func<HtmlControl, bool> predicate = null) =>
            ((Control) control).FindAllControls().CastOrNull<HtmlControl>();

        public static IEnumerable<WebControl> FindAllControls(this WebControl control, Func<WebControl, bool> predicate = null) =>
            ((Control) control).FindAllControls().CastOrNull<WebControl>();

        public static IEnumerable<HtmlControl> FindAllControlsByCssClass(this HtmlControl control, string cssClass)
        {
            return control.FindAllControls(c =>
            {
                var cls = c.Attributes["class"];
                return cls?.Contains(cssClass) == true;
            });
        }

        public static IEnumerable<WebControl> FindAllControlsByCssClass(this WebControl control, string cssClass)
        {
            return control.FindAllControls(c =>
            {
                var cls = c.Attributes["class"];
                return cls?.Contains(cssClass) == true;
            });
        }

        public static IEnumerable<Control> FindAllControlsByCssClass(this Page page, string cssClass)
        {
            return page.Controls.Cast<Control>().SelectMany(control => control.FindAllControls(c =>
            {
                if (c is UserControl)
                {
                    var cls = c.As<UserControl>().Attributes["class"];
                    return cls?.Contains(cssClass) == true;
                }
                if (c is WebControl)
                {
                    var cls = c.As<WebControl>().Attributes["class"];
                    return cls?.Contains(cssClass) == true;
                }
                //if (c is LiteralControl)
                //{
                //    var cls = c.As<LiteralControl>().Attributes["class"];
                //    return cls != null && cls.Contains(cssClass);
                //}
                if (c is HtmlGenericControl)
                {
                    var cls = c.As<HtmlGenericControl>().Attributes["class"];
                    return cls?.Contains(cssClass) == true;
                }
                if (c is HtmlInputText)
                {
                    var cls = c.As<HtmlInputText>().Attributes["class"];
                    return cls?.Contains(cssClass) == true;
                }
                if (c is HtmlTextArea)
                {
                    var cls = c.As<HtmlTextArea>().Attributes["class"];
                    return cls?.Contains(cssClass) == true;
                }
                return false;
            }));
        }

        public static IEnumerable<TControl> FindAllControlsByCssClass<TControl>(this Page page, string cssClass)
            where TControl : class
        {
            var result = FindAllControlsByCssClass(page, cssClass).OfType<TControl>();
            return result;
        }

        public static string RelativePath(this HttpServerUtility server, string path, HttpRequest request = null) => !path.IsNullOrEmpty()
            ? path.Replace((request ?? HttpContext.Current.Request).ServerVariables["APPL_PHYSICAL_PATH"], "~/").Replace(@"\", "/")
            : string.Empty;
    }
}