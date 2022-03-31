using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 50f;
    private float _xRotation;
    private static float sensMultiplier = 1f;
    private Character _character;
    private float _desiredX;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = _character.Camera.transform.localRotation.eulerAngles;
        _desiredX = rot.y + mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _character.Camera.transform.localRotation = Quaternion.Euler(_xRotation, _desiredX, 0);
        _character.transform.localRotation = Quaternion.Euler(0, _desiredX, 0);
    }
}
