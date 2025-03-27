namespace APBD_1
{
    class Program
    {
        private static readonly List<ContainerShip> Ships = [];
        private static readonly List<IContainer> Containers = [];
        
        public static void Main(string[] args) 
        {
            while (true) 
            {
                Console.WriteLine("\n=== Container Ship Management ===");
                Console.WriteLine("1. Add New Ship");
                Console.WriteLine("2. Add New Container");
                Console.WriteLine("3. Load Container to Ship");
                Console.WriteLine("4. Transfer Container Between Ships");
                Console.WriteLine("5. Show All Ships");
                Console.WriteLine("6. Show Unloaded Containers");
                Console.WriteLine("7. Add Cargo to Container");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");
                
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewShip();
                        break;
                    case "2":
                        AddNewContainer();
                        break;
                    case "3":
                        LoadContainerToShip();
                        break;
                    case "4":
                        TransferContainerBetweenShips();
                        break;
                    case "5":
                        ShowAllShips();
                        break;
                    case "6":
                        ShowUnloadedContainers();
                        break;
                    case "7":
                        AddCargoToContainer();
                        break;
                    case "8":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private static void AddNewShip()
        {
            Console.Write("Enter max speed (knots): ");
            var maxSpeed = double.Parse(Console.ReadLine() ?? "10.0");

            Console.Write("Enter max container count: ");
            var maxCount = int.Parse(Console.ReadLine() ?? "10");

            Console.Write("Enter max mass (kg): ");
            var maxMass = double.Parse(Console.ReadLine() ?? "100000");

            var ship = new ContainerShip(maxSpeed, maxCount, maxMass);
            Ships.Add(ship);

            Console.WriteLine("New ship added!");
        } 
        
        private static void AddNewContainer()
        {
            Console.WriteLine("Choose container type:");
            Console.WriteLine("1. Gas");
            Console.WriteLine("2. Liquid");
            Console.WriteLine("3. Cooling (default)");
            Console.Write("Your choice: ");
            var typeChoice = int.Parse(Console.ReadLine() ?? "3");

            Console.Write("Enter height[cm]: ");
            var height = int.Parse(Console.ReadLine() ?? "200");

            Console.Write("Enter depth[cm]: ");
            var depth = int.Parse(Console.ReadLine() ?? "300");

            Console.Write("Enter mass[kg]: ");
            var mass = double.Parse(Console.ReadLine() ?? "500");

            Console.Write("Enter max capacity[kg]: ");
            var maxCapacity = double.Parse(Console.ReadLine() ?? "200");

            IContainer container;
            switch (typeChoice)
            {
                case 1:
                    Console.WriteLine("Choose pressure calculator type:");
                    Console.WriteLine("1. Default");
                    Console.WriteLine("2. Custom");
                    Console.Write("Your choice: ");
                    var calculatorChoice = int.Parse(Console.ReadLine() ?? "1");

                    PressureCalculator calculator;
                    if (calculatorChoice == 2)
                    {
                        Console.Write("Enter custom pressure factor (e.g., 2.0): ");
                        var factor = double.Parse(Console.ReadLine() ?? "1.0");

                        Console.Write("Enter pressure offset (e.g., 10): ");
                        var offset = double.Parse(Console.ReadLine() ?? "1.0");

                        var parameters = new Dictionary<string, double>
                        {
                            { "factor", factor },
                            { "offset", offset }
                        };

                        calculator = new PressureCalculator(
                            basicModel: g => 1.5 * g.Mass,
                            parameters: parameters,
                            customModel: (g, p) => p["factor"] * g.Mass + p["offset"]
                        );
                        Console.WriteLine("Custom pressure calculator created.");
                    }
                    else
                    {
                        calculator = new PressureCalculator(g => 1.5 * g.Mass);
                        Console.WriteLine("Default pressure calculator created.");
                    }

                    container = new GasContainer(height, depth, mass, maxCapacity, calculator);
                    break;

                case 2:
                    Console.Write("Is hazardous (true/false): ");
                    var isHazardous = bool.Parse(Console.ReadLine() ?? "0");
                    container = new LiquidContainer(height, depth, mass, maxCapacity, isHazardous);
                    break;

                case 3:
                    Console.Write("Enter temperature: ");
                    var temperature = double.Parse(Console.ReadLine() ?? "0.0");
                    container = new CoolingContainer(height, depth, mass, maxCapacity, temperature);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            Containers.Add(container);
            Console.WriteLine("New container added!");
        }
        
        private static void LoadContainerToShip()
        {
            if (Ships.Count == 0 || Containers.Count == 0)
            {
                Console.WriteLine("No ships or containers available!");
                return;
            }

            Console.WriteLine("Available Ships:");
            for (var i = 0; i < Ships.Count; i++)
                Console.WriteLine($"{i + 1}. {Ships[i]}");

            Console.Write("Choose ship (Default=1): ");
            var shipIndex = int.Parse(Console.ReadLine() ?? "1") - 1;

            Console.WriteLine("\nAvailable Containers:");
            for (var i = 0; i < Containers.Count; i++)
                Console.WriteLine($"{i + 1}. {Containers[i]}");

            Console.Write("Choose container (Default=1): ");
            var containerIndex = int.Parse(Console.ReadLine() ?? "1") - 1;

            var ship = Ships[shipIndex];
            var container = Containers[containerIndex];

            if (ship.LoadContainer(container))
            {
                Console.WriteLine("Container loaded successfully!");
                Containers.RemoveAt(containerIndex);
            }
            else
            {
                Console.WriteLine("Failed to load container!");
            }
        }
        
        public static void TransferContainerBetweenShips()
        {
            Console.WriteLine("Available Ships:");
            for (var i = 0; i < Ships.Count; i++)
                Console.WriteLine($"{i + 1}. {Ships[i]}");

            if (Ships.Count < 2)
            {
                Console.WriteLine("At least 2 ships required to transfer!");
                return;
            }

            Console.Write("Choose source ship(Default=1): ");
            var sourceIndex = int.Parse(Console.ReadLine() ?? "1") - 1;

            Console.Write("Choose target ship(Default=2): ");
            var targetIndex = int.Parse(Console.ReadLine() ?? "2") - 1;

            Console.WriteLine("Available Containers on source ship:");
            var sourceShip = Ships[sourceIndex];
            var targetShip = Ships[targetIndex];

            var containerList = new List<IContainer>(sourceShip.Containers);
            for (var i = 0; i < containerList.Count; i++)
                Console.WriteLine($"{i + 1}. {containerList[i]}");

            Console.Write("Choose container to transfer(Default=1): ");
            var containerChoice = int.Parse(Console.ReadLine() ?? "1") - 1;

            var success = sourceShip.TransferContainer(containerList[containerChoice], targetShip);
            Console.WriteLine(success ? "Transfer successful!" : "Transfer failed!");
        }
        
        private static void ShowAllShips()
        {
            if (Ships.Count == 0)
            {
                Console.WriteLine("No ships available.");
                return;
            }
            Console.Write("======================================================\n");
            foreach (var ship in Ships)
            {
                Console.WriteLine(ship);
                Console.Write("======================================================");
            }
            Console.Write("======================================================\n");
        }   
        
        
        private static void ShowUnloadedContainers()
        {
            if (Containers.Count == 0)
            {
                Console.WriteLine("No unloaded containers available.");
                return;
            }

            Console.WriteLine("=== Unloaded Containers ===");
            for (var i = 0; i < Containers.Count; i++)
                Console.WriteLine($"{i + 1}. {Containers[i]}");
            Console.WriteLine("=============================");
        }
        
        private static void AddCargoToContainer()
        {
            if (Containers.Count == 0)
            {
                Console.WriteLine("No containers available.");
                return;
            }

            Console.WriteLine("Available Containers:");
            for (var i = 0; i < Containers.Count; i++)
                Console.WriteLine($"{i + 1}. {Containers[i]}");
            
            Console.Write("Choose container: ");
            var containerIndex = int.Parse(Console.ReadLine() ?? "1") - 1;

            var container = Containers[containerIndex];

            Console.Write("Enter product name: ");
            var name = Console.ReadLine() ?? "Unnamed";

            Console.Write("Enter product mass [kg]: ");
            var mass = double.Parse(Console.ReadLine() ?? "10");

            var minTemp = 0.0;
            if (container is CoolingContainer)
            {
                Console.Write("Enter minimum temperature [°C]: ");
                minTemp = double.Parse(Console.ReadLine() ?? "0");
            }

            var product = new Product(name, mass, minTemp);

            try
            {
                switch (container)
                {
                    case GasContainer gasContainer:
                        gasContainer.LoadContainer(product);
                        Console.WriteLine("Product loaded into gas container.");
                        break;
                    case LiquidContainer liquidContainer:
                        var list = new List<Product> { product };
                        liquidContainer.LoadContainer(list);
                        Console.WriteLine("Product loaded into liquid container.");
                        break;
                    case CoolingContainer coolingContainer:
                        coolingContainer.LoadContainer(product);
                        Console.WriteLine("Product loaded into cooling container.");
                        break;
                    default:
                        Console.WriteLine("Unsupported container type.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load product: {ex.Message}");
            }
        }
        
    }
}
