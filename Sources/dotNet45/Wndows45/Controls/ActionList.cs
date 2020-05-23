using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using Mohammad.Win.Actions;
using Action = Mohammad.Win.Actions.Action;

namespace Mohammad.Win.Controls
{
    /// <summary>
    ///     Maintains a list of actions that can be used by components and controls, such as menu items and buttons.
    /// </summary>
    [ProvideProperty("Action", typeof(Component))]
    [ToolboxBitmap(typeof(ActionList), "Images.ActionList.bmp")]
    [ToolboxItemFilter("System.Windows.Forms")]
    [Description("Maintains a list of actions that can be used by components and controls, such as menu items and buttons.")]
    public class ActionList : Component, IExtenderProvider, ISupportInitialize
    {
        private readonly Dictionary<Component, Action> _Targets;
        private ContainerControl _ContainerControl;
        private bool _Enabled = true;
        private bool _Initializing;

        /// <summary>
        ///     The tip which will be shown on MouseOver.
        /// </summary>
        [Description("The tip which will be shown on MouseOver.")]
        public ToolTip ToolTip { get; private set; }

        /// <summary>
        ///     Gets or sets whether client controls and menu items are enabled.
        /// </summary>
        [Description("Gets or sets whether client controls and menu items are enabled.")]
        [DefaultValue(true)]
        public bool Enabled
        {
            get { return this._Enabled; }
            set
            {
                if (this._Enabled == value)
                    return;
                this._Enabled = value;
                this.refreshActions();
            }
        }

        /// <summary>
        ///     Lists the actions maintained by the action list.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Lists the actions maintained by the action list.")]
        public ActionCollection Actions { get; }

        /// <summary>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<Type, ActionTargetDescriptionInfo> TypesDescription { get; }

        /// <summary>
        /// </summary>
        public ContainerControl ContainerControl
        {
            get { return this._ContainerControl; }
            set
            {
                if (this._ContainerControl == value)
                    return;
                this._ContainerControl = value;
                if (this.DesignMode)
                    return;
                Form containerForm;
                if ((containerForm = this._ContainerControl as Form) == null)
                    containerForm = this._ContainerControl.ParentForm;
                if (containerForm == null)
                    return;
                if (this.ContainerControl is IActionListContainer)
                    (this.ContainerControl as IActionListContainer).RegisterActionList(this);
                containerForm.KeyPreview = true;
                containerForm.KeyDown += this.form_KeyDown;
            }
        }

        /// <summary>
        /// </summary>
        [Browsable(false)]
        public Control ActiveControl { get { return this.getActiveControl(this.ContainerControl); } }

        /// <summary>
        /// </summary>
        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;
                if (value == null)
                    return;
                var host1 = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (host1 == null)
                    return;
                var container = host1.RootComponent as ContainerControl;
                if (container != null)
                    this.ContainerControl = container;
            }
        }

        /// <summary>
        ///     Creates an instance of ActionList.
        /// </summary>
        public ActionList()
        {
            this.Actions = new ActionCollection(this);
            this._Targets = new Dictionary<Component, Action>();
            this.TypesDescription = new Dictionary<Type, ActionTargetDescriptionInfo>();
            this._Enabled = true;
            this.ToolTip = new ToolTip();

            if (this.DesignMode)
                return;
            Application.Idle += this.Application_Idle;
        }

        private void Application_Idle(object sender, EventArgs e) { this.OnUpdate(EventArgs.Empty); }

        /// <summary>
        ///     Occurs when the application is idle so that the action list can update a specific action in the list.
        /// </summary>
        [Description("Occurs when the application is idle so that the action list can update a specific action in the list.")]
        public event EventHandler Update;

        /// <summary>
        ///     Generates an Update event.
        /// </summary>
        /// <param name="eventArgs"></param>
        protected virtual void OnUpdate(EventArgs eventArgs)
        {
            if (this.Update != null)
                this.Update(this, eventArgs);

            foreach (var action in this.Actions)
                action.DoUpdate();
        }

        /// <summary>
        ///     Return the action assigned to a component
        /// </summary>
        /// <param name="extendee">Component</param>
        /// <returns>asssigned action</returns>
        [DefaultValue(null)]
        public Action GetAction(Component extendee)
        {
            return this._Targets.ContainsKey(extendee) ? this._Targets[extendee] : null;
        }

        /// <summary>
        ///     Assigns the acction to component
        /// </summary>
        /// <param name="extendee">Component</param>
        /// <param name="action">Action</param>
        public void SetAction(Component extendee, Action action)
        {
            if (!this._Initializing)
            {
                if (extendee == null)
                    throw new ArgumentNullException("extendee");
                if (action != null && action.ActionList != this)
                    throw new ArgumentException("The Action you selected is owned by another ActionList");
            }

            if (this._Targets.ContainsKey(extendee))
            {
                this._Targets[extendee].InternalRemoveTarget(extendee);
                this._Targets.Remove(extendee);
            }

            if (action == null)
                return;
            if (!this.TypesDescription.ContainsKey(extendee.GetType()))
                this.TypesDescription.Add(extendee.GetType(), new ActionTargetDescriptionInfo(extendee.GetType()));

            this._Targets.Add(extendee, action);
            action.InternalAddTarget(extendee);
        }

        /// <summary>
        ///     The types which are supported by ActionList
        /// </summary>
        /// <returns></returns>
        protected virtual Type[] GetSupportedTypes()
        {
            return new[] {typeof(ButtonBase), typeof(ToolStripButton), typeof(ToolStripMenuItem), typeof(ToolBarButton), typeof(MenuItem)};
        }

        private void refreshActions()
        {
            /* questo metodo effettua il refresh dello stato Enabled e CheckState
             * di ogni action */
            if (this.DesignMode)
                return;

            foreach (var action in this.Actions)
                action.RefreshEnabledCheckState();
        }

        private void checkInternalCollections()
        {
            /* questo metodo verifica che ogni action su targets
             * appartenga a questa actionList e che abbia la proprietà
             * ActionList correttamente impostata */
            foreach (var action in this._Targets.Values)
                if (!this.Actions.Contains(action) || action.ActionList != this)
                    throw new InvalidOperationException("Action owned by another action list or invalid Action.ActionList");
        }

        private Control getActiveControl(IContainerControl containerControl)
        {
            if (containerControl == null)
                return null;
            if (containerControl.ActiveControl is ContainerControl)
                return this.getActiveControl((ContainerControl) containerControl.ActiveControl);
            return containerControl.ActiveControl;
        }

        private void form_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (var action in this.Actions)
                if (action.ShortcutKeys == e.KeyData)
                    action.ExecuteShortcut();
        }

        #region IExtenderProvider Members

        bool IExtenderProvider.CanExtend(object extendee)
        {
            var targetType = extendee.GetType();

            foreach (var t in this.GetSupportedTypes())
                if (t.IsAssignableFrom(targetType))
                    return true;

            return false;
        }

        #endregion

        #region ISupportInitialize Members

        /// <summary>
        ///     Initializing started.
        /// </summary>
        public void BeginInit()
        {
            this._Initializing = true;
        }

        /// <summary>
        ///     Initializing ended.
        /// </summary>
        public void EndInit()
        {
            this._Initializing = false;
            this.checkInternalCollections();
            this.refreshActions();
        }

        #endregion
    }
}