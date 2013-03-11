//Abhishek Ravi
//Inian Parameshwaran

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ManagedWinapi;

namespace F9S1.RememberMe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Hotkey hotkey1;
        protected override void OnStartup(StartupEventArgs e)
        {
            hotkey1 = new ManagedWinapi.Hotkey();
            hotkey1.Ctrl = true;
            hotkey1.Shift = true;
            hotkey1.KeyCode = System.Windows.Forms.Keys.OemSemicolon;
            hotkey1.HotkeyPressed += new EventHandler(hotkey_HotkeyPressed);
            try
            {
                hotkey1.Enabled = true;
            }
            catch (ManagedWinapi.HotkeyAlreadyInUseException)
            {
                System.Windows.MessageBox.Show("Could not register hotkey (already in use).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            base.OnStartup(e);
        }

        void hotkey_HotkeyPressed(object sender, EventArgs e)
        {
            try
            {
                var main = App.Current.MainWindow as MainWindow;
                bool isActive = false;
                foreach (var wnd in Application.Current.Windows.OfType<Window>())
                    if (wnd.IsActive) isActive = true;
                if (main.WindowState == WindowState.Normal && isActive)
                    main.WindowState = WindowState.Minimized;
                else
                {
                    main.Show();
                    main.WindowState = WindowState.Normal;
                    main.Activate();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            hotkey1.Dispose();
            base.OnExit(e);
        }
    }
}
