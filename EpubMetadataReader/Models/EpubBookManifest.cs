using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpubMetadataReader.Models
{
    /// <summary>
    /// This holds a list of the Epub's content files.
    /// </summary>
    public class EpubBookManifest
    {
        /// <summary>
        /// List of Epub Content Files
        /// </summary>
        public ItemList Item { get; set; }
        /// <summary>
        /// Instantiates a new list of Items contained in an Epub
        /// </summary>
        public EpubBookManifest()
        {
            Item = new ItemList();
        }
    }
    /// <summary>
    /// Information about a file used for displaying an Epub's information
    /// </summary>
    public class Item
    {
        /// <summary>
        /// An Item ID referenced by the Spine
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// The Location of the file
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// The Type of file
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// If the file isn't specified in the format, this is the ID of its fallback
        /// </summary>
        public string Fallback { get; set; }
        /// <summary>
        /// Any required namespace for this file.
        /// </summary>
        public string RequiredNamespace { get; set; }
        /// <summary>
        /// A style fallback if the style isn't supported.
        /// </summary>
        public string FallbackStyle { get; set; }
        /// <summary>
        /// Any modules required to display this File
        /// </summary>
        public string RequiredModules { get; set; }
    }
    /// <summary>
    /// An easy way to instantiate an object of List<Item>
    /// </summary>
    public class ItemList : List<Item>
    {

    }
}
