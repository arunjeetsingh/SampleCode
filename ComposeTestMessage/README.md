# Compose Test Message 

This sample demonstrates how a Windows 10 app can use the 
ChatMesageManager.ShowComposeSmsMessageAsync API safely. By checking for the
availability of transports we ensure [ShowComposeSmsMessageAsync](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.applicationmodel.chat.chatmessagemanager.showcomposesmsmessageasync.aspx) is only called
if text message support is available on the current device. This means that
the code also works great on desktop where SMS messaging is not available.
More details can be found here: [ChatMessageManager.ShowComposeSmsMessageAsync 
Crashes on Windows 10 Desktop](https://aruntalkstech.wordpress.com/2015/07/24/chatmessagemanager-showcomposesmsmessageasync-crashes-on-windows-10-desktop/)

	
## Windows 10 Samples

Unless otherwise noted, most of these code samples target Windows 10 and are
universal. That is, they can be run on any device that runs Windows 10. To
run these samples, you will need Visual Studio 2015 and the Windows 10 Software
Development Kit (SDK). These can be found at <http://developer.windows.com>.

If you'd like a free copy of Windows 10 please become a 
[Windows Insider](http://insider.windows.com) 

Happy hacking!

[@aruntalkstech](http://www.twitter.com/aruntalkstech)