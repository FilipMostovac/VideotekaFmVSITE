using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideotekaFm.Models
{
    public class Clan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno!")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno!")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "E-mail adresa je obavezna!")]
        [EmailAddress(ErrorMessage = "E-mail adresa treba biti formata: example@example.com")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Broj telefona mora biti drugog formata!")]
        public string Telefon { get; set; }
        public string Adresa { get; set; }

        [NotMapped]
        public string PrezimeIme 
        {
            get => Prezime + ' ' + Ime;
        }

        public List<Rezervacija> Rezervacije { get; set; }
    }
}
