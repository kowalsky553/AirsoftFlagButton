using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BfButton.Models;
using Xamarin.Forms;

namespace BfButton
{
    public partial class MainPage : ContentPage
    {
        private TeamColor CurrentColor = TeamColor.Neutral;
        private double ContestValue = 0;
        private double CaptureTimeInSeconds = 10;
        private double CaptureIterationTimeInSeconds = 1;
        private bool ShouldStopTimer = false;
        private Dictionary<TeamColor, int> SidesPoints = new Dictionary<TeamColor, int>()
        {
            {TeamColor.Blue, 0},
            {TeamColor.Green, 0},
            {TeamColor.Yellow, 0 }
        };


        public MainPage()
        {
            InitializeComponent();
            //InitializeScoresFromLogs();
        }

        private void YellowButton_Clicked(object sender, EventArgs e) => SetButtonColor(TeamColor.Yellow);

        private void GreenButton_Clicked(object sender, EventArgs e) => SetButtonColor(TeamColor.Green);

        private void BlueButton_Clicked(object sender, EventArgs e) => SetButtonColor(TeamColor.Blue) ;

        private void SetButtonColor(TeamColor teamColor)
        {
            ShouldStopTimer = false;
            if (CurrentColor != TeamColor.Neutral)
            {
                StopContest();
                return;
            }
            if (CurrentColor == TeamColor.Neutral)
            {
                CurrentColor = teamColor;
                Device.StartTimer(TimeSpan.FromSeconds(CaptureIterationTimeInSeconds), () =>
                {
                    if (ShouldStopTimer)
                    {
                        ShouldStopTimer = false;
                        return false;
                    }
                    ContestValue += (CaptureIterationTimeInSeconds / CaptureTimeInSeconds);
                    if (ContestValue >= 1)
                    {
                        UpdateTeamScore(teamColor);
                        StopContest();
                        return false;
                    }
                    CaptureProgressBar.Progress = ContestValue;
                    return true;
                });
            }

        }

        private void StopContest()
        {
            CurrentColor = TeamColor.Neutral;
            CaptureProgressBar.Progress = 0;
            ContestValue = 0;
            ShouldStopTimer = true;
        }

        private string GetLogFileNameFromTeam(TeamColor teamColor)
        {
            switch (teamColor)
            {
                case TeamColor.Yellow:
                    return "Yellow.txt";
                case TeamColor.Blue:
                    return "Blue.txt";
                case TeamColor.Green:
                    return "Green.txt";
                default:
                    return "Empty.txt";
            }
        }

        private Label GetLabelNameFromTeam(TeamColor teamColor)
        {
            switch (teamColor)
            {
                case TeamColor.Yellow:
                    return YellowScoreLabel;
                case TeamColor.Blue:
                    return BlueScoreLabel;
                case TeamColor.Green:
                    return GreenScoreLabel;
                default:
                    throw new Exception("NotSupportedColor");
            }
        }

        private void UpdateTeamScore(TeamColor teamColor)
        {
            SidesPoints[teamColor]++;
            GetLabelNameFromTeam(teamColor).Text = $"Счет: {SidesPoints[teamColor].ToString()}";
           // File.WriteAllText(GetLogFileNameFromTeam(teamColor), SidesPoints[teamColor].ToString());
        }

        private void InitializeScoresFromLogs()
        {
            var sides = new[] { TeamColor.Blue, TeamColor.Green, TeamColor.Yellow };
            foreach (var side in sides)
            {
                try
                {
                    SidesPoints[side] = int.Parse(File.ReadAllText(GetLogFileNameFromTeam(side)));
                    GetLabelNameFromTeam(side).Text = $"Счет: {SidesPoints[side].ToString()}";
                }
                catch (FileNotFoundException)
                {
                }
            }
        }
    }
}
