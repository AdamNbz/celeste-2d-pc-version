using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sinx {
public class SimpleMotion : MonoBehaviour {
    [SerializeField] SimpleMotionType simpleMotionType;
    [SerializeField] InputAction flipAction;
    
    Vector3 initialPosition;

    void OnEnable() {
        flipAction.Enable();
    }

    void OnDisable() {
        flipAction.Disable();
    }

    void Start() {
        initialPosition = transform.position;
        
        // Set default binding if not configured in Inspector
        if (flipAction.bindings.Count == 0) {
            flipAction.AddBinding("<Keyboard>/space");
        }
    }

    void Update() {
        if (flipAction.WasPressedThisFrame()) {
            GetComponent<HairMovement>().FlipX = !GetComponent<HairMovement>().FlipX;
        }
        float t = Time.time*Mathf.PI*2f;
        switch (simpleMotionType) {
            case SimpleMotionType.Circular:
                transform.position = initialPosition + new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0f)*2f;
                break;
            case SimpleMotionType.Jump:
                transform.position = initialPosition + new Vector3(0f, Mathf.Pow(Mathf.Max(Mathf.Sin(t), 0f), .5f), 0f)*2f;
                break;
            case SimpleMotionType.SideToSide:
                transform.position = initialPosition + new Vector3(Mathf.Pow(Mathf.Abs(Mathf.Sin(t)), .25f) * Mathf.Sign(Mathf.Sin(t)), 0f, 0f)*2f;
                break;
            default:
                break;
        }    
    }
}

public enum SimpleMotionType {
    Circular,
    Jump,
    SideToSide
}

}

