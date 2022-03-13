namespace Lagalike.GraphML.Parser.Tests
{
    using System.Collections.Generic;

    /// <summary>
    ///     This test data need to unit tests.
    /// </summary>
    public class TestData
    {
        private static readonly Dictionary<Scene, CustomVertex> Scenes = new()
        {
            {
                Scene.Scene1Root, new CustomVertex("Сцена1 - бла-бла много текста", ToGraphMlVertexNumber(Scene.Scene1Root))
            },
            {
                Scene.Scene2, new CustomVertex("Сцена 2 бла-бла", ToGraphMlVertexNumber(Scene.Scene2))
            },
            {
                Scene.Scene3, new CustomVertex("Сцена 3 бла-бла", ToGraphMlVertexNumber(Scene.Scene3))
            },
            {
                Scene.Scene4, new CustomVertex("Сцена 4 бла-бла", ToGraphMlVertexNumber(Scene.Scene4))
            },
            {
                Scene.Scene5, new CustomVertex("Сцена 5 бла-бла", ToGraphMlVertexNumber(Scene.Scene5))
            },
            {
                Scene.Scene6, new CustomVertex("Сцена 6 бла-бла", ToGraphMlVertexNumber(Scene.Scene6))
            },
            {
                Scene.Scene7, new CustomVertex("Сцена 7 бла-бла", ToGraphMlVertexNumber(Scene.Scene7))
            },
            {
                Scene.Scene8, new CustomVertex("Сцена 8 бла-бла", ToGraphMlVertexNumber(Scene.Scene8))
            }
        };

        private static readonly CustomEdge[] Edges =
        {
            new(Scenes[Scene.Scene1Root], Scenes[Scene.Scene2], "вар1"),
            new(Scenes[Scene.Scene2], Scenes[Scene.Scene5], "вар2.5"),
            new(Scenes[Scene.Scene2], Scenes[Scene.Scene6], "вар2.6"),
            new(Scenes[Scene.Scene1Root], Scenes[Scene.Scene3], "вар2"),
            new(Scenes[Scene.Scene3], Scenes[Scene.Scene6], "вар3.6"),
            new(Scenes[Scene.Scene1Root], Scenes[Scene.Scene4], "вар3"),
            new(Scenes[Scene.Scene4], Scenes[Scene.Scene7], "вар4.7"),
            new(Scenes[Scene.Scene4], Scenes[Scene.Scene8], "вар4.8"),
        };

        /// <summary>
        ///     Get a extected graph for the <see cref="scene-choises.graphml" />.
        /// </summary>
        /// <returns>Returns a graph representation.</returns>
        public Graph GetExpectedGraph()
        {
            var graph = new Graph();
            graph.AddVerticesAndEdgeRange(Edges);

            return graph;
        }

        private static string ToGraphMlVertexNumber(Scene scene)
        {
            return $"n{scene:D}";
        }
    }

    internal enum Scene
    {
        Scene1Root = 0,

        Scene2,

        Scene3,

        Scene4,

        Scene5,

        Scene6,

        Scene7,

        Scene8
    }
}