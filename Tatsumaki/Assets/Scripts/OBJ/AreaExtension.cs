using UnityEngine;

public enum Area
{
    FirstArea,
    SecondArea,
    ThirdArea,
    FourthArea
}

public static class AreaExtension
{
    public static Vector2 GetAreaSign(this Area area)
    {
        return area switch
        {
            Area.FirstArea => new Vector2(1, 1),
            Area.SecondArea => new Vector2(-1, 1),
            Area.ThirdArea => new Vector2(-1, -1),
            Area.FourthArea => new Vector2(1, -1),
            _ => Vector2.zero,
        };
    }

    public static Area GetNextArea(this Area area)
    {
        int nextAreaIndex = ((int)area + Random.Range(1, 4)) % 4;
        return (Area)nextAreaIndex;
    }
}