using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI miTexto; // Texto principal (ej: puntaje, mensajes)
    [SerializeField] private GameObject iconoVida; // Prefab de un ícono de vida
    [SerializeField] private GameObject contenedorIconosVida; // Padre donde se instancian los iconos

    // 🔹 Actualiza el texto del HUD
    public void ActualizarTextoHUD(string nuevoTexto)
    {
        Debug.Log("SE LLAMA: " + nuevoTexto);
        if (miTexto != null)
            miTexto.text = nuevoTexto;
    }

    // 🔹 Actualiza la cantidad de vidas en pantalla
    public void ActualizarVidasHUD(int vidas)
    {
        Debug.Log("ESTÁS ACTUALIZANDO VIDAS");

        if (EstaVacioContenedor())
        {
            CargarContenedor(vidas);
            return;
        }

        if (CantidadIconosVidas() > vidas)
        {
            EliminarUltimoIcono();
        }
        else if (CantidadIconosVidas() < vidas)
        {
            CrearIcono();
        }
    }

    // ----------------- Métodos auxiliares -----------------

    private int CantidadIconosVidas()
    {
        return contenedorIconosVida.transform.childCount;
    }

    private void EliminarUltimoIcono()
    {
        Transform contenedor = contenedorIconosVida.transform;
        if (contenedor.childCount > 0)
        {
            Destroy(contenedor.GetChild(contenedor.childCount - 1).gameObject);
        }
    }

    private void CargarContenedor(int cantidadIconos)
    {
        for (int i = 0; i < cantidadIconos; i++)
        {
            CrearIcono();
        }
    }

    private void CrearIcono()
    {
        if (iconoVida != null && contenedorIconosVida != null)
        {
            Instantiate(iconoVida, contenedorIconosVida.transform);
        }
    }

    private bool EstaVacioContenedor()
    {
        return contenedorIconosVida.transform.childCount == 0;
    }
}


