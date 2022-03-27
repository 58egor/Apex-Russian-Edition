using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveInput : MonoBehaviour
{
    private Character _character;
    private void Awake()
    {
        _character = GetComponent<Character>();   
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        _character.MovementSM.SetXY(x, y);
    }
}
