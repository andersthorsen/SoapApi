using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Moq;

namespace SoapApi.Tests
{
    [TestFixture]
    public class MergeStreamTests
    {
        [Test]
        public void Should_Read_Both_Streams_When_ReadingMore_Data_Than_First_Can_Provide()
        {
            var ms1 = new MemoryStream(new byte[] {0, 1, 2, 3, 4});
            var ms2 = new MemoryStream(new byte[] {5, 6, 7, 8, 9});

            var merged = new MergeStream(ms1, ms2);

            var buffer = new byte[7];

            merged.Read(buffer, 0, 7);

            buffer.Should().BeEquivalentTo(new byte[] {0, 1, 2, 3, 4, 5, 6});
        }

        [Test]
        public void Should_Continue_On_Second_Stream_When_ReadingMore_Data_Than_First_Can_Provide()
        {
            var ms1 = new MemoryStream(new byte[] { 0, 1, 2, 3, 4 });
            var ms2 = new MemoryStream(new byte[] { 5, 6, 7, 8, 9 });

            var merged = new MergeStream(ms1, ms2);

            var buffer = new byte[7];

            merged.Read(buffer, 0, 7);

            buffer = new byte[7];

            var read = merged.Read(buffer, 0, 7);
            buffer.Should().BeEquivalentTo(new byte[] {7, 8, 9, 0, 0, 0, 0});
        }

        [Test]
        public void Should_Return_Bytes_Read_When_Reading_Less_Than_Provided()
        {
            var ms1 = new MemoryStream(new byte[] { 0, 1, 2, 3, 4 });
            var ms2 = new MemoryStream(new byte[] { 5, 6, 7, 8, 9 });

            var merged = new MergeStream(ms1, ms2);

            var buffer = new byte[7];

            merged.Read(buffer, 0, 7);

            buffer = new byte[7];

            var read = merged.Read(buffer, 0, 7);
            read.Should().Be(3);
        }

        [Test]
        public void Should_Return_Length_Of_Underlying_Streams_Combined()
        {
            var ms1 = new MemoryStream(new byte[999]);
            var ms2 = new MemoryStream(new byte[999]);

            var merged = new MergeStream(ms1, ms2);

            merged.Length.Should().Be(1998);
        }

        [Test]
        public void Should_Return_Length_NegativeOne_If_Any_Underlying_Does_So()
        {
            var ms1 = new MemoryStream(new byte[999]);
            var ms2 = Mock.Of<Stream>();

            Mock.Get(ms2).SetupGet(x => x.Length).Returns(-1);

            var merged = new MergeStream(ms1, ms2);

            merged.Length.Should().Be(-1);
        }

        [Test]
        public void Shuld_Return_CanRead_True_When_Underlying_Streams_Can_Read()
        {
            var ms1 = new MemoryStream(new byte[999]);
            var ms2 = new MemoryStream(new byte[999]);

            var merged = new MergeStream(ms1, ms2);

            merged.CanRead.Should().BeTrue();
        }

        [Test]
        public void Shuld_Return_CanRead_False_When_Not_All_Underlying_Streams_Can_Read()
        {
            var ms1 = new MemoryStream(new byte[999]);
            var ms2 = Mock.Of<Stream>();

            Mock.Get(ms2).SetupGet(x => x.CanRead).Returns(false);

            var merged = new MergeStream(ms1, ms2);

            merged.CanRead.Should().BeFalse();
        }

        [Test]
        public void Shuld_Return_CanSeek_True_When_Underlying_Streams_Can_Seek()
        {
            var ms1 = new MemoryStream(new byte[999]);
            var ms2 = new MemoryStream(new byte[999]);

            var merged = new MergeStream(ms1, ms2);

            merged.CanSeek.Should().BeTrue();
        }

        [Test]
        public void Shuld_Return_CanSeek_False_When_Not_All_Underlying_Streams_Can_Seek()
        {
            var ms1 = new MemoryStream(new byte[999]);
            var ms2 = Mock.Of<Stream>();

            Mock.Get(ms2).SetupGet(x => x.CanSeek).Returns(false);

            var merged = new MergeStream(ms1, ms2);

            merged.CanSeek.Should().BeFalse();
        }

        [Test]
        public void Shuld_Return_CanWrite_False()
        {
            var ms1 = new MemoryStream(new byte[999]);
            var ms2 = Mock.Of<Stream>();

            Mock.Get(ms2).SetupGet(x => x.CanWrite).Returns(true);

            var merged = new MergeStream(ms1, ms2);

            merged.CanWrite.Should().BeFalse();
        }

        [Test]
        public void Shuld_Return_Position_For_Current_Stream_Plus_LengthOf_Previous_Stream_When_First_Stream_Is_Current()
        {
            var ms1 = Mock.Of<Stream>();
            var ms2 = Mock.Of<Stream>();

            Mock.Get(ms1).SetupGet(x => x.Length).Returns(999);
            Mock.Get(ms1).SetupGet(x => x.Position).Returns(234);

            var merged = new MergeStream(ms1, ms2);

            merged.Position.Should().Be(234);
        }

        [Test]
        public void Shuld_Return_Position_For_Current_Stream_Plus_LengthOf_Previous_Stream_When_Second_Stream_Is_Current()
        {
            var ms1 = Mock.Of<Stream>();
            var ms2 = Mock.Of<Stream>();

            Mock.Get(ms1).SetupGet(x => x.Length).Returns(234);
            Mock.Get(ms1).SetupGet(x => x.Position).Returns(234);

            Mock.Get(ms2).SetupGet(x => x.Length).Returns(999);
            Mock.Get(ms2).SetupGet(x => x.Position).Returns(234);

            var merged = new MergeStream(ms1, ms2);

            merged.Position.Should().Be(468);
        }
    }
}
