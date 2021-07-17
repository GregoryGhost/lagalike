namespace Lagalike.GraphML.Parser
{
    using QuikGraph;

    public class Graph : AdjacencyGraph<CustomVertex, CustomEdge>
    {
    }

    public record CustomEdge(CustomVertex Source, CustomVertex Target, string Text) : IEdge<CustomVertex>;

    public record CustomVertex(string Text);

    internal record Vertex(string Id, string Text) : CustomVertex(Text);

    internal record GraphMlEdge(string SourceId, string TargetId, string Text) : IGraphMlElement;

    internal record GraphMlVertex(string Id, string Text) : Vertex(Id, Text), IGraphMlElement;

    internal interface IGraphMlElement
    {
    }
}