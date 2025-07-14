using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LoginLockoutInfo
{
    public int Tentativas { get; set; }
    public DateTime? BloqueadoAte { get; set; }
}

public static class LoginLockoutHelper
{
    private static string LockoutFile => Path.Combine(AppConfig.GetDatabasePath(), "loginLockout.json");

    public static LoginLockoutInfo GetInfo(string user)
    {
        var dict = LoadLockouts();
        if (!dict.ContainsKey(user))
            dict[user] = new LoginLockoutInfo();
        return dict[user];
    }

    public static void SetInfo(string user, int tentativas, DateTime? bloqueadoAte)
    {
        var dict = LoadLockouts();
        dict[user] = new LoginLockoutInfo { Tentativas = tentativas, BloqueadoAte = bloqueadoAte };
        SaveLockouts(dict);
    }

    public static bool EstaBloqueado(string user, out TimeSpan? tempoRestante)
    {
        var info = GetInfo(user);
        tempoRestante = null;
        if (info.BloqueadoAte.HasValue && info.BloqueadoAte.Value > DateTime.Now)
        {
            tempoRestante = info.BloqueadoAte.Value - DateTime.Now;
            return true;
        }
        return false;
    }

    public static void RegistrarTentativa(string user, int tentativasPermitidas, int tempoBloqueioMin, bool sucesso)
    {
        var info = GetInfo(user);
        if (sucesso)
        {
            SetInfo(user, 0, null);
        }
        else
        {
            int novasTentativas = info.Tentativas + 1;
            DateTime? bloqueio = null;
            if (novasTentativas >= tentativasPermitidas)
                bloqueio = DateTime.Now.AddMinutes(tempoBloqueioMin);

            SetInfo(user, novasTentativas, bloqueio);
        }
    }

    public static List<string> GetAllLockedUsers()
    {
        var locked = new List<string>();
        var dict = LoadLockouts();
        foreach (var kvp in dict)
        {
            if (kvp.Value.BloqueadoAte.HasValue && kvp.Value.BloqueadoAte.Value > DateTime.Now)
                locked.Add(kvp.Key);
        }
        return locked;
    }

    public static void ResetUser(string user)
    {
        SetInfo(user, 0, null);
    }

    private static Dictionary<string, LoginLockoutInfo> LoadLockouts()
    {
        if (!File.Exists(LockoutFile))
            return new Dictionary<string, LoginLockoutInfo>();
        return JsonConvert.DeserializeObject<Dictionary<string, LoginLockoutInfo>>(File.ReadAllText(LockoutFile));
    }

    private static void SaveLockouts(Dictionary<string, LoginLockoutInfo> lockouts)
    {
        File.WriteAllText(LockoutFile, JsonConvert.SerializeObject(lockouts, Formatting.Indented));
    }
}
