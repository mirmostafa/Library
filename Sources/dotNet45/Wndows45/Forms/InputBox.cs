using Microsoft.VisualBasic;

namespace Mohammad.Win.Forms
{
    public class InputBox
    {
        public static string Ask(string prompt, string title = "", string defaultResponse = "", int xPos = -1, int yPos = -1)
        {
            return Interaction.InputBox(prompt, title, defaultResponse, xPos, yPos);
        }
    }
}