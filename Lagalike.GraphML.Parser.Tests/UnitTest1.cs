using NUnit.Framework;

namespace Lagalike.GraphML.Parser.Tests
{
    using System.IO;

    using FluentAssertions.CSharpFunctionalExtensions;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadGraphMlFile()
        {
            var fileReader = new FileReader();
            const string TestGraphMlFileName = "scenes-choices.graphml";
            var actualProgramPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(actualProgramPath, TestGraphMlFileName);
            var readedFile = fileReader.ReadGraphMlFile(TestGraphMlFileName);

            readedFile.Should().BeSuccess();
        }
    }
}