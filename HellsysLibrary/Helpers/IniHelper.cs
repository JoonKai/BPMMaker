using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HellsysLibrary.Helpers
{
    public class IniHelper
    {
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string savePath { get => AppDomain.CurrentDomain.BaseDirectory + "\\settings.ini"; }

        public string Read(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, savePath);
            return temp.ToString().Trim();
        }
        public string Read(string section, string key, string init_val)
        {
            /** 저장변수 */
            string val = Read(section, key);

            /** 읽은값이 Null 이면 init_val 값으로 변경 */
            if (val == null || val == "")
            {
                val = init_val;
                Write(section, key, val);
            }

            return val;
        }
        public string Write(string section, string key, string val)
        {
            string strError;

            try
            {
                WritePrivateProfileString(section, key, val, savePath);
                strError = null;

                return strError;
            }
            catch (Exception exError)
            {
                strError = string.Format("[SYSTEM] : {0}", exError.Message);

                return strError;
            }
        }
    }
}
