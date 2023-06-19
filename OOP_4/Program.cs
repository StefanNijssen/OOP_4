using System;
using System.Collections.Generic;

public abstract class Pokemon
{
    public string Name;
    public string Strength;
    public string Weakness;

    protected Pokemon(string name, string strength, string weakness)
    {
        Name = name;
        Strength = strength;
        Weakness = weakness;
    }

    public abstract void BattleCry();
}

public class Squirtle : Pokemon
{
    public Squirtle(string name) : base(name, "Water", "Grass")
    {
    }

    public override void BattleCry()
    {
        Console.WriteLine(Name + "!!!");
    }
}

public class Bulbasaur : Pokemon
{
    public Bulbasaur(string name) : base(name, "Grass", "Fire")
    {
    }

    public override void BattleCry()
    {
        Console.WriteLine(Name + "!!!");
    }
}

public class Charmander : Pokemon
{
    public Charmander(string name) : base(name, "Fire", "Water")
    {
    }

    public override void BattleCry()
    {
        Console.WriteLine(Name + "!!!");
    }
}

public class Pokeball
{
    public bool IsOpen;
    public List<Pokemon> EnclosedPokemons;

    public Pokeball()
    {
        EnclosedPokemons = new List<Pokemon>();
    }

    public void Throw()
    {
        if (!IsOpen && EnclosedPokemons.Count > 0)
        {
            Console.WriteLine("Pokeball is thrown!");
            IsOpen = true;
            ReleasePokemons();
            EnclosedPokemons.Clear(); // Clear the enclosed Pokémon after releasing them
            IsOpen = false; // Set IsOpen back to false after all Pokémon are released
        }
        else
        {
            Console.WriteLine("Pokeball is empty or already open.");
        }
    }

    private void ReleasePokemons()
    {
        foreach (Pokemon pokemon in EnclosedPokemons)
        {
            Console.WriteLine(pokemon.Name + ", I choose you!");
            pokemon.BattleCry();
        }
    }

    public void Return()
    {
        if (IsOpen && EnclosedPokemons.Count > 0)
        {
            foreach (Pokemon pokemon in EnclosedPokemons)
            {
                Console.WriteLine(pokemon.Name + ", come back!");
            }

            EnclosedPokemons.Clear();
            IsOpen = false;
        }
        else
        {
            Console.WriteLine("Pokeball is already closed or empty.");
        }
    }

    public void EnclosePokemon(Pokemon pokemon)
    {
        if (!IsOpen)
        {
            EnclosedPokemons.Add(pokemon);
        }
        else
        {
            Console.WriteLine("Cannot enclose a Pokemon. Pokeball is already open.");
        }
    }
}

public class Trainer
{
    public string Name;
    public List<Pokeball> Belt;

    public Trainer(string name)
    {
        Name = name;
        Belt = new List<Pokeball>();
        InitializeBeltWithPokemon();
    }

    private void InitializeBeltWithPokemon()
    {
        for (int i = 0; i < 2; i++)
        {
            Squirtle squirtle = new Squirtle("Squirtle" + (i + 1));
            Bulbasaur bulbasaur = new Bulbasaur("Bulbasaur" + (i + 1));
            Charmander charmander = new Charmander("Charmander" + (i + 1));

            Pokeball pokeball1 = new Pokeball();
            pokeball1.EnclosePokemon(squirtle);
            Belt.Add(pokeball1);

            Pokeball pokeball2 = new Pokeball();
            pokeball2.EnclosePokemon(bulbasaur);
            Belt.Add(pokeball2);

            Pokeball pokeball3 = new Pokeball();
            pokeball3.EnclosePokemon(charmander);
            Belt.Add(pokeball3);
        }
    }

    public void ThrowPokeballs()
    {
        foreach (Pokeball pokeball in Belt)
        {
            if (pokeball.EnclosedPokemons.Count > 0)
            {
                pokeball.Throw();
            }
        }
    }

    public void ReturnPokeballs()
    {
        foreach (Pokeball pokeball in Belt)
        {
            pokeball.Return();
        }
    }
}

public class Battle
{
    private static int roundCounter;
    private static int battleCounter;

    private static void UpdateScoreboard()
    {
        roundCounter++;
        battleCounter++;
    }

    public static void StartBattle(Trainer trainer1, Trainer trainer2)
    {
        Console.WriteLine("=== Battle Start ===");

        roundCounter = 0;
        battleCounter = 0;

        Random random = new Random();

        while (trainer1.Belt.Count > 0 && trainer2.Belt.Count > 0)
        {
            Console.WriteLine("=== Round " + (roundCounter + 1) + " ===");

            int index1 = random.Next(trainer1.Belt.Count);
            int index2 = random.Next(trainer2.Belt.Count);

            Pokemon pokemon1 = trainer1.Belt[index1].EnclosedPokemons[0];
            Pokemon pokemon2 = trainer2.Belt[index2].EnclosedPokemons[0];

            Console.WriteLine(trainer1.Name + " throws a pokeball with " + pokemon1.Name);
            Console.WriteLine(trainer2.Name + " throws a pokeball with " + pokemon2.Name);

            if (pokemon1.Strength == pokemon2.Weakness)
            {
                Console.WriteLine(pokemon1.Name + " wins!");
                trainer2.Belt[index2].Return();
                trainer2.Belt.RemoveAt(index2);
            }
            else if (pokemon2.Strength == pokemon1.Weakness)
            {
                Console.WriteLine(pokemon2.Name + " wins!");
                trainer1.Belt[index1].Return();
                trainer1.Belt.RemoveAt(index1);
            }
            else
            {
                Console.WriteLine("It's a draw!");
                if (roundCounter > 0)
                {
                    trainer1.Belt[index1].Return();
                    trainer2.Belt[index2].Return();
                }
                else
                {
                    trainer1.Belt[index1].Return();
                    trainer2.Belt[index2].Return();
                    trainer1.Belt.RemoveAt(index1);
                    trainer2.Belt.RemoveAt(index2);
                }
            }

            UpdateScoreboard();
        }

        if (trainer1.Belt.Count > trainer2.Belt.Count)
        {
            Console.WriteLine(trainer1.Name + " wins the battle!");
        }
        else if (trainer2.Belt.Count > trainer1.Belt.Count)
        {
            Console.WriteLine(trainer2.Name + " wins the battle!");
        }
        else
        {
            Console.WriteLine("It's a draw!");
        }

        Console.WriteLine("=== Battle End ===");
    }
}

public class Arena
{
    private static int roundCounter;
    private static int battleCounter;

    public static void Main(string[] args)
    {
        Console.WriteLine("Naam trainer 1:");
        string nameTrainer1 = Console.ReadLine();
        Trainer trainer1 = new Trainer(nameTrainer1);

        Console.WriteLine("Naam trainer 2:");
        string nameTrainer2 = Console.ReadLine();
        Trainer trainer2 = new Trainer(nameTrainer2);

        StartArenaBattle(trainer1, trainer2);

        Console.WriteLine("Total Battles: " + battleCounter);
        Console.WriteLine("Total Rounds: " + roundCounter);
    }

    private static void UpdateScoreboard()
    {
        roundCounter++;
        battleCounter++;
    }

    public static void StartArenaBattle(Trainer trainer1, Trainer trainer2)
    {
        Console.WriteLine("=== Arena Battle Start ===");

        roundCounter = 0;
        battleCounter = 0;

        Console.WriteLine("Enter the number of battles:");
        int numBattles = int.Parse(Console.ReadLine());

        for (int i = 0; i < numBattles; i++)
        {
            Console.WriteLine("=== Battle " + (battleCounter + 1) + " ===");

            Battle.StartBattle(trainer1, trainer2);

            UpdateScoreboard();

            Console.WriteLine("Press Enter to start the next battle...");
            Console.ReadLine();
        }

        Console.WriteLine("=== Arena Battle End ===");
    }
}