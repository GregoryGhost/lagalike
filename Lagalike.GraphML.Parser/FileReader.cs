namespace Lagalike.GraphML.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    using CSharpFunctionalExtensions;

    using QuikGraph;

    using FileName = System.String;

    public class FileReader
    {
        public Result<XDocument, ParseError> ReadGraphMlFile(string fileName)
        {
            return CheckExistsGraphmlFile(fileName).Bind(ReadFile);
        }

        private static Result<XDocument, ParseError> ReadFile(string fileName)
        {
            var xDoc = XDocument.Load(fileName);


            return xDoc;
        }

        private static Result<string, ParseError> CheckExistsGraphmlFile(string filename)
        {
            return File.Exists(filename) ? filename : ParseError.NotFoundFile;
        }
    }

    public record CustomEdge : IEdge<Vertex>
    {
        public string Text { get; init; } = null!;

        public Vertex Source { get; init; } = null!;

        public Vertex Target { get; init; } = null!;
    }

    public record Vertex
    {
        public string Id { get; init; } = null!;

        public string Text { get; init; } = null!;
    }
}