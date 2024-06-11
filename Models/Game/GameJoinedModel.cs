﻿using Microsoft.CodeAnalysis;

namespace GameZone.Models.Game
{
    public class GameJoinedModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; } 

        public string ReleasedOn { get; set; } = null!;

        public string Genre { get; set; } = null!;
        public string Publisher { get; set; } = null!;
    }
}
