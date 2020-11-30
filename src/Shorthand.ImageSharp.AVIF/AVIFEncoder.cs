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
            var tempPath = Path.GetTempPath();
            var randomId = Guid.NewGuid();
            var randomIdPath = Path.Combine(tempPath, randomId.ToString());

            var processArguments = $"{randomIdPath}.png {randomIdPath}.avif";


            if(Lossless) {
                processArguments = $"--lossless {randomIdPath}.png {randomIdPath}.avif";
            }

            var psi = new ProcessStartInfo {
                FileName = Native.CAVIF,
                Arguments = processArguments,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            try {
                image.SaveAsPng($"{randomIdPath}.png");

                var process = Process.Start(psi);
                process.WaitForExit((Int32)TimeSpan.FromSeconds(20).TotalMilliseconds);

                using(var fs = File.OpenRead($"{randomIdPath}.avif")) {
                    fs.CopyTo(stream);
                }
            } finally {
                if(File.Exists($"{randomIdPath}.png"))
                    File.Delete($"{randomIdPath}.png");

                if(File.Exists($"{randomIdPath}.avif"))
                    File.Delete($"{randomIdPath}.avif");
            }
        }

        public async Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel> {
            var tempPath = Path.GetTempPath();
            var randomId = Guid.NewGuid();
            var randomIdPath = Path.Combine(tempPath, randomId.ToString());

            var processArguments = $"{randomIdPath}.png {randomIdPath}.avif";

            if(Lossless) {
                processArguments = $"--lossless {randomIdPath}.png {randomIdPath}.avif";
            }

            var psi = new ProcessStartInfo {
                FileName = Native.CAVIF,
                Arguments = processArguments,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            try {
                image.SaveAsPng($"{randomIdPath}.png");

                var process = Process.Start(psi);
                process.WaitForExit((Int32)TimeSpan.FromSeconds(20).TotalMilliseconds);

                using(var fs = File.OpenRead($"{randomIdPath}.avif")) {
                    await fs.CopyToAsync(stream).ConfigureAwait(false);
                }
            } finally {
                if(File.Exists($"{randomIdPath}.png"))
                    File.Delete($"{randomIdPath}.png");

                if(File.Exists($"{randomIdPath}.avif"))
                    File.Delete($"{randomIdPath}.avif");
            }
        }
    }
}
