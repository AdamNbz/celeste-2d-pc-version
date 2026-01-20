using UnityEngine;

public class StringHelper
{
    public static string StandardizeClassNameString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }
        // Trim leading and trailing whitespace
        string standardized = input.Trim();
        // Convert to lowercase
        standardized = standardized.ToLowerInvariant();
        while (standardized.Contains(" "))
        {
            standardized = standardized.Replace(" ", "");
        }
        while (standardized.Contains("_"))
        {
            standardized = standardized.Replace("_", "");
        }

        for (int i=0; i < standardized.Length; i++)
        {
            if (!char.IsLetter(standardized[i]))
            {
                standardized = standardized.Remove(i, 1);
            }
        }
        return standardized;
    }
}
