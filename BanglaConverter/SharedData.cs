using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaConverter
{
    internal static class SharedData
    {
        /// <summary>
        /// A delegate type used to perform actions on UI elements
        /// that are inaccessible from the calling class.
        /// </summary>
        internal delegate void UICallback();
    }
}
