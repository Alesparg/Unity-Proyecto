using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.DocumentationSortingAttribute;

public class Jugador : MonoBehaviour
{
    [SerializeField]
    private PerfilJugador perfilJugador;
    public PerfilJugador PerfilJugador { get => perfilJugador; }


    //---------- Eventos del Jugador ----------//

    [SerializeField]
    private UnityEvent<int> OnLivesChanged;

    [SerializeField]
    private UnityEvent<string> OnTextChanged;

    private void Start()
    {
        if (perfilJugador != null)
        {
            OnLivesChanged.Invoke(perfilJugador.Vida);
            OnTextChanged.Invoke(perfilJugador.Vida.ToString());
        }
        else
        {
            Debug.LogWarning("PerfilJugador no está asignado en el Inspector!");
        }
    }

    public void ModificarVida(int puntos)
    {
        if (perfilJugador != null)
        {
            perfilJugador.Vida += puntos;
            OnTextChanged.Invoke(perfilJugador.Vida.ToString());
            OnLivesChanged.Invoke(perfilJugador.Vida);
            Debug.Log(EstasVivo());
        }
        else
        {
            Debug.LogWarning("PerfilJugador no está asignado en el Inspector!");
        }
    }

    private bool EstasVivo()
    {
        if (perfilJugador != null)
        {
            return perfilJugador.Vida > 0;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("meta")) { return; }

        Debug.Log("GANASTE");
    }
}

