// ================================================================ v0.1.0 =====
// Tingen Lieutenant: A workstation GUI for the Tingen web service.
// Repository: https://github.com/spectrum-health-systems/Tingen-Lieutenant
// Documentation: https://github.com/spectrum-health-systems/Tingen-Documentation
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// =============================================================== b250129 =====

// u250129_code
// u250129_documentation

using System.IO;
using System.Windows;
using System.Windows.Media;

using TingenLieutenant.Du;

namespace TingenLieutenant
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        //-- Defined here so it can be used everywhere.
        public Session session { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            session = Session.Create();















            ///* The path to the configuration file is hard coded, and should not be modified.
            //   */
            //LieutenantConfig ltntConfig = LieutenantConfig.Load($@"{commonPaths["ltntDataRoot"]}/{commonNames["ltntConfigFile"]}");
            //session = Session.Create(ltntConfig.ServerUnc, ltntConfig.ServiceDataRoot);




            VerifyComponents(session);

            var serviceDetail = ServiceDetails.Load(session.TngnDataRoot);

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
            Verify.ServerAccess(session.TngnDataRoot);
            Verify.TingenConfigurationFile(session.TngnDataRoot);
            Verify.SessionRoot(session.CurrentSessionDataRoot);
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
            var t = Directory.GetFiles(session.TngnDataRoot);


            var u=8;
        }

        private void btnAlerts_Click_1(object sender, RoutedEventArgs e)
        {
            DuExplorer.OpenFolder($@"{session.TngnDataRoot}\LIVE\Messages\Alerts");
        }



    }
}