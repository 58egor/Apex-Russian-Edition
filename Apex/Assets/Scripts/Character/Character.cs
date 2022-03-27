using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public CharacterMoveInput MoveInput {get;private set;}
    public MovementSM MovementSM { get; private set; }


    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        MoveInput = GetComponent<CharacterMoveInput>();
        MovementSM = GetComponent<MovementSM>();

        MovementSM.Init(this);
    }

    private void FixedUpdate()
    {
        AddExtraGravity();
    }

    private void AddExtraGravity()
    {
        Rigidbody.AddForce(Vector3.down * Time.deltaTime * 10);
    }
}
