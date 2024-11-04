using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tourist_places.Models
{
    public class TouristPlaceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TouristPlaceTypeId { set; get; }

        [Required]
        [MaxLength(50)]
        public string TouristPlaceTypeName { set; get; }

        public List<TouristPlace> touristPlaces { set; get; }

    }
}