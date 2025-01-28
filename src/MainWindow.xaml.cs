// ================================================================= 0.1.0 =====
// Tingen Lieutenant: A workstation GUI for the Tingen web service.
// Repository: https://github.com/spectrum-health-systems/Tingen-Commander
// Documentation: https://github.com/spectrum-health-systems/Tingen-Documentation
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 250128 =====

// u250128_code
// u250128_documentation

using System.Windows;

namespace TingenLieutenant
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Session ltntSession = Session.Create();

            Verify.ServerAccess($@"{ltntSession.TngnServiceDataRoot}\{ltntSession.LtntVerificationFile}");
        }
    }
}