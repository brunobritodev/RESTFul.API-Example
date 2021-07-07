using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTFul.Api.Service
{
    public static class StringUtils
    {

        public static string Urlize(this string value)
        {
            return value
                .Replace("'", "")
                .Replace("-", " ")
                .Replace(",", "")
                .Replace(".", string.Empty)
                .ReduceWhitespace()
                .Replace(" ", "-")
                .ToLower()
                .RemoveDiacritics();
        }

        static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static String ReduceWhitespace(this String value)
        {
            var newString = new StringBuilder();
            bool previousIsWhitespace = false;
            for (int i = 0; i < value.Length; i++)
            {
                if (Char.IsWhiteSpace(value[i]))
                {
                    if (previousIsWhitespace)
                    {
                        continue;
                    }

                    previousIsWhitespace = true;
                }
                else
                {
                    previousIsWhitespace = false;
                }

                newString.Append(value[i]);
            }

            return newString.ToString();
        }
    }
}
