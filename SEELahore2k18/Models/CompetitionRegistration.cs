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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class CompetitionRegistration
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0]{1})\)?([0-9]{3})\)?([0-9]{3})?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]

        public string ContactNo { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string EmailId { get; set; }

        [MaxLength(13)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CNIC must be 13 Digits Numeric")]
        public string CNIC { get; set; }
        public Nullable<int> InstituteId { get; set; }
        public Nullable<int> RequestStatusId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public Nullable<int> CompetitionId { get; set; }
    
        public virtual Competition Competition { get; set; }
        public virtual Institute Institute { get; set; }
        public virtual RequestStatu RequestStatu { get; set; }
    }
}
