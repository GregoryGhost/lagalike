namespace Lagalike.GraphML.Parser
{
    using System.IO;

    using CSharpFunctionalExtensions;

    /// <summary>
    ///     The loader works with parsing a file in the GraphML format.
    /// </summary>
    public class Loader
    {
        private readonly FileReader _fileReader;

        private readonly Parser _parser;

        /// <summary>
        ///     Construct the loader.
        /// </summary>
        /// <param name="fileReader">A file reader of files in the GraphML format.</param>
        /// <param name="parser">A parser converts a XML representation of a file in the GraphML format to a adjacency graph.</param>
        public Loader(FileReader fileReader, Parser parser)
        {
            _fileReader = fileReader;
            _parser = parser;
        }

        /// <summary>
        ///     Parse a file of the GraphML format.
        /// </summary>
        /// <param name="fileName">A file name.</param>
        /// <returns>Returns a graph representation of a file in the GraphML format or errors of the parsing proccess.</returns>
        public Result<Graph, ParseError> ParseFile(string fileName)
        {
            var parsedGraph = _fileReader.ReadGraphMlFile(fileName).Bind(_parser.Parse);

            return parsedGraph;
        }

        public Result<Graph, ParseError> ParseFile(MemoryStream document)
        {
            var parsedGraph = _fileReader.ReadGraphMlFile(document).Bind(_parser.Parse);

            return parsedGraph;
        }
    }
}