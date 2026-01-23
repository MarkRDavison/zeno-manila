namespace zeno.manilla.engine.Utility;

public sealed class AmountRange
{
    public int Min { get; set; } = 0;
    public int Max { get; set; } = int.MaxValue;
    public int Current { get; set; } = 0;


    public int Available => Max - Current;

    public void Merge(AmountRange other)
    {
        Min = Math.Min(other.Min, Min);
        Max = other.Max + Max;
        Current = other.Current + Current;
    }

    public AmountRange Clone()
    {
        return new AmountRange
        {
            Min = Min,
            Current = Current,
            Max = Max
        };
    }
}