using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [SerializeField] float velocidad = 5f;

    void Update()
    {
        transform.position += Vector3.left * velocidad * Time.deltaTime;
    }
}
