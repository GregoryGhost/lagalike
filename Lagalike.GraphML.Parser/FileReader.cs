namespace Lagalike.GraphML.Parser
{
    using System.IO;
    using System.Xml.Linq;

    using CSharpFunctionalExtensions;

    using FileName = System.String;

    public class FileReader
    {
        public Result<XDocument, ParseError> ReadGraphMlFile(string fileName)
        {
            return CheckExistsGraphmlFile(fileName).Bind(ReadFile);
        }

        private static Result<string, ParseError> CheckExistsGraphmlFile(string filename)
        {
            return File.Exists(filename) ? filename : ParseError.NotFoundFile;
        }

        private static Result<XDocument, ParseError> ReadFile(string fileName)
        {
            var xDoc = XDocument.Load(fileName);

            return xDoc;
        }
    }
}