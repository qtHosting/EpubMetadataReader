using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpubMetadataReader.Models;
using System.IO.Compression;
using System.Xml.Linq;
using System.Xml;

namespace EpubMetadataReader
{
    public class Epub
    {
        #region PrivateVariables
        private List<string> AcceptedVersions = new List<string>() { "2.0", "3.0" };
        #endregion

        #region PublicVariables
        /// <summary>
        /// The Package information for this Book
        /// </summary>
        public EpubBookPackage BookPackage { get; set; }
        /// <summary>
        /// The Metadata information for this Book
        /// </summary>
        public EpubBookMetadata BookMetadata { get; set; }
        /// <summary>
        /// The Manifest information for this Book
        /// </summary>
        public EpubBookManifest BookManifest { get; set; }
        /// <summary>
        /// The Spine information for this Book
        /// </summary>
        public EpubBookSpine BookSpine { get; set; }
        /// <summary>
        /// The Guide information for this Book
        /// </summary>
        public EpubBookGuide BookGuide { get; set; }
        /// <summary>
        /// The Book being loaded from the Epub
        /// </summary>
        public EpubBook Book { get; set; }
        #endregion

        #region ClassInstantiation
        /// <summary>
        /// Instantiates a new Epub object with blanks EpubBook Lists
        /// </summary>
        public Epub()
        {
            Reset();
        }
        /// <summary>
        /// Instantiates a new Empty Epub Object and loads data from a specified Epub.
        /// </summary>
        /// <param name="strEpubLocation">The location of the Epub file you want to pull data from</param>
        public Epub(string strEpubLocation)
        {
            Reset();
            GetEbookInformation(strEpubLocation);
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// Loads the Book Object with all of the Epub Data.
        /// </summary>
        private void LoadEbookInformation()
        {
            Book.Guide.Add(BookGuide);
            Book.Manifest.Add(BookManifest);
            Book.Metadata.Add(BookMetadata);
            Book.Package.Add(BookPackage);
            Book.Spine.Add(BookSpine);
        }
        /// <summary>
        /// Searches the Epub file for OPF information and uses it.
        /// </summary>
        /// <param name="strEpubLocation">The location of the Epub</param>
        private void GetOPFFromEpub(string strEpubLocation)
        {
            // Epubs are just fancy Zip Files
            using (ZipArchive zip = ZipFile.Open(strEpubLocation, ZipArchiveMode.Read))
            {
                // Checking each file in the Zip
                foreach(ZipArchiveEntry entry in zip.Entries)
                {
                    // Searching for anything with an OPF file type - ignoring everything else.
                    if(entry.Name.Contains(".opf"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(entry.Open());
                        if(ValidateXMLDoc(doc))
                        {
                            GetDataFromOPFFile(doc);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Validates if the XMLDoc conforms to the standards. Does not load any information if it does not.
        /// </summary>
        /// <param name="doc">An XML Document</param>
        /// <returns>True if the document has all of the required information.</returns>
        private bool ValidateXMLDoc(XmlDocument doc)
        {
            string version = doc.DocumentElement.Attributes["version"].Value;
            if (doc.DocumentElement.Name == "package" && AcceptedVersions.Contains(version))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Gets all data from OPF files
        /// </summary>
        /// <param name="doc">XML Document</param>
        private void GetDataFromOPFFile(XmlDocument doc)
        {
            //Instantiate Package here so that we can use it for each OPF file independently.
            //We still want to provide all packages to the developer, though, so we pass this
            //by reference so that we can add it to the list of packages, but still sift through
            //this package's data.
            Package pack = DealWithPackageData(doc.DocumentElement);
            // We have to deal with Namespaces or we won't be able to switch/case properly
            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("dc", pack.DCNameSpace);
            manager.AddNamespace("opf", pack.OPFNameSpace);

            // Checking the nodes for the Data Groups we want to extract from.
            foreach (XmlNode node in doc.DocumentElement)
            {
                switch(node.Name)
                {
                    case "metadata":
                        {
                            DealWithMetadata(node);
                            break;
                        }
                    case "manifest":
                        {
                            DealWithManifest(node);
                            break;
                        }
                    case "spine":
                        {
                            DealWithSpine(node);
                            break;
                        }
                    case "guide":
                        {
                            DealWithGuide(node);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Gets all associated package data from the OPF File
        /// </summary>
        /// <param name="doc">XML Element - first Element in the XML file</param>
        /// <param name="package">Referenced Package Object</param>
        private Package DealWithPackageData(XmlElement doc)
        {
            Package package = new Package();
            if(doc.Attributes["xmlns"] != null)
            {
                package.OPFNameSpace = doc.Attributes["xmlns"].Value;
            }
            else
            {
                package.OPFNameSpace = "http://www.idpf.org/2007/opf";
            }

            if (doc.Attributes["xmlns:dc"] != null)
            {
                package.DCNameSpace = doc.Attributes["xmlns:dc"].Value;
            }
            else
            {
                package.DCNameSpace = "http://purl.org/dc/elements/1.1/";
            }

            if (doc.Attributes["version"] != null)
            {
                package.Version = doc.Attributes["version"].Value;
            }

            if (doc.Attributes["unique-identifier"] != null)
            {
                package.UniqueIdentifier = doc.Attributes["unique-identifier"].Value;
            }

            if(!BookPackage.Package.Exists(pack => pack.Equals(package)))
            {
                BookPackage.Package.Add(package);
            }
            return package;
        }
        /// <summary>
        /// This gets all of the Guide data out of the OPF
        /// </summary>
        /// <param name="node">XML Child Node named Guide</param>
        private void DealWithGuide(XmlNode node)
        {
            foreach(XmlNode child in node)
            {
                Reference references = new Reference();
                switch(child.Name)
                {
                    case "":
                        {
                            if (child.Attributes["type"] != null)
                            {
                                references.Type = child.Attributes["type"].Value;
                            }
                            if (child.Attributes["title"] != null)
                            {
                                references.Title = child.Attributes["title"].Value;
                            }
                            if (child.Attributes["href"] != null)
                            {
                                references.Href = child.Attributes["href"].Value;
                            }

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                if(!BookGuide.References.Exists(refs => refs.Equals(references)))
                {
                    BookGuide.References.Add(references);
                }
            }
        }
        /// <summary>
        /// Gets the Spine information from the OPF file
        /// </summary>
        /// <param name="node">XML Child Node named Spine</param>
        private void DealWithSpine(XmlNode node)
        {
            foreach(XmlNode child in node)
            {
                ItemRef itemRef = new ItemRef();
                
                switch(child.Name)
                {
                    case "itemref":
                        {
                            if (child.Attributes["idref"] != null)
                            {
                                itemRef.IdRef = child.Attributes["idref"].Value;
                            }

                            if (child.Attributes["linear"] != null)
                            {
                                itemRef.Linear = child.Attributes["linear"].Value;
                            }

                            if(!BookSpine.ItemRefs.Exists(item => item.Equals(itemRef)))
                            {
                                BookSpine.ItemRefs.Add(itemRef);
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }                
            }
        }
        /// <summary>
        /// Gets the Manifest Data from the OPF File
        /// </summary>
        /// <param name="node">XML Child Node named Manifest</param>
        private void DealWithManifest(XmlNode node)
        {
            foreach(XmlNode child in node.ChildNodes)
            {
                Item item = new Item();
                switch(child.Name)
                {
                    case "item":
                        {
                            if(child.Attributes["id"] != null)
                            {
                                item.ID = child.Attributes["id"].Value;
                            }

                            if (child.Attributes["href"] != null)
                            {
                                item.Href = child.Attributes["href"].Value;
                            }

                            if (child.Attributes["media-type"] != null)
                            {
                                item.MediaType = child.Attributes["media-type"].Value;
                            }

                            if (child.Attributes["fallback"] != null)
                            {
                                item.Fallback = child.Attributes["fallback"].Value;
                            }

                            if (child.Attributes["fallback-style"] != null)
                            {
                                item.FallbackStyle = child.Attributes["fallback-style"].Value;
                            }

                            if (child.Attributes["required-namespace"] != null)
                            {
                                item.RequiredNamespace = child.Attributes["required-namespace"].Value;
                            }

                            if (child.Attributes["required-modules"] != null)
                            {
                                item.RequiredModules = child.Attributes["required-modules"].Value;
                            }

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                if(!BookManifest.Item.Exists(i => i.Equals(item)))
                {
                    BookManifest.Item.Add(item);
                }
            }
        }
        /// <summary>
        /// Gets the Metadata from the OPF File
        /// </summary>
        /// <param name="node">XML Child Node named Metadata</param>
        private void DealWithMetadata(XmlNode node)
        {
            foreach(XmlNode child in node.ChildNodes)
            {
                Contributor con = new Contributor();
                switch (child.Name)
                {
                    case "dc:title":
                        {
                            if (!BookMetadata.Title.Exists(title => title.Equals(child.InnerText)))
                            {
                                BookMetadata.Title.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:publisher":
                        {
                            if (!BookMetadata.Publisher.Exists(publisher => publisher.Equals(child.InnerText)))
                            {
                                BookMetadata.Publisher.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:language":
                        {
                            if(!BookMetadata.Language.Exists(lang => lang.Equals(child.InnerText)))
                            {
                                BookMetadata.Language.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:description":
                        {
                            if (!BookMetadata.Description.Exists(desc => desc.Equals(child.InnerText)))
                            {
                                BookMetadata.Description.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:creator":
                        {
                            GetContributorContent(ref con, child);

                            if(!BookMetadata.Creator.Exists(cre => cre.Equals(con)))
                            {
                                BookMetadata.Creator.Add(con);
                            }
                            break;
                        }
                    case "dc:contributor":
                        {
                            GetContributorContent(ref con, child);

                            if (!BookMetadata.Contributor.Exists(cont => cont.Equals(con)))
                            {
                                BookMetadata.Contributor.Add(con);
                            }
                            break;
                        }
                    case "dc:subject":
                        {
                            if (!BookMetadata.Subject.Exists(sub => sub.Equals(child.InnerText)))
                            {
                                BookMetadata.Subject.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:date":
                        {
                            Date date = new Date();
                            if(child.Attributes["event"] != null)
                            {
                                date.Event = child.Attributes["event"].Value;
                            }
                            date.EventDate = child.InnerText;

                            BookMetadata.Date.Add(date);
                            break;
                        }
                    case "dc:type":
                        {
                            if (!BookMetadata.Type.Exists(type => type.Equals(child.InnerText)))
                            {
                                BookMetadata.Type.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:format":
                        {
                            if(!BookMetadata.Format.Exists(form => form.Equals(child.InnerText)))
                            {
                                BookMetadata.Format.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:identifier":
                        {
                            Identifier id = new Identifier();

                            if(child.Attributes["id"] != null)
                            {
                                id.ID = child.Attributes["id"].Value;
                            }

                            if(child.Attributes["scheme"] != null)
                            {
                                id.Scheme = child.Attributes["schme"].Value;
                            }

                            id.UniqueID = child.InnerText;

                            if(!BookMetadata.Identifier.Exists(ident => ident.Equals(id)))
                            {
                                BookMetadata.Identifier.Add(id);
                            }
                            break;
                        }
                    case "dc:source":
                        {
                            if(!BookMetadata.Source.Exists(source => source.Equals(child.InnerText)))
                            {
                                BookMetadata.Source.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:relation":
                        {
                            if(!BookMetadata.Relation.Exists(relation => relation.Equals(child.InnerText)))
                            {
                                BookMetadata.Relation.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:coverage":
                        {
                            if(!BookMetadata.Coverage.Exists(coverage => coverage.Equals(child.InnerText)))
                            {
                                BookMetadata.Coverage.Add(child.InnerText);
                            }
                            break;
                        }
                    case "dc:rights":
                        {
                            if(!BookMetadata.Rights.Exists(rights => rights.Equals(child.InnerText)))
                            {
                                BookMetadata.Rights.Add(child.InnerText);
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Contributor content is either for Creators or Contributors. This helps set it to make the Switch/Case cleaner.
        /// </summary>
        /// <param name="con">A Contributor Object passed by Reference</param>
        /// <param name="child">XML Child Node named dc:contributor or dc:creator</param>
        private void GetContributorContent (ref Contributor con, XmlNode child)
        {
            if (child.Attributes["opf:file-as"] != null)
            {
                con.FileAs = child.Attributes["opf:file-as"].Value;
            }

            if (child.Attributes["opf:role"] != null)
            {
                con.Attribute = child.Attributes["opf:role"].Value;
            }

            con.Name = child.InnerText;
        }
        /// <summary>
        /// Loads Book information from a specific Epub file.
        /// </summary>
        /// <param name="strEpubLocation"></param>
        private void GetEbookInformation(string strEpubLocation)
        {
            GetOPFFromEpub(strEpubLocation);
            LoadEbookInformation();
        }
        #endregion
       
        #region PublicMethods

        /// <summary>
        /// For use when you want to read information from a specific Epub.
        /// </summary>
        /// <param name="strEpubLocation">The location of the Epub File</param>
        /// <returns>An EpubBook data object filled with the information from the Epub File</returns>
        public EpubBook ReadEbook(string strEpubLocation)
        {
            GetEbookInformation(strEpubLocation);
            return Book;
        }
        /// <summary>
        /// For use when an Ebook's inforamtion has already been loaded.
        /// </summary>
        /// <returns>An EpubBook data object filled with the information from the Epub File</returns>
        public EpubBook ReadEbook()
        {
            return Book;
        }
        /// <summary>
        /// This resets the data object to hold information from a new Epub.
        /// </summary>
        public void Reset()
        {
            BookPackage = new EpubBookPackage();
            BookMetadata = new EpubBookMetadata();
            BookManifest = new EpubBookManifest();
            BookSpine = new EpubBookSpine();
            BookGuide = new EpubBookGuide();
            Book = new EpubBook();
        }
        #endregion
    }
}
