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
                Scene.Scene1Root, new CustomVertex("Сцена1 - бла-бла много текста")
            },
            {
                Scene.Scene2, new CustomVertex("Сцена 2 бла-бла")
            },
            {
                Scene.Scene3, new CustomVertex("Сцена 3 бла-бла")
            },
            {
                Scene.Scene4, new CustomVertex("Сцена 4 бла-бла")
            },
            {
                Scene.Scene5, new CustomVertex("Сцена 5 бла-бла")
            },
            {
                Scene.Scene6, new CustomVertex("Сцена 6 бла-бла")
            },
            {
                Scene.Scene7, new CustomVertex("Сцена 7 бла-бла")
            },
            {
                Scene.Scene8, new CustomVertex("Сцена 8 бла-бла")
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