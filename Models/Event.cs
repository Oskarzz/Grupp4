using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventMVC.Models
{
    public class Event
    {
        public int Id { get; set; }
        [DisplayName("Datum")]
        public string Date { get; set; }
        [DisplayName("Start tid")]
        public string StartTime { get; set; }
        [DisplayName("Slut tid")]
        public string EndTime { get; set; }
        [DisplayName("Plats")]
        public string Location { get; set; }

        [DisplayName ("Stad")]
        public string City { get; set; }
        [DisplayName("Beskrivning")]
        public string Description { get; set; }
        [DisplayName("Pris")]
        public decimal Price { get; set; }
        [DisplayName("Namn")]
        public string Name { get; set; }
        [DisplayName("Kategori")]
        public string Category { get; set; }
        [DisplayName("Prioritet")]
        public int Prioritize { get; set; }
        [DisplayName("Biljett max")]
        public int TicketsMax { get; set; }
        [DisplayName("Lokal")]
        public int LocaleId { get; set; }
        [DisplayName("Arrangör")]
        public int HostId { get; set; }
        [DisplayName("Bild")]
        public byte[] Picture { get; set; }
        public List<Booking> Booking { get; set; }

        [NotMapped]
        public Microsoft.AspNetCore.Http.IFormFile UploadedPic { get; set; }
    }
}
