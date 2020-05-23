#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Library35.Windows.Controls;

namespace Library35.Windows.Actions
{
	/// <summary>
	///     Action is the base class for actions meant to be used with menu items and controls.
	/// </summary>
	[ToolboxBitmap(typeof (Action), "Images.Action.bmp")]
	[DefaultEvent("Execute")]
	[StandardAction]
	public class Action : Component, IPermissionalControl
	{
		private ActionList actionList;

		/// <summary>
		///     Creates a new instance
		/// </summary>
		public Action()
		{
			this.targets = new List<Component>();
			this._enabled = true;
			this._text = string.Empty;
			this.WorkingState = ActionWorkingState.Listening;
			this.shortcutKeys = Keys.None;
			this.toolTipText = string.Empty;
			this.visible = true;

			this.clickEventHandler = this.target_Click;
			this.checkStateChangedEventHandler = this.target_CheckStateChanged;
		}

		/// <summary>
		/// </summary>
		protected ActionWorkingState WorkingState { get; set; }

		/// <summary>
		///     The parent ActionList
		/// </summary>
		protected internal ActionList ActionList
		{
			get { return this.actionList; }
			set
			{
				if (this.actionList != value)
					this.actionList = value;
			}
		}

		public object Tag { get; set; }

		#region common properties
		private bool _AutoCheck;
		private bool _enabled;
		private string _text;
		private CheckState checkState;
		private Image image;
		private Keys shortcutKeys;
		private string toolTipText;
		private bool visible;

		/// <summary>
		///     Represents the text of client controls and menu items.
		/// </summary>
		[DefaultValue("")]
		[UpdatableProperty]
		[Localizable(true)]
		public string Text
		{
			get { return this._text; }
			set
			{
				if (this._text != value)
				{
					this._text = value;
					this.UpdateAllTargets("Text", value);
				}
			}
		}

		/// <summary>
		///     Gets or sets whether client controls and menu items appear checked.
		/// </summary>
		[DefaultValue(false)]
		public bool Checked
		{
			get { return (this.checkState != CheckState.Unchecked); }
			set
			{
				if (value != this.Checked)
					this.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
			}
		}

		/// <summary>
		///     Gets or sets client controls and menu items check appearance.
		/// </summary>
		[DefaultValue(CheckState.Unchecked)]
		[UpdatableProperty]
		public CheckState CheckState
		{
			get { return this.checkState; }
			set
			{
				if (this.checkState != value)
				{
					this.checkState = value;
					this.UpdateAllTargets("CheckState", value);
				}
			}
		}

		/// <summary>
		///     Gets or sets whether client controls and menu items are enabled.
		/// </summary>
		[DefaultValue(true)]
		[UpdatableProperty]
		public bool Enabled
		{
			get
			{
				if (this.ActionList != null)
					return this._enabled && this.ActionList.Enabled;
				return this._enabled;
			}
			set
			{
				if (this._enabled != value)
				{
					this._enabled = value;
					this.UpdateAllTargets("Enabled", value);
				}
			}
		}

		/// <summary>
		///     Gets or sets the Image property for client controls and menu items.
		/// </summary>
		[DefaultValue(null)]
		[UpdatableProperty]
		public Image Image
		{
			get { return this.image; }
			set
			{
				if (this.image != value)
				{
					this.image = value;
					this.UpdateAllTargets("Image", value);
				}
			}
		}

		/// <summary>
		///     Gets or sets whether the Checked property toggles when the action executes.
		/// </summary>
		[DefaultValue(false)]
		[UpdatableProperty]
		public bool AutoCheck
		{
			get { return this._AutoCheck; }
			set
			{
				if (this._AutoCheck != value)
				{
					this._AutoCheck = value;
					this.UpdateAllTargets("AutoCheck", value);
				}
			}
		}

		/// <summary>
		///     Gets or sets the ShortCut property for client menu items.
		/// </summary>
		[DefaultValue(Keys.None)]
		[UpdatableProperty]
		[Localizable(true)]
		public Keys ShortcutKeys
		{
			get { return this.shortcutKeys; }
			set
			{
				if (this.shortcutKeys != value)
				{
					this.shortcutKeys = value;
					var kc = new KeysConverter();
					var s = (string)kc.ConvertTo(value, typeof (string));
					this.UpdateAllTargets("ShortcutKeyDisplayString", s);
				}
			}
		}

		/// <summary>
		///     Gets or sets the Visible property for client controls and menu items.
		/// </summary>
		[DefaultValue(true)]
		[UpdatableProperty]
		public bool Visible
		{
			get { return this.visible; }
			set
			{
				if (this.visible != value)
				{
					this.visible = value;
					this.UpdateAllTargets("Visible", value);
				}
			}
		}

		/// <summary>
		///     Gets or set the ToolTip for client controls and menu items.
		/// </summary>
		[DefaultValue("")]
		[UpdatableProperty]
		[Localizable(true)]
		public string ToolTipText
		{
			get { return this.toolTipText; }
			set
			{
				if (this.toolTipText != value)
				{
					this.toolTipText = value;
					this.UpdateAllTargets("ToolTipText", value);
				}
			}
		}
		#endregion

		#region updating targets
		internal void RefreshEnabledCheckState()
		{
			this.UpdateAllTargets("Enabled", this.Enabled);
			this.UpdateAllTargets("CheckState", this.CheckState);
		}

		/// <summary>
		///     Loops an targets an calls their update, one by one.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		protected void UpdateAllTargets(string propertyName, object value)
		{
			foreach (var c in this.targets)
				this.updateProperty(c, propertyName, value);
		}

		private void updateProperty(Component target, string propertyName, object value)
		{
			this.WorkingState = ActionWorkingState.Driving;
			try
			{
				if (this.ActionList != null)
					if (!this.SpecialUpdateProperty(target, propertyName, value))
						this.ActionList.TypesDescription[target.GetType()].SetValue(propertyName, target, value);
			}
			finally
			{
				this.WorkingState = ActionWorkingState.Listening;
			}
		}

		/// <summary>
		///     Sets the properties such as ToolTip which are common.
		/// </summary>
		/// <param name="target">Target component</param>
		/// <param name="propertyName">Property name to be set.</param>
		/// <param name="value">The value of property</param>
		/// <returns></returns>
		protected virtual bool SpecialUpdateProperty(Component target, string propertyName, object value)
		{
			if (propertyName == "ToolTipText")
			{
				var c = target as Control;

				if (c != null && this.ActionList.ToolTip.CanExtend(c))
					this.ActionList.ToolTip.SetToolTip(c, (string)value);
				return true;
			}
			return false;
		}

		private void refreshState(Component target)
		{
			var properties = TypeDescriptor.GetProperties(this,
				new Attribute[]
				{
					new UpdatablePropertyAttribute()
				});

			foreach (PropertyDescriptor property in properties)
				this.updateProperty(target, property.Name, property.GetValue(this));
		}
		#endregion

		#region Hook su eventi target
		private readonly EventHandler checkStateChangedEventHandler;
		private readonly EventHandler clickEventHandler;

		/// <summary>
		///     Adds new handlers to common events
		/// </summary>
		/// <param name="extendee"></param>
		protected virtual void AddHandler(Component extendee)
		{
			var clickEvent = extendee.GetType().GetEvent("Click");
			if (clickEvent != null)
				clickEvent.AddEventHandler(extendee, this.clickEventHandler);

			var checkStateChangedEvent = extendee.GetType().GetEvent("CheckStateChanged");
			if (checkStateChangedEvent != null)
				checkStateChangedEvent.AddEventHandler(extendee, this.checkStateChangedEventHandler);

			var button = extendee as ToolBarButton;
			if (button != null)
				button.Parent.ButtonClick += this.toolbar_ButtonClick;
		}

		/// <summary>
		///     Removes handlers from common events
		/// </summary>
		/// <param name="extendee"></param>
		protected virtual void RemoveHandler(Component extendee)
		{
			var clickEvent = extendee.GetType().GetEvent("Click");
			if (clickEvent != null)
				clickEvent.RemoveEventHandler(extendee, this.clickEventHandler);

			var checkStateChangedEvent = extendee.GetType().GetEvent("CheckStateChanged");
			if (checkStateChangedEvent != null)
				checkStateChangedEvent.RemoveEventHandler(extendee, this.checkStateChangedEventHandler);

			var button = extendee as ToolBarButton;
			if (button != null)
				button.Parent.ButtonClick -= this.toolbar_ButtonClick;
		}
		#endregion

		#region Handling eventi target

		#region Click
		private void toolbar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (this.targets.Contains(e.Button))
				this.handleClick(e.Button, e);
		}

		private void target_Click(object sender, EventArgs e)
		{
			this.handleClick(sender, e);
		}

		private void handleClick(object sender, EventArgs e)
		{
			if (this.WorkingState == ActionWorkingState.Listening)
			{
				var target = sender as Component;
				Debug.Assert(target != null, "Target non è un component su handleClick");
				Debug.Assert(this.targets.Contains(target), "Target non esiste su collection targets su handleClick");

				this.PerformExecute();
			}
		}
		#endregion

		#region CheckStateChanged
		internal bool InterceptingCheckStateChanged { get; set; }

		private void target_CheckStateChanged(object sender, EventArgs e)
		{
			this.handleCheckStateChanged(sender, e);
		}

		private void handleCheckStateChanged(object sender, EventArgs e)
		{
			if (this.WorkingState == ActionWorkingState.Listening)
			{
				var target = sender as Component;
				this.CheckState = (CheckState)this.ActionList.TypesDescription[sender.GetType()].GetValue("CheckState", target);
			}
		}
		#endregion

		#endregion

		#region Action execution
		/// <summary>
		///     Prforms the Execute event if enebled.
		/// </summary>
		public void PerformExecute()
		{
			if (!this.Enabled)
				return;

			var e = new CancelEventArgs();
			this.OnExecuting(e);
			if (e.Cancel)
				return;
			this.OnExecute(EventArgs.Empty);
			this.OnExecuted(EventArgs.Empty);
		}

		internal void ExecuteShortcut()
		{
			if (!this.Enabled)
				return;

			if (this.AutoCheck)
				this.Checked = !this.Checked;
			this.PerformExecute();
		}
		#endregion

		#region Events and event handlers
		/// <summary>
		///     Occurs when the action is going to be executed.
		/// </summary>
		public event CancelEventHandler Executing;

		/// <summary>
		///     Raised Executing event
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnExecuting(CancelEventArgs e)
		{
			if (this.Executing != null)
				this.Executing(this, e);
		}

		/// <summary>
		///     Occurs when the client event that is linked to it fires.
		/// </summary>
		public event EventHandler Execute;

		/// <summary>
		///     Raises Execute event
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnExecute(EventArgs e)
		{
			if (this.Execute != null)
				this.Execute(this, e);
		}

		/// <summary>
		///     Occurs when the execution is accomplished.
		/// </summary>
		public event EventHandler Executed;

		/// <summary>
		///     Raises Executed event
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnExecuted(EventArgs e)
		{
			if (this.Executed != null)
				this.Executed(this, e);
		}

		/// <summary>
		///     Occurs when the application is idle or when the action list updates.
		/// </summary>
		public event EventHandler Update;

		/// <summary>
		///     Raises Update event
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnUpdate(EventArgs e)
		{
			if (this.Update != null)
				this.Update(this, e);
		}

		/// <summary>
		///     Performs Update event
		/// </summary>
		public void DoUpdate()
		{
			this.OnUpdate(EventArgs.Empty);
		}
		#endregion

		#region Gestione di collection di oggetti associati
		private readonly List<Component> targets;

		internal void InternalRemoveTarget(Component extendee)
		{
			this.targets.Remove(extendee);
			this.RemoveHandler(extendee);
			this.OnRemovingTarget(extendee);
		}

		internal void InternalAddTarget(Component extendee)
		{
			this.targets.Add(extendee);
			this.refreshState(extendee);
			this.AddHandler(extendee);
			this.OnAddingTarget(extendee);
		}

		/// <summary>
		///     Does nothing
		/// </summary>
		/// <param name="extendee"></param>
		protected virtual void OnRemovingTarget(Component extendee)
		{
		}

		/// <summary>
		///     Does nothing
		/// </summary>
		/// <param name="extendee"></param>
		protected virtual void OnAddingTarget(Component extendee)
		{
		}
		#endregion

		#region IPermissionalControl Members
		public string PermissionKey { get; set; }
		#endregion

		#region Nested type: ActionWorkingState
		/// <summary>
		/// </summary>
		protected enum ActionWorkingState
		{
			/// <summary>
			/// </summary>
			Listening,
			/// <summary>
			/// </summary>
			Driving
		}
		#endregion
	}
}