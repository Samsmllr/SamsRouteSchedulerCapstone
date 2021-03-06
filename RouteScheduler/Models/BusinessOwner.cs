﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RouteScheduler.Models
{
    public class BusinessOwner
    {
        [Key]
        [Display(Name = "Business Owner Id")]
        public int BusinessId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address {get; set;}

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public int ZipCode { get; set; }

        public Double Latitude { get; set; }

        public Double Longitude { get; set; }

        [Required]
        [Display(Name = "Daily Start Time")]
        public TimeSpan DayStart { get; set; }

        [Required]
        [Display(Name = "Daily End Time")]
        public TimeSpan DayEnd { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Display(Name = "Business Description")]
        [Required(ErrorMessage = "Description of business required.")]
        [AllowHtml]
        public string BusinessDetails { get; set; }
    }
}