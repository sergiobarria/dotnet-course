using CompanyEmployees.Models;

namespace CompanyEmployees.Services;

public interface IPlayerGenerator
{
    Player CreateNewPlayer();
}

public class PlayerGenerator : IPlayerGenerator
{
    private readonly string[] _femaleNames = { "Imoen", "Jaheira", "Dynaheir", "Branwen", "Bodhi" };
    private readonly string[] _maleNames = { "Jon Irenicus", "Kagain", "Minsc", "Xzar", "Drizzt Do'Urden" };

    public Player CreateNewPlayer()
    {
        var random = new Random();
        var playerNameIndex = random.Next(5);
        var playerGenderIndex = random.Next(2);
        var playerHairColorIndex = random.Next(7);
        var playerAge = random.Next(18, 100);
        var strength = random.Next(8, 18);

        var playerName = playerGenderIndex is 0 ? _maleNames[playerNameIndex] : _femaleNames[playerNameIndex];

        return new Player
        {
            Name = playerName,
            Gender = (Gender)playerGenderIndex,
            HairColor = (HairColor)playerHairColorIndex,
            Age = playerAge,
            Strength = strength,
            Race = "Human"
        };
    }
}