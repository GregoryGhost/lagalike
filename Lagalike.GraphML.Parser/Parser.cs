namespace Lagalike.GraphML.Parser
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    using CSharpFunctionalExtensions;

    using MoreLinq.Extensions;

    /// <summary>
    ///     The parser converts a XML representation of a file in the GraphML format to a adjacency graph.
    /// </summary>
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

        /// <summary>
        ///     Parse a XML document to a adjacency graph.
        /// </summary>
        /// <param name="xmlDocument">A XML document with data in the GraphML format.</param>
        /// <returns>Returns a adjacency graph or errors of parsing proccess..</returns>
        public Result<Graph, ParseError> Parse(XContainer xmlDocument)
        {
            return ParseGraphMlFormat(xmlDocument).Bind(PrepareGraph);
        }

        private static Graph CollectGraphElements(GraphMlEdge graphMlEdge,
            IReadOnlyDictionary<string, CustomVertex> vertexIndexes, Graph graph)
        {
            var (sourceVertexId, targetVertexId, edgeText) = graphMlEdge;
            var isFoundSourceVertex = vertexIndexes.TryGetValue(sourceVertexId, out var sourceVertex);
            var isFoundTargetVertex = vertexIndexes.TryGetValue(targetVertexId, out var targetVertex);
            if (!isFoundSourceVertex || !isFoundTargetVertex || sourceVertex == null || targetVertex == null)
                return graph;

            var edge = new CustomEdge(sourceVertex, targetVertex, edgeText);
            graph.AddVerticesAndEdge(edge);

            return graph;
        }

        private static List<IGraphMlElement> CollectGraphMlElements(List<IGraphMlElement> elements, XElement node)
        {
            var graphMlElementType = node.Name.LocalName.ToLowerInvariant();
            switch (graphMlElementType)
            {
                case NODE:
                {
                    var nodeId = FindNodeAttributeByKey(node, NODE_ID_KEY) ?? "empty";
                    var nodeText = FindNodeTextByAttributeKey(node, NODE_TEXT_ATTR_KEY) ?? "";
                    var vertex = new GraphMlVertex(nodeId, nodeText);
                    elements.Add(vertex);

                    return elements;
                }
                case EDGE:
                {
                    var sourceNodeId = FindNodeAttributeByKey(node, VERTEX_SOURCE_ATTR_KEY) ?? "empty";
                    var targetNodeId = FindNodeAttributeByKey(node, VERTEX_TARGET_ATTR_KEY) ?? "empty";
                    var edgeText = FindNodeTextByAttributeKey(node, EDGE_TEXT_ATTR_KEY) ?? "";
                    var edge = new GraphMlEdge(sourceNodeId, targetNodeId, edgeText);
                    elements.Add(edge);

                    return elements;
                }
                default:
                    Debug.WriteLine("Can't match xElement type. It was ignored.");
                    return elements;
            }
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
                    var wasFoundKey = FindNodeAttributeByKey(x, ATTR_TAG_KEY) == attributeKey;
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
                CollectGraphMlElements);

            return parsedGraphMlElements.Any()
                ? parsedGraphMlElements
                : Result.Failure<List<IGraphMlElement>, ParseError>(ParseError.ParseGraphMlFormat);
        }

        private static Result<Graph, ParseError> PrepareGraph(List<IGraphMlElement> parsedGraphMlElements)
        {
            var (edges, vertixes) = parsedGraphMlElements.Partition(x => x is GraphMlEdge);
            var vertexIndexes = vertixes.Cast<Vertex>().ToDictionary(
                x => x.Id,
                vertex => new CustomVertex(vertex.Text, vertex.Id));
            var preparedGraph = edges.Cast<GraphMlEdge>().Aggregate(
                new Graph(),
                (graph, graphMlEdge) => CollectGraphElements(graphMlEdge, vertexIndexes, graph));

            return preparedGraph;
        }
    }
}