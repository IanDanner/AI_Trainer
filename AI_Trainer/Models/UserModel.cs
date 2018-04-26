using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace AI_Trainer.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Player Playing { get; set; }
        public Game CreatedGame { get; set; }
    }
}