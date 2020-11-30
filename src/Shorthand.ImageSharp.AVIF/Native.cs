using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Shorthand.ImageSharp.AVIF
{
    public static class Native {
        public static string CAVIF => Path.Combine("native", OSFolder, "avifenc") + ExecutableExtension;

        private static string OSFolder {
            get {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return "linux";

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    if(Environment.Is64BitProcess)
                        return "win-x64";

                    throw new InvalidOperationException("Only x64 is supported on Windows.");
                }

                throw new InvalidOperationException("Somehow this system seems unsupported.");
            }
        }

        private static string ExecutableExtension {
            get {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return string.Empty;

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return ".exe";

                throw new InvalidOperationException("Somehow this system seems unsupported.");
            }
        }
    }
}
