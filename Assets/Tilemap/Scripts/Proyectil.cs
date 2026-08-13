using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Proyectil : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 30f)]
    protected float speed = 10f;

    protected Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Mover();
    }

    protected abstract void Mover();
}
