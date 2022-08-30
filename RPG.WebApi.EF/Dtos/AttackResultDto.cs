﻿namespace RPG.WebApi.EF.Dtos
{
    public class AttackResultDto
    {
        public string Attacker { get; set; } = string.Empty;
        public string Opponent { get; set; } = string.Empty;
        public int AttackerHP { get; set; }
        public int OpponentHp { get; set; }
        public int Damage { get; set; }
    }
}
