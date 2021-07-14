namespace Lagalike.GraphML.Parser
{
    using System.ComponentModel;

    public enum ParseError
    {
        [Description("File not found for provided filename")]
        NotFoundFile,

        [Description("Wrong format xml document")]
        WrongFormatXmlDocument,

        [Description("Wrong gotten GraphML format")]
        ParseGraphMlFormat,

        [Description("Not found node by id")]
        PrepareGraph,
    }
}