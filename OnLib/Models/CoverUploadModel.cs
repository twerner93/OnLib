using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class CoverUploadModel
    {
        public int TitelId { get; set; }
        public bool Neu { get; set; }
        public HttpPostedFile File { get; set; }
    }
}