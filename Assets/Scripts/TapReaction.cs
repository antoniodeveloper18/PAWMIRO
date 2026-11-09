using UnityEngine;
using UnityEngine.InputSystem;

public class TapReaction : MonoBehaviour
{
    PetNeeds needs;

    void Awake() => needs = GetComponent<PetNeeds>();

    void Update()
    {
        var pointer = Pointer.current;
        if (pointer == null || !pointer.press.wasPressedThisFrame) return;

        Ray ray = Camera.main.ScreenPointToRay(pointer.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
        {
            Debug.Log("¡Guau!");
            needs.Pet();
        }
    }
}