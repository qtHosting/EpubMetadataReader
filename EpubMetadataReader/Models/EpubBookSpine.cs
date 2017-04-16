using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpubMetadataReader.Models
{
    /// <summary>
    /// An object designed to hold the information from the Spine portion of the Epub
    /// </summary>
    public class EpubBookSpine
    {
        /// <summary>
        /// A list object of Spine Items
        /// </summary>
        public ItemRefList ItemRefs { get; set; }
        /// <summary>
        /// Instantiates a new list of Spine Items
        /// </summary>
        public EpubBookSpine()
        {
            ItemRefs = new ItemRefList();
        }

    }
    /// <summary>
    /// Spine Items
    /// </summary>
    public class ItemRef
    {
        /// <summary>
        /// A reference to a manifest item's ID
        /// </summary>
        public string IdRef { get; set; }
        /// <summary>
        /// A designation of whether or not an object is auxiliary to the main flow of the Epub
        /// </summary>
        public string Linear { get; set; }
    }
    /// <summary>
    /// An easy way to instatiate list objects of List<ItemRef>
    /// </summary>
    public class ItemRefList : List<ItemRef>
    {

    }

}
