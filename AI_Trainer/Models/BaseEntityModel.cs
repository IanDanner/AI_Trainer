using System;
using System.ComponentModel.DataAnnotations;

namespace AI_Trainer.Models
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
        }
        [DataType(DataType.Date)]
        public DateTime Created_at { get; set; }

        [DataType(DataType.Date)]
        public DateTime Updated_at { get; set; }
    }
}