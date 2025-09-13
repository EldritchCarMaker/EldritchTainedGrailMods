using System;
using System.Reflection;
using Awaken.TG.Main.Fights.NPCs;
using Awaken.TG.Main.Heroes.CharacterCreators.Data;
using HarmonyLib;

namespace GenderUnlocker.Patches.ViewPatches;

[HarmonyPatch(typeof(CharacterCreatorTemplate))]
internal class CharacterCreatorTemplatePatches
{
    [HarmonyPatch(nameof(CharacterCreatorTemplate.Beard))]
    [HarmonyPatch(nameof(CharacterCreatorTemplate.Eyebrow))]
    [HarmonyPatch(nameof(CharacterCreatorTemplate.FaceSkin))]
    [HarmonyPatch(nameof(CharacterCreatorTemplate.Hair))]
    [HarmonyPatch(nameof(CharacterCreatorTemplate.HeadShape))]
    public static void Prefix(CharacterCreatorTemplate __instance, ref Gender gender, MethodBase __originalMethod)
    {
        gender = __originalMethod.Name switch//God I fucking hate this too
        {
            "Beard" => Plugin.GenderData.BeardGender.Value,
            "Eyebrow" => Plugin.GenderData.EyebrowGender.Value,
            "HeadShape" => Plugin.GenderData.FacesGender.Value,
            "FaceSkin" => Plugin.GenderData.FacesDetailsGender.Value,
            "Hair" => Plugin.GenderData.HairsGender.Value,
            _ => throw new NotImplementedException()
        };

    }
    /*
    [HarmonyPatch(nameof(CharacterCreatorTemplate.BodyNormal))]
    [HarmonyPatch(nameof(CharacterCreatorTemplate.BodyTattoos))]
    */
}
