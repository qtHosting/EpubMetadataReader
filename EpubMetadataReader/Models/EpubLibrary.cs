using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpubMetadataReader.Models
{
    /// <summary>
    /// An entire Library of Epub information.
    /// </summary>
    public class EpubLibrary
    {
        /// <summary>
        /// A list of Epub books in your library.
        /// </summary>
        public List<EpubBook> Books { get; set; }
        /// <summary>
        /// Instantiates a new Epub Library with an empty book list.
        /// </summary>
        public EpubLibrary()
        {
            Books = new List<EpubBook>();
        }
    }
    
    public class EpubBook
    {
        /// <summary>
        /// Epub Package Information for a single book.
        /// </summary>
        public List<EpubBookPackage> Package { get; set; }
        /// <summary>
        /// Epub Metadata Information for a single book.
        /// </summary>
        public List<EpubBookMetadata> Metadata { get; set; }
        /// <summary>
        /// Epub Manifest Information for a single book.
        /// </summary>
        public List<EpubBookManifest> Manifest { get; set; }
        /// <summary>
        /// Epub Spine Information for a single book.
        /// </summary>
        public List<EpubBookSpine> Spine { get; set; }
        /// <summary>
        /// Epub Guid Information for a single book.
        /// </summary>
        public List<EpubBookGuide> Guide { get; set; }
        /// <summary>
        /// Creates a new Epub Book with empty Package, Metadata, Manifest, Spine, and Guide list objects.
        /// </summary>
        public EpubBook()
        {
            Package = new List<EpubBookPackage>();
            Metadata = new List<EpubBookMetadata>();
            Manifest = new List<EpubBookManifest>();
            Spine = new List<EpubBookSpine>();
            Guide = new List<EpubBookGuide>();
        }
    }
}
