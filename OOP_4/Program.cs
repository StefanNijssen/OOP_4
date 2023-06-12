using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class Battle
    {
        private Trainer trainer1;
        private Trainer trainer2;

        public Battle(Trainer trainer1, Trainer trainer2)
        {
            this.trainer1 = trainer1;
            this.trainer2 = trainer2;
        }

        public void Start()
        {
            int round = 1;

            while (trainer1.Belt.Count > 0 && trainer2.Belt.Count > 0)
            {
                Console.WriteLine("Round " + round);
                Console.WriteLine();

                bool anyPokemonReturned = false;

                foreach (Trainer trainer in new[] { trainer1, trainer2 })
                {
                    Console.WriteLine("Trainer " + trainer.Name + " throws a Pokéball!");

                    int pokeballIndex = new Random().Next(trainer.Belt.Count);
                    trainer.ThrowPokeball(pokeballIndex);

                    anyPokemonReturned |= trainer.ReturnPokemon();
                }

                if (!anyPokemonReturned)
                {
                    Console.WriteLine("No Pokémon were returned.");
                }

                Console.WriteLine();
                round++;
            }

            Console.WriteLine("Battle has ended!");
            Console.WriteLine();

            DetermineWinner();
        }

        private void DetermineWinner()
        {
            int trainer1PokemonCount = CountRemainingPokemons(trainer1);
            int trainer2PokemonCount = CountRemainingPokemons(trainer2);

            if (trainer1PokemonCount > trainer2PokemonCount)
            {
                Console.WriteLine("Trainer " + trainer1.Name + " wins the battle!");
            }
            else if (trainer2PokemonCount > trainer1PokemonCount)
            {
                Console.WriteLine("Trainer " + trainer2.Name + " wins the battle!");
            }
            else
            {
                Console.WriteLine("The battle ends in a draw!");
            }
        }

        private int CountRemainingPokemons(Trainer trainer)
        {
            int count = 0;

            foreach (Pokeball pokeball in trainer.Belt)
            {
                if (!pokeball.IsOpen && pokeball.EnclosedPokemons.Count > 0)
                {
                    count += pokeball.EnclosedPokemons.Count;
                }
            }

            return count;
        }
    }
    public abstract class Pokemon
    {
        public string name;
        public string strength;
        public string weakness;

        protected Pokemon(string name, string strength, string weakness)
        {
            this.name = name;
            this.strength = strength;
            this.weakness = weakness;
        }

        public abstract void BattleCry();
    }

    public class Squirtle : Pokemon
    {
        public Squirtle(string name) : base(name, "Water", "Leaf")
        {
        }

        public override void BattleCry()
        {
            Console.WriteLine(name + "!!!");
        }
    }

    public class Bulbasaur : Pokemon
    {
        public Bulbasaur(string name) : base(name, "Grass", "Fire")
        {
        }

        public override void BattleCry()
        {
            Console.WriteLine(name + "!!!");
        }
    }

    public class Charmander : Pokemon
    {
        public Charmander(string name) : base(name, "Fire", "Water")
        {
        }

        public override void BattleCry()
        {
            Console.WriteLine(name + "!!!");
        }
    }


    class Pokeball
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
                Console.WriteLine(pokemon.name + ", I choose you!");
                pokemon.BattleCry();
            }
        }

        public void Return()
        {
            if (IsOpen && EnclosedPokemons.Count > 0)
            {
                foreach (Pokemon pokemon in EnclosedPokemons)
                {
                    Console.WriteLine(pokemon.name + ", come back!");
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

    class Trainer
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

        public void ThrowPokeball(int index)
        {
            if (index < 0 || index >= Belt.Count)
            {
                Console.WriteLine("Ongeldige Pokeball-index.");
                return;
            }

            Pokeball pokeball = Belt[index];
            if (!pokeball.IsOpen && pokeball.EnclosedPokemons != null)
            {
                Console.WriteLine("Trainer " + Name + " gooit een pokeball!");
                pokeball.Throw();
                return;
            }

            Console.WriteLine("De geselecteerde Pokeball kan niet worden gegooid.");
        }
        public void ReturnPokemon()
        {
            bool anyPokemonReturned = false;

            foreach (Pokeball pokeball in Belt)
            {
                if (pokeball.IsOpen && pokeball.EnclosedPokemons.Count > 0)
                {
                    pokeball.Return();
                    foreach (Pokemon pokemon in pokeball.EnclosedPokemons)
                    {
                        Console.WriteLine(pokemon.name + " goes back to Trainer " + Name + "'s pokeball.");
                    }
                    anyPokemonReturned = true;
                }
            }

            if (!anyPokemonReturned)
            {
                Console.WriteLine("There is no open pokeball to return.");
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            bool replayGame = true;

            while (replayGame)
            {
                Console.WriteLine("Naam trainer 1:");
                string nameTrainer1 = Console.ReadLine();
                Trainer trainer1 = new Trainer(nameTrainer1);

                Console.WriteLine("Naam trainer 2:");
                string nameTrainer2 = Console.ReadLine();
                Trainer trainer2 = new Trainer(nameTrainer2);

                Console.WriteLine("Druk op Enter om het spel te starten...");
                Console.ReadLine();

                bool gameRunning = true;
                int i = 0;
                while (gameRunning)
                {
                    Console.WriteLine("===== Trainer 1 =====");
                    trainer1.ThrowPokeball(i);
                    Console.WriteLine();

                    Console.WriteLine("===== Trainer 2 =====");
                    trainer2.ThrowPokeball(i);
                    Console.WriteLine();

                    Console.WriteLine("Wil je doorgaan met het spel? (ja/nee)");
                    string continueResponse = Console.ReadLine();
                    if (continueResponse.ToLower() == "ja")
                    {
                        Console.WriteLine("===== Trainer 1 =====");
                        trainer1.ReturnPokemon();
                        Console.WriteLine();

                        Console.WriteLine("===== Trainer 2 =====");
                        trainer2.ReturnPokemon();
                        Console.WriteLine();
                    }
                    else
                    {
                        gameRunning = false;
                    }

                    Console.WriteLine("Wil je doorgaan met het spel? (ja/nee)");
                    continueResponse = Console.ReadLine();
                    if (continueResponse.ToLower() == "nee")
                    {
                        gameRunning = false;
                    }
                    i++;
                }

                Console.WriteLine("Wil je het spel opnieuw spelen? (ja/nee)");
                string replayResponse = Console.ReadLine();
                if (replayResponse.ToLower() != "ja")
                {
                    replayGame = false;
                }
            }

            Console.WriteLine("Het spel is beëindigd.");
            Console.ReadLine();
        }
    }
}