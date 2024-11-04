using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tourist_places.Models
{
    public class TouristPlace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TouristPlaceId { set; get; }

        [Required]
        [MaxLength(50)]
        public string TouristPlaceName { set; get; }

        [Required]
        [MaxLength(200)]
        public string TouristPlaceDescription { set; get; }

        


        [MaxLength(200)]
        public string ImagePath { set; get; }

        [Required]
        public int TouristPlaceTypeId { set; get; }

        public TouristPlaceType touristPlaceType { set; get; }



        [NotMapped]
        public HttpPostedFileBase Files { set; get; }

        [NotMapped]
        public string TpImageFile { set; get; }

    }
}