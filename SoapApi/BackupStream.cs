using System;
using System.IO;

namespace SoapApi
{
    public class BackupStream : Stream
    {
        public BackupStream(Stream innerStream)
        {
            InnerStream = innerStream;
            Stream = new MemoryStream();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {

            var read = InnerStream.Read(buffer, offset, count);

            if (read > 0)
            {
                Stream.Write(buffer, offset, read);
            }

            return read;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return InnerStream.Length; }
        }

        public override long Position
        {
            get
            {
                return InnerStream.Position;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public MemoryStream Stream { get; private set; }
        public Stream InnerStream { get; private set; }
    }
}