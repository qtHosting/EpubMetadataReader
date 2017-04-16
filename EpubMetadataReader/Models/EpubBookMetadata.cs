using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpubMetadataReader.Models
{
    public class EpubBookMetadata
    {
        /// <summary>
        /// A list of titles for the Epub
        /// </summary>
        public List<string> Title { get; set; }
        /// <summary>
        /// A list of Primary Creators/Authors for the Epub
        /// </summary>
        public List<Contributor> Creator { get; set; }
        /// <summary>
        /// A list of Subjects the Epub is about
        /// </summary>
        public List<string> Subject { get; set; }
        /// <summary>
        /// A list of descriptions describing the content of the Epub
        /// </summary>
        public List<string> Description { get; set; }
        /// <summary>
        /// A list of publishers responsible for the Epub
        /// </summary>
        public List<string> Publisher { get; set; }
        /// <summary>
        /// A list of Contributors who are not authors/creators of the Epub
        /// Artists, Annotators, Corporate Authors, Arrangers, and anyone else associated with the Epub
        /// </summary>
        public List<Contributor> Contributor { get; set; }
        /// <summary>
        /// A list of Publication Dates, and possible associated events (Creation, Modification, Publication, etc.)
        /// </summary>
        public List<Date> Date { get; set; }
        /// <summary>
        /// A list of Categories, Functions, Genres, etc. for the Epub
        /// </summary>
        public List<string> Type { get; set; }
        /// <summary>
        /// A list of Media Types for the Epub
        /// </summary>
        public List<string> Format { get; set; }
        /// <summary>
        /// A list of Identifier elements used in the Epub
        /// </summary>
        public List<Identifier> Identifier { get; set; }
        /// <summary>
        /// A list of prior resources from which the Epub was derived.
        /// </summary>
        public List<string> Source { get; set; }
        /// <summary>
        /// A list of languages that the Epub has
        /// </summary>
        public List<string> Language { get; set; }
        /// <summary>
        /// A list of auxiliary resources
        /// </summary>
        public List<string> Relation { get; set; }
        /// <summary>
        /// A list designed to explain the scope or extent of the Epub
        /// </summary>
        public List<string> Coverage { get; set; }
        /// <summary>
        /// A list of statements/references to rights-based information, ie Copyright
        /// </summary>
        public List<string> Rights { get; set; }
        
        /// <summary>
        /// Instatiates the lists for holding Metadata found in Open Publication Format documents.
        /// </summary>
        public EpubBookMetadata()
        {
            Title = new List<string>();
            Creator = new List<Contributor>();
            Subject = new List<string>();
            Description = new List<string>();
            Publisher = new List<string>();
            Contributor = new List<Contributor>();
            Date = new List<Date>();
            Type = new List<string>();
            Format = new List<string>();
            Identifier = new List<Identifier>();
            Source = new List<string>();
            Language = new List<string>();
            Relation = new List<string>();
            Coverage = new List<string>();
            Rights = new List<string>();
        }
    }

    /// <summary>
    /// This object defines the different metadata that a contributor has.
    /// Examples include Authors, Artists, Researchers, Corporate Authors, and many more. 
    /// </summary>
    public class Contributor
    {
        /// <summary>
        /// The specific type of Contributor
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// The Name of the Contributor
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// How the Contributor should be listed
        /// </summary>
        public string FileAs { get; set; }
    }
    /// <summary>
    /// This object holds information related to Publication Dates - it has an event field for further sorting.
    /// </summary>
    public class Date
    {
        /// <summary>
        /// The Date of the Event
        /// </summary>
        public string EventDate { get; set; }
        /// <summary>
        /// The type of Event
        /// </summary>
        public string Event { get; set; }
    }
    /// <summary>
    /// This is an Identifier object. The ID can be used to match with the Unique-Identifier in the OPF Package.
    /// </summary>
    public class Identifier
    {
        /// <summary>
        /// This should match the Unique-Identifier in the Package section
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// The Organization or Authority that created the text contained within the Identifier Element. IE ISBN or DOI
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// The Publication Identifier
        /// </summary>
        public string UniqueID { get; set; }
    }
}
