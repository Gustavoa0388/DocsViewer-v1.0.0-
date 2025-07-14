using System;
using System.IO;
using Newtonsoft.Json;

public class AppConfig
{
    public string DatabasePath { get; set; }
    public string DocumentsPath { get; set; }
    public string LogoPath { get; set; }
    public string MetadataPath { get; set; }
    public string UpdatePath { get; set; }      // <-- aqui!
    public int LoginLockoutTentativas { get; set; } = 3;
    public int LoginLockoutTempo { get; set; } = 1; // em minutos


    private static string GetConfigFilePath(string databasePath)
        => Path.Combine(databasePath, "appsettings.json");

    public static AppConfig Load()
    {
        // Busca o caminho inicial padrão
        string databasePath = @"\\D4MDP574\Doc Viewer$\Banco de dados";
        string configFile = GetConfigFilePath(databasePath);

        // Tenta buscar o caminho real do último arquivo salvo (isso evita loop)
        if (File.Exists(configFile))
        {
            // Lê o databasePath salvo (pode estar atualizado)
            var temp = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configFile));
            if (!string.IsNullOrEmpty(temp.DatabasePath))
            {
                databasePath = temp.DatabasePath;
                configFile = GetConfigFilePath(databasePath);
            }
        }

        // Agora carrega a configuração real
        if (File.Exists(configFile))
        {
            return JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configFile));
        }

        // Se não existir, retorna padrão
        return new AppConfig
        {
            DatabasePath = databasePath,
            DocumentsPath = "",
            MetadataPath = ""
        };
    }

    public void Save()
    {
        // Sempre salva no diretório do banco centralizado
        string configFile = GetConfigFilePath(this.DatabasePath);

        if (!Directory.Exists(this.DatabasePath))
            Directory.CreateDirectory(this.DatabasePath);

        // Backup
        if (File.Exists(configFile))
            File.Copy(configFile, configFile + ".bak", true);

        File.WriteAllText(configFile, JsonConvert.SerializeObject(this, Formatting.Indented));
    }

    public static string GetDatabasePath() => Load().DatabasePath;
    public static string GetDocumentsPath() => Load().DocumentsPath;
    public static string GetMetadataPath() => Load().MetadataPath;
}
