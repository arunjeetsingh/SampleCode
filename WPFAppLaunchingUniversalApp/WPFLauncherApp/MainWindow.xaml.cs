using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.System;

namespace WPFLauncherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Uri uri = new Uri("com.aruntalkstech.universaltarget:DoSomething?With=This");
        const string TargetPackageFamilyName = "06c4afd0-0d59-4cef-a927-39fc6c4a9f5c_876gvmnfevegr";

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnActivated(EventArgs e)
        {
            //Test whether the app we want to launch is installed
            var supportStatus = await Launcher.QueryUriSupportAsync(uri, LaunchQuerySupportType.Uri, TargetPackageFamilyName);
            if (supportStatus != LaunchQuerySupportStatus.Available)
            {
                Status.Text = "Can't launch com.aruntalkstech.universaltarget: because the app we need is " + supportStatus.ToString();
            }
        }

        private async void LaunchTargetApp_Click(object sender, RoutedEventArgs e)
        {
            var options = new LauncherOptions { TargetApplicationPackageFamilyName = TargetPackageFamilyName };
            bool success = await Launcher.LaunchUriAsync(uri, options);
            Debug.WriteLine(success);
        }
    }
}
