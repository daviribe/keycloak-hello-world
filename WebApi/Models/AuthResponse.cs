using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApi.Models
{
    public partial class AuthResponse
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }

        [JsonProperty("expires_in")] public long ExpiresIn { get; set; }

        [JsonProperty("refresh_expires_in")] public long RefreshExpiresIn { get; set; }

        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }

        [JsonProperty("token_type")] public string TokenType { get; set; }

        [JsonProperty("id_token")] public string IdToken { get; set; }

        [JsonProperty("not-before-policy")] public long NotBeforePolicy { get; set; }

        [JsonProperty("session_state")] public Guid SessionState { get; set; }

        [JsonProperty("scope")] public string Scope { get; set; }
    }

    public partial class AuthResponse
    {
        public static AuthResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AuthResponse>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this AuthResponse self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new()
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }
}