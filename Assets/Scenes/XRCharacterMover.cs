using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class XRCharacterMover : MonoBehaviour
{
    public InputActionReference moveInput;
    public float speed = 1.5f;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (moveInput == null || characterController == null) return;

        Vector2 input = moveInput.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y);
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0;

        characterController.Move(direction * speed * Time.deltaTime);
    }
}
