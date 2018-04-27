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
        public User()
        {
            Playing = new List<Player>();
            CreatedGames = new List<Game>();
        }
        public List<Player> Playing { get; set; }
        public List<Game> CreatedGames { get; set; }
    }
}