using Xamarin.Forms;

namespace BfButton.Helpers
{
    public static class LabelHelper
    {
        public static void UpdateTimerText(int secondsLeft, Label label)
        {
            label.Text = $"{secondsLeft / 60}m:{secondsLeft % 60}s";
        }
    }
}