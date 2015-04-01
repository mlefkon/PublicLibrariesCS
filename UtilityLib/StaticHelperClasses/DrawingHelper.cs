// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).

namespace UtilityLib {
    public static class DrawingHelper {
        public static System.Drawing.Icon GetEmbeddedResourceIcon(string IconName) { 
            // Note: IconName format: NameSpace/OptionalRootFolder/OptionalSubFolder/FileName.XXX
            System.Reflection.Assembly a = System.Reflection.Assembly.GetCallingAssembly();
            System.IO.Stream st = a.GetManifestResourceStream(IconName);
            System.Drawing.Icon icn = new System.Drawing.Icon(st);
            return(icn);
        }
        public static System.Drawing.Bitmap GetEmbeddedResourceBitmap(string BitmapName) { 
            // Note: IconName format: NameSpace/OptionalRootFolder/OptionalSubFolder/FileName.XXX
            System.Reflection.Assembly a = System.Reflection.Assembly.GetCallingAssembly();
            System.IO.Stream st = a.GetManifestResourceStream(BitmapName);
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(st);
            return(img);
        }
    }
}
