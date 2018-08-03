using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.IO
{
    public class WrappingLoggingStream : Stream
    {
        readonly Stream stream;
        readonly bool ownStream;

        public override bool CanRead => stream.CanRead;

        public override bool CanSeek => stream.CanSeek;

        public override bool CanWrite => stream.CanWrite;

        public override long Length => stream.Length;

        public override long Position { get => stream.Position; set => stream.Position = value; }

        public WrappingLoggingStream(
            Stream stream,
            bool ownStream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
            this.ownStream = ownStream;
        }

        protected override void Dispose(bool disposing)
        {
            if (ownStream)
            {
                stream.Dispose();
            }
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var originalPosition = CanSeek ? Position : (long?)null;
            var result = stream.Read(buffer, offset, count);
            Console.WriteLine($"{nameof(Read)}[+{originalPosition}(, {offset}, {count}) = {result}");
            return result;
        }

        public async override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var originalPosition = CanSeek ? Position : (long?)null;
            var result = await base.ReadAsync(buffer, offset, count, cancellationToken);
            Console.WriteLine($"{nameof(ReadAsync)}[+{originalPosition}](, {offset}, {count}) = {result}");
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            stream.Write(buffer, offset, count);
        }
    }
}
