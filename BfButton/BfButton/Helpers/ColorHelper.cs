using System;
using BfButton.Models;
using Xamarin.Forms;

namespace BfButton.Helpers
{
    public class ColorHelper
    {
        
        private static string YellowFileName = "Yellow.txt";
        private static string GreenFileName = "Green.txt";
        private static string BlueFileName = "Blue.txt";
        
        public static Color GetColorFromTeamColor(TeamColor teamColor)
        {
            switch (teamColor)
            {
                case TeamColor.Blue:
                    return Color.DodgerBlue;
                case TeamColor.Green:
                    return Color.LawnGreen;
                case TeamColor.Yellow:
                    return Color.Yellow;
                default:
                    return Color.White;
            }
        }


        public static string GetLogFileNameFromTeam(TeamColor teamColor)
        {
            switch (teamColor)
            {
                case TeamColor.Yellow:
                    return YellowFileName;
                case TeamColor.Blue:
                    return BlueFileName;
                case TeamColor.Green:
                    return GreenFileName;
                default:
                    return "Empty.txt";
            }
        }
    }
}