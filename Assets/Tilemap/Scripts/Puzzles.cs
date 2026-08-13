using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{

    [SerializeField] private GameObject bolsa;
    [SerializeField] private Transform padreObjetivos;

    private Queue<GameObject> objetivos;
    private Stack<GameObject> items;
    private Dictionary<String, GameObject> inventario;

    private Progresion progresionJugador;

    private void Awake()
    {
        objetivos = new Queue<GameObject>();
        items = new Stack<GameObject>();
        inventario = new Dictionary<String, GameObject>();
        CargarObjetivos();
        VerObjetivos();

        progresionJugador = GetComponent<Progresion>();
    }

    private void CargarObjetivos()
    {
        foreach (Transform objetivo in padreObjetivos)
        {
            objetivos.Enqueue(objetivo.gameObject);
        }
    }

    private void VerObjetivos()
    {
        foreach (GameObject objetivo in objetivos)
        {
            Debug.Log(objetivo.name);
        }
    }

    private bool EsObjetivoActual(GameObject objetivoActual, GameObject objetivoReal)
    {
        return objetivoActual == objetivoReal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Coleccionable")) { return; }
        if (objetivos.Count == 0) { return; }


        GameObject objetivo = objetivos.Peek();

        if (EsObjetivoActual(collision.gameObject, objetivo))
        {
            objetivo.SetActive(false);
            objetivos.Dequeue();
            items.Push(objetivo);
            inventario.Add(objetivo.name, objetivo);
            VerObjetivos();
            objetivo.transform.SetParent(bolsa.transform);

            progresionJugador.GanarExperiencia(10);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1) && inventario.ContainsKey("Regadera"))
        {
            UsarInventario(inventario["Regadera"]);
        }

        if (Input.GetKeyDown(KeyCode.F2) && inventario.ContainsKey("Canasta"))
        {
            UsarInventario(inventario["Canasta"]);
        }
        ;
        if (Input.GetKeyDown(KeyCode.F3) && inventario.ContainsKey("Calabaza"))
        {
            UsarInventario(inventario["Calabaza"]);
        }
    }

    private void UsarItem()
    {
        GameObject item = items.Pop();
        item.transform.SetParent(null);
        item.transform.position = transform.position;
        item.SetActive(true);
    }

    private void UsarInventario(GameObject item)
    {
        item.transform.SetParent(null);
        item.transform.position = transform.position;
        item.SetActive(true);
    }
}
