namespace Lagalike.GraphML.Parser
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    using CSharpFunctionalExtensions;

    using FileName = System.String;

    /// <summary>
    ///     The file reader of files in the GraphML format.
    /// </summary>
    public class FileReader
    {
        /// <summary>
        ///     Read a file of the GraphML format.
        /// </summary>
        /// <param name="fileName">A file name.</param>
        /// <returns>Returns parsed a file of the GraphML format as a XML document.</returns>
        public Result<XDocument, ParseError> ReadGraphMlFile(string fileName)
        {
            return CheckExistsGraphmlFile(fileName).Bind(ReadFile);
        }

        public Result<XDocument, ParseError> ReadGraphMlFile(MemoryStream document)
        {
            try
            {
                document.Position = 0;
                return XDocument.Load(document);
            }
            catch (Exception)
            {
                return Result.Failure<XDocument, ParseError>(ParseError.WrongFormatXmlDocument);
            }
        }

        private static Result<string, ParseError> CheckExistsGraphmlFile(string filename)
        {
            return File.Exists(filename) ? filename : ParseError.NotFoundFile;
        }

        private static Result<XDocument, ParseError> ReadFile(string fileName)
        {
            try
            {
                var xDoc = XDocument.Load(fileName);

                return xDoc;
            }
            catch (Exception)
            {
                return Result.Failure<XDocument, ParseError>(ParseError.WrongFormatXmlDocument);
            }
        }
    }
}