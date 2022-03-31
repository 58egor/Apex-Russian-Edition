using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public CharacterMoveInput MoveInput { get; private set; }
    public MovementSM MovementSM { get; private set; }
    [field: SerializeField] public Transform Camera { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        MoveInput = GetComponent<CharacterMoveInput>();
        MovementSM = GetComponent<MovementSM>();

        MovementSM.Init(this);
    }
}
