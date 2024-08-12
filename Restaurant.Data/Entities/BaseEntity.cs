using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        protected BaseEntity(int id)
        { this.Id = id; }
        protected BaseEntity()
        { this.Id = 0; }
    }
}
