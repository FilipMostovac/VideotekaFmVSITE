using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideotekaFm.Models
{
    public class Zanr
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv žanra je obavezan!")]
        public string Naziv { get; set; }
        public List<Film> Filmovi { get; set; }
    }
}

