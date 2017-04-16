using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpubMetadataReader;
using EpubMetadataReader.Models;
using System.IO;
using static System.Net.Mime.MediaTypeNames;


namespace EpubTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //If you want to have a full Library of Epub information, instantiate a Library First.
            EpubLibrary Library = new EpubLibrary();

            //Instantiate a new Epub Reader
            Epub epubBook = new Epub();
            //We are going to get every Epub out of this directory
            foreach (string directory in Directory.EnumerateDirectories(@"\\BATCOMPUTER\Public\Shared Books\epub\"))
            {
                //We are looking for only the files that are Epub. Ignore everything else.
                foreach (string file in Directory.EnumerateFiles(directory, "*.epub"))
                {
                    //Read a book by passing the file location
                    //Add book to Library of books
                    Library.Books.Add(epubBook.ReadEbook(file));
                    //We want to reset our epubBook object so it is ready for new data
                    //If you don't do this, you'll have tons of duplicate data.
                    epubBook.Reset();
                }
            }

            //We want to add all of the book details to a file
            //To do that, we're going to make a new String Builder
            //to hold the data before we write it.
            StringBuilder sb = new StringBuilder();

            //Going through each book in the Library
            foreach (EpubBook book in Library.Books)
            {
                //For this example, we're going to get just the Metadata info
                //But you can do this for the Metadata, Manifest, Package, Spine, and Guide
                foreach(EpubBookMetadata bookInfo in book.Metadata)
                {
                    //These GetData methods are designed to get the information from 
                    //one of the Metadata List<Object>s. I chose t6 build it this way
                    //for simplicity; you can build yours however you want.
                    GetData(bookInfo.Title, ref sb);
                    GetData(bookInfo.Subject, ref sb);
                    GetData(bookInfo.Rights, ref sb);
                    GetData(bookInfo.Contributor, ref sb);
                    GetData(bookInfo.Creator, ref sb);
                    GetData(bookInfo.Identifier, ref sb);
                    //To give us a clear delineation in the file for new books.
                    sb.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    WriteToFile(sb);
                    sb.Clear();
                }
            }
        }
        /// <summary>
        /// This gets information from one of the List<string> objects in the Metadata.
        /// </summary>
        /// <param name="bookInfo">Any of the Book items that are List<string> objects</param>
        /// <param name="sb">Referencing String Builder</param>
        private static void GetData(List<string> bookInfo, ref StringBuilder sb)
        {
            foreach(var item in bookInfo)
            {
                sb.AppendLine(item.ToString());
            }
        }
        /// <summary>
        /// This gets information from the List<contributor> objects in the Metadata.
        /// </summary>
        /// <param name="con">A creator or contributor object.</param>
        /// <param name="sb">Referencing String Builder</param>
        private static void GetData(List<Contributor> con, ref StringBuilder sb)
        {
            foreach(var item in con)
            {
                sb.AppendLine("Attribute: " + item.Attribute);
                sb.AppendLine("File As: " + item.FileAs);
                sb.AppendLine("Name: " + item.Name);
            }
        }
        /// <summary>
        /// This gets information from the List<identifier> object in the Metadata.
        /// </summary>
        /// <param name="id">A List<Identifier> object</param>
        /// <param name="sb">Referencing String Builder</param>
        private static void GetData(List<Identifier> id, ref StringBuilder sb)
        {
            foreach(var item in id)
            {
                sb.AppendLine(item.ID + " " + item.UniqueID);
                sb.AppendLine(item.Scheme);
            }
        }
        /// <summary>
        /// This gets information from the List<date> object in the Metadata
        /// </summary>
        /// <param name="date">A List<date> object</param>
        /// <param name="sb">Referencing String Builder</param>
        private static void GetData(List<Date> date, ref StringBuilder sb)
        {
            foreach(var item in date)
            {
                sb.AppendLine("Event: " + item.Event);
                sb.AppendLine("Date: " + item.EventDate);
            }
        }

        private static void WriteToFile(StringBuilder sb)
        {
            //Writing the data to a file.
            using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "\\file.txt", true))
            {
                writer.WriteLine(sb);
            }
        }
    }
}
