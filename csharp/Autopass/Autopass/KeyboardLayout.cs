using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Threading;


namespace Autopass
{
    public class KeyboardLayout
    {
        [DllImport("user32.dll")]
         private static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
        

        [DllImport("user32.dll")]
        private static extern long GetKeyboardLayoutName(StringBuilder pwszKLID);

        private static uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
        private static int HWND_BROADCAST = 0xffff;
        private static string en_US = "00000409";
        private static uint KLF_ACTIVATE = 1;
        const int KL_NAMELENGTH = 9;
        
        //each keyboard layout is defined in Windows as a hex code
        public static string GetLayoutCode()
        {
            var name = new StringBuilder(KL_NAMELENGTH);
            GetKeyboardLayoutName(name);
            return name.ToString();
        }
       
        public static void SwitchToDefaultLanguage(String locale)
        {
            PostMessage(HWND_BROADCAST, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(locale, KLF_ACTIVATE));
        }

        public static void SwitchToEnUSLanguage()
        {
            PostMessage(HWND_BROADCAST, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(en_US, KLF_ACTIVATE));
        }
    }
}
