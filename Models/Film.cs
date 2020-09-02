using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideotekaFm.Models
{
    public class Film
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naslov filma je obavezan!")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Trajanje filma je obavezno!")]
        public int Trajanje { get; set; }

        [Required(ErrorMessage = "Količina filmova je obavezna!")]
        [Display(Name = "Količina")]
        public int Kolicina { get; set; }

        [Display(Name = "Žanr")]
        public int ZanrId { get; set; }

        [Display(Name = "Žanr")]
        public Zanr Zanr { get; set; }

        [Display(Name = "Putanja do slike filma (npr. ./filmovi/ImeFilma.jpg)")]
        public string ImgPath { get; set; }
        public string YoutubeLink { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }

    }
}
