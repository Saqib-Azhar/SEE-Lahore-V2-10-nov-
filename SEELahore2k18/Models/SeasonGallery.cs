//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SEELahore2k18.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SeasonGallery
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string Image { get; set; }
        public Nullable<int> SeasonId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Season Season { get; set; }
    }
}