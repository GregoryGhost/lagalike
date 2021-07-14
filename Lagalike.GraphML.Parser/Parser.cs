namespace Lagalike.GraphML.Parser
{
    using System;

    using CSharpFunctionalExtensions;

    public class Parser
    {
        public Result<Graph, ParseError> Parse(GraphNodes nodes)
        {
            return ParseGraphMlFormat(nodes).Bind(PrepareGraph);
        }

        private Result<Graph, ParseError> ParseGraphMlFormat(GraphNodes nodes)
        {
            throw new NotImplementedException();
        }

        private Result<Graph, ParseError> PrepareGraph(Graph parsedGraph)
        {
            throw new NotImplementedException();
        }
    }
}