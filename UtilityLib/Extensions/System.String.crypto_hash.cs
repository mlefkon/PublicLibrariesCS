// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Text;
using System.Security.Cryptography;

namespace UtilityLib {
    public static partial class StringExtensions {
        static SHA256 _sha256 = null; // more efficient to just create these objects once.
        static SHA512 _sha512 = null;
        static MD5    _md5    = null;
        static readonly object _lockerMD5 = new object();    // Instance methods of Crypto-objects are not thread safe.
        static readonly object _lockerSHA256 = new object();
        static readonly object _lockerSHA512 = new object();
        public static byte[] HashMD5(this string Text) { 
                if (_md5 == null) _md5 = MD5.Create();
                lock (_lockerMD5) {
                    return _md5.ComputeHash(Encoding.UTF8.GetBytes(Text));
                }
            }
            public static string HashMD5Hex(this string Text) { 
                return BitConverter.ToString(Text.HashMD5()).Replace("-", ""); // produces a 32 HEX string
            }
            public static string HashMD5Base64(this string Text) { // produces 22 char string
                return Convert.ToBase64String(Text.HashMD5()).TrimEnd('='); // Bas64 will append '=' to end to assure reconvert to same length. Can be trimmed off.
            }
        public static byte[] HashSHA256(this string Text) { 
                if (_sha256 == null) _sha256 = SHA256.Create();
                lock (_lockerSHA256) {
                    return _sha256.ComputeHash(Encoding.UTF8.GetBytes(Text));
                }
            }
            public static string HashSHA256Hex(this string Text) { 
                return BitConverter.ToString(Text.HashSHA256()); // produces a 64 char string
            }
            public static string HashSHA256Base64(this string Text) { 
                return Convert.ToBase64String(Text.HashSHA256()).TrimEnd('='); // Bas64 will append '=' to end to assure reconvert to same length. Can be trimmed off.
            }
        public static byte[] HashSHA512(this string Text) { 
                if (_sha512 == null) _sha512 = SHA512.Create();
                lock (_lockerSHA512) {
                    return _sha512.ComputeHash(Encoding.UTF8.GetBytes(Text)); // produces a 128 char string
                }
            } 
            public static string HashSHA512Hex(this string Text) { 
                return BitConverter.ToString(Text.HashSHA512()); // produces a 128 char string
            } 
            public static string HashSHA512Base64(this string Text) { 
                return Convert.ToBase64String(Text.HashSHA512()).TrimEnd('='); // Bas64 will append '=' to end to assure reconvert to same length. Can be trimmed off.
            } 
    }
}
