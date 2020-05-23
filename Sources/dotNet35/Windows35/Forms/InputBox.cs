#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Microsoft.VisualBasic;

namespace Library35.Windows.Forms
{
	public class InputBox
	{
		public static string Ask(string prompt, string title = "", string defaultResponse = "", int xPos = -1, int yPos = -1)
		{
			return Interaction.InputBox(prompt, title, defaultResponse, xPos, yPos);
		}
	}
}