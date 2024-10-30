namespace CipherForge;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Please choose an option by typing the corresponding number:");
        Console.WriteLine("1: Convert an AES key to the keychain format");
        Console.WriteLine("2: Convert a keychain to an AES key");
        Console.Write("Enter your choice (1 or 2): ");

        if (!int.TryParse(Console.ReadLine(), out var choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Invalid input. Please restart the program and enter either 1 or 2.");
            return;
        }
        
        var result = string.Empty;
        
        switch (choice)
        {
            case 1:
            {
                Console.WriteLine("Please enter the AES Key");
                var aes = Console.ReadLine()?.SubstringAfter("0x");
                
                if (string.IsNullOrEmpty(aes))
                {
                    Console.WriteLine("Invalid AES Key. Please restart the program");
                    return;
                }
                
                Console.WriteLine("Please enter the GUID");
                var guid = Console.ReadLine()?.Replace("-", string.Empty);

                if (string.IsNullOrEmpty(guid))
                {
                    Console.WriteLine("Invalid GUID. Please restart the program");
                    return;
                }
                
                result = ConvertHexToKeychain(aes, guid);
                break;
            }
            case 2:
            {
                Console.WriteLine("Please enter the keychain");
                var keyChain = Console.ReadLine()?.SubstringAfter(":");

                if (string.IsNullOrEmpty(keyChain))
                {
                    Console.WriteLine("Invalid keychain. Please restart the program");
                    return;
                }
                
                result = ConvertKeychainToHex(keyChain);
                break;
            }
        }
        
        Console.WriteLine(result);
    }

    private static string ConvertHexToKeychain(string aes, string guid)
    {
        var length = aes.Length;
        var bytes = new byte[length / 2];
        for (var i = 0; i < length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(aes.Substring(i, 2), 16);
        }

        var base64String = Convert.ToBase64String(bytes);
        return $"{guid}:{base64String}";
    }
    
    private static string ConvertKeychainToHex(string keyChain)
    {
        var bytes = Convert.FromBase64String(keyChain);
        var aes = BitConverter.ToString(bytes).Replace("-", string.Empty);

        return $"0x{aes}";
    }
    
    // https://github.com/FabianFG/CUE4Parse/blob/master/CUE4Parse/Utils/StringUtils.cs#L52-L57
    private static string SubstringAfter(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var index = s.IndexOf(delimiter, comparisonType);
        return index == -1 ? s : s.Substring(index + delimiter.Length, s.Length - index - delimiter.Length);
    }
}