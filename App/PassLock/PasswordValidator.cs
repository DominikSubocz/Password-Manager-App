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

        public string validatePasswords(string password1, string password2)
        {


            // Create a Regex object with the pattern
            Regex upperCaseRegex = new Regex(@"(?=.*[A-Z])");
            Regex digitPattern = new Regex(@"(?=.*\d)");
            Regex specialCharPattern = new Regex(@"(?=.*[^\da-zA-Z])");
            int maxDistance = 2;
            int byteSize = 0;
            int length = password1.Length;
            int charsetSize = 94;
            int repetitionCount = 0;
            bool hasRepetition = false;

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

            else if (length <8)
            {
                output = "Password needs to be at least 8 characters long";

            } else
            {
                foreach (string commonPassword in commonPasswords)
                {
                    if (password1 == commonPassword)
                    {
                    } else
                    {
                        int similarityDistance = Compute(password1, commonPassword);
                        if( similarityDistance <= maxDistance)
                        {
                            output = "Weak password, similarity between common password too high!";
                        }
                        else
                        {
                            if (password1.Contains(commonPassword) || (password1.Contains(commonPassword.ToLower())))
                            {
                                output = "Your password contains a phrase or sequence commonly used in passwords. This makes it more vulnerable to attacks. Please choose a more unique combination.";

                            } else
                            {
                                output = "";
                            }
                        }
                        
                    }
                }

                double charsetResult = length * (Math.Log(charsetSize) / Math.Log(2));
                passwordStrength = Convert.ToInt32(charsetResult);

                foreach (char c in password1)
                {
                    if (char.IsUpper(c))
                    {
                        passwordStrength += 10;
                    }

                    if(char.IsDigit(c))
                    {
                        passwordStrength += 10;
                    }


                }

                hasRepetition = Regex.IsMatch(password1, @"(\w)\1{2,}");


                if (length < 30)
                {
                    if (hasRepetition)
                    {
                        passwordStrength -= 75;
                        output = "Sequential patterns in your password make it easier to guess. Please use a more random sequence.";
                    }
                } else
                {
                    if (hasRepetition)
                    {
                        passwordStrength -= 25;
                    }
                }






                Debug.WriteLine("Strength: " + passwordStrength);
                Debug.WriteLine(password1);
            }

            Debug.WriteLine(byteSize);
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
