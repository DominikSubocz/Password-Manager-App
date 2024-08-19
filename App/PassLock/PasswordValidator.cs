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
            string nameOutput = checkCommonNames(password1, length);

           if(complexityOutput != "")
            {
                output = complexityOutput;
            } else
            {

                    if(commonOutput != "")
                    {
                        output = commonOutput;
                    } else
                    {
                        if(nameOutput != "")
                        {
                            output = nameOutput;
                        }
                    }
            }

            Debug.WriteLine(password1 + "is: " + passwordStrength + " points strong") ;
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

        public double getPasswordStrength() { 

            return passwordStrength;
        
        }
        

        public SolidColorBrush getBarColor() {
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
            Regex upperCaseRegex = new Regex(@"(?=.*[A-Z])");
            Regex digitPattern = new Regex(@"(?=.*\d)");
            Regex symbolPattern = new Regex(@"(?=.*[^\da-zA-Z])");
            bool hasRepetition = false;

            string message = "";



            if (upperCaseRegex.IsMatch(password) == false)
            {

                message = "At least one capital letter is required";

            }
            else if (digitPattern.IsMatch(password) == false)
            {
                message = "At least one digit is required.";
            }

            else if (symbolPattern.IsMatch(password) == false)
            {
                message = "At least one special character is required.";
            }
            else
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c))
                    {
                        passwordStrength += 10;
                    }

                    if (char.IsLower(c))
                    {
                        passwordStrength += 5;
                    }

                    if (char.IsDigit(c))
                    {
                        passwordStrength += 15;
                    }

                    if (char.IsSymbol(c))
                    {
                        passwordStrength += 20;
                    }
                }

                hasRepetition = Regex.IsMatch(password, @"(\w)\1{2,}");

                if (length < 30)
                {
                    if (hasRepetition)
                    {
                        passwordStrength -= 95;
                        message = "Sequential patterns in your password make it easier to guess. Please use a more random sequence.";
                    }
                }
                else
                {
                    if (hasRepetition)
                    {
                        passwordStrength -= 55;
                    }
                }

            }

            return message;

        }

        public string checkCommonPasswords(string password)
        {
            int maxDistance = 2;
            string message = "";

            foreach (string commonPassword in commonPasswords)
            {
                if (password == commonPassword)
                {
                    message = "This password is too common and may be easily guessed. Try something more complex.";
                    passwordStrength = 0;
                    break;
                } else
                {
                    int similarityDistance = Compute(password, commonPassword);
                    if (similarityDistance <= maxDistance)
                    {
                        message = "Your password is too similar to a common password. Please choose a more distinct password.";
                        double penalty = (0.6 * passwordStrength);
                        passwordStrength = passwordStrength - penalty;
                        break;
                    }
                    else
                    {
                        if (password.ToLower().Contains(commonPassword.ToLower()))
                        {
                            message = "Your password contains a common password phrase: " + "'" + commonPassword + "'" + " Consider using a unique and complex password to enhance your security.";
                            double penalty = (0.7 * passwordStrength);
                            passwordStrength = passwordStrength - penalty;
                            break;

                        }
                    }
                }
            }

            return message;

        }

        public string checkCommonNames(string password, int length) 
        {

            string message = "";

            foreach (string firstName in firstNames)
            {
                if(length < 20)
                {
                    if (password.ToLower().Contains(firstName.ToLower()))
                    {
                        message = "Your password contains a common name, which can make it easier to guess. To enhance security, please avoid using names and choose a more complex password.";
                        double penalty = (0.35 * passwordStrength);
                        passwordStrength = passwordStrength - penalty;
                        break;
                    }
                    else
                    {
                        message = "";
                    }
                }

            }

            return message;

        }


    }
}
