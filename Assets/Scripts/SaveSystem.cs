using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public PetNeeds pet;

    const string Key = "PawmiroSave";
    float timer;

    [Serializable]
    class SaveData
    {
        public float hunger, energy, happiness;
        public bool isSleeping;
        public long lastSaveTicks;
    }

    void Start()
    {
        if (pet == null)
        {
            Debug.LogError("SaveSystem: falta arrastrar el Perro al hueco Pet");
            enabled = false;
            return;
        }
        Load();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            timer = 0f;
            Save();
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) Save();
    }

    void OnApplicationQuit() => Save();

    void OnDisable()
    {
        if (pet != null) Save();
    }

    void Save()
    {
        var data = new SaveData
        {
            hunger = pet.hunger,
            energy = pet.energy,
            happiness = pet.happiness,
            isSleeping = pet.isSleeping,
            lastSaveTicks = DateTime.UtcNow.Ticks
        };
        PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    void Load()
    {
        if (!PlayerPrefs.HasKey(Key))
        {
            Debug.Log("No hay partida guardada");
            return;
        }

        var data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(Key));
        float totalAway = (float)Math.Max(0, (DateTime.UtcNow - new DateTime(data.lastSaveTicks)).TotalSeconds);
        float secondsAway = totalAway;

        pet.hunger = data.hunger;
        pet.energy = data.energy;
        pet.happiness = data.happiness;
        pet.isSleeping = data.isSleeping;

        // Si dormía, simulamos el tiempo dormido hasta que se llene la energía
        if (pet.isSleeping)
        {
            float secondsToFull = (100f - pet.energy) / (100f / pet.sleepRechargeTime);
            float sleepSeconds = Mathf.Min(secondsAway, secondsToFull);

            pet.energy = Mathf.Min(100, pet.energy + 100f / pet.sleepRechargeTime * sleepSeconds);
            pet.hunger = Mathf.Max(0, pet.hunger - 100f / (pet.hungerDrainTime * 3f) * sleepSeconds);

            secondsAway -= sleepSeconds;
            if (pet.energy >= 100f) pet.isSleeping = false;
        }

        // El tiempo restante (despierto) aplica el desgaste normal
        pet.hunger = Mathf.Max(0, pet.hunger - 100f / pet.hungerDrainTime * secondsAway);
        pet.energy = Mathf.Max(0, pet.energy - 100f / pet.energyDrainTime * secondsAway);

        // La felicidad baja durante todo el tiempo fuera, duerma o no
        pet.happiness = Mathf.Max(0, pet.happiness - 100f / pet.happinessDrainTime * totalAway);

        Debug.Log("Partida cargada: hambre " + pet.hunger + ", durmiendo: " + pet.isSleeping);
    }
}