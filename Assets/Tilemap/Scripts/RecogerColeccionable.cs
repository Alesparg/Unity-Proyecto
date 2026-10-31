using UnityEngine;

public class RecogerColeccionable : MonoBehaviour
{
    // El contador ahora vive en el GameManager. Se mantiene el nombre público
    // "Recogidos" para no romper a quien ya lo consultaba (CartelVictoria).
    public static int Recogidos => GameManager.Existe ? GameManager.Instance.EstrellasRecogidas : respaldo;

    private static int respaldo;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ReiniciarContador()
    {
        respaldo = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) { return; }

        if (GameManager.Existe) { GameManager.Instance.RegistrarEstrella(); }
        else { respaldo++; }

        gameObject.SetActive(false);
    }
}
