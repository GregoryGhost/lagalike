namespace Lagalike.GraphML.Parser
{
    using QuikGraph;

    /// <summary>
    ///     A graph represetation of parsed file in the GraphML format.
    /// </summary>
    public class Graph : AdjacencyGraph<CustomVertex, CustomEdge>
    {
    }

    /// <summary>
    ///     A edge of a parsed graph.
    /// </summary>
    /// <param name="Source">A vertex from.</param>
    /// <param name="Target">A vertex to.</param>
    /// <param name="Text">A text information in the edge.</param>
    public record CustomEdge(CustomVertex Source, CustomVertex Target, string Text) : IEdge<CustomVertex>;

    /// <summary>
    ///     A vertex of a parsed graph.
    /// </summary>
    /// <param name="Text">A text information.</param>
    public record CustomVertex(string Text);

    /// <summary>
    ///     A vertex of a prepared graph.
    /// </summary>
    /// <param name="Id">A the GraphML node id.</param>
    /// <param name="Text">A text information.</param>
    internal record Vertex(string Id, string Text) : CustomVertex(Text);

    /// <summary>
    ///     A edge parsed from a the GraphML file.
    /// </summary>
    /// <param name="SourceId">A id of the GraphML node from.</param>
    /// <param name="TargetId">A id of the GraphML node to.</param>
    /// <param name="Text">A text information.</param>
    internal record GraphMlEdge(string SourceId, string TargetId, string Text) : IGraphMlElement;

    /// <summary>
    ///     A vertex parsed from a the GraphML file.
    /// </summary>
    /// <param name="Id">A the GraphML node id.</param>
    /// <param name="Text">A text information.</param>
    internal record GraphMlVertex(string Id, string Text) : Vertex(Id, Text), IGraphMlElement;

    /// <summary>
    ///     A contract for the GraphML elements - edges, vertexes.
    /// </summary>
    internal interface IGraphMlElement
    {
    }
}