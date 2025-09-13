using Awaken.TG.Main.Fights.NPCs;
using Awaken.TG.Main.Scenes.SceneConstructors;
using HarmonyLib;

namespace GenderUnlocker.Patches;

[HarmonyPatch(typeof(CommonReferences))]
internal class CommonReferencesPatches
{
    [HarmonyPatch(nameof(CommonReferences.RefreshLocsGender))]
    public static void Prefix(ref Gender gender)
    {
        gender = Plugin.GenderData.localizationGender.Value;
    }
}
