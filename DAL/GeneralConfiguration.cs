using System;

namespace YamyProject
{
    class GeneralConfiguration
    {
        public static string NumberToWords(decimal number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            long integerPart = (long)number;
            int decimalPart = (int)((number - integerPart) * 100); 

            string words = ConvertWholeNumber(integerPart) + " Dirham";

            if (decimalPart > 0)
            {
                words += " and " + ConvertWholeNumber(decimalPart) + " Fels";
            }

            return words.Trim();
        }

        static string ConvertWholeNumber(long number)
        {
            if (number == 0)
                return "zero";

            string words = "";

            string[] units = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

            string[] tens = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            string[] thousands = { "", "thousand", "million", "billion" };

            int groupIndex = 0;

            while (number > 0)
            {
                int numPart = (int)(number % 1000);
                if (numPart > 0)
                {
                    string part = ConvertThreeDigitNumber(numPart);
                    words = part + (groupIndex > 0 ? " " + thousands[groupIndex] : "") + (words.Length > 0 ? " " + words : "");
                }
                number /= 1000;
                groupIndex++;
            }

            return words.Trim();
        }

        static string ConvertThreeDigitNumber(int number)
        {
            string[] units = { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

            string[] tens = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            string result = "";

            if (number >= 100)
            {
                result += units[number / 100] + " hundred";
                number %= 100;
                if (number > 0)
                    result += " and ";
            }

            if (number > 0)
            {
                if (number < 20)
                    result += units[number];
                else
                {
                    result += tens[number / 10];
                    if (number % 10 > 0)
                        result += "-" + units[number % 10];
                }
            }

            return result;
        }
    }
}
