namespace Lagalike.GraphML.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    using CSharpFunctionalExtensions;

    using MoreLinq.Extensions;

    public class Parser
    {
        private const string ATTR_TAG_KEY = "key";

        private const string EDGE = "edge";

        private const string EDGE_TEXT_ATTR_KEY = "d10";

        private const string NODE = "node";

        private const string NODE_ID_KEY = "id";

        private const string NODE_TEXT_ATTR_KEY = "d3";

        private const string TAG_DATA = "data";

        private const string TAG_LABEL = "Label";

        private const string TAG_LABEL_TEXT = "Label.Text";

        private const string TAG_LIST = "List";

        private const string VERTEX_SOURCE_ATTR_KEY = "source";

        private const string VERTEX_TARGET_ATTR_KEY = "target";

        public Result<Graph, ParseError> Parse(XContainer xDoc)
        {
            return ParseGraphMlFormat(xDoc).Bind(PrepareGraph);
        }

        private static string? FindNodeAttributeByKey(XElement node, string attributeKey)
        {
            var foundNode = node.Attributes().FirstOrDefault(x => x.Name.LocalName.Contains(attributeKey));

            return foundNode?.Value;
        }

        private static string? FindNodeTextByAttributeKey(XContainer node, string attributeKey)
        {
            var data = node.Elements().FirstOrDefault(
                x =>
                {
                    var wasFoundKey = FindNodeAttributeByKey(x, ATTR_TAG_KEY) != null;
                    var wasFoundTag = x.Name.LocalName == TAG_DATA;
                    
                    return wasFoundTag && wasFoundKey;
                });
            var nodeList = data?.Elements().FirstOrDefault(x => x.Name.LocalName == TAG_LIST);
            var nodeLabel = nodeList?.Elements().FirstOrDefault(x => x.Name.LocalName == TAG_LABEL);
            var nodeLabelText = nodeLabel?.Elements().FirstOrDefault(x => x.Name.LocalName == TAG_LABEL_TEXT);

            return nodeLabelText?.Value;
        }

        private static Result<List<IGraphMlElement>, ParseError> ParseGraphMlFormat(XContainer xDoc)
        {
            var parsedGraphMlElements = xDoc.Descendants().Aggregate(
                new List<IGraphMlElement>(),
                (acc, node) =>
                {
                    switch (node.Name.LocalName)
                    {
                        case NODE:
                            var vertex = new GraphMlVertex
                            {
                                Id = FindNodeAttributeByKey(node, NODE_ID_KEY) ?? "empty",
                                Text = FindNodeTextByAttributeKey(node, NODE_TEXT_ATTR_KEY) ?? ""
                            };
                            acc.Add(vertex);
                            return acc;
                        case EDGE:
                            var edge = new GraphMlEdge
                            {
                                SourceId = FindNodeAttributeByKey(node, VERTEX_SOURCE_ATTR_KEY) ?? "empty",
                                TargetId = FindNodeAttributeByKey(node, VERTEX_TARGET_ATTR_KEY) ?? "empty",
                                Text = FindNodeTextByAttributeKey(node, EDGE_TEXT_ATTR_KEY) ?? ""
                            };
                            acc.Add(edge);

                            return acc;
                        default:
                            Debug.WriteLine("Can't match xElement type. It was ignored.");
                            return acc;
                    }
                });

            return parsedGraphMlElements.Any()
                ? parsedGraphMlElements
                : Result.Failure<List<IGraphMlElement>, ParseError>(ParseError.ParseGraphMlFormat);
        }

        private static Result<Graph, ParseError> PrepareGraph(List<IGraphMlElement> parsedGraphMlElements)
        {
            var (edges, vertixes) = parsedGraphMlElements.Partition(x => x is GraphMlEdge);
            var vertexIndexes = vertixes.Cast<Vertex>().ToDictionary(x => x.Id);
            var graph = edges.Cast<GraphMlEdge>().Aggregate(
                new Graph(),
                (acc, x) =>
                {
                    var isFoundSourceVertex = vertexIndexes.TryGetValue(x.SourceId, out var sourceVertex);
                    var isFoundTargetVertex = vertexIndexes.TryGetValue(x.SourceId, out var targetVertex);
                    if (!isFoundSourceVertex || !isFoundTargetVertex || sourceVertex == null || targetVertex == null)
                        return acc;

                    var edge = new CustomEdge
                    {
                        Source = sourceVertex,
                        Target = targetVertex,
                        Text = x.Text
                    };
                    acc.AddVerticesAndEdge(edge);

                    return acc;
                });

            return graph;
        }
    }

    internal record GraphMlEdge : IGraphMlElement
    {
        public string SourceId { get; init; } = null!;

        public string TargetId { get; init; } = null!;

        public string Text { get; init; } = null!;
    }

    internal record GraphMlVertex : Vertex, IGraphMlElement
    {
    }

    internal interface IGraphMlElement
    {
    }
}