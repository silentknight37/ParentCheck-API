using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class LibraryEnvelop
    {
        public LibraryEnvelop(List<LibraryDTO> librarieFiles)
        {
            this.LibrarieFiles = librarieFiles;
        }

        public List<LibraryDTO> LibrarieFiles { get; set; }
    }
}
