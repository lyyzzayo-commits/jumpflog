using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CameraClimb cameraClimb;
    [SerializeField] private DifficultyDirector difficultyDirector;
    [SerializeField] private LavaManager lavaManager;
    [SerializeField] private ObstacleSpawner spawner;
    [SerializeField] private Transform player;
    [SerializeField] private DragController dragController;
    [SerializeField] private PlayerResetter playerResetter;

    [Header("Start Settings")]
    [SerializeField] private float cameraStartOffsetY = 1.5f;
    [SerializeField] private float lavaStartOffsetFromBottom = 2.0f;

    [Header("Lava Reset")]
    [SerializeField] private bool useSceneStartOnReset = true;

    [Header("UI")]
    [SerializeField] GameObject TapToStartUI;
    [SerializeField] GameObject GameOverUI;
    private float score = 0f;

    private float timeSurvived;
    private GameState state;

    void Awake()
    {
        if (spawner != null && cameraClimb != null)
            spawner.Init(cameraClimb);

        ReadyGame();
    }

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

        
    }

    private void ApplyStateLocks()
    {
        bool playing = (state == GameState.Playing);

        if (dragController != null) dragController.EnableInput(playing);

        // 용암/스포너/카메라 차단/허용
        if (lavaManager != null) lavaManager.SetActive(playing);
        if (spawner != null) spawner.SetActive(playing);
        if (cameraClimb != null) cameraClimb.SetActive(playing);
    }


    public void StartGame()
    {
        state = GameState.Playing;
        timeSurvived = 0f;

        playerResetter.SetPhysicsActive(true);

        float cameraStartY = player.position.y;
        cameraClimb.ResetY(cameraStartY);
        cameraClimb.SetTarget(player);
        cameraClimb.SetActive(true);
        playerResetter.ResetPlayer();

        if (lavaManager != null)
        {
            if (useSceneStartOnReset)
            {
                lavaManager.ResetToSceneStart(startSpeed: 0f, active: true);
            }
            else
            {
                float lavaStartY = cameraClimb.BottomY + lavaStartOffsetFromBottom;
                lavaManager.ResetLava(lavaStartY, startSpeed: 0f, active: true);
            }
        }
        spawner.ResetSpawner();
            
        ApplyStateLocks();

    }

    public void ReadyGame()
    {
        state = GameState.Ready;
        
        if(failTrigger != null) failTrigger.ResetTrigger();

        playerResetter.SetPhysicsActive(false);

        TapToStartUI.SetActive(true);
        GameOverUI.SetActive(false);

        ApplyStateLocks();
    }

    public void TapToStart()
    {

        TapToStartUI.SetActive(false);
        GameOverUI.SetActive(false);

        StartGame();
    }

    public void GameOver()
    {
        if (state == GameState.GameOver) return;

        state = GameState.GameOver;

        GameOverUI.SetActive(true);
        TapToStartUI.SetActive(false);

        ApplyStateLocks();
        playerResetter.SetPhysicsActive(false);

        ReadyGame();
    }

    [SerializeField] private FailTrigger failTrigger;

    private GameState gameState = GameState.Ready;

    private void OnEnable()
    {
        Debug.Log("구독 시작");
        if (failTrigger != null)
        {
            failTrigger.OnGameOver += HandleGameOver;
            Debug.Log("찐 구독");
        }
    }

    private void OnDisable()
    {
        if (failTrigger != null)
            failTrigger.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        GameOver();
    }
    
} 