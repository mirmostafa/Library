#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Drawing;

namespace Library40.Win.Renderers.Internal
{
	/// <summary>
	///     Provides colors used by WindowsVista style rendering
	/// </summary>
	/// <remarks>
	///     2007 José Manuel Menéndez Poo
	///     Visit my blog for upgrades and other renderers - www.menendezpoo.com
	/// </remarks>
	public class WindowsVistaColorTable
	{
		#region Fields
		#endregion

		#region Ctor
		public WindowsVistaColorTable()
		{
			this.BackgroundNorth = Color.Black;
			this.BackgroundSouth = Color.Black;

			this.GlossyEffectNorth = Color.FromArgb(217, 0x68, 0x7C, 0xAC);
			//GlossyEffectNorth = Color.Red;
			this.GlossyEffectSouth = Color.FromArgb(74, 0xAA, 0xB5, 0xD0);

			this.BackgroundBorder = Color.FromArgb(0x85, 0x85, 0x87);
			this.BackgroundGlow = Color.FromArgb(0x43, 0x53, 0x7A);

			this.Text = Color.White;

			this.ButtonOuterBorder = Color.FromArgb(0x75, 0x7D, 0x95);
			this.ButtonInnerBorder = Color.FromArgb(0xBF, 0xC4, 0xCE);
			this.ButtonInnerBorderPressed = Color.FromArgb(0x4b, 0x4b, 0x4b);
			this.ButtonBorder = Color.FromArgb(0x03, 0x07, 0x0D);
			this.ButtonFillNorth = Color.FromArgb(85, Color.White);
			this.ButtonFillSouth = Color.FromArgb(1, Color.White);
			this.ButtonFillNorthPressed = Color.FromArgb(150, Color.Black);
			this.ButtonFillSouthPressed = Color.FromArgb(100, Color.Black);

			this.Glow = Color.FromArgb(0x30, 0x73, 0xCE);
			//Glow = Color.Red;
			this.DropDownArrow = Color.White;

			this.MenuHighlight = Color.FromArgb(0xA8, 0xD8, 0xEB);
			//MenuHighlight = Color.Red;
			this.MenuHighlightNorth = Color.FromArgb(25, this.MenuHighlight);
			this.MenuHighlightSouth = Color.FromArgb(102, this.MenuHighlight);
			this.MenuBackground = Color.FromArgb(0xF1, 0xF1, 0xF1);
			this.MenuDark = Color.FromArgb(0xE2, 0xE3, 0xE3);
			this.MenuLight = Color.White;

			this.SeparatorNorth = this.BackgroundSouth;
			this.SeparatorSouth = this.GlossyEffectNorth;

			//MenuText = Color.Black;
			this.MenuText = Color.White;
			this.ToolStripDropDownMenuText = Color.Black;
			//MenuText = Color.Red;

			this.CheckedGlow = Color.FromArgb(0x57, 0xC6, 0xEF);
			this.CheckedGlowHot = Color.FromArgb(0x70, 0xD4, 0xFF);
			this.CheckedButtonFill = Color.FromArgb(0x18, 0x38, 0x9E);
			this.CheckedButtonFillHot = Color.FromArgb(0x0F, 0x3A, 0xBF);
		}
		#endregion

		#region Properties
		public Color CheckedGlowHot { get; set; }

		public Color CheckedButtonFillHot { get; set; }

		public Color CheckedButtonFill { get; set; }

		public Color CheckedGlow { get; set; }

		public Color MenuText { get; set; }

		public Color ToolStripDropDownMenuText { get; set; }

		public Color SeparatorNorth { get; set; }

		public Color SeparatorSouth { get; set; }

		public Color MenuLight { get; set; }

		public Color MenuDark { get; set; }

		public Color MenuBackground { get; set; }

		public Color MenuHighlightSouth { get; set; }

		public Color MenuHighlightNorth { get; set; }

		public Color MenuHighlight { get; set; }

		/// <summary>
		///     Gets or sets the color for the dropwown arrow
		/// </summary>
		public Color DropDownArrow { get; set; }

		/// <summary>
		///     Gets or sets the south color of the button fill when pressed
		/// </summary>
		public Color ButtonFillSouthPressed { get; set; }

		/// <summary>
		///     Gets or sets the south color of the button fill
		/// </summary>
		public Color ButtonFillSouth { get; set; }

		/// <summary>
		///     Gets or sets the color of the inner border when pressed
		/// </summary>
		public Color ButtonInnerBorderPressed { get; set; }

		/// <summary>
		///     Gets or sets the glow color
		/// </summary>
		public Color Glow { get; set; }

		/// <summary>
		///     Gets or sets the buttons fill color
		/// </summary>
		public Color ButtonFillNorth { get; set; }

		/// <summary>
		///     Gets or sets the buttons fill color when pressed
		/// </summary>
		public Color ButtonFillNorthPressed { get; set; }

		/// <summary>
		///     Gets or sets the buttons inner border color
		/// </summary>
		public Color ButtonInnerBorder { get; set; }

		/// <summary>
		///     Gets or sets the buttons border color
		/// </summary>
		public Color ButtonBorder { get; set; }

		/// <summary>
		///     Gets or sets the buttons outer border color
		/// </summary>
		public Color ButtonOuterBorder { get; set; }

		/// <summary>
		///     Gets or sets the color of the text
		/// </summary>
		public Color Text { get; set; }

		/// <summary>
		///     Gets or sets the background glow color
		/// </summary>
		public Color BackgroundGlow { get; set; }

		/// <summary>
		///     Gets or sets the color of the background border
		/// </summary>
		public Color BackgroundBorder { get; set; }

		/// <summary>
		///     Background north part
		/// </summary>
		public Color BackgroundNorth { get; set; }

		/// <summary>
		///     Background south color
		/// </summary>
		public Color BackgroundSouth { get; set; }

		/// <summary>
		///     Gets ors sets the glossy effect north color
		/// </summary>
		public Color GlossyEffectNorth { get; set; }

		/// <summary>
		///     Gets or sets the glossy effect south color
		/// </summary>
		public Color GlossyEffectSouth { get; set; }
		#endregion
	}
}