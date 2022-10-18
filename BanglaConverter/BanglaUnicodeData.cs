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
            ShortI = 0x87,
            LongI = 0x88,
            ShortU = 0x89,
            LongU = 0x8A,
            RI = 0x8B,
            E = 0x8F,
            OI = 0x90,
            O = 0x93,
            OU = 0x94,
            // Vowel Signs
            AKar = 0xBE,
            ShortIKar = 0xBF,
            LongIKar = 0xC0,
            ShortUKar = 0xC1,
            LongUKar = 0xC2,
            RIKar = 0xC3,
            EKar = 0xC7,
            OIKar = 0xC8,
            OKar = 0xCB,
            OUKar = 0xCC,
            // Diacritics
            Chandrabindu = 0x81,
            Anusvar = 0x82,
            Bisorgo = 0x83,
            UnderDot = 0xBC,
            Hosont = 0xCD,
            // Misc.
            KhondoT = 0xCE,
            BDTaka = 0xF3,
            Shunno = 0xE6,
            // Consonants
            K = 0x95,
            G = 0x97,
            UNG = 0x99,
            C = 0x9A,
            J = 0x9C,
            IY = 0x9E,
            RetroflexT = 0x9F,
            RetroflexD = 0xA1,
            MurdhonnoN = 0xA3,
            DentalT = 0xA4,
            DentalD = 0xA6,
            DentalN = 0xA8,
            P = 0xAA,
            B = 0xAC,
            M = 0xAE,
            Y = 0xAF,
            R = 0xB0,
            L = 0xB2,
            TalobboS = 0xB6,
            MurdhonnoS = 0xB7,
            DentalS = 0xB8,
            H = 0xB9,
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

            if ((int)CodePoint.K <= codePointValue && codePointValue <= (int)CodePoint.H)
            {
                return true;
            }
            else if (codePointValue == (int)CodePoint.KhondoT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the character is a Bangla full vowel or vowel sign.
        /// This method will return false positives for certain reserved code points.
        /// </summary>
        public static bool IsBanglaVowel(char ch)
        {
            // Subtracts the offset from the character's numeric value.
            int codePointValue = ch - CODE_POINT_OFFSET;

            // Checks if the character is a full vowel.
            if ((int)CodePoint.FirstVowel <= codePointValue && codePointValue <= (int)CodePoint.OU)
            {
                return true;
            }
            // Checks if the character is a vowel sign.
            else if ((int)CodePoint.AKar <= codePointValue && codePointValue <= (int)CodePoint.OUKar)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
