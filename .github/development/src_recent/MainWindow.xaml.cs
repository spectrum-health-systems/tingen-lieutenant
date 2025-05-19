/* ================================================================ v0.1.0 =====
 * Tingen Lieutenant: A workstation GUI for the Tingen web service.
 * Repository: https://github.com/spectrum-health-systems/Tingen-Lieutenant
 * Documentation: https://github.com/spectrum-health-systems/Tingen-Documentation
 * Copyright (c) A Pretty Cool Program. All rights reserved.
 * Licensed under the Apache 2.0 license.
 * =============================================================== b250130 =====
 */

/* Please see the TingenLieutenant_README.md file for more information.
*/

// u250130_code
// u250130_documentation
// u250130_xmldocumentation

using System.IO;
using System.Windows;
using System.Windows.Media;

using TingenLieutenant.Du;
using TingenLieutenant.Session;

namespace TingenLieutenant
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        //-- Defined here so it can be used everywhere.
        public LieutenantSession LtntSession { get; set; }

        /// <summary>Comment.</summary>
        public MainWindow()
        {
            InitializeComponent();

            LtntSession = LieutenantSession.Create();















            ///* The path to the configuration file is hard coded, and should not be modified.
            //   */
            //LieutenantConfig ltntConfig = LieutenantConfig.Load($@"{commonPaths["ltntDataRoot"]}/{commonNames["ltntConfigFile"]}");
            //session = Session.Create(ltntConfig.ServerUnc, ltntConfig.ServiceDataRoot);




            VerifyComponents(LtntSession);

            var serviceDetail = ServiceDetails.Load(LtntSession.TngnDataRoot);

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

        internal static void VerifyComponents(LieutenantSession session)
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
            var t = Directory.GetFiles(LtntSession.TngnDataRoot);


            var u=8;
        }

        private void btnAlerts_Click_1(object sender, RoutedEventArgs e)
        {
            DuWindowsExplorer.OpenFolder($@"{LtntSession.TngnDataRoot}\LIVE\Messages\Alerts");
        }



    }
}