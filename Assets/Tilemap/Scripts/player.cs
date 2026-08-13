using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int lives = 3;

    public UnityEvent<int> OnLivesChanged;

    public void LoseLife()
    {
        lives--;
        OnLivesChanged.Invoke(lives);

        if (lives <= 0)
        {
            // Aquí puedes manejar la situación de Game Over
        }
    }
}
