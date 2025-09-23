using Services.Domain;
using System;
using Services.Contracts.Logs;
namespace UI
{
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal static extern bool AllocConsole();

        internal static bool testEnvironment = true;
        internal static bool productionEnvironment = false;
    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (NativeMethods.testEnvironment)
            {
                NativeMethods.AllocConsole(); // Opens the console
                Logger _loger = Logger.Current;
                ConsoleAppender _appender = new ConsoleAppender();
                _loger.AddAppender(_appender);
                Console.WriteLine("Test environment - Console appender available for debugging");
            }
            if(NativeMethods.productionEnvironment)
            {
                Logger _loger = Logger.Current;
                FileAppender _appender = new FileAppender("log.txt");
                _loger.AddAppender(_appender);
            }
            Console.WriteLine("StockHelper initialized!");
            ApplicationConfiguration.Initialize();
            Application.Run(new frmLogIn());
            //Test_Patent_Family();
        }

        private static void Test_Patent_Family()
        {
            // This is a test method for Patent & Family with User
            Console.WriteLine("This is a test method for Patent & Family");
            Component p1 = new Patent
            {
                Name = "A001",
                Id = Guid.NewGuid(),
                Perm = PermissionTypes.Employee,
            };
            Console.WriteLine($"Patent 1:\n{p1.Name}\n {p1.Perm}");
            Component p2 = new Patent
            {
                Name = "A002",
                Id = Guid.NewGuid(),
                Perm = PermissionTypes.Employee,
            };
            Console.WriteLine($"Patent 2:\n{p2.Name}\n {p2.Perm}");
            Component p3 = new Patent
            {
                Name = "A003",
                Id = Guid.NewGuid(),
                Perm = PermissionTypes.Employee,
            };
            Component f1 = new Family
            {
                Name = "Administrator",
                Id = Guid.NewGuid(),
                Perm = PermissionTypes.Manager,
            };
            f1.AddChild(p1);
            f1.AddChild(p2);
            f1.AddChild(p3);
            Console.WriteLine($"Family 1:\n{f1.Name}\n {f1.Perm}\n Patents:");
            foreach (var child in f1.Children)
            {
                Console.WriteLine($" - {child.Name}");
            }

            Console.ReadKey();

            Console.Clear();
            User u1 = new User
            {
                Id = Guid.NewGuid(),
                Name = "admin",
                Password = "admin",
            };
            u1.Permissions.Add(f1);
            Console.WriteLine($"User 1:\n{u1.Name}\n Permissions:");
            foreach (var perm in u1.Permissions)
            {
                Console.WriteLine($" - {perm.Name} ({perm.GetType().Name})");
                if (perm is Family family)
                {
                    Console.WriteLine("   Patents:");
                    foreach (var child in family.Children)
                    {
                        Console.WriteLine($"    - {child.Name}");
                    }
                }
            }
            Console.ReadKey();
        }
    }
}