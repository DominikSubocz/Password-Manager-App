using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PassLock
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();



            SQL sql = new SQL();

            sql.createDB();
            bool usersExist = sql.usersExist();
            if(usersExist == false)
            {
                SignUpPanel.Visibility = Visibility.Visible;
                LoginTxt.Text = "Create New Account!";

            }
            else
            {
                SignUpPanel.Visibility = Visibility.Collapsed;
            }

        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            Users users = new Users();
            string pass1 = Password1.Password;
            string pass2 = Password2.Password;

            string output = users.validatePasswords(pass1, pass2);
            if (output == "")
            {

                Debug.WriteLine("Password validation successful!");
                ErrTxt.Text = "";

            }
            else
            {
                ErrTxt.Text = output;
            }
        }

        private void Password1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordValidator passVal = new PasswordValidator();
            PasswordStrengthBox.Width = passVal.getPasswordStrength();

        }
    }
}
