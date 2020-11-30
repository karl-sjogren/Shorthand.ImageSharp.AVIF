using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.Formats;

namespace Shorthand.ImageSharp.AVIF {
    public class AVIFFormat : IImageFormat {
        public string Name => "AVIF";

        public string DefaultMimeType => "image/avif";

        public IEnumerable<string> MimeTypes => AVIFConstants.MimeTypes;

        public IEnumerable<string> FileExtensions => AVIFConstants.FileExtensions;
    }
}
