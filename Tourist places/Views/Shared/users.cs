using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tourist_places.Models
{
    public class users
    {
        [Key]
        [Required] 
        public string UserName {  get; set; }
        [Required]
        public string Password { get; set; }
               
    }
}