namespace Lagalike.GraphML.Parser.Tests
{
    using System.Collections.Generic;

    public class TestData
    {
        public Graph GetExpectedGraph()
        {
            var scenes = new Dictionary<Scene, CustomVertex>
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
            var edges = new[]
            {
                new CustomEdge(scenes[Scene.Scene1Root], scenes[Scene.Scene2], "вар1"),
                new CustomEdge(scenes[Scene.Scene2], scenes[Scene.Scene5], "вар2.5"),
                new CustomEdge(scenes[Scene.Scene2], scenes[Scene.Scene6], "вар2.6"),
                new CustomEdge(scenes[Scene.Scene1Root], scenes[Scene.Scene3], "вар2"),
                new CustomEdge(scenes[Scene.Scene3], scenes[Scene.Scene6], "вар3.6"),
                new CustomEdge(scenes[Scene.Scene1Root], scenes[Scene.Scene4], "вар3"),
                new CustomEdge(scenes[Scene.Scene4], scenes[Scene.Scene7], "вар4.7"),
                new CustomEdge(scenes[Scene.Scene4], scenes[Scene.Scene8], "вар4.8"),
            };
            var graph = new Graph();
            graph.AddVerticesAndEdgeRange(edges);

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