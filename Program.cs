// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

/*

    -----------------------------------------------------
    |                        |           |              |
    |    Garage 0            | Bathroom 1|  Room 2      |
    |                        |           |              |
    --------------     ----------    ---------     ------
    |       |                                           |
    |Entrance|  Hallway 4                               |
    |    3  |                                           |
    --------|------     ----|-----   -------|----    ----
            |               |               |           |
            |    Living         Kitchen     |   Closet  |
            |    room              6        |    7      |
            |         5     |               |           |
            |---------------|---------------|-----------| 
*/

using System.Transactions;

class Location
{
    public string Name { get; private set; }
    public string? Item { get; private set; }
    public int[] AdjacentLocations { get; private set; }
    public string[] ItemPrompt { get; private set; }

    public void Prompt()
    {
        Console.WriteLine(Item != null ? ItemPrompt[0] : ItemPrompt[1]);
    }

    public string? GrabItem()
    {
        if (Item != null)
        {
            string item = Item;
            Item = null;
            return item;
        }

        return null;
    }

    public Location(string name, string? item, int[] adjacentLocations, string[] itemPrompt)
    {
        Name = name;
        Item = item;
        AdjacentLocations = adjacentLocations;
        ItemPrompt = itemPrompt;
    }
}

class Player
{
    public List<string> Items { get; private set; } = new List<string>();
    public Location CurrentLocation { get; set; }

    public Player(Location currentLocation)
    {
        CurrentLocation = currentLocation;
    }
}

class Game
{

    static Location[] AllLocations = {
            new Location("Garage", "Grease", new []{4}, new []{"I need some grease for my car.", "I already got the grease!"}),
            new Location("Bathroom", null, new []{4}, new []{"", "Looking good today!"}),
            new Location("Bedroom", "Briefcase", new []{4}, new []{"I need to grab my briefcase before going out", "I got my briefcase"}),
            new Location("Entrance", null, new[]{4, 8}, new[]{"", "I need to clean the carpet later."}),
            new Location("Hallway", null, new[]{0, 1, 2, 3, 5, 6, 7}, new[]{"", "What's next?"}),
            new Location("Living room", "Money", new[]{4, 6}, new[]{"I almost forgot the money!", "I got the bills already."}),
            new Location("Kitchen", null, new[]{4, 5}, new[]{"", "That omelette was killer!"}),
            new Location("Closet", "Gas can", new[]{4}, new[]{"I need a can to go get gas later", "I have the can with me"}),
            new Location("Outside", null, new[]{-1}, new[]{"", "I made it!"})
        };
    static public Player Player { get; private set; } = new Player(AllLocations[4]);

    static void Main(string[] args)
    {

        Player player = new Player(AllLocations[4]);

        while (player.CurrentLocation != AllLocations[8] && player.Items.Count < 4)
        {
            printChoices();
            processChoice();
        }
    }

    static void printChoices()
    {
        Location current = Player.CurrentLocation;
        current.Prompt();

        Console.WriteLine("\n1) Interact");
        Console.WriteLine("2) Move to");
        Console.WriteLine("3) Quit\n");
        Console.Write("Enter your choice: ");
    }

    static void processChoice()
    {
        int choice;
        Location current = Player.CurrentLocation;

        if (int.TryParse(Console.ReadLine(), out choice))
        {
            switch (choice)
            {
                case 1:
                    if (current.Item != null)
                    {
                        string? item = current.GrabItem();
                        if (item != null) Player.Items.Add(item);
                    }

                    if (current.Name == "Entrance" && Player.Items.Count == 4)
                    {
                        Console.WriteLine("1) Go out!");
                        Console.WriteLine("2) Back");

                        int finalChoice;

                        if (int.TryParse(Console.ReadLine(), out finalChoice))
                        {
                            if (finalChoice == 1)
                            {
                                Console.WriteLine("I'm making it on time!");
                                Environment.Exit(0);
                            }
                        }
                    }
                    break;

                case 2:
                    int index = 1;
                    int locationChoice;
                    foreach (int location in current.AdjacentLocations)
                    {
                        if (location != 8) Console.WriteLine(index++ + ") Move to " + AllLocations[location].Name);
                    }

                    Console.Write("Enter your choice: ");

                    if (int.TryParse(Console.ReadLine(), out locationChoice))
                    {
                        if (current.Name == "Entrance" && locationChoice == 2 && Player.Items.Count < 4) return;
                        Player.CurrentLocation = AllLocations[current.AdjacentLocations[locationChoice - 1]];
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                    }

                    break;

                case 3:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. ");
                    break;
            }
        }
    }
}