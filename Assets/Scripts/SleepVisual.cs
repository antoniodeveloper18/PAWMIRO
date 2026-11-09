using UnityEngine;

public class SleepVisual : MonoBehaviour
{
    PetNeeds needs;
    Renderer rend;

    void Awake()
    {
        needs = GetComponent<PetNeeds>();
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.color = needs.isSleeping ? new Color(0.4f, 0.4f, 0.7f) : Color.white;
    }
}