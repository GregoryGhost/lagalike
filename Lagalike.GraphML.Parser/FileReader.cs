namespace Lagalike.GraphML.Parser
{
    using System;
    using System.IO;

    using CSharpFunctionalExtensions;

    using FileName = System.String;

    public class FileReader
    {
        public Result<GraphNodes, ParseError> ReadGraphMlFile(string fileName)
        {
            return CheckExistsGraphmlFile(fileName).Bind(ReadFile);
        }

        private static Result<GraphNodes, ParseError> ReadFile(string fileName)
        {
            throw new NotImplementedException();
        }

        private static Result<string, ParseError> CheckExistsGraphmlFile(string filename)
        {
            return File.Exists(filename) ? filename : ParseError.NotFoundFile;
        }
    }
}