using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace PassLock
{
    public class PasswordValidator
    {

        private int passwordStrength = 0;
        private SolidColorBrush strengthBoxColor = new SolidColorBrush(Windows.UI.Colors.Red);
        private string output = "";
        private readonly List<string> commonPasswords;

        public PasswordValidator()
        {
            commonPasswords = new List<string>(File.ReadAllLines("10-million-password-list-top-1000000.txt"));
        }

        public string returnOutputMsg() { return output; }

        public string validatePasswords(string password1, string password2)
        {


            // Create a Regex object with the pattern
            Regex upperCaseRegex = new Regex(@"(?=.*[A-Z])");
            Regex digitPattern = new Regex(@"(?=.*\d)");
            Regex specialCharPattern = new Regex(@"(?=.*[^\da-zA-Z])");
            int maxDistance = 2;

            // Return true if the password matches the pattern, otherwise false
            if (upperCaseRegex.IsMatch(password1) == false)
            {
                output = "At least one capital letter is required.";
            }

            else if(digitPattern.IsMatch(password1) == false)
            {
                output = "At least one digit is required.";

            }

            else if (specialCharPattern.IsMatch(password1) == false)
            {
                output = "At least one special character is required.";

            }

            else if (password1.Length <8)
            {
                output = "Password needs to be at least 8 characters long";

            } else
            {
                foreach (string commonPassword in commonPasswords)
                {
                    if (password1 == commonPassword)
                    {
                        Debug.WriteLine("Weak password!");
                    } else
                    {
                        int similarityDistance = Compute(password1, commonPassword);
                        if( similarityDistance <= maxDistance)
                        {
                            output = "Weak password, similarity between common password too high!";
                        }
                    }
                }


            }


            return output;
        }

        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public int getPasswordStrength() { return passwordStrength; }

        public void validatePasswordsMatch(string password1, string password2)
        {
            if (password1 != password2)
            {
                output = "Passwords do not match!";
            }
        }


    }
}
