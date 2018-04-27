using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace AI_Trainer.Models
{
    public class Game : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Game()
        {
            Players = new List<Player>();
        }
        public List<Player> Players { get; set; }
    }
}