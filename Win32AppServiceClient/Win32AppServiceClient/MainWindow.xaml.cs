using System;
using System.Collections.Generic;
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
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace Win32AppServiceClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GenerateRandomNumber_Click(object sender, RoutedEventArgs e)
        {
            //Parse user input
            int minValueInput = 0;
            bool valueParsed = int.TryParse(MinValue.Text, out minValueInput);
            if (!valueParsed)
            {
                NotifyUser("The Minimum Value should be a valid integer", NotifyType.ErrorMessage);
                return;
            }

            int maxValueInput = 0;
            valueParsed = int.TryParse(MaxValue.Text, out maxValueInput);
            if (!valueParsed)
            {
                NotifyUser("The Maximum Value should be a valid integer", NotifyType.ErrorMessage);
                return;
            }

            if (maxValueInput <= minValueInput)
            {
                NotifyUser("Maximum Value must be larger than Minimum Value", NotifyType.ErrorMessage);
                return;
            }

            using (var connection = new AppServiceConnection())
            {
                //Set up a new app service connection
                connection.AppServiceName = "com.microsoft.randomnumbergenerator";
                connection.PackageFamilyName = "Microsoft.SDKSamples.AppServicesProvider.CS_876gvmnfevegr";

                AppServiceConnectionStatus status = await connection.OpenAsync();

                //The new connection opened successfully
                if (status == AppServiceConnectionStatus.Success)
                {
                    NotifyUser("Connection established", NotifyType.StatusMessage);
                }

                //If something went wrong. Lets figure out what it was and show the 
                //user a meaningful message and walk away
                switch (status)
                {
                    case AppServiceConnectionStatus.AppNotInstalled:
                        NotifyUser("The app AppServicesProvider is not installed. Deploy AppServicesProvider to this device and try again.", NotifyType.ErrorMessage);
                        return;

                    case AppServiceConnectionStatus.AppUnavailable:
                        NotifyUser("The app AppServicesProvider is not available. This could be because it is currently being updated or was installed to a removable device that is no longer available.", NotifyType.ErrorMessage);
                        return;

                    case AppServiceConnectionStatus.AppServiceUnavailable:
                        NotifyUser(string.Format("The app AppServicesProvider is installed but it does not provide the app service {0}.", connection.AppServiceName), NotifyType.ErrorMessage);
                        return;

                    case AppServiceConnectionStatus.Unknown:
                        NotifyUser("An unkown error occurred while we were trying to open an AppServiceConnection.", NotifyType.ErrorMessage);
                        return;
                }

                //Set up the inputs and send a message to the service
                var inputs = new ValueSet();
                inputs.Add("minvalue", minValueInput);
                inputs.Add("maxvalue", maxValueInput);
                AppServiceResponse response = await connection.SendMessageAsync(inputs);

                //If the service responded with success display the result and walk away
                if (response.Status == AppServiceResponseStatus.Success &&
                    response.Message.ContainsKey("result"))
                {
                    var resultText = response.Message["result"].ToString();
                    if (!string.IsNullOrEmpty(resultText))
                    {
                        Result.Text = resultText;
                        NotifyUser("App service responded with a result", NotifyType.StatusMessage);
                    }
                    else
                    {
                        NotifyUser("App service did not respond with a result", NotifyType.ErrorMessage);
                    }

                    return;
                }

                //Something went wrong while sending a message. Let display
                //a meaningful error message
                switch (response.Status)
                {
                    case AppServiceResponseStatus.Failure:
                        NotifyUser("The service failed to acknowledge the message we sent it. It may have been terminated or it's RequestReceived handler might not be handling incoming messages correctly.", NotifyType.ErrorMessage);
                        return;

                    case AppServiceResponseStatus.ResourceLimitsExceeded:
                        NotifyUser("The service exceeded the resources allocated to it and had to be terminated.", NotifyType.ErrorMessage);
                        return;

                    case AppServiceResponseStatus.Unknown:
                        NotifyUser("An unkown error occurred while we were trying to send a message to the service.", NotifyType.ErrorMessage);
                        return;
                }
            }
        }

        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Colors.Red);
                    break;
            }

            StatusBlock.Text = strMessage;
        }
    }

    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}
