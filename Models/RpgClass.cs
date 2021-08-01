using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RpgClass
{
    Knight,
    Assasin,
    Mage
}