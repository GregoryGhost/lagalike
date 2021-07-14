namespace Lagalike.GraphML.Parser
{
    using System.ComponentModel;

    using EnumWithValues;

    [EnumWithValues("ParseErrors", convertEnumValue: true, throwIfCastFails: true)]
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