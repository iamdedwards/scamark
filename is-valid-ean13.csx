#!/usr/bin/env dotnet-script

// Parse command line arguments
var args = Args.ToArray();
string eanCode = args.Length > 0 ? args[0] : null;

if (string.IsNullOrEmpty(eanCode))
{
    Console.WriteLine("Usage: dotnet script ./is-valid-ean13.csx <ean-code>");
    Console.WriteLine("Example: dotnet script ./is-valid-ean13.csx 1234567890123");
    Environment.Exit(1);
}

// Validate EAN code
bool isValid = ValidateEAN13(eanCode);

Console.WriteLine($"EAN Code: {eanCode}");
Console.WriteLine($"Valid: {(isValid ? "Yes" : "No")}");

Environment.Exit(isValid ? 0 : 1);

static bool ValidateEAN13(string ean)
{
    // Remove any whitespace
    ean = ean.Trim();

    // Must be exactly 13 digits 
    if (ean.Length != 13)
        return false;

    var checksum = 0;
    // Iterate through the first 12 digits of the EAN code to calculate the checksum
    for (int i = 0; i < ean.Length - 1; i++)
    {
        // EAN codes should only contain digits
        if (!char.IsDigit(ean[i]))
            return false;

        var digitValue = (int)char.GetNumericValue(ean[i]);

        // Checksum is the sum of the digits, weighted by their position
        // Even indexed digits (zero-based) are multiplied by 1, odd indexed digits by 3
        var isOddIndex = i % 2 == 1;
        var weightedValue = isOddIndex ? digitValue * 3 : digitValue;
        checksum += weightedValue;
    }

    // Calculate check digit
    int checkDigit = (10 - (checksum % 10)) % 10;

    // Compare the calculated check digit with the last digit of the EAN code
    return checkDigit == (int)char.GetNumericValue(ean[12]);    
}
