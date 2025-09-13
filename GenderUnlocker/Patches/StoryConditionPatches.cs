using Awaken.TG.Main.Stories.Conditions;
using Awaken.TG.Main.Stories.Runtime.Nodes;
using HarmonyLib;

namespace GenderUnlocker.Patches;

[HarmonyPatch(typeof(CGender))]
internal class StoryConditionPatches
{
    [HarmonyPatch(nameof(CGender.Fulfilled))]
    public static void Postfix(ref bool __result, CGender __instance)
    {
        __result = __instance.gender == Plugin.GenderData.localizationGender.Value;
    }
}
