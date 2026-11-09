using UnityEngine;

public class PetNeeds : MonoBehaviour
{
    [Range(0, 100)] public float hunger = 100f;
    [Range(0, 100)] public float energy = 100f;
    [Range(0, 100)] public float happiness = 100f;

    [Header("Segundos para vaciarse del todo")]
    public float hungerDrainTime = 300f;
    public float energyDrainTime = 600f;
    public float happinessDrainTime = 400f;

    [Header("Sueño")]
    public float sleepRechargeTime = 60f;   // segundos para recargar de 0 a 100 durmiendo
    public bool isSleeping;

    void Update()
    {
        if (isSleeping)
        {
            energy = Mathf.Min(100, energy + 100f / sleepRechargeTime * Time.deltaTime);
            // Durmiendo el hambre baja 3 veces más despacio
            hunger = Mathf.Max(0, hunger - 100f / (hungerDrainTime * 3f) * Time.deltaTime);

            if (energy >= 100f) isSleeping = false;
        }
        else
        {
            hunger = Mathf.Max(0, hunger - 100f / hungerDrainTime * Time.deltaTime);
            energy = Mathf.Max(0, energy - 100f / energyDrainTime * Time.deltaTime);
        }

        happiness = Mathf.Max(0, happiness - 100f / happinessDrainTime * Time.deltaTime);
    }

    public void Feed()
    {
        if (isSleeping) return;
        hunger = Mathf.Min(100, hunger + 30f);
    }

    public void Pet()
    {
        if (isSleeping) return;
        happiness = Mathf.Min(100, happiness + 10f);
    }

    public void ToggleSleep() => isSleeping = !isSleeping;
}