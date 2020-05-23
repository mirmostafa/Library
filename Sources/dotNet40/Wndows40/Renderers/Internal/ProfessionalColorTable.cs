#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;

namespace Library40.Win.Renderers.Internal
{
	/// <summary>
	///     Provides colors used for Microsoft Office display elements.
	/// </summary>
	public class ProfessionalColorTable : System.Windows.Forms.ProfessionalColorTable
	{
		#region Enums
		/// <summary>
		///     Gets or sets the KnownColors appearance of the ProfessionalColorTable.
		/// </summary>
		public enum KnownColors
		{
			/// <summary>
			///     The starting color of the gradient used when the button is pressed down.
			/// </summary>
			ButtonPressedGradientBegin,
			/// <summary>
			///     The end color of the gradient used when the button is pressed down.
			/// </summary>
			ButtonPressedGradientEnd,
			/// <summary>
			///     The middle color of the gradient used when the button is pressed down.
			/// </summary>
			ButtonPressedGradientMiddle,
			/// <summary>
			///     The starting color of the gradient used when the button is selected.
			/// </summary>
			ButtonSelectedGradientBegin,
			/// <summary>
			///     The border color to use with the ButtonSelectedGradientBegin,
			///     ButtonSelectedGradientMiddle,
			///     and ButtonSelectedGradientEnd colors.
			/// </summary>
			ButtonSelectedBorder,
			/// <summary>
			///     The end color of the gradient used when the button is selected.
			/// </summary>
			ButtonSelectedGradientEnd,
			/// <summary>
			///     The middle color of the gradient used when the button is selected.
			/// </summary>
			ButtonSelectedGradientMiddle,
			/// <summary>
			///     The border color to use with ButtonSelectedHighlight.
			/// </summary>
			ButtonSelectedHighlightBorder,
			/// <summary>
			///     The solid color to use when the check box is selected and gradients are being used.
			/// </summary>
			CheckBackground,
			/// <summary>
			///     The solid color to use when the check box is selected and gradients are being used.
			/// </summary>
			CheckSelectedBackground,
			/// <summary>
			///     The color to use for shadow effects on the grip or move handle.
			/// </summary>
			GripDark,
			/// <summary>
			///     The color to use for highlight effects on the grip or move handle.
			/// </summary>
			GripLight,
			/// <summary>
			///     The starting color of the gradient used in the image margin
			///     of a ToolStripDropDownMenu.
			/// </summary>
			ImageMarginGradientBegin,
			/// <summary>
			///     The border color or a MenuStrip.
			/// </summary>
			MenuBorder,
			/// <summary>
			///     The border color to use with a ToolStripMenuItem.
			/// </summary>
			MenuItemBorder,
			/// <summary>
			///     The starting color of the gradient used when a top-level
			///     ToolStripMenuItem is pressed down.
			/// </summary>
			MenuItemPressedGradientBegin,
			/// <summary>
			///     The end color of the gradient used when a top-level
			///     ToolStripMenuItem is pressed down.
			/// </summary>
			MenuItemPressedGradientEnd,
			/// <summary>
			///     The middle color of the gradient used when a top-level
			///     ToolStripMenuItem is pressed down.
			/// </summary>
			MenuItemPressedGradientMiddle,
			/// <summary>
			///     The text color of a top-level ToolStripMenuItem.
			/// </summary>
			MenuItemText,
			/// <summary>
			///     The solid color to use when a ToolStripMenuItem other
			///     than the top-level ToolStripMenuItem is selected.
			/// </summary>
			MenuItemSelected,
			/// <summary>
			///     The starting color of the gradient used when the ToolStripMenuItem is selected.
			/// </summary>
			MenuItemSelectedGradientBegin,
			/// <summary>
			///     The end color of the gradient used when the ToolStripMenuItem is selected.
			/// </summary>
			MenuItemSelectedGradientEnd,
			/// <summary>
			///     The starting color of the gradient used in the MenuStrip.
			/// </summary>
			MenuStripGradientBegin,
			/// <summary>
			///     The end color of the gradient used in the MenuStrip.
			/// </summary>
			MenuStripGradientEnd,
			/// <summary>
			///     The starting color of the gradient used in the ToolStripOverflowButton.
			/// </summary>
			OverflowButtonGradientBegin,
			/// <summary>
			///     The end color of the gradient used in the ToolStripOverflowButton.
			/// </summary>
			OverflowButtonGradientEnd,
			/// <summary>
			///     The middle color of the gradient used in the ToolStripOverflowButton.
			/// </summary>
			OverflowButtonGradientMiddle,
			/// <summary>
			///     The starting color of the gradient used in the ToolStripContainer.
			/// </summary>
			RaftingContainerGradientBegin,
			/// <summary>
			///     The end color of the gradient used in the ToolStripContainer.
			/// </summary>
			RaftingContainerGradientEnd,
			/// <summary>
			///     The color to use to for shadow effects on the ToolStripSeparator.
			/// </summary>
			SeparatorDark,
			/// <summary>
			///     The color to use to for highlight effects on the ToolStripSeparator.
			/// </summary>
			SeparatorLight,
			/// <summary>
			///     The starting color of the gradient used on the StatusStrip.
			/// </summary>
			StatusStripGradientBegin,
			/// <summary>
			///     The end color of the gradient used on the StatusStrip.
			/// </summary>
			StatusStripGradientEnd,
			/// <summary>
			///     The text color used on the StatusStrip.
			/// </summary>
			StatusStripText,
			/// <summary>
			///     The border color to use on the bottom edge of the ToolStrip.
			/// </summary>
			ToolStripBorder,
			/// <summary>
			///     The starting color of the gradient used in the ToolStripContentPanel.
			/// </summary>
			ToolStripContentPanelGradientBegin,
			/// <summary>
			///     The end color of the gradient used in the ToolStripContentPanel.
			/// </summary>
			ToolStripContentPanelGradientEnd,
			/// <summary>
			///     The solid background color of the ToolStripDropDown.
			/// </summary>
			ToolStripDropDownBackground,
			/// <summary>
			///     The starting color of the gradient used in the ToolStrip background.
			/// </summary>
			ToolStripGradientBegin,
			/// <summary>
			///     The end color of the gradient used in the ToolStrip background.
			/// </summary>
			ToolStripGradientEnd,
			/// <summary>
			///     The middle color of the gradient used in the ToolStrip background.
			/// </summary>
			ToolStripGradientMiddle,
			/// <summary>
			///     The starting color of the gradient used in the ToolStripPanel.
			/// </summary>
			ToolStripPanelGradientBegin,
			/// <summary>
			///     The end color of the gradient used in the ToolStripPanel.
			/// </summary>
			ToolStripPanelGradientEnd,
			/// <summary>
			/// </summary>
			LastKnownColor = SeparatorLight
		}
		#endregion

		#region FieldsPrivate
		private Dictionary<KnownColors, Color> m_dictionaryRGBTable;
		#endregion

		#region Properties
		/// <summary>
		///     Gets the starting color of the gradient used when the button is pressed down.
		/// </summary>
		public override Color ButtonPressedGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ButtonPressedGradientBegin);
				return base.ButtonPressedGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used when the button is pressed down.
		/// </summary>
		public override Color ButtonPressedGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ButtonPressedGradientEnd);
				return base.ButtonPressedGradientEnd;
			}
		}

		/// <summary>
		///     Gets the middle color of the gradient used when the button is pressed down.
		/// </summary>
		public override Color ButtonPressedGradientMiddle
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ButtonPressedGradientMiddle);
				return base.ButtonPressedGradientMiddle;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used when the button is selected.
		/// </summary>
		public override Color ButtonSelectedBorder
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ButtonSelectedBorder);
				return base.ButtonSelectedBorder;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used when the button is selected.
		/// </summary>
		public override Color ButtonSelectedGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ButtonSelectedGradientBegin);
				return base.ButtonSelectedGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used when the button is selected.
		/// </summary>
		public override Color ButtonSelectedGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ButtonSelectedGradientEnd);
				return base.ButtonSelectedGradientEnd;
			}
		}

		/// <summary>
		///     Gets the middle color of the gradient used when the button is selected.
		/// </summary>
		public override Color ButtonSelectedGradientMiddle
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ButtonSelectedGradientMiddle);
				return base.ButtonSelectedGradientMiddle;
			}
		}

		/// <summary>
		///     Gets the border color to use with ButtonSelectedHighlight.
		/// </summary>
		public override Color ButtonSelectedHighlightBorder
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ButtonSelectedHighlightBorder);
				return base.ButtonSelectedHighlightBorder;
			}
		}

		/// <summary>
		///     Gets the solid color to use when the check box is selected and gradients are being used.
		/// </summary>
		public override Color CheckBackground
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.CheckBackground);
				return base.CheckBackground;
			}
		}

		/// <summary>
		///     Gets the solid color to use when the check box is selected and gradients are being used.
		/// </summary>
		public override Color CheckSelectedBackground
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.CheckSelectedBackground);
				return base.CheckSelectedBackground;
			}
		}

		/// <summary>
		///     Gets the color to use for shadow effects on the grip or move handle.
		/// </summary>
		public override Color GripDark
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.GripDark);
				return base.GripDark;
			}
		}

		/// <summary>
		///     Gets the color to use for highlight effects on the grip or move handle.
		/// </summary>
		public override Color GripLight
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.GripLight);
				return base.GripLight;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used in the image margin of a ToolStripDropDownMenu.
		/// </summary>
		public override Color ImageMarginGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ImageMarginGradientBegin);
				return base.ImageMarginGradientBegin;
			}
		}

		/// <summary>
		///     Gets the border color or a MenuStrip.
		/// </summary>
		public override Color MenuBorder
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuBorder);
				return base.MenuBorder;
			}
		}

		/// <summary>
		///     Gets the border color to use with a ToolStripMenuItem.
		/// </summary>
		public override Color MenuItemBorder
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuItemBorder);
				return base.MenuItemBorder;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used when a top-level ToolStripMenuItem is pressed down.
		/// </summary>
		public override Color MenuItemPressedGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuItemPressedGradientBegin);
				return base.MenuItemPressedGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used when a top-level ToolStripMenuItem is pressed down.
		/// </summary>
		public override Color MenuItemPressedGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuItemPressedGradientEnd);
				return base.MenuItemPressedGradientEnd;
			}
		}

		/// <summary>
		///     Gets the middle color of the gradient used when a top-level ToolStripMenuItem is pressed down.
		/// </summary>
		public override Color MenuItemPressedGradientMiddle
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuItemPressedGradientMiddle);
				return base.MenuItemPressedGradientMiddle;
			}
		}

		/// <summary>
		///     Gets the solid color to use when a ToolStripMenuItem other than the top-level ToolStripMenuItem is selected.
		/// </summary>
		public override Color MenuItemSelected
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuItemSelected);
				return base.MenuItemSelected;
			}
		}

		/// <summary>
		///     Gets the text color of a top-level ToolStripMenuItem.
		/// </summary>
		public virtual Color MenuItemText
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuItemText);
				//return base.MenuItemSelected;
				return Color.Empty;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used when the ToolStripMenuItem is selected.
		/// </summary>
		public override Color MenuItemSelectedGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuItemSelectedGradientBegin);
				return base.MenuItemSelectedGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used when the ToolStripMenuItem is selected.
		/// </summary>
		public override Color MenuItemSelectedGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuItemSelectedGradientEnd);
				return base.MenuItemSelectedGradientEnd;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used in the MenuStrip.
		/// </summary>
		public override Color MenuStripGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuStripGradientBegin);
				return base.MenuStripGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used in the MenuStrip.
		/// </summary>
		public override Color MenuStripGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.MenuStripGradientEnd);
				return base.MenuStripGradientEnd;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used in the ToolStripOverflowButton.
		/// </summary>
		public override Color OverflowButtonGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.OverflowButtonGradientBegin);
				return base.OverflowButtonGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used in the ToolStripOverflowButton.
		/// </summary>
		public override Color OverflowButtonGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.OverflowButtonGradientEnd);
				return base.OverflowButtonGradientEnd;
			}
		}

		/// <summary>
		///     Gets the middle color of the gradient used in the ToolStripOverflowButton.
		/// </summary>
		public override Color OverflowButtonGradientMiddle
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.OverflowButtonGradientMiddle);
				return base.OverflowButtonGradientMiddle;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used in the ToolStripContainer.
		/// </summary>
		public override Color RaftingContainerGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.RaftingContainerGradientBegin);
				return base.RaftingContainerGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used in the ToolStripContainer.
		/// </summary>
		public override Color RaftingContainerGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.RaftingContainerGradientEnd);
				return base.RaftingContainerGradientEnd;
			}
		}

		/// <summary>
		///     Gets the color to use to for shadow effects on the ToolStripSeparator.
		/// </summary>
		public override Color SeparatorDark
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.SeparatorDark);
				return base.SeparatorDark;
			}
		}

		/// <summary>
		///     Gets the color to use to for highlight effects on the ToolStripSeparator.
		/// </summary>
		public override Color SeparatorLight
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.SeparatorLight);
				return base.SeparatorLight;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used on the StatusStrip.
		/// </summary>
		public override Color StatusStripGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.StatusStripGradientBegin);
				return base.StatusStripGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used on the StatusStrip.
		/// </summary>
		public override Color StatusStripGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.StatusStripGradientEnd);
				return base.StatusStripGradientEnd;
			}
		}

		/// <summary>
		///     Gets the text color used on the StatusStrip.
		/// </summary>
		public virtual Color StatusStripText
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.StatusStripText);
				//return base.MenuItemSelected;
				return Color.Empty;
			}
		}

		/// <summary>
		///     Gets the border color to use on the bottom edge of the ToolStrip.
		/// </summary>
		public override Color ToolStripBorder
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripBorder);
				return base.ToolStripBorder;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used in the ToolStripContentPanel.
		/// </summary>
		public override Color ToolStripContentPanelGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripContentPanelGradientBegin);
				return base.ToolStripContentPanelGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used in the ToolStripContentPanel.
		/// </summary>
		public override Color ToolStripContentPanelGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripContentPanelGradientEnd);
				return base.ToolStripContentPanelGradientEnd;
			}
		}

		/// <summary>
		///     Gets the solid background color of the ToolStripDropDown.
		/// </summary>
		public override Color ToolStripDropDownBackground
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripDropDownBackground);
				return base.ToolStripDropDownBackground;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used in the ToolStrip background.
		/// </summary>
		public override Color ToolStripGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripGradientBegin);
				return base.ToolStripGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used in the ToolStrip background.
		/// </summary>
		public override Color ToolStripGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripGradientEnd);
				return base.ToolStripGradientEnd;
			}
		}

		/// <summary>
		///     Gets the middle color of the gradient used in the ToolStrip background.
		/// </summary>
		public override Color ToolStripGradientMiddle
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripGradientMiddle);
				return base.ToolStripGradientMiddle;
			}
		}

		/// <summary>
		///     Gets the starting color of the gradient used in the ToolStripPanel.
		/// </summary>
		public override Color ToolStripPanelGradientBegin
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripPanelGradientBegin);
				return base.ToolStripPanelGradientBegin;
			}
		}

		/// <summary>
		///     Gets the end color of the gradient used in the ToolStripPanel.
		/// </summary>
		public override Color ToolStripPanelGradientEnd
		{
			get
			{
				if (this.UseBaseColorTable == false)
					return this.FromKnownColor(KnownColors.ToolStripPanelGradientEnd);
				return base.ToolStripPanelGradientEnd;
			}
		}

		private bool UseBaseColorTable
		{
			get
			{
				var flag1 = (DisplayInformation.IsLunaTheme == false || ((DisplayInformation.ColorScheme != "HomeStead") && (DisplayInformation.ColorScheme != "NormalColor"))) &&
				            (DisplayInformation.IsAeroTheme == false);
				if (flag1 && (this.m_dictionaryRGBTable != null))
				{
					this.m_dictionaryRGBTable.Clear();
					this.m_dictionaryRGBTable = null;
				}
				return flag1;
			}
		}

		private Dictionary<KnownColors, Color> ColorTable
		{
			get
			{
				if (this.m_dictionaryRGBTable == null)
				{
					this.m_dictionaryRGBTable = new Dictionary<KnownColors, Color>((int)KnownColors.LastKnownColor);
					this.InitColors(ref this.m_dictionaryRGBTable);
				}
				return this.m_dictionaryRGBTable;
			}
		}

		internal Color FromKnownColor(KnownColors color)
		{
			return this.ColorTable[color];
		}
		#endregion

		#region MethodsPublic
		#endregion

		#region MethodsProtected
		/// <summary>
		///     Initialize a color Dictionary with defined colors
		/// </summary>
		/// <param name="rgbTable">Dictionary with defined colors</param>
		protected virtual void InitColors(ref Dictionary<KnownColors, Color> rgbTable)
		{
		}
		#endregion

		#region MethodsPrivate
		private static class DisplayInformation
		{
			#region FieldsPrivate
			private const string m_strAeroFileName = "aero.msstyles";
			private const string m_strLunaFileName = "luna.msstyles";
			private static bool m_bIsAeroTheme;
			[ThreadStatic]
			private static bool m_bIsLunaTheme;
			[ThreadStatic]
			private static string m_strColorScheme;
			#endregion

			#region Properties
			public static string ColorScheme
			{
				get { return m_strColorScheme; }
			}

			internal static bool IsLunaTheme
			{
				get { return m_bIsLunaTheme; }
			}

			internal static bool IsAeroTheme
			{
				get { return m_bIsAeroTheme; }
			}
			#endregion

			#region MethodsPrivate
			static DisplayInformation()
			{
				SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
				SetScheme();
			}

			private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
			{
				SetScheme();
			}

			private static void SetScheme()
			{
				m_bIsLunaTheme = false;
				if (VisualStyleRenderer.IsSupported)
				{
					m_strColorScheme = VisualStyleInformation.ColorScheme;

					if (!VisualStyleInformation.IsEnabledByUser)
						return;
					var stringBuilder = new StringBuilder(0x200);
					NativeMethods.GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, null, 0, null, 0);
					m_bIsLunaTheme = string.Equals(m_strLunaFileName, Path.GetFileName(stringBuilder.ToString()), StringComparison.InvariantCultureIgnoreCase);
					m_bIsAeroTheme = string.Equals(m_strAeroFileName, Path.GetFileName(stringBuilder.ToString()), StringComparison.InvariantCultureIgnoreCase);
				}
				else
					m_strColorScheme = null;
			}
			#endregion

			#region Nested type: NativeMethods
			private static class NativeMethods
			{
				[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
				public static extern int GetCurrentThemeName(StringBuilder pszThemeFileName,
					int dwMaxNameChars,
					StringBuilder pszColorBuff,
					int dwMaxColorChars,
					StringBuilder pszSizeBuff,
					int cchMaxSizeChars);
			}
			#endregion
		}
		#endregion
	}
}