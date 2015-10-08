using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrackForegroundWindow
{
    public class ActiveWindowTimeTracker
    {
        private Dictionary<string, TimeSpan> _windowTimeSpan;
        private int _windowMaxStringLength;

        public ActiveWindowTimeTracker()
        {
            _windowMaxStringLength = 0;
            _windowTimeSpan = new Dictionary<string, TimeSpan>();
        }

        public void StartTracking()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    string windowTitle = GetActiveWindowTitle();

                    Thread.Sleep(1000);

                    lock (_windowTimeSpan)
                    {
                        if (String.IsNullOrEmpty(windowTitle))
                            windowTitle = String.Empty;

                        if (windowTitle.Length > _windowMaxStringLength)
                            _windowMaxStringLength = windowTitle.Length;

                        if (_windowTimeSpan.ContainsKey(windowTitle))
                        {
                            _windowTimeSpan[windowTitle] = _windowTimeSpan[windowTitle].Add(TimeSpan.FromSeconds(1));
                        }
                        else
                        {
                            _windowTimeSpan.Add(windowTitle, TimeSpan.FromSeconds(1));
                        }
                    }
                }
            });
        }

        public override string ToString()
        {
            StringBuilder returnString = new StringBuilder();

            lock (_windowTimeSpan)
            {
                foreach (KeyValuePair<string, TimeSpan> kvp in _windowTimeSpan)
                {
                    returnString.AppendLine(String.Format("{0}: {1}", kvp.Key, kvp.Value.ToString().PadLeft(_windowMaxStringLength+2-kvp.Key.Length)));
                }
            }

            return returnString.ToString();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
    }


}
