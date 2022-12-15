using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Distress;

[JsonConverter(typeof(PacketValueConverter))]
public class PacketValue : IComparable<PacketValue>, IComparable
{
    public PacketValue(int intValue)
    {
        IntValue = intValue;
        IsArray = false; // Default false
    }
    
    public PacketValue(PacketValue[] arrayValue)
    {
        ArrayValue = arrayValue;
        IsArray = true;
    }
    
    [MemberNotNullWhen(true, nameof(ArrayValue))]
    [MemberNotNullWhen(false, nameof(IntValue))]
    public bool IsArray { get; set; }
    
    public int? IntValue { get; }
    
    public PacketValue[]? ArrayValue { get; }

    public int CompareTo(PacketValue? second)
    {
        if (ReferenceEquals(this, second)) return 0;
        if (ReferenceEquals(null, second)) return 1;
        
        return ComparePacketValues(this, second);
    }

    private static int ComparePacketValues(PacketValue left, PacketValue right)
    {
        if (left.IsArray)
        {
            if (right.IsArray)
            {
                return CompareArrays(left.ArrayValue, right.ArrayValue);
            }
            
            var rightArray = new[] { right };
            return CompareArrays(left.ArrayValue, rightArray);
        }

        if (right.IsArray)
        {
            // Compare integer with array
            var leftArray = new[] { left };
            return CompareArrays(leftArray, right.ArrayValue);
        }
        else
        {
            // Compare two integers
            return (left.IntValue.Value - right.IntValue.Value);
        }
    }

    private static int CompareArrays(PacketValue[] first, PacketValue[] second)
    {
        if (first.Length > second.Length)
        {
            return -1;
        }

        for (var idx = 0; idx != first.Length; idx++)
        {
            var compared = first[idx].CompareTo(second[idx]);
            if (compared != 0)
            {
                return compared;
            }
        }
        
        // Otherwise they are equal
        return 0;
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is PacketValue other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(PacketValue)}");
    }
}