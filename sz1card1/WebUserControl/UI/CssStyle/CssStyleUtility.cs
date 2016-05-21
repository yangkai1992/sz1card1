using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebUserControl.UI.CssStyle
{
    /// <summary>
    /// Utility class for working with Cascading Style Sheet (CSS)
    /// style attributes
    /// </summary>
    /// <remarks>
    /// The <b>CssStyleUtility</b> class encapsulates a
    /// <see cref="StringDictionary">StringDictionary</see>, allowing for
    /// the setting and retrieval of CSS style properties.  The constructor
    /// takes a string parameter, representing the text in a style attribute,
    /// and parses the string to populate the internal StringDictionary.
    /// The <see cref="StyleTable">StyleTable</see> property provides access
    /// to the internal dictionary, and the <see cref="ToString">ToString()</see>
    /// method outputs the items of the dictionary as a complete CSS style string.
    /// </remarks>
    public class CssStyleUtility
    {
        // this class would probably not be necessary if we could
        // use the CssStyleCollection instead; too bad we can't.
        // we'll use this class to assist in handling css style
        // properties, particularly when we need to add our own
        // style attributes

        private StringDictionary _table = new StringDictionary();

        /// <summary>
        /// Returns the internal dictionary of style properties
        /// encapsulated within this CssStyleUtility object
        /// </summary>
        public StringDictionary StyleTable
        {
            get { return _table; }
        }

        /// <summary>
        /// Constructs a CssStyleUtility object, given a string of text
        /// in the form of a CSS style attribute (in the form of
        /// "key1: value1; key2: value2;" etc.)
        /// </summary>
        /// <param name="cssStyleString">the CSS style attribute text to be parsed</param>
        public CssStyleUtility(string cssStyleString)
        {
            // cssStyleString should be something like this:
            // "border: 1px solid gray; background-color: blue;"
            // parse it
            string[] arr = cssStyleString.Split(';');
            foreach (string sPair in arr)
            {
                string[] arr2 = sPair.Split(':');
                // valid key and value pair?
                if (arr2.Length == 2)
                {
                    // include in our internal hashtable
                    _table.Add(arr2[0].Trim().ToLower(), arr2[1].Trim());
                }
            }

        }

        /// <summary>
        /// Joins the property items within this CssStyleUtility object
        /// to return a single text string in the form of a CSS style attribute
        /// </summary>
        /// <returns>the CSS style attribute text</returns>
        public override string ToString()
        {
            string sReturn = "";
            foreach (string key in _table.Keys)
            {
                if (sReturn.Length > 0) sReturn += "; ";
                sReturn += string.Format("{0}: {1}", key, _table[key]);
            }
            return sReturn;
        }

    }
}
