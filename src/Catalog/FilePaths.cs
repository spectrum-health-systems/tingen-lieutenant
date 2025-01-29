// u250129_code
// u250129_documentation

namespace TingenLieutenant.Catalog
{
    /// <summary>Catalogs of file paths.</summary>
    /// <include file='XmlDoc/TingenLieutenant.Catalog.FilePaths_doc.xml' path='TingenLieutenant.Catalog.FilePaths/Type[@name="Class"]/FilePaths/*'/>
    internal class FilePaths
    {
        /// <summary>The name of the Tingen Lieutenant data root directory.</summary>
        public string LtntDataRoot { get; set; }
        public string LtntSesssionRoot { get; set; }

        /// <summary>The name of the Tingen data root directory.</summary>
        public string TngnDataRoot { get; set; }

        internal static FilePaths GetCommon()
        {
            /* The data roots for Tingen Lieutenant and the Tingen web service are hard coded, and should not be modified.
               */
            const string ltntDataRoot = "./AppData";
            const string tngnDataRoot = "TingenData";

            return new FilePaths()
            {
                LtntDataRoot     = ltntDataRoot,
                LtntSesssionRoot = $@"{ltntDataRoot}/Session",
                TngnDataRoot     = tngnDataRoot
            };
        }

        internal static FilePaths AddTingenServerPaths(FilePaths filePaths, string serverUnc, string tngnDataRoot)
        {
            filePaths.TngnDataRoot = $@"\\{serverUnc}\{tngnDataRoot}";

            return filePaths;
        }
    }
}