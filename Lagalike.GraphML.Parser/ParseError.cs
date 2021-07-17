namespace Lagalike.GraphML.Parser
{
    using EnumWithValues;

    [EnumWithValues("ParseErrors", true, true)]
    public enum ParseError
    {
        [EnumValue("File not found for provided filename")]
        NotFoundFile,

        [EnumValue("Wrong format xml document")]
        WrongFormatXmlDocument,

        [EnumValue("Wrong gotten GraphML format")]
        ParseGraphMlFormat,

        [EnumValue("Not found node by id")]
        PrepareGraph,
    }
}