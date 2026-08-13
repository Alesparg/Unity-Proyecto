using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saltar : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float fuerzaSalto = 5f;

    [SerializeField] private AudioClip jumpSFX; // Asignar salto.mp3 desde el Inspector
    [SerializeField] private AudioClip collisionSFX; // Asignar golpe-seco-98447.mp3 desde el Inspector

    private bool puedoSaltar = true;
    private bool saltando = false;

    private Rigidbody2D miRigidbody2D;
    private AudioSource miAudioSource;

    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAudioSource = GetComponent<AudioSource>();
        if (miAudioSource == null)
        {
            miAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && puedoSaltar)
        {
            puedoSaltar = false;
            saltando = true;

            // Reproducir sonido de salto
            miAudioSource.PlayOneShot(jumpSFX);
        }
    }

    private void FixedUpdate()
    {
        if (saltando)
        {
            miRigidbody2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltando = false; // Evitar que se aplique fuerza más de una vez
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        puedoSaltar = true;

        // Reproducir sonido de colisión
        miAudioSource.PlayOneShot(collisionSFX);
    }
}

