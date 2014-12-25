using System.IO;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace SoapApi.Tests
{
    [TestFixture]
    public class BackupStreamTests
    {
        [Test]
        public void Shuld_Copy_Read_Data_Read()
        {
            var ms = new MemoryStream();
            ms.WriteByte(1);
            ms.WriteByte(2);
            ms.WriteByte(3);
            ms.WriteByte(4);
            ms.WriteByte(5);
            ms.WriteByte(6);

            ms.Seek(0, SeekOrigin.Begin);

            var backupStream = new BackupStream(ms);

            backupStream.ReadByte();
            backupStream.ReadByte();

            backupStream.Stream.Length.Should().Be(2);

        }

        public void CanRead_Should_Return_True()
        {
            var ms = new MemoryStream();
            var backupStream = new BackupStream(ms);

            backupStream.CanRead.Should().Be(true);
        }

        [Test]
        public void CanWrite_Should_Return_False()
        {
            var ms = new MemoryStream();
            var backupStream = new BackupStream(ms);

            backupStream.CanWrite.Should().Be(false);
        }

        [Test]
        public void CanSeek_Should_Return_False()
        {
            var ms = new MemoryStream();
            var backupStream = new BackupStream(ms);

            backupStream.CanSeek.Should().Be(false);
        }

        [Test]
        public void CanSeek_Should_Return_Length_Of_Underlying_Stream()
        {
            var ms = Mock.Of<Stream>();

            Mock.Get(ms).Setup(x => x.Length).Returns(12);

            var backupStream = new BackupStream(ms);

            backupStream.Length.Should().Be(12);
        }

        [Test]
        public void CanSeek_Should_Return_Position_Of_Underlying_Stream()
        {
            var ms = Mock.Of<Stream>();

            Mock.Get(ms).Setup(x => x.Position).Returns(12);

            var backupStream = new BackupStream(ms);

            backupStream.Position.Should().Be(12);
        }
    }
}
