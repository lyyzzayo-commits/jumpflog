using UnityEngine;

public class LavaManager : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameManager gameManager;
    
    public void ResetPosition(float y) //위치 리셋
    {
        
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    public void ResetLava(float startY, float startSpeed = 0f, bool active = false) //상태 리셋
    {
        ResetPosition(startY);
        speed = startSpeed;
        isActive = active;
    }

    public void SetActive(bool active) 
    {
        isActive = active;
    }

    public void SetSpeed(float unitsPerSec)//속도 설정
    {
        speed = unitsPerSec;
    }

    public void Tick(float deltaTime)//용암 이동 로직
    {
        
        if (!isActive) return;

        Vector3 pos  = transform.position;
        pos.y += speed * deltaTime;
        transform.position = pos;

    }
    private bool triggered;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Frog")) return;

        triggered = true;

        gameManager.GameOver();
    }
}
