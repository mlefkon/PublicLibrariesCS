// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.IO;
using System.Linq;

namespace UtilityLib {
    public class LogFile {
        private string _filePathName;
        public LogFile(string FilePathName) {
            _filePathName = FilePathName;
        }
        public int MaxFileSizeKB { set { _maxBytes = 1024 *        value; } }
        public int MaxFileSizeMB { set { _maxBytes = 1024 * 1024 * value; } }
            private Int64 _maxBytes = 0;
        private void WriteToFile(string Text) {
            FileInfo fi = new FileInfo(_filePathName);
            lock (this) {
                if (!fi.Exists) {
                    Directory.CreateDirectory(fi.Directory.ToString());
                    File.WriteAllLines(_filePathName, new string[] {DateTime.Now.ToString() + " - LOG INITIALIZED"} );
                    fi = new FileInfo(_filePathName);
                }
                if (_maxBytes != 0 && fi.Length > _maxBytes) TrimFile(25);
                File.AppendAllLines(_filePathName, new string[] {Text});
            }
        }
        private void TrimFile(int TrimPercent) {
            if (TrimPercent < 1 || TrimPercent > 99) throw new Exception("Percent should be 1 to 99");
            string[] lines = File.ReadAllLines(_filePathName);
            int startLine = Convert.ToInt32( lines.Length * TrimPercent/100 );
            File.WriteAllLines(_filePathName, lines.Where( (s,i) => i>=startLine ));
            File.AppendAllLines(_filePathName, new string[] {DateTime.Now.ToString() + " - LOG TRIMMED (by "+TrimPercent+"%)"});
        }
        public void LogInfo(string Text) {
            WriteToFile(DateTime.Now.ToString() + " - INFO: " + MultilinePrepend(Text, "    ", false) );
        }
        public void LogError(Exception Ex) {
            string text = 
                DateTime.Now.ToString() + " - ERROR: " + Environment.NewLine +
                "    Source: " + Ex.Source + Environment.NewLine +
                "    Message: " + Environment.NewLine + MultilinePrepend(Ex.MessageFull(Environment.NewLine), "        - ") + Environment.NewLine +
                "    TargetSite: " + Ex.TargetSite + Environment.NewLine +
                "    StackTrace: " + Environment.NewLine + MultilinePrepend(Ex.StackTrace, "        - ")
                ;
                //Ex.Data ?
            WriteToFile(text);
        }
	    private string MultilinePrepend(string Text, string PrependText, bool IncludeFirstLine = true) {
		    return String.Join(Environment.NewLine, Text.Split(new char[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
                                                        .Select((t,i) => (i == 0 && IncludeFirstLine == false ? "" : PrependText) + t )
								                        .ToArray());
        }
    }
}
