using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaConverter
{
    internal class InputEvent
    {
        public SharedData.InputEventType InputEventType { get; private set; }

        /// <summary>
        /// Represents the name of the key pressed or released, or the name of the button clicked.
        /// </summary>
        public string Value { get; private set; }

        public InputEvent(SharedData.InputEventType inputEventType, string value)
        {
            InputEventType = inputEventType;
            Value = value;
        }
    }
}
