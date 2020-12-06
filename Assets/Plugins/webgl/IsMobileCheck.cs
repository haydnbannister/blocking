using System.Runtime.InteropServices;

public static class IsMobileCheck
{
    [DllImport("__Internal")]
    public static extern bool IsMobile();
}
