using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaConverter
{
    internal interface ITextEditor
    {
        /// <summary>
        /// Responds to the input event processor's converter
        /// being enabled or disabled.
        /// </summary>
        /// <param name="enabled">Indicates whether the converter is now enabled.</param>
        public void ConverterToggleHandler(bool enabled);
        /// <summary>
        /// Responds to the vowel mode being changed.
        /// </summary>
        /// <param name="vowelMode">Indicates the new vowel mode.</param>
        public void VowelModeChangeHandler(SharedData.VowelMode vowelMode);
        /// <summary>
        /// Responds to input event processor state changes not handled
        /// by other event handlers.
        /// </summary>
        public void ProcessorStateChangeHandler();
        /// <summary>
        /// Stores the text in the document.
        /// </summary>
        /// <param name="text"></param>
        public void WriteText(string text);
        /// <summary>
        /// The text currently in the document.
        /// </summary>
        public string CurrentText { get; }
        /// <summary>
        /// Returns the position of the cursor in the input element,
        /// or the starting position of the selected text.
        /// </summary>
        public int CurrentPosition { get; }
    }
}
