using ChangeBaseNumberSystem.Models;
using System;

namespace B4_NumberConverter
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var converter = new NumberConverter();

            while (true)
            {
                Console.WriteLine("CHANGE BASE NUMBER SYSTEM (2, 10, 16)");
                Console.WriteLine("1) Binary (BIN)");
                Console.WriteLine("2) Decimal (DEC)");
                Console.WriteLine("3) Hexadecimal (HEX)");
                Console.WriteLine("Chon he co so dau vao (1-3) hoac B de thoat:");

                string? inChoice = Console.ReadLine();
                if (string.Equals(inChoice, "b", StringComparison.OrdinalIgnoreCase)) break;
                if (!TryMapChoice(inChoice, out BaseSystem fromBase))
                {
                    Console.WriteLine("Invalid choice!\n");
                    continue;
                }

                Console.WriteLine("Chon he so dau ra (1-3):");
                string? outChoice = Console.ReadLine();
                if (!TryMapChoice(outChoice, out BaseSystem toBase))
                {
                    Console.WriteLine("Invalid choice!\n");
                    continue;
                }

                Console.Write($"Value Enter ({Label(fromBase)}): ");
                string inputValue = Console.ReadLine() ?? string.Empty;

                if (!converter.TryConvert(inputValue, fromBase, toBase, out string? output, out string? error))
                {
                    Console.WriteLine($"Error: {error}\n");
                    continue;
                }

                Console.WriteLine($"Result of ({Label(toBase)}): {output}\n");
            }

            Console.WriteLine("Exit.");
        }

        static bool TryMapChoice(string? choice, out BaseSystem result)
        {
            result = BaseSystem.Decimal;
            switch (choice?.Trim())
            {
                case "1": result = BaseSystem.Binary; return true;
                case "2": result = BaseSystem.Decimal; return true;
                case "3": result = BaseSystem.Hexadecimal; return true;
                default: return false;
            }
        }

        static string Label(BaseSystem b) => b switch
        {
            BaseSystem.Binary => "BIN",
            BaseSystem.Decimal => "DEC",
            BaseSystem.Hexadecimal => "HEX",
            _ => b.ToString()
        };
    }
}
