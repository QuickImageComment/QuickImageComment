// suggested by Microsoft Copilot
using System;
using System.Drawing;

public sealed class DpiMonitor : IDisposable
{
    public event EventHandler<DpiChangedEventArgs> DpiChanged;

    private int _currentDpi;

    public DpiMonitor()
    {
        _currentDpi = GetSystemDpi();
        Microsoft.Win32.SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
    }

    private void OnDisplaySettingsChanged(object sender, EventArgs e)
    {
        int newDpi = GetSystemDpi();
        if (newDpi != _currentDpi)
        {
            int old = _currentDpi;
            _currentDpi = newDpi;
            DpiChanged?.Invoke(this, new DpiChangedEventArgs(old, newDpi));
        }
    }

    public void Dispose()
    {
        Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
    }

    private static int GetSystemDpi()
    {
        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            return (int)g.DpiX;
    }
}

public sealed class DpiChangedEventArgs : EventArgs
{
    public int OldDpi { get; }
    public int NewDpi { get; }

    public DpiChangedEventArgs(int oldDpi, int newDpi)
    {
        OldDpi = oldDpi;
        NewDpi = newDpi;
    }
}
