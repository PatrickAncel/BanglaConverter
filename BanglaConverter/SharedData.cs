using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaConverter
{
    public static class SharedData
    {

        /// <summary>
        /// When a vowel key is pressed, this determines whether to output the full vowel or its vowel sign.
        /// </summary>
        public enum VowelMode { FullVowel = 0, VowelSign = 1 }

        /// <summary>
        /// Indicates the means by which the user interacted with the UI.
        /// </summary>
        internal enum InputEventType { KeyUp, KeyDown, ButtonClick }

        /// <summary>
        /// A delegate type used to perform actions on UI elements
        /// that are inaccessible from the calling class.
        /// </summary>
        internal delegate void UICallback();

        /// <summary>
        /// A delegate type used to send output from a keypress processor
        /// back to the UI.
        /// </summary>
        /// <param name="output">The string to deliver to the UI.</param>
        internal delegate void UIOutputDeliverer(string output);

        /// <summary>
        /// A delegate type used by a keypress processor to accept input
        /// from the UI.
        /// </summary>
        /// <returns></returns>
        internal delegate string UIInputReceiver();

        internal class KeyModifierCollection
        {
            private static readonly string shiftKeyName = Keys.ShiftKey.ToString();
            private static readonly string ctrlKeyName = Keys.ControlKey.ToString();
            private static readonly string altKeyName = Keys.Alt.ToString();


            /// <summary>
            /// A dictionary mapping all of the modifier key names to bools
            /// indicating whether they are active.
            /// </summary>
            private Dictionary<string, bool> modifierActivityByKey
                = new Dictionary<string, bool>
                {
                    [shiftKeyName] = false,
                    [ctrlKeyName] = false,
                    [altKeyName] = false
                };

            /// <summary>
            /// Gets or sets whether the shift modifier is active.
            /// </summary>
            internal bool Shift
            {
                get { return modifierActivityByKey[shiftKeyName]; }
                set { modifierActivityByKey[shiftKeyName] = value; }
            }
            /// <summary>
            /// Gets or sets whether the control modifier is active.
            /// </summary>
            internal bool Ctrl 
            {
                get { return modifierActivityByKey[ctrlKeyName]; }
                set { modifierActivityByKey[ctrlKeyName] = value; }
            }

            /// <summary>
            /// Gets or sets whether the alt modifier is active.
            /// </summary>
            internal bool Alt
            {
                get { return modifierActivityByKey[altKeyName]; }
                set { modifierActivityByKey[altKeyName] = value; }
            }

            /// <summary>
            /// Returns a copy of this KeyModifierCollection.
            /// </summary>
            /// <returns>
            /// A KeyModifierCollection with the same key modifiers active as this one.
            /// </returns>
            internal KeyModifierCollection Copy()
            {
                return new KeyModifierCollection
                {
                    Shift = this.Shift,
                    Ctrl = this.Ctrl,
                    Alt = this.Alt
                };
            }

            /// <summary>
            /// Returns whether the provided key name is 
            /// the name of a valid key modifier.
            /// </summary>
            /// <param name="keyName"></param>
            /// <returns></returns>
            internal bool KeyIsModifier(string keyName)
            {
                return modifierActivityByKey.ContainsKey(keyName);
            }

            /// <summary>
            /// Returns a copy of this KeyModifierCollection but with the
            /// modifier corresponding to the specified key enabled
            /// or disabled, depending on the value of the active parameter.
            /// If the key does not represent a valid key modifier, the result
            /// is just a copy of this object with nothing changed.
            /// </summary>
            /// <param name="keyName"></param>
            /// <param name="active"></param>
            /// <returns></returns>
            private KeyModifierCollection WithKeyEnabledOrDisabled(string keyName, bool active)
            {
                KeyModifierCollection newCollection = this.Copy();

                // Checks if the key is an actual modifier.
                if (this.KeyIsModifier(keyName))
                {
                    // Activates or deactivates the key modifier in the copy.
                    newCollection.modifierActivityByKey[keyName] = active;
                }

                return newCollection;
            }

            /// <summary>
            /// Returns a copy of this KeyModifierCollection but with the
            /// modifier corresponding to the specified key enabled.
            /// If the key does not represent a valid key modifier, the result
            /// is just a copy of this object with nothing changed.
            /// </summary>
            /// <param name="keyName"></param>
            /// <returns></returns>
            internal KeyModifierCollection WithKeyEnabled(string keyName)
            {
                return WithKeyEnabledOrDisabled(keyName, true);
            }

            /// <summary>
            /// Returns a copy of this KeyModifierCollection but with the
            /// modifier corresponding to the specified key disabled.
            /// If the key does not represent a valid key modifier, the result
            /// is just a copy of this object with nothing changed.
            /// </summary>
            /// <param name="keyName"></param>
            /// <returns></returns>
            internal KeyModifierCollection WithKeyDisabled(string keyName)
            {
                return WithKeyEnabledOrDisabled(keyName, false);
            }
        }
    }
}
