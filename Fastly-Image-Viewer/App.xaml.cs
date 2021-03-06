﻿using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Fastly_Image_Viewer
{
    public partial class App : Application, ISingleInstanceApp
    {
        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance("Fastly_Image_Viewer"))
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();

                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            MainWindow mainWindow = this.MainWindow as MainWindow;

            if (args.Count > 1)
                mainWindow.OpenImage(args[1]);
                
            if (!mainWindow.IsActive)
                mainWindow.Show();

            return true;
        }
    }
}
