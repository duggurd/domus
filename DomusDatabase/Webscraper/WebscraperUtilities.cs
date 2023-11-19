namespace Domus;

public static class WebscraperUtilities
{
    ///<summary>
    ///Gets all public properties and values for object
    ///</summary>
    public static Dictionary<string, string> GetPropsAndValues(object thing)
    {
        Dictionary<string, string> propsValues = new();

        foreach (var prop in thing.GetType().GetProperties())
        {
            propsValues[prop.Name] = prop.GetValue(thing)!.ToString() ?? "null";
        }
        return propsValues;
    }

    ///<summary>
    ///Extracts all numbers from a string
    ///</summary>
    ///
    ///To do:
    ///  Add float handling
    public static string ExtractNums(string data)
    {
        
        if (data.Contains("-"))
        {
            data = data.Split('-')[0];
        }
        
        var numbers = "1234567890";
        var res = String.Concat(data.Where(c => numbers.Contains(c)));

        return res;
    }

    ///<summary>
    ///Parses address, removing bloat, returning only needed info.
    ///</summary>
    public static string ParseAddress(string address)
    {
        if (address.Contains("-"))
        {
            address = address.Split("-")[0];
        }
        else if (address.Count<char>(c => c == ',') > 2)
        {
            var adr = address.Split(address.First(c => c == ','))[0];
            System.Console.WriteLine(adr);
            var kommune = address.Split(address.Last(c => c == ','))[1];

            address = adr + kommune;
            System.Console.WriteLine(address);
        }
        if (address.Contains(','))
        {
            address = address.Remove(address.IndexOf(','), 1);
        }
        
        return address;
    }
}