// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System.IO;
using System.IO.Compression;

namespace UtilityLib {
    public static partial class ByteArrayExtensions {
        public static byte[] Decompress(this byte[] ZippedData) {
            byte[] decompressedData = null;
            using (MemoryStream outputStream = new MemoryStream()) {
                using (MemoryStream inputStream = new MemoryStream(ZippedData)) {
                    using (GZipStream zip = new GZipStream(inputStream, CompressionMode.Decompress)) {
                        zip.CopyTo(outputStream);
                    }
                }
                decompressedData = outputStream.ToArray();
            }
            return decompressedData;
        }
        public static byte[] Compress(this byte[] PlainData) {
            byte[] compressesData = null;
            using (MemoryStream outputStream = new MemoryStream()) {
                using (GZipStream zip = new GZipStream(outputStream, CompressionMode.Compress)) {
                    zip.Write(PlainData, 0, PlainData.Length);                    
                }
                //Dont get the MemoryStream data before the GZipStream is closed since it doesn’t yet contain complete compressed data.
                //GZipStream writes additional data including footer information when its been disposed
                compressesData = outputStream.ToArray();
            }
            return compressesData;
        }
    }
}
