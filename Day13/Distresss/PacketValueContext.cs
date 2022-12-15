using System.Text.Json.Serialization;
using Distress;

[JsonSerializable(typeof(PacketValue))]
[JsonSerializable(typeof(PacketValue[]))]
public partial class PacketValueContext : JsonSerializerContext
{
}