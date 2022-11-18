using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanglaConverter.BanglaUnicodeData;

namespace BanglaConverter
{
    internal static class KeypressProcessor
    {
        /// <summary>
        /// When a vowel key is pressed, this determines whether to output the full vowel or its vowel sign.
        /// </summary>
        public enum VowelMode { FullVowel = 0, VowelSign = 1}

        public static VowelMode CurrentVowelMode { get; private set; } = VowelMode.FullVowel;

        private static void ToggleVowelMode()
        {
            CurrentVowelMode = 1 - CurrentVowelMode;
            VowelModeChangeCallback();
        }

        private static void SetVowelMode(VowelMode newVowelMode)
        {
            // Toggles the vowel mode if the current vowel mode does not match the desired one.
            if (CurrentVowelMode != newVowelMode)
            {
                ToggleVowelMode();
            }
        }

        /// <summary>
        /// When a key is pressed, determines whether to convert the character to Bangla or leave as is.
        /// </summary>
        public enum LanguageMode { Bangla = 0, English = 1}

        public static LanguageMode CurrentLanguageMode { get; private set; } = LanguageMode.Bangla;

        private static void ToggleLanguageMode()
        {
            CurrentLanguageMode = 1 - CurrentLanguageMode;
            LanguageModeChangeCallback();
        }

        /// <summary>
        /// Each modifier is a bit flag. Modifiers can be combined with the bitwise OR operator.
        /// </summary>
        public enum KeyModifier
        {
            None    = 0b000,
            Shift   = 0b001,
            Ctrl    = 0b010,
            Alt     = 0b100
        }

        public static KeyModifier CurrentKeyModifiers { get; private set; } = KeyModifier.None;

        private static void AddKeyModifier(KeyModifier keyModifier)
        {
            CurrentKeyModifiers |= keyModifier;
            KeyModifiersChangeCallback();
        }

        private static void RemoveKeyModifier(KeyModifier keyModifier)
        {
            CurrentKeyModifiers &= ~keyModifier;
            KeyModifiersChangeCallback();
        }

        public static bool HasModifier(KeyModifier keyModifier)
        {
            return (CurrentKeyModifiers | keyModifier) == CurrentKeyModifiers;
        }

        internal delegate void UICallback();
        internal static UICallback LanguageModeChangeCallback { get; set; } = () => { };
        internal static UICallback VowelModeChangeCallback { get; set; } = () => { };
        internal static UICallback KeyModifiersChangeCallback { get; set; } = () => { };

        internal delegate void UIOutputDeliverer(string output);
        /// <summary>
        /// This is the delegate used to send output back to the UI.
        /// </summary>
        internal static UIOutputDeliverer DeliverOutput { get; set; } = (string output) => { };

        internal delegate string UIInputReceiver();

        /// <summary>
        /// This is the delegate used to receive text input from the UI.
        /// This allows the KeypressProcessor to decide the appropriate vowel-mode.
        /// </summary>
        internal static UIInputReceiver ReadTextBeforeCursor { get; set; } = () => throw new NotImplementedException();

        private class VowelKey
        {
            public readonly Keys KeyboardKey;
            public readonly CodePoint StandardFullVowel;
            public readonly CodePoint StandardVowelSign;
            public readonly CodePoint ShiftFullVowel;
            public readonly CodePoint ShiftVowelSign;
            
            public VowelKey (Keys keyboardKey, CodePoint standardFullVowel, CodePoint standardVowelSign,
                CodePoint shiftFullVowel, CodePoint shiftVowelSign)
            {
                KeyboardKey = keyboardKey;
                StandardFullVowel = standardFullVowel;
                StandardVowelSign = standardVowelSign;
                ShiftFullVowel = shiftFullVowel;
                ShiftVowelSign = shiftVowelSign;
            }

            /// <summary>
            /// Gets the appropriate vowel, based on the current vowel mode and whether shift was held.
            /// </summary>
            /// <returns></returns>
            public CodePoint GetAppropriateVowel(bool shift)
            {
                if (CurrentVowelMode == VowelMode.FullVowel)
                {
                    return shift ? ShiftFullVowel : StandardFullVowel;
                }
                else
                {
                    return shift ? ShiftVowelSign : StandardVowelSign;
                }
            }
        }

        private static readonly Dictionary<Keys, VowelKey> vowelKeys = new Dictionary<Keys, VowelKey>
        {
            // There is no vowel sign for the first vowel.
            [Keys.A] = new VowelKey(Keys.A, CodePoint.A, CodePoint.AKar, CodePoint.FirstVowel, CodePoint.Invalid),
            [Keys.I] = new VowelKey(Keys.I, CodePoint.HrasvaI, CodePoint.HrasvaIKar, CodePoint.DirghaI, CodePoint.DirghaIKar),
            [Keys.U] = new VowelKey(Keys.U, CodePoint.HrasvaU, CodePoint.HrasvaUKar, CodePoint.DirghaU, CodePoint.DirghaUKar),
            // The R key only represents a vowel (RI) when shift is held.
            [Keys.R] = new VowelKey(Keys.R, CodePoint.Invalid, CodePoint.Invalid, CodePoint.RI, CodePoint.RIKar),
            [Keys.E] = new VowelKey(Keys.E, CodePoint.E, CodePoint.EKar, CodePoint.OI, CodePoint.OIKar),
            [Keys.O] = new VowelKey(Keys.O, CodePoint.O, CodePoint.OKar, CodePoint.OU, CodePoint.OUKar)
        };

        /// <summary>
        /// Returns the list of active Bangla characters (those that can be typed at the current moment)
        /// based on whether the shift key is held. For example, if shift is held, the returned
        /// list will include DirghaI (because typing I while holding shift produces DirghaI), but if
        /// shift is not held, the list will include HrasvaI instead.
        /// </summary>
        public static List<CodePoint> GetActiveBanglaLetters()
        {
            List<CodePoint> activeLetters = new List<CodePoint>();

            // If the language is English, there are no active Bangla letters.
            if (CurrentLanguageMode != LanguageMode.English)
            {
                bool shift = HasModifier(KeyModifier.Shift);

                // Iterates over the vowel keys.
                foreach (VowelKey vowelKey in vowelKeys.Values)
                {
                    CodePoint appropriateVowel = vowelKey.GetAppropriateVowel(shift);

                    // The Invalid CodePoint means there is no appropriate vowel for the VowelKey.
                    if (appropriateVowel != CodePoint.Invalid)
                    {
                        activeLetters.Add(appropriateVowel);
                    }
                }

                // The consonant Ra is active if shift is not held.
                if (!shift)
                {
                    activeLetters.Add(CodePoint.Ra);
                }
                
            }

            return activeLetters;
        }

        /// <summary>
        /// Returns true if the key pressed is A, I, U, E, or O; or if the key is R and shift was being held.
        /// </summary>
        private static bool KeyWasVowel(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.I || e.KeyCode == Keys.U || e.KeyCode == Keys.E || e.KeyCode == Keys.O)
            {
                return true;
            }
            else if (e.KeyCode == Keys.R && e.Shift)
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
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool KeyWasAspirableConsonant(KeyEventArgs e)
        {
            return e.KeyCode == Keys.K || e.KeyCode == Keys.G
                || e.KeyCode == Keys.C || e.KeyCode == Keys.J
                || e.KeyCode == Keys.T || e.KeyCode == Keys.D
                || e.KeyCode == Keys.P || e.KeyCode == Keys.B;
        }

        private static string GetAppropriateAspirableConsonant(KeyEventArgs e)
        {
            // First determines which pair of consonants the key or key combination represents.
            // The pair is represented by the unaspirated version of the consonant. Later, it will
            // be determined whether to add one to the code point to indicate aspiration.
            CodePoint consonant = CodePoint.Invalid;

            switch (e.KeyCode)
            {
                case Keys.K:
                    consonant = CodePoint.Ka;
                    break;
                case Keys.G:
                    consonant = CodePoint.Ga;
                    break;
                case Keys.C:
                    consonant = CodePoint.Ca;
                    break;
                case Keys.J:
                    consonant = CodePoint.Ja;
                    break;
                case Keys.T:
                    if (e.Alt)
                        consonant = CodePoint.KhondoTa;
                    else
                        consonant = e.Control ? CodePoint.RetroflexTa : CodePoint.DentalTa;
                    break;
                case Keys.D:
                    consonant = e.Control ? CodePoint.RetroflexDa : CodePoint.DentalDa;
                    break;
                case Keys.P:
                    consonant = CodePoint.Pa;
                    break;
                case Keys.B:
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
                if (consonant == CodePoint.KhondoTa || !e.Shift)
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
        private static string GetConsonantForNKey(KeyEventArgs e)
        {
            string banglaChar = "";

            if (e.KeyCode == Keys.N)
            {
                CodePoint codePoint;

                if (e.Control)
                {
                    codePoint = e.Shift ? CodePoint.Iya : CodePoint.Unga;
                }
                else
                {
                    codePoint = e.Shift ? CodePoint.MurdhonnoNa : CodePoint.DentalNa;
                }

                banglaChar = "" + (char)(CODE_POINT_OFFSET + codePoint);
            }

            return banglaChar;
        }

        /// <summary>
        /// Gets the appropriate Bangla sibilant character based on the control keys pressed.
        /// </summary>
        private static string GetAppropriateSibilant(KeyEventArgs e)
        {
            string banglaChar = "";

            if (e.KeyCode == Keys.S)
            {
                CodePoint codePoint;
                // Shift and control together are an invalid combination for S.
                if (e.Shift && e.Control)
                {
                    codePoint = CodePoint.Invalid;
                }
                else if (e.Shift)
                {
                    codePoint = CodePoint.TalobboSa;
                }
                else if (e.Control)
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
            }

            return banglaChar;
        }

        /// <summary>
        /// Converts key event data into a Bangla character, according to the current vowel mode (if applicable).
        /// </summary>
        public static string ConvertToBangla(KeyEventArgs e)
        {
            string banglaText = "";

            if (KeyWasVowel(e))
            {
                VowelKey vowelKey = vowelKeys[e.KeyCode];
                CodePoint codePoint = vowelKey.GetAppropriateVowel(e.Shift);
                if (codePoint != CodePoint.Invalid)
                {
                    banglaText = "" + (char)(CODE_POINT_OFFSET + codePoint);
                }    
            }
            else if (KeyWasAspirableConsonant(e))
            {
                banglaText = GetAppropriateAspirableConsonant(e);
            }
            else if (e.KeyCode == Keys.N)
            {
                banglaText = GetConsonantForNKey(e);
            }
            else if (e.KeyCode == Keys.S)
            {
                banglaText = GetAppropriateSibilant(e);
            }
            else if (e.KeyCode == Keys.M)
            {
                banglaText = MakeString(CodePoint.Ma);
            }
            else if (e.KeyCode == Keys.Y)
            {
                banglaText = MakeString(CodePoint.Ya);
            }
            else if (e.KeyCode == Keys.R)
            {
                banglaText = MakeString(CodePoint.Ra);
            }
            else if (e.KeyCode == Keys.L)
            {
                banglaText = MakeString(CodePoint.La);
            }
            else if (e.KeyCode == Keys.H)
            {
                banglaText = MakeString(CodePoint.Ha);
            }
            else if (e.KeyCode == Keys.X)
            {
                banglaText = MakeString(CodePoint.Ka, CodePoint.Hosonto, CodePoint.MurdhonnoSa);
            }
            else if (e.KeyCode == Keys.Oemtilde)
            {
                banglaText = MakeString(CodePoint.Candrabindu);
            }
            else if (e.KeyCode == Keys.Oem1)
            {
                banglaText = e.Shift ? MakeString(CodePoint.Bisarga) : MakeString(CodePoint.Anusvar);
            }
            else if (e.KeyCode == Keys.OemPeriod)
            {
                banglaText = MakeString(CodePoint.UnderDot);
            }
            else if (e.KeyCode == Keys.Oemcomma)
            {
                banglaText = MakeString(CodePoint.Hosonto);
            }
            else if (e.KeyCode == Keys.Oem5)
            {
                banglaText = MakeString(CodePoint.Dahri);
            }

            return banglaText;
        }

        public static void KeyUpHandler(KeyEventArgs e)
        {
            // Checks if the shift key was just released.
            if (e.KeyCode == Keys.ShiftKey)
            {
                RemoveKeyModifier(KeyModifier.Shift);
            }
            // Checks if the control key was just released.
            else if (e.KeyCode == Keys.ControlKey)
            {
                RemoveKeyModifier(KeyModifier.Ctrl);
            }
            // Checks if the alt key was just released.
            else if (e.KeyCode == Keys.Alt)
            {
                RemoveKeyModifier(KeyModifier.Alt);
            }
        }

        public static void KeyDownHandler(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                AddKeyModifier(KeyModifier.Shift);
            }   
            else if (e.KeyCode == Keys.ControlKey)
            {
                AddKeyModifier(KeyModifier.Ctrl);
            }
            else if (e.KeyCode == Keys.Alt)
            {
                AddKeyModifier(KeyModifier.Alt);
            }
            // The F1 key always toggles the language, regardless of the current language.
            else if (e.KeyCode == Keys.F1)
            {
                ToggleLanguageMode();
                e.SuppressKeyPress = true;
            }
            // If any other key was pressed and the language mode is English, do nothing to the input.
            else if (CurrentLanguageMode == LanguageMode.English)
            {
                return;
            }
            // If the key combination is Ctrl + C, Ctrl + V, Ctrl + X, or Ctrl + A, do nothing to the input.
            else if (e.Control && !e.Shift && !e.Alt && (e.KeyCode == Keys.C || e.KeyCode == Keys.V || e.KeyCode == Keys.X || e.KeyCode == Keys.A))
            {
                return;
            }
            // Checks if the '/' key was pressed, but only if shift was not held.
            else if (e.KeyCode == Keys.OemQuestion && !e.Shift)
            {
                ToggleVowelMode();
                e.SuppressKeyPress = true;
            }
            else
            {
                // Converts the keypress to Bangla text.
                string banglaText = ConvertToBangla(e);
                // If the keypress has translated into nonempty text, deliver the output and auto-set the vowel mode.
                // Also, suppress the keypress to avoid inserting extra text.
                if (banglaText != "")
                {
                    DeliverOutput(banglaText);
                    AutoSetVowelMode();
                    e.SuppressKeyPress = true;
                }
                // If the key pressed was a letter, regardless of whether the event produced Bangla text,
                // the keypress needs to be supressed. This prevents the insertion of English letters into
                // the text.
                else if (Keys.A <= e.KeyCode && e.KeyCode <= Keys.Z)
                {
                    e.SuppressKeyPress = true;
                }
            }
        }

        /// <summary>
        /// Automatically sets the vowel mode according to what lies directly before the cursor / selection area.
        /// </summary>
        public static void AutoSetVowelMode()
        {
            string textBeforeCursor = ReadTextBeforeCursor();

            // If there is no text before the cursor and the user types a vowel, it should probably be a full vowel,
            // because it is at the start of the text.
            if (textBeforeCursor == string.Empty)
            {
                SetVowelMode(VowelMode.FullVowel);
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
                    SetVowelMode(VowelMode.VowelSign);
                }
                else
                {
                    // Because a vowel sign, anusvar, or bisorgo is already attached to the consonant or conjunct,
                    // if the next character is a vowel, the user probably intends it to be a full vowel.
                    SetVowelMode(VowelMode.FullVowel);
                }
            }
            // If the first character is not a Bangla consonant, there is nothing to add a vowel sign to, so enter full-vowel mode.
            else
            {
                SetVowelMode(VowelMode.FullVowel);
            }

        }

    }
}
