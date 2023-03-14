using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BfButton.Helpers;
using BfButton.Models;
using Xamarin.Forms;

namespace BfButton.Views
{
    public partial class MainPage : ContentPage
    {
        private TeamColor CurrentColor = TeamColor.Neutral;
        private double ContestValue = 0;
        private double CaptureTimeInSeconds = 5;
        private double CaptureIterationTimeInSeconds = 1;
        private int SecondsLeft;
        private bool ShouldStopTimer = false;
        private Label[] ScoreLabels;

        private Dictionary<TeamColor, int> SidesPoints = new Dictionary<TeamColor, int>()
        {
            {TeamColor.Blue, 0},
            {TeamColor.Green, 0},
            {TeamColor.Yellow, 0}
        };


        public MainPage()
        {
            InitializeComponent();
            InitializeScoresFromLogs();
            ScoreLabels = new Label[] {YellowScoreLabel, BlueScoreLabel, GreenScoreLabel};
        }

        private void YellowButton_Clicked(object sender, EventArgs e) => Button_Clicked(TeamColor.Yellow);

        private void GreenButton_Clicked(object sender, EventArgs e) => Button_Clicked(TeamColor.Green);

        private void BlueButton_Clicked(object sender, EventArgs e) => Button_Clicked(TeamColor.Yellow);

        private void Button_Clicked(TeamColor teamColor)
        {
            if (CurrentColor == TeamColor.Neutral)
            {
                SetButtonColor(teamColor);
            }
            else
            {
                StopContest();
            }
        }

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
                SecondsLeft = (int) CaptureTimeInSeconds;
                TimerLabel.BackgroundColor = ColorHelper.GetColorFromTeamColor(teamColor);
                Device.StartTimer(TimeSpan.FromSeconds(CaptureIterationTimeInSeconds), () =>
                {
                    if (ShouldStopTimer)
                    {
                        ShouldStopTimer = false;
                        return false;
                    }

                    ContestValue += (CaptureIterationTimeInSeconds / CaptureTimeInSeconds);
                    SecondsLeft -= (int) CaptureIterationTimeInSeconds;
                    UpdateTimerText(SecondsLeft);
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
            TimerLabel.BackgroundColor = Color.White;
            UpdateTimerText((int) CaptureTimeInSeconds);
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

        private void UpdateTimerText(int secondsLeft)
        {
            TimerLabel.Text = $"{secondsLeft / 60}m:{secondsLeft % 60}s";
        }

        private void UpdateTeamScore(TeamColor teamColor)
        {
            SidesPoints[teamColor]++;
            GetLabelNameFromTeam(teamColor).Text = $"Счет: {SidesPoints[teamColor].ToString()}";
            FilesHelper.WriteScore(ColorHelper.GetLogFileNameFromTeam(teamColor), SidesPoints[teamColor]);
        }

        private void InitializeScoresFromLogs()
        {
            var sides = new[] {TeamColor.Blue, TeamColor.Green, TeamColor.Yellow};
            foreach (var side in sides)
            {
                try
                {
                    SidesPoints[side] = FilesHelper.ReadScore(ColorHelper.GetLogFileNameFromTeam(side));
                    GetLabelNameFromTeam(side).Text = $"Счет: {SidesPoints[side].ToString()}";
                }
                catch (FileNotFoundException)
                {
                }
            }
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            if (DestroyPointEntry.Text.ToLower() == "kill")
            {
                StopContest();
                App.Current.MainPage = new DestroyPointPage();
                DestroyPointEntry.Text = "";
            }
        }
    }
}