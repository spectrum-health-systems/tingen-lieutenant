// ================================================================= 0.1.0 =====
// Tingen Lieutenant: A workstation GUI for the Tingen web service.
// Repository: https://github.com/spectrum-health-systems/Tingen-Commander
// Documentation: https://github.com/spectrum-health-systems/Tingen-Documentation
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 250128 =====

// u250128_code
// u250128_documentation

using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;

using TingenLieutenant.Du;

namespace TingenLieutenant
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        public Session session { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            /* The path to the configuration file is hard coded, and should not be modified.
               */
            Configuration config = Configuration.Load(@"./AppData/TngnLtnt.settings");

            //Session session = Session.Create(config.ServerUnc, config.ServiceDataRoot);

            session = Session.Create(config.ServerUnc, config.ServiceDataRoot);

            VerifyComponents(session);

            var serviceDetail = ServiceDetails.Load(session.ServiceDataRoot);

            if (serviceDetail.ServiceMode.Equals("enabled", StringComparison.CurrentCultureIgnoreCase))
            {
                SetLabelProperties(lblStatus, "Enabled", Brushes.LightGreen, Brushes.White);
            }
            else if (serviceDetail.ServiceMode.Equals("disabled", StringComparison.CurrentCultureIgnoreCase))
            {
                SetLabelProperties(lblStatus, "Disabled", Brushes.LightCoral, Brushes.White);
            }
            else
            {
                SetLabelProperties(lblStatus, "Unknown", Brushes.LightGray, Brushes.Black);
            }

            lblVersion.Content = serviceDetail.ServiceVersion;
            lblBuild.Content   = serviceDetail.ServiceBuild;
            lblUpdated.Content = serviceDetail.ServiceUpdated;

        }

        internal static void VerifyComponents(Session session)
        {
            Verify.ServerAccess(session.ServiceDataRoot);
            Verify.ServiceDetailsFile(session.ServiceDataRoot);
            Verify.SessionRoot(session.SessionDataRoot);
        }

        internal void SetLabelProperties(System.Windows.Controls.Label label, string content, Brush background, Brush foreground)
        {
            label.Content    = content;
            label.Background = background;
            label.Foreground = foreground;
        }

        private void btnDocumentation_Click(object sender, RoutedEventArgs e)
        {
            DuInternet.OpenUrl("https://github.com/spectrum-health-systems/Tingen-Documentation");
        }

        private void btnAlerts_Click(object sender, RoutedEventArgs e)
        {
            //var t = Directory.GetFiles(@"\\SHS-AZU-NSWS-01\TingenData\");
            var t = Directory.GetFiles(session.ServiceDataRoot);


            var u=8;
        }

        private void btnAlerts_Click_1(object sender, RoutedEventArgs e)
        {
            DuExplorer.OpenFolder($@"{session.ServiceDataRoot}\LIVE\Messages\Alerts");
        }
    }
}