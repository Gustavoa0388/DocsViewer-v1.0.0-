using System;
using System.IO;
using Newtonsoft.Json;

namespace DocsViewer
{ 
public class LogoConfig
{
    public string LogoBase64 { get; set; }

    public static string GetLogoConfigPath()
    {
        // Lê o caminho do banco (DatabasePath) do AppConfig
        var appConfig = AppConfig.Load();
        string path = Path.Combine(appConfig.DatabasePath, "logoconfig.json");
        return path;
    }

    public static void SaveLogo(string imagePath)
    {
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        string base64String = Convert.ToBase64String(imageBytes);

        var config = new LogoConfig { LogoBase64 = base64String };
        string json = JsonConvert.SerializeObject(config, Formatting.Indented);

        File.WriteAllText(GetLogoConfigPath(), json);
    }

    public static LogoConfig LoadLogoConfig()
    {
        string path = GetLogoConfigPath();
        if (!File.Exists(path)) return null;
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<LogoConfig>(json);
    }
}
}
