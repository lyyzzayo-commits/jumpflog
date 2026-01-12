using System;
using UnityEngine;

[Serializable] public struct ObstacleSpawnSettings
{
    public float interval;
    public float spawnMarginY;
    public float xMin;
    public float xMax;
    public float fallSpeedMin;
    public float fallSpeedMax;
}