// u250129_code
// u250129_documentation

namespace TingenLieutenant.Catalog
{
    /// <summary>Catalogs of file names.</summary>
    /// <include file='XmlDoc/TingenLieutenant.Catalog.FileNames_doc.xml' path='TingenLieutenant.Catalog.FileNames/Type[@name="Class"]/FileNames/*'/>
    internal class FileNames
    {
        /// <summary>The name of the configuration file for the Tingen Lieutenant.</summary>
        /// <remarks>This is hard coded, and should not be modified.</remarks>
        public string LtntConfig { get; set; } = "TngnLtnt.config";

        /// <summary>Add comment here</summary>
        /// <returns></returns>
        internal static FileNames GetCommon()
        {
            return new FileNames()
            {
                // modifications go here
            };
        }
    }
}