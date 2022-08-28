using RPG.WebApi.EF.Models;

namespace RPG.WebApi.EF.Dtos
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoints { get; set; } = 10;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public Rpg Class { get; set; } = Rpg.Knight;
    }
}
