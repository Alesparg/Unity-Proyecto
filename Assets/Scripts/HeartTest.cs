using UnityEngine;

public class HeartTest : MonoBehaviour
{
    void Update()
    {
        // Presiona L para perder un corazón
        if (Input.GetKeyDown(KeyCode.L))
        {
            HeartSystem heartSystem = FindObjectOfType<HeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.LoseHeart();
            }
            else
            {
                Debug.LogError("No se encontró HeartSystem!");
            }
        }
        
        // Presiona G para ganar un corazón
        if (Input.GetKeyDown(KeyCode.G))
        {
            HeartSystem heartSystem = FindObjectOfType<HeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.GainHeart();
            }
        }
        
        // Presiona R para resetear
        if (Input.GetKeyDown(KeyCode.R))
        {
            HeartSystem heartSystem = FindObjectOfType<HeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.ResetHearts();
            }
        }
    }
}
