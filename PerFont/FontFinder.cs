using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PerFont
{
    public static class FontFinder
    {
        private static string[] Fonts = System.Drawing.FontFamily.Families.Select(ff => ff.Name).ToArray();

        public static string[] Find(int charCode)
        {
            var r = new List<string>();

            GlyphTypeface glyph;

            foreach (string familyName in Fonts)
            {
                var font = new FontFamily(familyName);
                foreach (Typeface tf in font.GetTypefaces())
                {
                    if (tf.TryGetGlyphTypeface(out glyph))
                    {
                        if (glyph.CharacterToGlyphMap.ContainsKey(charCode))
                        {
                            //Debug.Print("Found in {0} font.", familyName);
                            r.Add(familyName);
                            break;
                        }
                    }
                }
            }

            return r.ToArray();
        }
    }
}
