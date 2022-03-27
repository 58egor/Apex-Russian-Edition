using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    private MovementSM _movementSM;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _movementSM = GetComponent<MovementSM>();

        _movementSM.Init(this);
    }
}
