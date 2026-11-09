using UnityEngine;
using UnityEngine.UI;

public class NeedsUI : MonoBehaviour
{
    public PetNeeds pet;
    public Slider hungerBar;
    public Slider energyBar;
    public Slider happinessBar;

    void Update()
    {
        hungerBar.value = pet.hunger;
        energyBar.value = pet.energy;
        happinessBar.value = pet.happiness;
    }
}