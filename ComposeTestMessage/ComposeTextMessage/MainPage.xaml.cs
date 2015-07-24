using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Chat;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ComposeTextMessage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ComposeMessage_Click(object sender, RoutedEventArgs e)
        {
            //Check if the current device has any transports that can deliver
            //a text message. Note that calling this API requires the chat
            //capability (see Package.appxmanifest)
            var transports = await ChatMessageManager.GetTransportsAsync();

            //If there are transports available, call ShowComposeSmsMessageAsync
            if (transports.Any())
            {
                var textMessage = new ChatMessage();
                textMessage.Body = Message.Text;
                await ChatMessageManager.ShowComposeSmsMessageAsync(textMessage);
            }
        }
    }
}
