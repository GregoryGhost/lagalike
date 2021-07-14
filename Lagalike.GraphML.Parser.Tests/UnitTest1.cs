namespace Lagalike.GraphML.Parser.Tests
{
    using System.Diagnostics;
    using System.IO;

    using FluentAssertions.CSharpFunctionalExtensions;

    using NUnit.Framework;

    public class Tests
    {
        [Test]
        public void ParseGraphMlFile()
        {
            const string TestGraphMlFileName = "scenes-choices.graphml";
            var actualProgramPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(actualProgramPath, TestGraphMlFileName);

            var loader = GetLoader();
            var parsedGraph = loader.ParseFile(filePath);

            parsedGraph.Should().BeSuccess();
        }
        
        [Test]
        public void ParseNoFoundGraphMlFile()
        {
            const string TestGraphMlFileName = "no-existed.graphml";
            var actualProgramPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(actualProgramPath, TestGraphMlFileName);

            var loader = GetLoader();
            var parsedGraph = loader.ParseFile(filePath);
            
            parsedGraph.Should().BeFailure(ParseErrors.NotFoundFile);
        }

        [SetUp]
        public void Setup()
        {
        }

        private static Loader GetLoader()
        {
            return new Loader(new FileReader(), new Parser());
        }
    }
}