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
        internal static UIOutputDeliverer DeliverOutput { get; set; } = (string output) => { };

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
            [Keys.I] = new VowelKey(Keys.I, CodePoint.ShortI, CodePoint.ShortIKar, CodePoint.LongI, CodePoint.LongIKar),
            [Keys.U] = new VowelKey(Keys.U, CodePoint.ShortU, CodePoint.ShortUKar, CodePoint.LongU, CodePoint.LongUKar),
            // The R key only represents a vowel (RI) when shift is held.
            [Keys.R] = new VowelKey(Keys.R, CodePoint.Invalid, CodePoint.Invalid, CodePoint.RI, CodePoint.RIKar),
            [Keys.E] = new VowelKey(Keys.E, CodePoint.E, CodePoint.EKar, CodePoint.OI, CodePoint.OIKar),
            [Keys.O] = new VowelKey(Keys.O, CodePoint.O, CodePoint.OKar, CodePoint.OU, CodePoint.OUKar)
        };

        /// <summary>
        /// Returns the list of active Bangla vowels (those that can be typed at the current moment)
        /// based on whether the shift key is held. For example, if shift is held, the returned
        /// list will include LongI (because typing I while holding shift produces LongI), but if
        /// shift is not held, the list will include ShortI instead.
        /// </summary>
        public static List<CodePoint> GetActiveBanglaVowels()
        {
            List<CodePoint> activeVowels = new List<CodePoint>();

            // If the language is English, there are no active Bangla vowels.
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
                        activeVowels.Add(appropriateVowel);
                    }
                }
                
            }

            return activeVowels;
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
                    consonant = CodePoint.K;
                    break;
                case Keys.G:
                    consonant = CodePoint.G;
                    break;
                case Keys.C:
                    consonant = CodePoint.C;
                    break;
                case Keys.J:
                    consonant = CodePoint.J;
                    break;
                case Keys.T:
                    if (e.Alt)
                        consonant = CodePoint.KhondoT;
                    else
                        consonant = e.Control ? CodePoint.RetroflexT : CodePoint.DentalT;
                    break;
                case Keys.D:
                    consonant = e.Control ? CodePoint.RetroflexD : CodePoint.DentalD;
                    break;
                case Keys.P:
                    consonant = CodePoint.P;
                    break;
                case Keys.B:
                    consonant = CodePoint.B;
                    break;
                default:
                    break;
            }
            
            string banglaChar = "";
            
            if (consonant != CodePoint.Invalid)
            {
                // The shift key switches the consonant to the aspirated version.
                // KhondoT is simply a variant of DentalT and has no aspirated version.
                if (consonant == CodePoint.KhondoT || !e.Shift)
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
                    codePoint = e.Shift ? CodePoint.IY : CodePoint.UNG;
                }
                else
                {
                    codePoint = e.Shift ? CodePoint.MurdhonnoN : CodePoint.DentalN;
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
                    codePoint = CodePoint.TalobboS;
                }
                else if (e.Control)
                {
                    codePoint = CodePoint.MurdhonnoS;
                }
                else
                {
                    codePoint = CodePoint.DentalS;
                }

                if (codePoint != CodePoint.Invalid)
                {
                    banglaChar = "" + (char)(CODE_POINT_OFFSET + codePoint);
                }
            }

            return banglaChar;
        }

        /// <summary>
        /// Converts key event data into a Bangla character, according to the current vowel mode, if applicable.
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
                banglaText = MakeString(CodePoint.M);
            }
            else if (e.KeyCode == Keys.Y)
            {
                banglaText = MakeString(CodePoint.Y);
            }
            else if (e.KeyCode == Keys.R)
            {
                banglaText = MakeString(CodePoint.R);
            }
            else if (e.KeyCode == Keys.L)
            {
                banglaText = MakeString(CodePoint.L);
            }
            else if (e.KeyCode == Keys.H)
            {
                banglaText = MakeString(CodePoint.H);
            }
            else if (e.KeyCode == Keys.X)
            {
                banglaText = MakeString(CodePoint.K, CodePoint.Hosont, CodePoint.MurdhonnoS);
            }
            else if (e.KeyCode == Keys.Oemtilde)
            {
                banglaText = MakeString(CodePoint.Chandrabindu);
            }
            else if (e.KeyCode == Keys.Oem1)
            {
                banglaText = e.Shift ? MakeString(CodePoint.Bisorgo) : MakeString(CodePoint.Anusvar);
            }
            else if (e.KeyCode == Keys.OemPeriod)
            {
                banglaText = MakeString(CodePoint.UnderDot);
            }
            else if (e.KeyCode == Keys.Oemcomma)
            {
                banglaText = MakeString(CodePoint.Hosont);
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
                // If the keypress has translated into nonempty text, deliver the output and suppress the keypress.
                // This check prevents text deletion and other odd behavior when a control key is pressed.
                if (banglaText != "")
                {
                    DeliverOutput(banglaText);
                    e.SuppressKeyPress = true;
                }
            }
        }

    }
}
