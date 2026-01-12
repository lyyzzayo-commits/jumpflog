using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraClimb cameraClimb;
    [SerializeField] private DifficultyDirector difficultyDirector;
    void Update()
    {
        float progress = cameraClimb.MaxY;
        var diff = difficultyDirector.Evaluate(height);
    }
}
