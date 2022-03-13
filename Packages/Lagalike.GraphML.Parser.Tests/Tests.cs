namespace Lagalike.GraphML.Parser.Tests
{
    using System.IO;

    using CSharpFunctionalExtensions;

    using FluentAssertions;
    using FluentAssertions.CSharpFunctionalExtensions;

    using NUnit.Framework;

    public class Tests
    {
        private const string NOT_FOUND_GRAPH_ML_FILE_NAME = "not-found-file.graphml";

        private const string TEST_GRAPH_ML_FILE_NAME = "scenes-choices.graphml";

        private readonly TestData _testData = new();

        [Test]
        public void ParseGraphMlFileShouldBeSuccess()
        {
            var parsedGraph = ParseGraphMlFile();

            parsedGraph.Should().BeSuccess();

            var expectedGraph = _testData.GetExpectedGraph();

            parsedGraph.Value.Vertices.Should().BeEquivalentTo(expectedGraph.Vertices);

            parsedGraph.Value.Edges.Should().BeEquivalentTo(expectedGraph.Edges);
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