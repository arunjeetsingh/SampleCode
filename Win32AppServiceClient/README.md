# Win32 AppService Client

This sample demostrates a Windows Presentation Foundation (WPF) app calling a
universal app service. App services are a new feature of universal Windows 10
apps that allow an app to provide services to other apps. These services can
be accessed by other apps as well as Win32 apps such as WPF or WinForms apps.

This sample demonstrates how a WPF app can call into an app service. To run
the sample you will need Visual Studio 2015 and the Windows SDK. Both of these
can be found at <http://developer.windows.com>.
	
With Visual Studio, open the solution Win32AppServiceClient.sln. There are three
projects in this solution:
* **RandomNumberService** - This project is a Windows Runtime component. It contains
the code for the background task that implements the app service we use for the
sample. RandomNumberGeneratorTask.cs contains the background task class. The 
background tasks' main entrypoint is the Run method. In this method, the 
RandomNumberGeneratorTask sets itself up with a deferral so it can stay running 
and provide the app service. It also uses the incoming AppServiceTriggerDetails
to attach an event handler to the AppServiceTriggerDetails.RequestReceived event.
This enables the RandomNumberGeneratprTask to respond to requests from the app
service client. The OnRequestReceived handler simply parses the input for minimum
and maximum integer values. It then generates a random number between those values
and returns it.

* **AppServicesProvider** - AppServicesProvider is the universal Windows app that
contains RandomNumberService. The AppServicesProvider project contains a reference to 
**RandomNumberService**. The Package.appxmanifest file in AppServicesProvider also 
contains a windows.appService extension. This is how the AppServicesProvider app 
exposes the RandomNumberService to other apps. In the Package.appxmanifest's uap:Extension 
element the EntryPoint attribute represents the type name of the background
task providing the app service. The Name attribute in the uap:AppService element
represents the name of the app service.

* **Win32AppServiceClient** - The Win32AppServicesClient is a WPF application. The
project file of this WPF application (Win32AppServiceClient.csproj) has been modified
to include references to the universal Windows 10 APIs. This enables us to use the 
Windows.ApplicationModel.AppService.AppServiceConnection class that lets an app
connect to an app service. The MainWindow.xaml page of Win32AppServiceClient does 
exactly that. It presents the user the option to provide minimum and maximum integer
values. The Click handler of the *Generate Random Number* button then calls the 
random number service exposed by AppServicesProvider with these values. The result
is then displayed on screen. 

Feel free to submit pull requests on this README.md or the code if you think it could
use improvement. Happy hacking!

[@aruntalkstech](https://twitter.com/aruntalkstech)