using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideotekaFm.Models
{
    public class Rezervacija
    {
        public int Id { get; set; }

        [Display(Name = "Član")]
        public int ClanId { get; set; }

        [Display(Name = "Član")]
        public Clan Clan { get; set; }

        [Display(Name = "Film")]
        public int FilmId { get; set; }

        public Film Film { get; set; }

        [Required(ErrorMessage = "Početak posudbe je obavezno polje!")]
        [Display(Name = "Početak posudbe")]
        public DateTime PocetakPosudbe { get; set; }

        [Display(Name = "Vraćeno")]
        public bool Vraceno { get; set; }
    }
}
