namespace Lagalike.GraphML.Parser.Tests
{
    using System.IO;

    using CSharpFunctionalExtensions;

    using FluentAssertions.CSharpFunctionalExtensions;

    using NUnit.Framework;

    public class Tests
    {
        private const string NOT_FOUND_GRAPH_ML_FILE_NAME = "not-found-file.graphml";

        private const string TEST_GRAPH_ML_FILE_NAME = "scenes-choices.graphml";

        [Test]
        public void ParseGraphMlFileShouldBeCorrect()
        {
            const string TestGraphMlFileName = "scenes-choices.graphml";
            var actualProgramPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(actualProgramPath, TestGraphMlFileName);

            var loader = GetLoader();
            var parsedGraph = loader.ParseFile(filePath);

            parsedGraph.Should().BeSuccess();
        }

        [Test]
        public void ParseGraphMlFileShouldBeSuccess()
        {
            var parsedGraph = ParseGraphMlFile();

            parsedGraph.Should().BeSuccess();
        }

        [Test]
        public void ParseNoFoundGraphMlFile()
        {
            var parsedGraph = ParseNotFoundGraphMlFile();

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

        private static Result<Graph, ParseError> ParseGraphMlFile(string fileName = TEST_GRAPH_ML_FILE_NAME)
        {
            var actualProgramPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(actualProgramPath, fileName);

            var loader = GetLoader();
            var parsedGraph = loader.ParseFile(filePath);

            return parsedGraph;
        }

        private static Result<Graph, ParseError> ParseNotFoundGraphMlFile()
        {
            return ParseGraphMlFile(NOT_FOUND_GRAPH_ML_FILE_NAME);
        }
    }
}