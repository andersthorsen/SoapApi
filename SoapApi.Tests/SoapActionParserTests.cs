using System;
using FluentAssertions;
using NUnit.Framework;

namespace SoapApi.Tests
{
    [TestFixture]
    public class SoapActionParserTests
    {
        [Test]
        public void Should_Return_LastPartOf_Url()
        {
            var str = "http://www.w3schools.com/webservices/CelsiusToFahrenheit";

            var parser = new SoapActionParser(new Uri(str));

            parser.GetActionName().Should().Be("CelsiusToFahrenheit");
        }

        [Test]
        public void Should_Return_NamespacePartOf_Url()
        {
            var str = "http://www.w3schools.com/webservices/CelsiusToFahrenheit";

            var parser = new SoapActionParser(new Uri(str));

            parser.GetNamespaceUri().Should().Be("http://www.w3schools.com/webservices");
        }
    }
}
