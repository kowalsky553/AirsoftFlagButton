using System;
using System.Linq;
using BfButton.Helpers;
using BfButton.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BfButton
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DestroyPointPage : ContentPage
    {
        private double DestroyTimeInSeconds = 5;
        private double DestroyIterationTimeInSeconds = 1;
        private int TimeLeftInSeconds = 5;
        private bool IsTimerRunning = false;
        private string Password;
        private bool ShouldStopTimer = false;
        private bool ShouldGoToMain = false;

        public DestroyPointPage()
        {
            InitializeComponent();
            LabelHelper.UpdateTimerText((int)DestroyTimeInSeconds, TimerLabel);
        }

        private void DestroyButton_OnClicked(object sender, EventArgs e)
        {
            if (ShouldGoToMain)
            {
                App.Current.MainPage = new MainPage();
            }

            if (DestroyPointEntry.Text.Length != 8)
            {
                return;
            }

            if (IsTimerRunning)
            {
                var defuseResults = TryDefuse(Password, DestroyPointEntry.Text);
                if (defuseResults.isDefused)
                {
                    ShouldStopTimer = true;
                    ShouldGoToMain = true;
                    DestroyPointEntry.Text = "Точка обезврежена";
                    DestroyButton.Text = "Назад";
                    Content.BackgroundColor = Color.Green;
                }
                else
                {
                    DestroyPointEntry.Text = defuseResults.updatePassword;

                }
            }
            else
            {


                DestroyPointEntry.Placeholder = "Пароль для дефуза";
                if (DestroyPointEntry.Text.Length == 8 && DestroyPointEntry.Text.All(c => char.IsDigit(c)))
                {
                    Password = DestroyPointEntry.Text;
                    DestroyPointEntry.Text = "";
                    DestroyProgressBar.Progress = 1;
                    TimeLeftInSeconds = (int)DestroyTimeInSeconds;
                    IsTimerRunning = true;
                    Device.StartTimer(TimeSpan.FromSeconds(DestroyIterationTimeInSeconds), () =>
                    {
                        if (ShouldStopTimer)
                        {
                            return false;
                        }

                        DestroyProgressBar.Progress -= (DestroyIterationTimeInSeconds / DestroyTimeInSeconds);
                        TimeLeftInSeconds -= (int)DestroyIterationTimeInSeconds;
                        LabelHelper.UpdateTimerText(TimeLeftInSeconds, TimerLabel);
                        if (DestroyProgressBar.Progress <= 0)
                        {
                            FilesHelper.ClearAllScore();
                            Content.BackgroundColor = Color.Red;
                            ShouldGoToMain = true;
                            DestroyPointEntry.Text = "Точка уничтожена";
                            DestroyButton.Text = "Go back";
                            return false;
                        }

                        return true;
                    });
                }
            }
        }

        private (string updatePassword, bool isDefused) TryDefuse(string realPassword, string testPassword)
        {
            var resultString = string.Empty;
            if (realPassword == testPassword)
            {
                return (string.Empty, true);
            }

            for (int i = 0; i < realPassword.Length; i++)
            {
                var nextChar = realPassword[i] == testPassword[i] ? testPassword[i] : '*';
                resultString = $"{resultString}{nextChar}";
            }

            return (resultString, false);
        }
    }
}