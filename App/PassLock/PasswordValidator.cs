using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace PassLock
{
    public class PasswordValidator
    {

        private int passwordStrength = 0;
        private SolidColorBrush strengthBoxColor = new SolidColorBrush(Windows.UI.Colors.Red);
        private string output = "";

        public void validatePasswords(string password1, string password2)
        {
            validateLength(password1);
            validatePasswordComplexity(password1);
            validatePasswordsMatch(password1,password2);
        }
        public int getPasswordStrength() { return passwordStrength; }

        public string getOutput() { return output; }

        public void validateLength(string password)
        {
            if ((password.Length >= 8) && (password.Length < 12))
            {

                passwordStrength = 60;

            }
            else if ((password.Length >= 12) && (password.Length < 15))
            {

                passwordStrength = 65;

            }
            else if ((password.Length >= 15) && (password.Length < 21))
            {
                passwordStrength = 70;


            }
            else if (password.Length >= 20)
            {
                passwordStrength = 75;


            }
        }

        public void validatePasswordComplexity(string password)
        {
            foreach (char c in password) {
            
                if(char.IsUpper(c))
                {
                    passwordStrength += 10;
                }

                if (char.IsDigit(c))
                {
                    passwordStrength += 10;
                }

                if (char.IsSymbol(c))
                {
                    passwordStrength += 10;
                }
            }
        }

        public void validatePasswordsMatch(string password1, string password2)
        {
            if (password1 != password2)
            {
                output = "Passwords do not match!";
            }
        }


    }
}
