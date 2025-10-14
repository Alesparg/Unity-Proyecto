using UnityEngine;

public class HeartTest : MonoBehaviour
{
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            HeartSystem heartSystem = Object.FindFirstObjectByType<HeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.LoseHeart();
            }
            else
            {
                Debug.LogError("No se encontró HeartSystem!");
            }
        }

        
        if (Input.GetKeyDown(KeyCode.G))
        {
            HeartSystem heartSystem = Object.FindFirstObjectByType<HeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.GainHeart();
            }
        }

        
        if (Input.GetKeyDown(KeyCode.R))
        {
            HeartSystem heartSystem = Object.FindFirstObjectByType<HeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.ResetHearts();
            }
        }
    }
}

