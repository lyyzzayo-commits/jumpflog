using System;
using UnityEngine;

public sealed class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject obstaclePrefab;

    [Header("Parent (optional)")]
    [SerializeField] private Transform spawnRoot;

    [Header("Safety")]
    [Tooltip("한 프레임에 너무 많이 스폰되는 사고를 막기 위한 상한")]
    [SerializeField] private int maxSpawnsPerTick = 5;

    private CameraClimb cameraClimb;

    private bool isActive = true;
    private float timer = 0f;

    private ObstacleSpawnSettings settings;

    // =========================
    // 초기화/상태
    // =========================
    public void Init(CameraClimb camera)
    {
        cameraClimb = camera;
    }

    public void ResetSpawner()
    {
        timer = 0f;
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }

    // =========================
    // 난이도 적용
    // =========================
    public void ApplySettings(ObstacleSpawnSettings s)
    {
        settings = Normalize(s);
    }

    private static ObstacleSpawnSettings Normalize(ObstacleSpawnSettings s)
    {
        // interval 최소 보정 (0/음수/너무 작음 방지)
        if (s.interval < 0.02f) s.interval = 0.02f;

        // xMin/xMax swap
        if (s.xMax < s.xMin)
        {
            float t = s.xMin;
            s.xMin = s.xMax;
            s.xMax = t;
        }

        // fallSpeed swap
        if (s.fallSpeedMax < s.fallSpeedMin)
        {
            float t = s.fallSpeedMin;
            s.fallSpeedMin = s.fallSpeedMax;
            s.fallSpeedMax = t;
        }

        return s;
    }

    // =========================
    // 스폰 루프
    // =========================
    public void Tick(float dt)
    {
        if (!isActive) return;
        if (cameraClimb == null) return;
        if (obstaclePrefab == null) return;

        // dt가 이상한 값이면 무시(일시정지/스파이크 방지)
        if (dt <= 0f) return;

        // interval이 0이 되면 무한 스폰이므로 방지
        if (settings.interval <= 0f) return;

        timer += dt;

        int spawned = 0;

        // dt가 큰 프레임에서도 스폰 누락 최소화
        while (timer >= settings.interval)
        {
            timer -= settings.interval;

            SpawnOnceInternal();

            spawned++;
            if (spawned >= maxSpawnsPerTick)
            {
                // 과도한 누적을 방지: 남은 timer도 제한
                timer = Mathf.Min(timer, settings.interval);
                break;
            }
        }
    }

    /// <summary>
    /// 디버그/테스트/버튼용: 강제로 1회 스폰
    /// </summary>
    public bool TrySpawnOnce()
    {
        if (!isActive) return false;
        if (cameraClimb == null) return false;
        if (obstaclePrefab == null) return false;

        SpawnOnceInternal();
        return true;
    }

    // =========================
    // 내부 스폰 1회
    // =========================
    private void SpawnOnceInternal()
    {
        float x = UnityEngine.Random.Range(settings.xMin, settings.xMax);
        float y = cameraClimb.TopY + settings.spawnMarginY;

        SpawnAt(x, y);
    }

    private void SpawnAt(float worldX, float worldY)
    {
        Vector3 pos = new Vector3(worldX, worldY, 0f);

        GameObject go = (spawnRoot != null)
            ? Instantiate(obstaclePrefab, pos, Quaternion.identity, spawnRoot)
            : Instantiate(obstaclePrefab, pos, Quaternion.identity);

        float fallSpeed = UnityEngine.Random.Range(settings.fallSpeedMin, settings.fallSpeedMax);

        var rb = go.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.down * fallSpeed;
    }
}

