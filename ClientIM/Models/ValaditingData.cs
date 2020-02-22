using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientIM.Models
{
    [MetadataType(typeof(ValaditingProfile))]
    public partial class Profile
    {

    }

    [MetadataType(typeof(ValaditingAddress))]
    public partial class Address
    {

    }

    [MetadataType(typeof(ValaditingContact))]
    public partial class Contact
    {

    }

    [MetadataType(typeof(ValaditingPicture))]
    public partial class Picture
    {

    }

    [MetadataType(typeof(ValaditingCountry))]
    public partial class Country
    {

    }

    public class ValaditingProfile
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter a name")]
        [StringLength(50, MinimumLength = 1)]
        public string first_name;
        [Display(Name = "Last Name")]
        public string last_name;
        [Display(Name = "Note")]
        public string notes;
        [Display(Name = "Gender")]
        public string gender;
        [Display(Name = "Profile picture")]
        public string profile_pic;
    }

    public class ValaditingAddress
    {
        [Display(Name = "Person Id")]
        public string person_id;
        [Display(Name = "Description")]
        public string desc;
        [Required(ErrorMessage = "Please enter a street")]
        [Display(Name = "Street")]
        public string street;
        [Required(ErrorMessage = "Please enter a city")]
        [Display(Name = "City")]
        public string city;
        [Display(Name = "State")]
        public string state;
        [Display(Name = "Zip Code")]
        public string zip_code;
    }
    public class ValaditingContact
    {
        [Required]
        [Display(Name = "Information Type")]
        public string type_info;
        [Required]
        [Display(Name = "Information")]
        public string info;
    }
    public class ValaditingPicture
    {
        [Display(Name = "Caption")]
        public string caption;
        [Required]
        [Display(Name = "Path")]
        public string path;
        [Display(Name = "Time Information")]
        public string time_info;
        [Display(Name = "Location Information")]
        public string loc_info;
    }
    public class ValaditingCountry
    {
        [Display(Name = "Country Name")]
        public string country_name;
        [Display(Name = "Country Id")]
        public string country_id;
    }
}