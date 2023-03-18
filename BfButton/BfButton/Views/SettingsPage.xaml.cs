using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BfButton.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BfButton.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            var settings = FilesHelper.ReadConfig();
            TimeToCaptureEntry.Text = settings.timeToCaprure.ToString();
            TimeToDestroyEntry.Text = settings.timeToDestroy.ToString();
        }

        private void ReadConfigs()
        {
            TimeToCaptureEntry.Text = FilesHelper.ReadScore(Constants.CaptureTimeSettingsFile).ToString();
            TimeToDestroyEntry.Text = FilesHelper.ReadScore(Constants.DestroyTimeSettingsFile).ToString();
        }

        private void SaveButton_OnClicked(object sender, EventArgs e)
        {
            FilesHelper.WriteScore(Constants.CaptureTimeSettingsFile, int.Parse(TimeToCaptureEntry.Text));
            FilesHelper.WriteScore(Constants.DestroyTimeSettingsFile, int.Parse(TimeToDestroyEntry.Text));
            App.Current.MainPage = new MainPage();
        }
    }
}