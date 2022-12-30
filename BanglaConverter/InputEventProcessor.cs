using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaConverter
{
    internal abstract class InputEventProcessor
    {
        /// <summary>
        /// The text editor that this processor interacts with.
        /// </summary>
        protected readonly ITextEditor editor;

        internal InputEventProcessor(ITextEditor editor)
        {
            this.editor = editor;
        }

        /// <summary>
        /// Determines whether to perform conversions on input events.
        /// </summary>
        internal bool ConverterEnabled { get; private set; } = true;

        protected void ToggleConverter()
        {
            // Toggles the converter.
            ConverterEnabled = !ConverterEnabled;
            // Lets the editor respond to the new state.
            editor.ConverterToggleHandler(ConverterEnabled);
        }

        internal SharedData.VowelMode CurrentVowelMode { get; private set; } = SharedData.VowelMode.FullVowel;

        protected void ToggleVowelMode()
        {
            // Sets the vowel mode to the opposite of its current value.
            CurrentVowelMode = CurrentVowelMode == SharedData.VowelMode.FullVowel
                ? SharedData.VowelMode.VowelSign
                : SharedData.VowelMode.FullVowel;
            // Lets the editor respond.
            editor.VowelModeChangeHandler(CurrentVowelMode);
        }

        protected void SetVowelMode(SharedData.VowelMode newVowelMode)
        {
            // Toggles the vowel mode if the current vowel mode does not match the desired one.
            if (CurrentVowelMode != newVowelMode)
            {
                ToggleVowelMode();
            }
        }

        /// <summary>
        /// Tracks which key modifiers are currently active, i.e., which modifier keys
        /// are currently being held.
        /// </summary>
        private SharedData.KeyModifierCollection activeKeyModifiers = new SharedData.KeyModifierCollection
        {
            Shift = false,
            Ctrl = false,
            Alt = false
        };

        protected bool ShiftKeyActive => activeKeyModifiers.Shift;
        protected bool CtrlKeyActive => activeKeyModifiers.Ctrl;
        protected bool AltKeyActive => activeKeyModifiers.Alt;

        protected void KeyUpHandler(string keyName)
        {
            // Checks if a modifier key was released.
            if (activeKeyModifiers.KeyIsModifier(keyName))
            {
                // Removes the modifier.
                activeKeyModifiers = activeKeyModifiers.WithKeyDisabled(keyName);
            }
            // The F1 key always toggles the converter.
            else if (keyName == "F1")
            {
                ToggleConverter();
            }
            // Checks if the '/' key was released,
            // but only if the converter is enabled and the shift is not held.
            else if (ConverterEnabled && !activeKeyModifiers.Shift && keyName == "OemQuestion")
            {
                ToggleVowelMode();
            }
        }

        protected virtual void KeyDownHandler(string keyName)
        {
            // Checks if a modifier key was pressed.
            if (activeKeyModifiers.KeyIsModifier(keyName))
            {
                // Adds the modifier.
                activeKeyModifiers = activeKeyModifiers.WithKeyEnabled(keyName);
            }
        }

        protected void ButtonClickHandler(string buttonName) { }

        public void InputEventHandler(SharedData.InputEventType inputEventType, string value)
        {
            switch (inputEventType)
            {
                case SharedData.InputEventType.KeyUp:
                    KeyUpHandler(value);
                    break;
                case SharedData.InputEventType.KeyDown:
                    KeyDownHandler(value);
                    break;
                case SharedData.InputEventType.ButtonClick:
                    ButtonClickHandler(value);
                    break;
                default:
                    break;
            }
        }
    }
}
