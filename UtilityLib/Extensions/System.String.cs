// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).

namespace UtilityLib {
    public static partial class StringExtensions {
        public static string TrimToLength(this string Text, int MaxLength, string AppendText = "") {
            return (Text.Length <= MaxLength) ? Text : Text.Substring(0,MaxLength) + AppendText; 
        }
        public static string ReplaceCR(this string Text, string Replacement) {
            return Text.Replace("\r", "\n").Replace("\n\n", "\n").Replace("\n", Replacement);
        }
        public static string EliminateAllWhitespace(this string Text) { return EliminateExcessWhitespace(Text).Replace(" ",""); }
        public static string EliminateExcessWhitespace(this string Text) { // Escape Sequences: msdn.microsoft.com/en-us/library/h21280bw.aspx
            Text = Text.Replace("\t", " ")
                       .Replace("\r", " ")
                       .Replace("\n", " ")
                       .Replace("\v", " ")
                       .Replace("\f", " ")
                       .Trim();
            while (Text != Text.Replace("  "," ")) Text = Text.Replace("  "," ");
            return Text;
        }
    }
}
