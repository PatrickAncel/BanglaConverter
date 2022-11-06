using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaConverter
{
    internal static class BanglaUnicodeData
    {
        /// <summary>
        /// Add this to each CodePoint value to get the correct Unicode code point.
        /// </summary>
        public const int CODE_POINT_OFFSET = 0x0900;

        /// <summary>
        /// The last two hex digits of the named Bangla characters.
        /// </summary>
        public enum CodePoint
        {
            // Full Vowels
            FirstVowel = 0x85,
            A = 0x86,
            HrasvaI = 0x87,
            DirghaI = 0x88,
            HrasvaU = 0x89,
            DirghaU = 0x8A,
            RI = 0x8B,
            E = 0x8F,
            OI = 0x90,
            O = 0x93,
            OU = 0x94,
            // Vowel Signs
            AKar = 0xBE,
            HrasvaIKar = 0xBF,
            DirghaIKar = 0xC0,
            HrasvaUKar = 0xC1,
            DirghaUKar = 0xC2,
            RIKar = 0xC3,
            EKar = 0xC7,
            OIKar = 0xC8,
            OKar = 0xCB,
            OUKar = 0xCC,
            // Diacritics
            Candrabindu = 0x81,
            Anusvar = 0x82,
            Bisarga = 0x83,
            UnderDot = 0xBC,
            Hosont = 0xCD,
            // Misc.
            KhondoTa = 0xCE,
            BDTaka = 0xF3,
            Shunno = 0xE6,
            // Consonants
            Ka = 0x95,
            Ga = 0x97,
            Unga = 0x99,
            Ca = 0x9A,
            Ja = 0x9C,
            Iya = 0x9E,
            RetroflexTa = 0x9F,
            RetroflexDa = 0xA1,
            MurdhonnoNa = 0xA3,
            DentalTa = 0xA4,
            DentalDa = 0xA6,
            DentalNa = 0xA8,
            Pa = 0xAA,
            Ba = 0xAC,
            Ma = 0xAE,
            Ya = 0xAF,
            Ra = 0xB0,
            La = 0xB2,
            TalobboSa = 0xB6,
            MurdhonnoSa = 0xB7,
            DentalSa = 0xB8,
            Ha = 0xB9,
            // A special value outside the Bangla Unicode block representing invalid input.
            Invalid = 0x00
        }

        /// <summary>
        /// Converts a sequence of partial code points into a string of Bangla characters.
        /// </summary>
        public static string MakeString(params CodePoint[] codePoints)
        {
            string text = "";
            foreach (int codePoint in codePoints)
            {
                if (codePoint != (int)CodePoint.Invalid)
                {
                    // Gets the full code point of the character and adds it to the string.
                    text += (char)(CODE_POINT_OFFSET + codePoint);
                }
            }
            return text;
        }

        /// <summary>
        /// Determines if the character is a Bangla consonant, not including U+09DC, U+09DD, and U+09DF.
        /// This method will return false positives for any of the reserved code points between U+0995 and U+09B9
        /// in the Bangla Unicode block.
        /// </summary>
        public static bool IsBanglaConsonant(char ch)
        {
            // Subtracts the offset from the character's numeric value.
            int codePointValue = ch - CODE_POINT_OFFSET;

            if ((int)CodePoint.Ka <= codePointValue && codePointValue <= (int)CodePoint.Ha)
            {
                return true;
            }
            else if (codePointValue == (int)CodePoint.KhondoTa)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the character is a Bangla full vowel.
        /// This method will return false positives for certain reserved code points.
        /// </summary>
        public static bool IsBanglaFullVowel(char ch)
        {
            // Subtracts the offset from the character's numeric value.
            int codePointValue = ch - CODE_POINT_OFFSET;

            return (int)CodePoint.FirstVowel <= codePointValue && codePointValue <= (int)CodePoint.OU;
        }

        /// <summary>
        /// Determines if the character is a Bangla vowel sign.
        /// This method will return false positives for certain reserved code points.
        /// </summary>
        public static bool IsBanglaVowelSign(char ch)
        {
            // Subtracts the offset from the character's numeric value.
            int codePointValue = ch - CODE_POINT_OFFSET;

            return (int)CodePoint.AKar <= codePointValue && codePointValue <= (int)CodePoint.OUKar;
        }

        /// <summary>
        /// Determines if the character is a Bangla full vowel or vowel sign.
        /// This method will return false positives for certain reserved code points.
        /// </summary>
        public static bool IsBanglaVowel(char ch)
        {
            return IsBanglaVowelSign(ch) || IsBanglaFullVowel(ch);
        }

        /// <summary>
        /// Determines if the character is a Bangla numeral.
        /// </summary>
        public static bool IsBanglaNumeral(char ch)
        {
            // Subtracts the offset from the character's numeric value.
            int codePointValue = ch - CODE_POINT_OFFSET;

            // The code point for the Bangla numeral 0.
            int shunno = (int)CodePoint.Shunno;
            // The code point for the Bangla numeral 9.
            int noy = shunno + 9;

            return shunno <= ch && ch <= noy;
        }

        /// <summary>
        /// Determines if the character is a Bangla diacritic.
        /// It may not recognize every Bangla diacritic yet.
        /// </summary>
        public static bool IsBanglaDiacritic(char ch)
        {
            // Subtracts the offset from the character's numeric value.
            int codePointValue = ch - CODE_POINT_OFFSET;

            return codePointValue == (int)CodePoint.Candrabindu
                || codePointValue == (int)CodePoint.Anusvar
                || codePointValue == (int)CodePoint.Bisarga
                || codePointValue == (int)CodePoint.UnderDot
                || codePointValue == (int)CodePoint.Hosont;
        }
    }
}
