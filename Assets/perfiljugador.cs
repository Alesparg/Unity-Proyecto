
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPerfilJugador", menuName = "SO/Perfil Jugador")]
public class PerfilJugador : ScriptableObject
{
    [Header("Configuraciones de Experiencia")]

    [SerializeField]
    [Tooltip("Rango de experiencia necesaria para el proximo nivel 10 a 50")]
    [Range(10, 50)]
    private int experienciaProximoNivel;
    public int ExperienciaProximoNivel { get => experienciaProximoNivel; set => experienciaProximoNivel = value; }

    [SerializeField]
    [Tooltip("Como aumenta la experiencia nivel a nivel")]
    [Range(10, 2000)]
    private int escalarExperiencia;
    public int EscalarExperiencia { get => escalarExperiencia; set => escalarExperiencia = value; }

    private int nivel;
    public int Nivel { get => nivel; set => nivel = value; }


    private int experiencia;
    public int Experiencia { get => experiencia; set => experiencia = value; }

    [Header("Configuraciones de Movimiento")]

    [SerializeField]
    [Range(5, 10)]
    float velocidad = 5f;
    public float VelocidadHorizontal { get => velocidad; set => velocidad = value; }


    [SerializeField] private float fuerzaSalto = 5f;
    public float FuerzaSalto { get => fuerzaSalto; set => fuerzaSalto = value; }

    [Header("Configuraciones de Atributos")]
    [SerializeField]
    [Range(1, 3)]
    private int vida = 1;
    public int Vida { get => vida; set => vida = value; }


    [Header("Configuraciones SFX")]

    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip collisionSFX;

    public AudioClip JumpSFX { get => jumpSFX; }
    public AudioClip CollisionSFX { get => collisionSFX; }

}