using System.Runtime.InteropServices;

namespace Enterprise.TextEncoding.ConsoleEncoding;

public static class ConsoleEncodingService
{
    public static void AllowSpecialCharacters()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            SetConsoleOutputCP(1256);
            SetConsoleCP(1256);
        }
        else
        {
            // TODO: Implement Linux-specific behavior or a cross-platform solution.
        }
    }

    public static void RevertEncoding()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
        }
        else
        {
            // TODO: Implement Linux-specific behavior or a cross-platform solution.
        }
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleOutputCP(uint wCodePageId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleCP(uint wCodePageId);
}