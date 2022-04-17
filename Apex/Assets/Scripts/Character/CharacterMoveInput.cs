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

        _character.MovementSM.IsCrounching = Input.GetButton("Crounch");
        _character.MovementSM.IsJumpButtonPressed = Input.GetButton("Jump");
        _character.MovementSM.IsRunning = Input.GetButton("Run");

        if (Input.GetButtonDown("Jump"))
            _character.MovementSM.JumpButtonIsProssed();

        _character.MovementSM.SetXY(x, y);
    }
}
