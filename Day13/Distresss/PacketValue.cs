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

     public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is PacketValue other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(PacketValue)}");
    }

    public int CompareTo(PacketValue? second)
    {
        if (ReferenceEquals(this, second)) return 0;
        if (ReferenceEquals(null, second)) return 1;
        
        return ComparePacketValues(this, second);
    }
    
    private static int ComparePacketValues(PacketValue first, PacketValue second)
    {
        if (first.IsArray)
        {
            if (second.IsArray)
            {
                return CompareArrays(first.ArrayValue, second.ArrayValue);
            }
            else
            {
                // Condition 3 - one list / one int
                var secondWrapper = new[] { second };
                return CompareArrays(first.ArrayValue, secondWrapper);
            }
        }
        else
        {
            if (second.IsArray)
            {
                // Condition 3 - one int / one list
                // Condition 3 - one list / one int
                var firstWrapped = new[] { first };
                return CompareArrays(firstWrapped, second.ArrayValue);
            }
            else
            {
                // Condition 1 - both ints.
                // Lower value should come first.
                return first.IntValue.Value - second.IntValue.Value;
            }
        }
    }
    
    private static int CompareArrays(PacketValue[] first, PacketValue[] second)
    {
        // Condition 2 - both are lists
        // Compare each value one-by-one:
        var end = Math.Max(first.Length, second.Length);
        for (var i = 0; i < end; i++)
        {
            // If first runs out of items first, then they are in order.
            if (i >= first.Length)
                return -1;

            // If second runs out of items first, then they are out of order.
            if (i >= second.Length)
                return 1;

            // Compare each item.
            // If there is a difference, then that is the result.
            var itemResult = first[i].CompareTo(second[i]);
            if (itemResult != 0)
            {
                return itemResult;
            }
        }

        // Otherwise they are equal
        return 0;
    }
}