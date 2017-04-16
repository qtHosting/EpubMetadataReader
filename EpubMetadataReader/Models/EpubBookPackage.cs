using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpubMetadataReader.Models
{
    /// <summary>
    /// This holds all of the Package Information that is used in the Epub File
    /// </summary>
    public class EpubBookPackage
    {
        /// <summary>
        /// Contains a list of all package information for this Epub.
        /// </summary>
        public PackageList Package { get; set; }
        /// <summary>
        /// Instantiates a new Package List;
        /// </summary>
        public EpubBookPackage()
        {
            Package = new PackageList();
        }
    }

    /// <summary>
    /// An Epub should normally have just one package, but it's possible to use multiple opf files, so we have to have a list of each for each Epub
    /// </summary>
    public class Package
    {
        /// <summary>
        /// A Unique Identifier
        /// </summary>
        public string UniqueIdentifier { get; set; }
        /// <summary>
        /// The Epub format version
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Namespace for Dublin Core Metadata
        /// </summary>
        public string DCNameSpace { get; set; }
        /// <summary>
        /// Namespace for Open Packaging Format
        /// </summary>
        public string OPFNameSpace { get; set; }
    }

    /// <summary>
    /// This allows us to easily instantiate objects that use List<Package>
    /// </summary>
    public class PackageList : List<Package>
    {

    }
}
