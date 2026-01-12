using UnityEngine;

public class LavaManager : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameManager gameManager;
    private float sceneStartY;
    
    public void ResetPosition(float y) //��ġ ����
    {
        
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }
    public void ResetToSceneStart(float startSpeed = 0f, bool active = false)
    {
        ResetPosition(sceneStartY);
        speed = startSpeed;
        isActive = active;
    }

    public void ResetLava(float startY, float startSpeed = 0f, bool active = false) //���� ����
    {
        ResetPosition(startY);
        speed = startSpeed;
        isActive = active;
    }

    public void SetActive(bool active) 
    {
        isActive = active;
    }

    public void SetSpeed(float unitsPerSec)//�ӵ� ����
    {
        speed = unitsPerSec;
    }

    public void Tick(float deltaTime)//��� �̵� ����
    {
        
        if (!isActive) return;

        Vector3 pos  = transform.position;
        pos.y += speed * deltaTime;
        transform.position = pos;

    }
}
