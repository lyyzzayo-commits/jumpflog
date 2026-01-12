using UnityEngine;

#region Data Structs

[System.Serializable]
public struct DifficultySnapshot
{
    public int tier;
    public float lavaSpeed;
    public ObstacleSpawnSettings obstacleSettings;
}

#endregion

public sealed class DifficultyDirector : MonoBehaviour
{
    #region Inspector - Tier Thresholds

    [Header("Tier (by progress)")]
    [SerializeField] private float tier1End = 20f;
    [SerializeField] private float tier2End = 50f;
    [SerializeField] private float tier3End = 90f;

    #endregion

    #region Inspector - Lava Speed (units/sec)

    [Header("Lava Speed (units/sec)")]
    [SerializeField] private float lavaSpeedT1 = 0.6f;
    [SerializeField] private float lavaSpeedT2 = 0.9f;
    [SerializeField] private float lavaSpeedT3 = 1.2f;
    [SerializeField] private float lavaSpeedT4 = 1.6f;

    #endregion

    #region Inspector - Obstacle Spawn Area

    [Header("Obstacle Spawn Area")]
    [SerializeField] private float spawnMarginY = 2.0f;
    [SerializeField] private float xMin = -10f;
    [SerializeField] private float xMax = 10f;

    #endregion

    #region Inspector - Obstacle Interval (sec)

    [Header("Obstacle Spawn Interval (sec)")]
    [SerializeField] private float intervalT1 = 1.2f;
    [SerializeField] private float intervalT2 = 0.95f;
    [SerializeField] private float intervalT3 = 0.75f;
    [SerializeField] private float intervalT4 = 0.60f;

    #endregion

    #region Inspector - Obstacle Fall Speed (units/sec)

    [Header("Obstacle Fall Speed (units/sec)")]
    [SerializeField] private float fallMinT1 = 2.5f;
    [SerializeField] private float fallMaxT1 = 3.5f;
    [SerializeField] private float fallMinT4 = 4.5f;
    [SerializeField] private float fallMaxT4 = 6.5f;

    #endregion

    #region Inspector - Progress Composition (optional)

    [Header("Progress Composition (optional)")]
    [Tooltip("progress = height + timeSurvived * timeToHeightScale")]
    [SerializeField] private float timeToHeightScale = 0.4f;

    #endregion

    #region Public API - Evaluate

    // 1) height�� ���� �⺻ �� (����: CameraClimb.MaxY)
    public DifficultySnapshot EvaluateByHeight(float height) => EvaluateByHeight(height, 0f);

    // 2) height + timeSurvived ȥ�� ��(����)
    public DifficultySnapshot EvaluateByHeight(float height, float timeSurvived)
    {
        float progress = ComposeProgress(height, timeSurvived);
        return EvaluateByProgress(progress);
    }

    // 3) �̹� progress�� ����ؼ� ���� ���� ��(�ɼ�)
    public DifficultySnapshot EvaluateByProgressPublic(float progress)
    {
        return EvaluateByProgress(progress);
    }

    #endregion

    #region Public API - Parts (Lava / Spawner)

    public int GetTier(float progress) //Ƽ�� ���ϱ�
    {
        if (progress < tier1End) return 1;
        if (progress < tier2End) return 2;
        if (progress < tier3End) return 3;
        return 4;
    }

    public float GetLavaSpeed(float progress) //��� �ӵ� ���ϱ�
    {
        int tier = GetTier(progress);
        return tier switch
        {
            1 => lavaSpeedT1,
            2 => lavaSpeedT2,
            3 => lavaSpeedT3,
            _ => lavaSpeedT4
        };
    }

    public ObstacleSpawnSettings GetObstacleSettings(float progress) //��ֹ� ����
    {
        int tier = GetTier(progress);

        var s = new ObstacleSpawnSettings
        {
            spawnMarginY = spawnMarginY,
            xMin = xMin,
            xMax = xMax,
            interval = tier switch
            {
                1 => intervalT1,
                2 => intervalT2,
                3 => intervalT3,
                _ => intervalT4
            }
        };

        // progress�� tier3End������ ���� ����, ���Ĵ� max(=1)�� ����
        float t = Remap01(progress, 0f, tier3End);
        s.fallSpeedMin = Mathf.Lerp(fallMinT1, fallMinT4, t);
        s.fallSpeedMax = Mathf.Lerp(fallMaxT1, fallMaxT4, t);

        return s;
    }

    #endregion

    #region Core - Internal Evaluate

    private DifficultySnapshot EvaluateByProgress(float progress) //DifficultySnapshot ���
    {
        int tier = GetTier(progress);
        return new DifficultySnapshot
        {
            tier = tier,
            lavaSpeed = GetLavaSpeed(progress),
            obstacleSettings = GetObstacleSettings(progress)
        };
    }

    #endregion

    #region Core - Progress Composition

    private float ComposeProgress(float height, float timeSurvived) //progress ���
    {
        return height + timeSurvived * timeToHeightScale;
    }

    #endregion

    #region Helpers (Internal)

    private static float Remap01(float value, float min, float max) //�ܼ� ��� �����
    {
        if (max <= min) return 0f;
        return Mathf.Clamp01((value - min) / (max - min));
    }

    // ���� �ڵ忡���� EaseIn/EaseOut/TierProgress01�� ������� �ʾƼ� �����ص� ��.
    // ���߿� � ���̵�(�ε巯�� ����)�� ���� �� �ٽ� �߰��ϸ� ��.

    #endregion
}
