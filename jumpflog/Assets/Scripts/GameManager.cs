using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CameraClimb cameraClimb;
    [SerializeField] private DifficultyDirector difficultyDirector;
    [SerializeField] private LavaManager lavaManager;
    [SerializeField] private ObstacleSpawner spawner;
    [SerializeField] private Transform player;

    [Header("Start Settings")]
    [SerializeField] private float cameraStartOffsetY = 1.5f;
    [SerializeField] private float lavaStartOffsetFromBottom = 2.0f;

    [Header("UI")]
    [SerializeField] GameObject TapToStartUI;
    [SerializeField] GameObject GameOverUI;
    private float score = 0f;

    private float timeSurvived;
    private GameState state;

    void Update()
    {
        if (state != GameState.Playing) return;

        float dt = Time.deltaTime;
        timeSurvived += dt;

        // 1. 카메라 이동
        cameraClimb.Tick(dt);

        // 2. 진행도 계산 (정의된 규칙)
        float height = cameraClimb.MaxY;

        // 3. 난이도 평가
        DifficultySnapshot diff = difficultyDirector.EvaluateByHeight(height, timeSurvived);

        // 4. 용암 적용
        lavaManager.SetSpeed(diff.lavaSpeed);
        lavaManager.Tick(dt);

        // 5. 장애물 스폰
        spawner.ApplySettings(diff.obstacleSettings);
        spawner.Tick(dt);

        score += Time.deltaTime;
    }



    public void StartGame()
    {
        state = GameState.Playing;
        timeSurvived = 0f;

        float cameraStartY = player.position.y;
        cameraClimb.ResetY(cameraStartY);
        cameraClimb.SetTarget(player);
        cameraClimb.SetActive(true);

        float lavaStartY = cameraClimb.BottomY - lavaStartOffsetFromBottom;
        lavaManager.ResetLava(lavaStartY, startSpeed: 0f, active: true);

        spawner.ResetSpawner();
        spawner.SetActive(true);
    }
    public void ReadyGame()
    {
        state = GameState.Ready;

        TapToStartUI.SetActive(true);
    }

    public void TapToStart()
    {
        StartGame();

        if (state == GameState.Ready)
        {
            TapToStartUI.SetActive(false);
        }
        if (state == GameState.GameOver)
        {
            GameOverUI.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (state == GameState.GameOver) return;

        state = GameState.GameOver;

        GameOverUI.SetActive(true);

        cameraClimb.SetActive(false);
        lavaManager.SetActive(false);
        spawner.SetActive(false);
    }
    
} 
