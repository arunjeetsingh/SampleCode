using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace WPFProtocolHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> inputs = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();

            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                Uri argUri;
                if (Uri.TryCreate(args[1], UriKind.Absolute, out argUri))
                {
                    var decoder = new WwwFormUrlDecoder(argUri.Query);
                    if (decoder.Any())
                    {
                        InputUrI.Text = string.Empty;

                        foreach (var entry in decoder)
                        {
                            InputUrI.Text += entry.Name + "=" + entry.Value + ",";
                            inputs[entry.Name] = entry.Value;
                        }

                        InputUrI.Text = InputUrI.Text.Remove(InputUrI.Text.Length - 1);
                    }
                }
            }
        }

        private void InstallProtocol_Click(object sender, RoutedEventArgs e)
        {
            using (var hkcr = Registry.ClassesRoot)
            {
                if (hkcr.GetSubKeyNames().Contains(SchemeName.Text))
                {
                    MessageBox.Show(string.Format("Looks like {0} is already installed.", SchemeName.Text));
                    return;
                }

                using (var schemeKey = hkcr.CreateSubKey(SchemeName.Text))
                {
                    //[HKEY_CLASSES_ROOT\com.aruntalkstech.wpftarget]
                    //@="Url:WPF Target Protocol"
                    //"URL Protocol"=""
                    //"UseOriginalUrlEncoding"=dword:00000001
                    schemeKey.SetValue(string.Empty, "Url: WPF Target Protocol");
                    schemeKey.SetValue("URL Protocol", string.Empty);
                    schemeKey.SetValue("UseOriginalUrlEncoding", 1, RegistryValueKind.DWord);

                    //[HKEY_CLASSES_ROOT\com.aruntalkstech.wpf\shell]
                    using (var shellKey = schemeKey.CreateSubKey("shell"))
                    {
                        //[HKEY_CLASSES_ROOT\com.aruntalkstech.wpf\shell\open]
                        using (var openKey = shellKey.CreateSubKey("open"))
                        {
                            //[HKEY_CLASSES_ROOT\com.aruntalkstech.wpf\shell\open\command]
                            using (var commandKey = openKey.CreateSubKey("command"))
                            {
                                //@="C:\\github\\SampleCode\\UniversalAppLaunchingWPFApp\\WPFProtocolHandler\\bin\\Debug\\WPFProtocolHandler.exe \"%1\""
                                commandKey.SetValue(string.Empty, Assembly.GetExecutingAssembly().Location + " %1");
                                commandKey.Close();
                            }
                            openKey.Close();
                        }
                        shellKey.Close();
                    }
                    schemeKey.Close();
                }
                hkcr.Close();
            }

            MessageBox.Show(string.Format("Custom scheme {0}: installed.", SchemeName.Text));
        }

        private void UninstallProtocol_Click(object sender, RoutedEventArgs e)
        {
            using (var hkcr = Registry.ClassesRoot)
            {
                if (!hkcr.GetSubKeyNames().Contains(SchemeName.Text))
                {
                    MessageBox.Show(string.Format("Looks like {0} is not installed.", SchemeName.Text));
                    return;
                }

                hkcr.DeleteSubKeyTree(SchemeName.Text);                
                hkcr.Close();
            }

            MessageBox.Show(string.Format("Custom scheme {0}: uninstalled.", SchemeName.Text));
        }
    }
}
