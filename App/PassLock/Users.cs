using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Power;
using Windows.UI.Xaml.Media;

namespace PassLock
{
    public class Users
    {




        public string validatePasswords(string password1, string password2)
        {
            Regex rg = new Regex("[^A-Za-z0-9]");
            string output = "";

            else if (rg.Match(password1).Success == false) {

                output = "Password must contain at least one special character!";

            } else
            {
                output = "";
            }

            return output;
        }

    }
}
