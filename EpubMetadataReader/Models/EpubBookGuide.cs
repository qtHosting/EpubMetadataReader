using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpubMetadataReader.Models
{
    /// <summary>
    /// This is a storage container for the Guide information stored in an eBook
    /// </summary>
    public class EpubBookGuide
    {
        /// <summary>
        /// A list of References for this Epub
        /// </summary>
        public ReferenceList References { get; set; }
        /// <summary>
        /// Instantiates an empty ReferenceList
        /// </summary>
        public EpubBookGuide()
        {
            References = new ReferenceList();
        }
    }
    /// <summary>
    /// This is an object to hold Reference information
    /// </summary>
    public class Reference
    {
        /// <summary>
        /// The type of file referenced. (Cover, Title-Page, Table of Contents, etc. etc.)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The title of the file
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The location of the file
        /// </summary>
        public string Href { get; set; }

    }
    /// <summary>
    /// This allows us to easily instantiate objects that use List<Reference>
    /// </summary>
    public class ReferenceList : List<Reference>
    {

    }

}
