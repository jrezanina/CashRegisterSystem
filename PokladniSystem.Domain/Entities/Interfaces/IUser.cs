using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities.Interfaces
{
    public interface IUser
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int? StoreId { get; set; }       
    }
}
