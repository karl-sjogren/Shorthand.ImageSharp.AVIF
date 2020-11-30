using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Xunit;

namespace Shorthand.ImageSharp.AVIF.Tests {
    public class AVIFEncoderTests {
        [Fact]
        public void EncodeSimpleImage() {
            using (var image = new Image<Rgba32>(20, 20)) {
                image.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FF6347")));
                using(var ms = new MemoryStream()) {
                    image.Save(ms, new AVIFEncoder());

                    Assert.True(ms.Length > 0, "Output stream should not be empty.");
                    Assert.InRange(ms.Length, 300, 340);
                }
            }
        }

        [Fact]
        public void EncodeFromFile() {
            using (var image = Image.Load("Resources/test.jpg")) {
                using(var ms = new MemoryStream()) {
                    image.Save(ms, new AVIFEncoder());

                    Assert.True(ms.Length > 0, "Output stream should not be empty.");
                    Assert.InRange(ms.Length, 54000, 58000);
                }
            }
        }
        [Fact]
        public async Task EncodeSimpleImageAsync() {
            using var image = new Image<Rgba32>(20, 20);
            image.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FF6347")));

            await using var ms = new MemoryStream();
            await image.SaveAsync(ms, new AVIFEncoder());

            Assert.True(ms.Length > 0, "Output stream should not be empty.");
            Assert.InRange(ms.Length, 300, 340);
        }

        [Fact]
        public async Task EncodeFromFileAsync() {
            using var image = await Image.LoadAsync("Resources/test.jpg");

            await using var ms = new MemoryStream();
            await image.SaveAsync(ms, new AVIFEncoder());

            Assert.True(ms.Length > 0, "Output stream should not be empty.");
            Assert.InRange(ms.Length, 55000, 58000);
        }

        [Fact]
        public async Task EncodeFromFileAndSetLosslessAsync() {
            using var image = await Image.LoadAsync("Resources/test.jpg");

            await using var ms = new MemoryStream();
            await image.SaveAsync(ms, new AVIFEncoder { Lossless = true });

            Assert.True(ms.Length > 0, "Output stream should not be empty.");
            Assert.InRange(ms.Length, 480000, 490000);
        }

        [Fact]
        public async Task EncodeFromFileAndSetLossyAsync() {
            using var image = await Image.LoadAsync("Resources/test.jpg");

            await using var ms = new MemoryStream();
            await image.SaveAsync(ms, new AVIFEncoder {Lossless = false });

            Assert.True(ms.Length > 0, "Output stream should not be empty.");
            Assert.InRange(ms.Length, 55000, 58000);
        }
    }
}
