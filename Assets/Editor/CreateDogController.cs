using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public static class CreateDogController
{
    const string FbxPath = "Assets/Art/Models/Perro_Base.fbx";
    const string ControllerPath = "Assets/Art/DogController.controller";

    [MenuItem("PAWMIRO/Crear DogController")]
    static void Create()
    {
        // Buscar los clips de animacion dentro del FBX
        var clips = new List<AnimationClip>();
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(FbxPath))
        {
            if (asset is AnimationClip clip && !clip.name.StartsWith("__preview__"))
                clips.Add(clip);
        }

        if (clips.Count == 0)
        {
            Debug.LogError("No se encontraron animaciones en " + FbxPath +
                           ". Comprueba la ruta y que el FBX se exporto con Bake Animation.");
            return;
        }

        // Si ya existe, lo recreamos
        if (AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath) != null)
            AssetDatabase.DeleteAsset(ControllerPath);

        var controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
        controller.AddParameter("isSleeping", AnimatorControllerParameterType.Bool);

        var sm = controller.layers[0].stateMachine;

        var idle = sm.AddState("Idle");
        idle.motion = clips[0];

        // Provisional: mientras no haya animacion de dormir real,
        // reutiliza el clip a poca velocidad para notar el cambio
        var sleep = sm.AddState("Sleep");
        sleep.motion = clips[0];
        sleep.speed = 0.2f;

        sm.defaultState = idle;

        var aDormir = idle.AddTransition(sleep);
        aDormir.hasExitTime = false;
        aDormir.duration = 0.25f;
        aDormir.AddCondition(AnimatorConditionMode.If, 0, "isSleeping");

        var aDespertar = sleep.AddTransition(idle);
        aDespertar.hasExitTime = false;
        aDespertar.duration = 0.25f;
        aDespertar.AddCondition(AnimatorConditionMode.IfNot, 0, "isSleeping");

        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("DogController creado en " + ControllerPath + " con " + clips.Count + " clip(s)");
        Selection.activeObject = controller;
    }
}