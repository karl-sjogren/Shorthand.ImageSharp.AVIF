using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace Shorthand.ImageSharp.AVIF {
    public class AVIFEncoder : IImageEncoder {
        public static AVIFEncoder Instance { get; } = new AVIFEncoder();

        //
        // Summary:
        //     Gets or sets the quality, that will be used to encode the image. Quality index
        //     must be between 0 and 100 (compression from max to min). Defaults to null (lossless).
        public bool Lossless { get; set; }

        public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel> {
            var randomId = Guid.NewGuid();
            var processArguments = $"{randomId}.png {randomId}.avif";

            if(Lossless) {
                processArguments = $"--lossless {randomId}.png {randomId}.avif";
            }

            var psi = new ProcessStartInfo {
                FileName = Native.CAVIF,
                Arguments = processArguments,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            try {
                image.SaveAsPng($"{randomId}.png");

                var process = Process.Start(psi);
                process.WaitForExit((Int32)TimeSpan.FromSeconds(20).TotalMilliseconds);

                using(var fs = File.OpenRead($"{randomId}.avif")) {
                    fs.CopyTo(stream);
                }
            } finally {
                if(File.Exists($"{randomId}.png"))
                    File.Delete($"{randomId}.png");

                if(File.Exists($"{randomId}.avif"))
                    File.Delete($"{randomId}.avif");
            }
        }

        public async Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel> {
            var randomId = Guid.NewGuid();
            var processArguments = $"{randomId}.png {randomId}.avif";

            if(Lossless) {
                processArguments = $"--lossless {randomId}.png {randomId}.avif";
            }

            var psi = new ProcessStartInfo {
                FileName = Native.CAVIF,
                Arguments = processArguments,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            try {
                image.SaveAsPng($"{randomId}.png");

                var process = Process.Start(psi);
                process.WaitForExit((Int32)TimeSpan.FromSeconds(20).TotalMilliseconds);

                using(var fs = File.OpenRead($"{randomId}.avif")) {
                    await fs.CopyToAsync(stream);
                }
            } finally {
                if(File.Exists($"{randomId}.png"))
                    File.Delete($"{randomId}.png");
                    
                if(File.Exists($"{randomId}.avif"))
                    File.Delete($"{randomId}.avif");
            }
        }
    }
}
