using System.Text.Json.Serialization;


namespace _NET_Course.Models
{
    /// <summary>
    /// An enum that containts all the availiable character classes.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight,
        Assasin,
        Mage
    }
}