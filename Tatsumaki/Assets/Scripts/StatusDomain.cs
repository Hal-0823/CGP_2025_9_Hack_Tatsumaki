using UnityEngine;

public class StatusDomain
{
    public StatusDomain()
    {
        Score = 0;
        Time = 60;
        UltraGauge = 0;
        MaxUltraGauge = 10;
        PlayerSize = 1.0f;
        PlayerSpeed = 0.5f;
        SpawnIntervalRange = new Vector2(0.2f, 1.5f);
        OBJSizeRange = new Vector2(0.2f, 1.5f);
        OBJLifeTime = 5.0f;
        FieldSize = 3.6f;
    }

    public int Time { get; set; }
    public int Score { get; set; }
    public float UltraGauge { get; set; }
    public float MaxUltraGauge { get; set; }
    public float PlayerSize { get; set; }
    public float PlayerSpeed { get; set; }
    public Vector2 SpawnIntervalRange { get; set; }
    public Vector2 OBJSizeRange { get; set; }
    public float OBJLifeTime { get; set; }
    public float FieldSize { get; set; }
}