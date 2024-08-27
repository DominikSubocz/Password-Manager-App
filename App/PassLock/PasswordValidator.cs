using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace PassLock
{




    public class PasswordValidator
    {

        private double passwordStrength = 0;
        private SolidColorBrush strengthBoxColor = new SolidColorBrush(Windows.UI.Colors.Red);
        private string output = "";
        private readonly List<string> commonPasswords;
        private readonly List<string> firstNames;
        private int basePenalty = 8;




        public PasswordValidator()
        {
            commonPasswords = new List<string>(File.ReadAllLines("10-million-password-list-top-1000000.txt"));
            firstNames = new List<string>(File.ReadAllLines("first-names.txt"));
        }

        public string validatePasswords(string password1, string password2)
        {

            int charsetSize = 94;
            int length = password1.Length;
            bool hasRepetition = false;


            double charsetResult = length * (Math.Log(charsetSize) / Math.Log(2));
            passwordStrength = Convert.ToInt32(charsetResult);

            string complexityOutput = checkPasswordComplexity(password1, length);
            string commonOutput = checkCommonPasswords(password1);
            //string nameOutput = checkCommonNames(password1, length);

            if (complexityOutput != "")
            {
                output = complexityOutput;
            }
            else
            {

                if (commonOutput != "")
                {
                    output = commonOutput;
                }
                //else
                //{
                //    if (nameOutput != "")
                //    {
                //        output = nameOutput;
                //    }
                //}
            }

            Debug.WriteLine(password1 + "is: " + passwordStrength + " points strong");
            Debug.WriteLine(basePenalty);
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

        public double getPasswordStrength()
        {

            return passwordStrength;

        }


        public SolidColorBrush getBarColor()
        {
            if (passwordStrength > 0)
            {
                if (passwordStrength > 100)
                {
                    strengthBoxColor = new SolidColorBrush(Windows.UI.Colors.Orange);
                }

                if (passwordStrength > 200)
                {
                    strengthBoxColor = new SolidColorBrush(Windows.UI.Colors.Yellow);

                }

                if (passwordStrength > 300)
                {
                    strengthBoxColor = new SolidColorBrush(Windows.UI.Colors.Green);
                }

                if (passwordStrength > 400)
                {
                    strengthBoxColor = new SolidColorBrush(Windows.UI.Colors.LightGreen);

                }

            }

            return strengthBoxColor;

        }

        public string validatePasswordsMatch(string password1, string password2)
        {
            string message = "";

            if (password1 != password2)
            {
                message = "Passwords do not match!";
            }

            return message;
        }

        public string checkPasswordComplexity(string password, int length)
        {

            bool hasRepetition = false;
            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigits = false;
            bool hasSymbols = false;
            double lengthFactor = length / 20.0;


            string message = "";

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpper = true;
                    passwordStrength += 12;
                }

                if (char.IsDigit(c))
                {
                    hasDigits = true;
                    passwordStrength += 25;
                }

                if (char.IsLower(c))
                {
                    hasLower = true;
                    passwordStrength += 8;
                }

                if (char.IsSymbol(c))
                {
                    hasSymbols = true;
                    passwordStrength += 16;

                }

                if (char.IsPunctuation(c))
                {
                    hasSymbols = true;
                    passwordStrength += 16;
                }
            }

            hasRepetition = Regex.IsMatch(password, @"(\w)\1{2,}");

            if (hasRepetition)
            {
                basePenalty = 10;
                message = "Your password contains repeated characters in a pattern that could weaken its security. Try a more random combination.";

            }
            else
            {
                basePenalty = 2;
                message = "";

                if (!hasUpper)
                {
                    message = "Your password must include at least one uppercase letter.";

                }
                else if (!hasLower)
                {
                    message = "Your password could be stronger by including lowercase letters. Consider adding a mix of uppercase, lowercase, numbers, and symbols to enhance your security.";
                }
                else if (!hasDigits)
                {
                    message = "Your password must contain at least one digit (0-9).";
                }
                else if (!hasSymbols)
                {
                    message = "Your password must include at least one special character (e.g., !, @, #, $).";

                }
                else
                {

                    if ((length > 8) && (length <= 12))
                    {
                        basePenalty = 8;

                    }
                    else if ((length > 12) && (length <= 16))
                    {
                        basePenalty = 6;

                    }
                    else if ((length > 16) && (length <= 20))
                    {

                        basePenalty = 5;

                    }
                    else if (length < 8)
                    {

                        message = "Your password must be at least 8 characters long.";

                    }

                }
            }



            int adjustedPenalty = (int)(10 * (basePenalty - lengthFactor));
            double penaltyResult = passwordStrength - adjustedPenalty;
            if (penaltyResult > 0)
            {
                passwordStrength = penaltyResult;

            }
            else
            {
                passwordStrength = 0;
            }

            return message;




        }

        public string checkCommonPasswords(string password)
        {
            int maxDistance = 2;
            string message = "";
            double penalty = 0;

            foreach (string commonPassword in commonPasswords)
            {
                if (password == commonPassword)
                {
                    message = "This password is too common and may be easily guessed. Try something more complex.";
                    passwordStrength = 0;
                    break;
                }
                else
                {
                    int similarityDistance = Compute(password, commonPassword);
                    if (similarityDistance <= maxDistance)
                    {
                        message = "Your password is too similar to a common password. Please choose a more distinct password.";
                        penalty = 0.6;
                        break;
                    }
                    else
                    {
                        if (password.ToLower().Contains(commonPassword.ToLower()))
                        {
                            message = "Your password contains a common password phrase: " + "'" + commonPassword + "'" + " Consider using a unique and complex password to enhance your security.";
                            penalty = 0.7;
                            break;

                        }
                    }
                }

            }
            if(penalty > 0)
            {
                passwordStrength = (passwordStrength * penalty);

            }
            return message;
        }
    }
}
