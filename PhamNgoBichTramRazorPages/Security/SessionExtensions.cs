using System.Text.Json;

namespace PhamNgoBichTramRazorPages.Security;

public static class SessionExtensions
{
    public static void SetJson<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        var s = session.GetString(key);
        return string.IsNullOrWhiteSpace(s) ? default : JsonSerializer.Deserialize<T>(s);
    }
}


