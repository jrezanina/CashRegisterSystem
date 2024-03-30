using PokladniSystem.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities
{
    public class Order : Entity<int>
    {
        [ForeignKey(nameof(Store))]
        public int StoreId { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public int TotalPrice { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string? ReceiptSrc { get; set; }

        public Store? Store { get; set; }
        public IUser? User { get; set; }

    }
}
