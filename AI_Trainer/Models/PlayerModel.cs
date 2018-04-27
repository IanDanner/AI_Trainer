using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace AI_Trainer.Models
{
    public class Player : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string AIName { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public byte[] OpponentMoves { get; set; }
    }
}