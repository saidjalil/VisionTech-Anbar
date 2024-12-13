using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VisionTech_Anbar_Project.ViewModel;
using Package = VisionTech_Anbar_Project.Entities.Package;

namespace VisionTech_Anbar_Project.Utilts;

public class Decoder
{
    public string GenerateHash(IConfiguration configuration, Data package)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Prevent circular references
            Formatting = Formatting.Indented // Pretty-print JSON for readability
        };
        
        var Salt = configuration["SecuritySettings:Salt"];
        
        // 1. Serialize the Package object to JSON
        string serializedPackage = JsonConvert.SerializeObject(package,settings);
        
        // 2. Combine the salt from the config with the serialized package data
        string saltedData = Salt + serializedPackage;
        
        // 3. Compute the SHA-256 hash
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedData));
            
            // 4. Convert hash bytes to a hexadecimal string
            return ConvertToHexString(hashBytes);
        }
    }
    
    private string ConvertToHexString(byte[] hashBytes)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2")); // Convert each byte to a 2-digit hexadecimal string
        }
        return sb.ToString();
    }
    
    public bool VerifyHash(string hash,string Salt , IConfiguration configuration, Data package)
    {
        // 1. Serialize the Package object to JSON
        string serializedPackage = JsonConvert.SerializeObject(package);
    
        // 2. Combine the salt with the serialized data
        string saltedData = Salt + serializedPackage;
    
        // 3. Compute the SHA-256 hash
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedData));
        
            // 4. Convert hash bytes to a hexadecimal string
            string computedHash = ConvertToHexString(hashBytes);
        
            // 5. Check if the newly computed hash matches the stored hash
            return computedHash == hash;
        }
    }
}