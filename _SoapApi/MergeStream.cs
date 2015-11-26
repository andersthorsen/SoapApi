using System;
using System.IO;
using System.Linq;

namespace SoapApi
{
    public class MergeStream : Stream
    {
        private readonly Stream[] _streams;
        private Stream _current;

        public MergeStream(params Stream[] streams)
        {
            _streams = streams;
            _current = _streams.First(x => x.Length == -1 || x.Position < x.Length);
        }

        public override bool CanRead
        {
            get { return _streams.All(x => x.CanRead) ; }
        }

        public override bool CanSeek
        {
            get { return _streams.All(x => x.CanSeek); }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get
            {
                if (_streams.Any(x => x.Length == -1))
                    return -1;

                return _streams.Sum(x => x.Length);
            }
        }

        public override long Position
        {
            get
            {
                long pos = 0;
                foreach (var stream in _streams)
                {
                    if (stream == _current)
                        return pos + _current.Position;

                    pos += stream.Length;
                }

                throw new InvalidOperationException("No current stream");
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            /* Try to read from current */

            var totalRead = _current.Read(buffer, offset, count);

            while (totalRead < count)
            {

                if (!MoveNextStream())
                {
                    return totalRead;
                }

                var read = _current.Read(buffer, offset + totalRead, count - totalRead);

                totalRead += read;
            }

            return totalRead;
        }

        private bool MoveNextStream()
        {
            var next = _streams.SkipWhile(x => x != _current).ElementAtOrDefault(1);

            if (next == null)
                return false;

            _current = next;
            return true;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {

            if (!CanSeek)
                throw new NotSupportedException();

            /* Find stream to set to current */

            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}