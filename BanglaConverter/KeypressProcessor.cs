using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanglaConverter.BanglaUnicodeData;

namespace BanglaConverter
{
    internal class KeypressProcessor : InputEventProcessor
    {

        public KeypressProcessor(ITextEditor document) : base(document)
        {

        }

        private class VowelKey
        {
            public readonly string KeyboardKeyName;
            public readonly CodePoint StandardFullVowel;
            public readonly CodePoint StandardVowelSign;
            public readonly CodePoint ShiftFullVowel;
            public readonly CodePoint ShiftVowelSign;

            public VowelKey(string keyboardKeyName, CodePoint standardFullVowel, CodePoint standardVowelSign,
                CodePoint shiftFullVowel, CodePoint shiftVowelSign)
            {
                KeyboardKeyName = keyboardKeyName;
                StandardFullVowel = standardFullVowel;
                StandardVowelSign = standardVowelSign;
                ShiftFullVowel = shiftFullVowel;
                ShiftVowelSign = shiftVowelSign;
            }

            /// <summary>
            /// Gets the appropriate vowel, based on the current vowel mode and whether shift was held.
            /// </summary>
            /// <returns></returns>
            public CodePoint GetAppropriateVowel(SharedData.VowelMode vowelMode, bool shift)
            {
                if (vowelMode == SharedData.VowelMode.FullVowel)
                {
                    return shift ? ShiftFullVowel : StandardFullVowel;
                }
                else
                {
                    return shift ? ShiftVowelSign : StandardVowelSign;
                }
            }
        }

        private static readonly Dictionary<string, VowelKey> vowelKeys = new Dictionary<string, VowelKey>
        {
            // There is no vowel sign for the first vowel.
            ["A"] = new VowelKey("A", CodePoint.A, CodePoint.AKar, CodePoint.FirstVowel, CodePoint.Invalid),
            ["I"] = new VowelKey("I", CodePoint.HrasvaI, CodePoint.HrasvaIKar, CodePoint.DirghaI, CodePoint.DirghaIKar),
            ["U"] = new VowelKey("U", CodePoint.HrasvaU, CodePoint.HrasvaUKar, CodePoint.DirghaU, CodePoint.DirghaUKar),
            // The R key only represents a vowel (RI) when shift is held.
            ["R"] = new VowelKey("R", CodePoint.Invalid, CodePoint.Invalid, CodePoint.RI, CodePoint.RIKar),
            ["E"] = new VowelKey("E", CodePoint.E, CodePoint.EKar, CodePoint.OI, CodePoint.OIKar),
            ["O"] = new VowelKey("O", CodePoint.O, CodePoint.OKar, CodePoint.OU, CodePoint.OUKar)
        };

        /// <summary>
        /// Returns the list of active Bangla characters (those that can be typed at the current moment)
        /// based on whether the shift key is held. For example, if shift is held, the returned
        /// list will include DirghaI (because typing I while holding shift produces DirghaI), but if
        /// shift is not held, the list will include HrasvaI instead.
        /// </summary>
        public List<CodePoint> GetActiveBanglaLetters()
        {
            List<CodePoint> activeLetters = new List<CodePoint>();

            // If the converter is off, there are no active Bangla letters.
            if (!ConverterEnabled)
            {
                // Iterates over the vowel keys.
                foreach (VowelKey vowelKey in vowelKeys.Values)
                {
                    CodePoint appropriateVowel = vowelKey.GetAppropriateVowel(CurrentVowelMode, ShiftKeyActive);

                    // The Invalid CodePoint means there is no appropriate vowel for the VowelKey.
                    if (appropriateVowel != CodePoint.Invalid)
                    {
                        activeLetters.Add(appropriateVowel);
                    }
                }

                // The consonant Ra is active if shift is not held.
                if (!ShiftKeyActive)
                {
                    activeLetters.Add(CodePoint.Ra);
                }

            }

            return activeLetters;
        }

        /// <summary>
        /// Returns true if the key pressed is A, I, U, E, or O; or if the key is R and shift was being held.
        /// </summary>
        private bool KeyWasVowel(string keyName)
        {
            if (keyName == "A" || keyName == "I" || keyName == "U" || keyName == "E" || keyName == "O")
            {
                return true;
            }
            else if (keyName == "R" && ShiftKeyActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indicates whether a key or key combination was pressed that represents a pair of consonants in
        /// which the aspirated version is one code point after the unaspirated version.
        /// </summary>
        private static bool KeyWasAspirableConsonant(string keyName)
        {
            return keyName == "K" || keyName == "G"
                || keyName == "C" || keyName == "J"
                || keyName == "T" || keyName == "D"
                || keyName == "P" || keyName == "B";
        }

        /// <summary>
        /// Indicates whether a key or key combination was pressed that represents a numeral.
        /// </summary>
        private bool KeyWasNumeral(string keyName)
        {
            //System.Diagnostics.Debug.WriteLine(keyName);

            // Checks if a standard numeral key was pressed, but it does not count if shift is held.
            bool wasStandardNumeral = !ShiftKeyActive
                // Checks if the key pressed was D0, D1, D2, ..., or D9. 
                && (keyName != null
                    && keyName.Length == 2
                    && keyName[0] == 'D'
                    && '0' <= keyName[1] && keyName[1] <= '9');

            // Checks if a num pad numeral key was pressed. It does not matter if shift is held.
            // The num pad numeral keys are NumPad0, NumPad1, NumPad2, ..., and NumPad9.
            bool wasNumPadNumeral = keyName != null
                && keyName.Length == 7
                && keyName.StartsWith("NumPad")
                && '0' <= keyName[6] && keyName[6] <= '9';

            return wasStandardNumeral || wasNumPadNumeral;
        }

        private string GetAppropriateAspirableConsonant(string keyName)
        {
            // First determines which pair of consonants the key or key combination represents.
            // The pair is represented by the unaspirated version of the consonant. Later, it will
            // be determined whether to add one to the code point to indicate aspiration.
            CodePoint consonant = CodePoint.Invalid;

            switch (keyName)
            {
                case "K":
                    consonant = CodePoint.Ka;
                    break;
                case "G":
                    consonant = CodePoint.Ga;
                    break;
                case "C":
                    consonant = CodePoint.Ca;
                    break;
                case "J":
                    consonant = CodePoint.Ja;
                    break;
                case "T":
                    if (AltKeyActive)
                        consonant = CodePoint.KhondoTa;
                    else
                        consonant = CtrlKeyActive ? CodePoint.RetroflexTa : CodePoint.DentalTa;
                    break;
                case "D":
                    consonant = CtrlKeyActive ? CodePoint.RetroflexDa : CodePoint.DentalDa;
                    break;
                case "P":
                    consonant = CodePoint.Pa;
                    break;
                case "B":
                    consonant = CodePoint.Ba;
                    break;
                default:
                    break;
            }
            
            string banglaChar = "";
            
            if (consonant != CodePoint.Invalid)
            {
                // The shift key switches the consonant to the aspirated version.
                // KhondoTa is simply a variant of DentalTa and has no aspirated version.
                if (consonant == CodePoint.KhondoTa || !ShiftKeyActive)
                {
                    banglaChar = "" + (char)(CODE_POINT_OFFSET + consonant);
                }
                else
                {
                    banglaChar = "" + (char)(CODE_POINT_OFFSET + consonant + 1);
                }
            }

            return banglaChar;
        }

        /// <summary>
        /// Determines which Bangla character to output when the N key is pressed.
        /// </summary>
        private string GetConsonantForNKey()
        {
            string banglaChar = "";

            CodePoint codePoint;

            if (CtrlKeyActive)
            {
                codePoint = ShiftKeyActive ? CodePoint.Iya : CodePoint.Unga;
            }
            else
            {
                codePoint = ShiftKeyActive ? CodePoint.MurdhonnoNa : CodePoint.DentalNa;
            }

            banglaChar = "" + (char)(CODE_POINT_OFFSET + codePoint);

            return banglaChar;
        }

        /// <summary>
        /// Gets the appropriate Bangla sibilant character based on the control keys pressed.
        /// </summary>
        private string GetAppropriateSibilant()
        {
            string banglaChar = "";

            CodePoint codePoint;
            // Shift and control together are an invalid combination for S.
            if (ShiftKeyActive && CtrlKeyActive)
            {
                codePoint = CodePoint.Invalid;
            }
            else if (ShiftKeyActive)
            {
                codePoint = CodePoint.TalobboSa;
            }
            else if (CtrlKeyActive)
            {
                codePoint = CodePoint.MurdhonnoSa;
            }
            else
            {
                codePoint = CodePoint.DentalSa;
            }

            if (codePoint != CodePoint.Invalid)
            {
                banglaChar = "" + (char)(CODE_POINT_OFFSET + codePoint);
            }

            return banglaChar;
        }

        /// <summary>
        /// Gets the appropriate Bangla numeral character based on the key pressed.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private string GetAppropriateNumeral(string keyName)
        {
            // The numeral (as a char) is indicated by the last character of the key name.
            char numeralAsChar = keyName[^1];

            // The offset of the ASCII zero character is subtracted from the char
            // to get the actual numeric value that it represents.
            int numberValue = (int)(numeralAsChar - '0');

            // The Unicode code point for the Bangla numeral 0 is added as an offset to the
            // numeric value to get the code point of the Bangla numeral.
            string banglaChar = "" + (char)(CODE_POINT_OFFSET + CodePoint.Shunno + numberValue);

            return banglaChar;
        }

        /// <summary>
        /// Converts key event data into a Bangla character, according to the
        /// current vowel mode (if applicable) and key modifiers.
        /// </summary>
        public string ConvertToBangla(string keyName)
        {
            string banglaText = "";

            if (KeyWasVowel(keyName))
            {
                VowelKey vowelKey = vowelKeys[keyName];
                CodePoint codePoint = vowelKey.GetAppropriateVowel(CurrentVowelMode, ShiftKeyActive);
                if (codePoint != CodePoint.Invalid)
                {
                    banglaText = "" + (char)(CODE_POINT_OFFSET + codePoint);
                }    
            }
            else if (KeyWasAspirableConsonant(keyName))
            {
                banglaText = GetAppropriateAspirableConsonant(keyName);
            }
            else if (KeyWasNumeral(keyName))
            {
                banglaText = GetAppropriateNumeral(keyName);
            }
            else
            {
                switch (keyName)
                {
                    case "N":
                        banglaText = GetConsonantForNKey();
                        break;
                    case "S":
                        banglaText = GetAppropriateSibilant();
                        break;
                    case "M":
                        banglaText = MakeString(CodePoint.Ma);
                        break;
                    case "Y":
                        banglaText = MakeString(CodePoint.Ya);
                        break;
                    case "R":
                        banglaText = MakeString(CodePoint.Ra);
                        break;
                    case "L":
                        banglaText = MakeString(CodePoint.La);
                        break;
                    case "H":
                        banglaText = MakeString(CodePoint.Ha);
                        break;
                    case "F":
                        banglaText = MakeString(CodePoint.Pha);
                        break;
                    case "V":
                        banglaText = MakeString(CodePoint.Bha);
                        break;
                    case "Z":
                        banglaText = MakeString(CodePoint.Ya);
                        break;
                    case "X":
                        banglaText = MakeString(CodePoint.Ka, CodePoint.Hosonto, CodePoint.MurdhonnoSa);
                        break;
                    case "Oemtilde":
                        banglaText = MakeString(CodePoint.Candrabindu);
                        break;
                    case "Oem1":
                        banglaText = ShiftKeyActive ? MakeString(CodePoint.Bisarga) : MakeString(CodePoint.Anusvar);
                        break;
                    case "OemPeriod":
                        banglaText = MakeString(CodePoint.UnderDot);
                        break;
                    case "Oemcomma":
                        banglaText = MakeString(CodePoint.Hosonto);
                        break;
                    case "Oem5":
                        banglaText = MakeString(CodePoint.Dahri);
                        break;
                    default:
                        break;
                }
            }

            return banglaText;
        }

        protected override void KeyDownHandler(string keyName)
        {
            base.KeyDownHandler(keyName);
            // If the converter is off, do nothing to the input.
            if (ConverterEnabled)
            {
                // Converts the keypress to Bangla text.
                string banglaText = ConvertToBangla(keyName);
                // If the keypress has translated into nonempty text, deliver the output and auto-set the vowel mode.
                if (banglaText != "")
                {
                    this.editor.WriteText(banglaText);
                    AutoSetVowelMode();
                }
            }
        }

        /// <summary>
        /// Gets the text immediately before the cursor position.
        /// Continues reading all text until reaching any character that is not
        /// a Bangla vowel-sign or diacritic.
        /// </summary>
        internal string GetTextBeforeCursor()
        {
            // Starts at the character directly before the cursor / selection area.
            int index = this.editor.CurrentPosition - 1;
            string text = "";

            // Keep going as long as the index is valid.
            while (index >= 0)
            {
                // Read the character at the index.
                char ch = this.editor.CurrentText[index];

                // Add the character to the front of the text.
                text = ch + text;

                // If the character is anything other than a Bangla vowel-sign or diacritic,
                // break out of the loop.
                if (!BanglaUnicodeData.IsBanglaVowelSign(ch) && !BanglaUnicodeData.IsBanglaDiacritic(ch))
                {
                    break;
                }

                // Move to the previous character.
                index--;
            }

            return text;
        }

        /// <summary>
        /// Automatically sets the vowel mode according to what lies directly before the cursor / selection area.
        /// </summary>
        public void AutoSetVowelMode()
        {
            string textBeforeCursor = GetTextBeforeCursor();

            // If there is no text before the cursor and the user types a vowel, it should probably be a full vowel,
            // because it is at the start of the text.
            if (textBeforeCursor == string.Empty)
            {
                SetVowelMode(SharedData.VowelMode.FullVowel);
                return;
            }

            bool startsWithConsonant = IsBanglaConsonant(textBeforeCursor[0]);

            // If the first character is a Bangla consonant and the syllable is not closed by a vowel sign, anusvar, or bisorgo,
            // then if the next character typed is a vowel, it should likely be a vowel sign. If the user wants to enter a full-vowel anyway
            // (because the preceding syllable is closed by an inherent vowel), they will need to switch back to full-vowel mode manually.
            if (startsWithConsonant)
            {
                bool canReceiveVowelSign = true;

                // Checks the remaining characters to see if the syllable can still receive a vowel sign.
                for (int i = 1; i < textBeforeCursor.Length; i++)
                {
                    char ch = textBeforeCursor[i];

                    if (IsBanglaVowelSign(ch)
                        || ch == (char)((int)CodePoint.Anusvar + CODE_POINT_OFFSET)
                        || ch == (char)((int)CodePoint.Bisarga + CODE_POINT_OFFSET))
                    {
                        canReceiveVowelSign = false;
                        break;
                    }
                }

                if (canReceiveVowelSign)
                {
                    // Because a vowel sign can still be attached to the consonant, the vowel mode will be set to vowel-sign mode.
                    // If the user intends for an inherent vowel to close the syllable and wants to type a full vowel next, they
                    // will need to switch to full-vowel mode manually.
                    SetVowelMode(SharedData.VowelMode.VowelSign);
                }
                else
                {
                    // Because a vowel sign, anusvar, or bisorgo is already attached to the consonant or conjunct,
                    // if the next character is a vowel, the user probably intends it to be a full vowel.
                    SetVowelMode(SharedData.VowelMode.FullVowel);
                }
            }
            // If the first character is not a Bangla consonant, there is nothing to add a vowel sign to, so enter full-vowel mode.
            else
            {
                SetVowelMode(SharedData.VowelMode.FullVowel);
            }

        }

    }
}
