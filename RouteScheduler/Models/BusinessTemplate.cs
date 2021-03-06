﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RouteScheduler.Models
{
    public class BusinessTemplate
    {
        [Key]
        [Display(Name = "Template Id")]
        public int TemplateId { get; set; }

        [ForeignKey("BusinessOwner")]
        public int BusinessId { get; set; }
        public virtual BusinessOwner BusinessOwner { get; set; }

        [Required]
        [Display(Name = "Job Name")]
        public string JobName { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Service Length")]
        public TimeSpan ServiceLength { get; set; }

        [Display(Name = "Job Details")]
        [Required(ErrorMessage = "Job details are required.")]
        [AllowHtml]
        public string JobDetails { get; set; }

    }
}