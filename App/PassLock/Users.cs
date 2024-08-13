using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Power;

namespace PassLock
{
    public class Users
    {
        public string validatePasswords(string password1, string password2)
        {
            Regex rg = new Regex("[^A-Za-z0-9]");
            string output = "";

            if (password1 != password2)
            {
                output = "Passwords do not match! Both password must be the same!";

            } else if (password1.Length < 12) {

                output = "Password must be at least 12 characters long!";

            } else if(rg.Match(password1).Success == false) {

                output = "Password must contain at least one special character!";

            } else
            {
                output = "";
            }

            return output;
        }
    }
}
