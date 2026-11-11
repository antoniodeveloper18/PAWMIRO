using UnityEngine;

public class SleepVisual : MonoBehaviour
{
    static readonly int IsSleepingHash = Animator.StringToHash("isSleeping");

    PetNeeds needs;
    Animator animator;
    bool hasParam;

    void Awake()
    {
        needs = GetComponent<PetNeeds>();
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("SleepVisual: no hay Animator en el perro");
            return;
        }

        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("SleepVisual: el Animator no tiene Controller asignado");
            return;
        }

        foreach (var p in animator.parameters)
            if (p.nameHash == IsSleepingHash) hasParam = true;

        if (!hasParam)
            Debug.LogWarning("SleepVisual: el Controller no tiene el parametro isSleeping");
    }

    void Update()
    {
        if (hasParam)
            animator.SetBool(IsSleepingHash, needs.isSleeping);
    }
}