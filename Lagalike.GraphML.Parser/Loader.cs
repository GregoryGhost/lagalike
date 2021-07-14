namespace Lagalike.GraphML.Parser
{
    using CSharpFunctionalExtensions;

    public class Loader
    {
        private readonly FileReader _fileReader;

        private readonly Parser _parser;

        public Loader(FileReader fileReader, Parser parser)
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