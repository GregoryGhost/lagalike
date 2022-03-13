namespace Lagalike.GraphML.Parser
{
    using EnumWithValues;

    /// <summary>
    ///     Errors of parsing a GraphML file.
    /// </summary>
    [EnumWithValues("ParseErrors", true, true)]
    public enum ParseError
    {
        /// <summary>
        ///     A file not found for provided filename.
        /// </summary>
        [EnumValue("The file not found for provided filename")]
        NotFoundFile,

        /// <summary>
        ///     A wrong format XML document.
        /// </summary>
        [EnumValue("Wrong format XML document")]
        WrongFormatXmlDocument,

        /// <summary>
        ///     The parser got a wrong data. Required a data in the GraphML format.
        /// </summary>
        [EnumValue("Wrong gotten GraphML format")]
        ParseGraphMlFormat,

        /// <summary>
        ///     The parser didn't find node by a id.
        /// </summary>
        [EnumValue("Not found node by id")]
        PrepareGraph,
    }
}