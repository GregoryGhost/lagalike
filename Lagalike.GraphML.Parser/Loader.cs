namespace Lagalike.GraphML.Parser
{
    using CSharpFunctionalExtensions;

    public class Loader
    {
        private readonly Reader _fileReader;

        private readonly Parser _parser;

        public Loader(Reader fileReader, Parser parser)
        {
            _fileReader = fileReader;
            _parser = parser;
        }

        public Result<Graph, ParseError> ParseFile(string fileName)
        {
            var parsedGraph = _fileReader.ReadGraphMlFile(fileName).Bind(_parser.Parse);

            return parsedGraph;
        }
    }
}