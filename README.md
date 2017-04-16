# EpubMetadataReader
A library for getting all of the metadata from an epub.

## Features
The EpubMetadataReader can quickly pull all of an Epub's information from within the OPF files located within. 

## The Future
This currently only supports Epub 2.0 files. (It will read 3.0 files, but miss some data) - Support for 3.0 will be added in the future.

## Example
```csharp
            //If you want to have a full Library of Epub information, instantiate a Library First.
            EpubLibrary Library = new EpubLibrary();
            
            //Instantiate a new Epub Reader
            Epub epubBook = new Epub();
            
            //We are going to get every Epub out of this directory
            foreach (string directory in Directory.EnumerateDirectories(StringDirectoryLocation))
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
```

You can find a more detailed example of using the EpubMetadataReader in EpubTest folder above.

## Installation
You can reference the EpubMetadataReader DLL directly in your project, or install it from NuGet package manager using the following command:

PM> Install-Package QiwiTrails.EpubMetadataReader
