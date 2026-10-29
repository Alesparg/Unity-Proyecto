using UnityEngine;

public class RecogerColeccionable : MonoBehaviour
{
    public static int Recogidos { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ReiniciarContador()
    {
        Recogidos = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) { return; }

        Recogidos++;
        gameObject.SetActive(false);
    }
}
