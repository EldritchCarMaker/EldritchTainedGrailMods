using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Awaken.TG.Main.AudioSystem;
using Awaken.TG.Main.Character.Features;
using Awaken.TG.Main.Fights.NPCs;
using Awaken.TG.Main.Heroes.CharacterCreators;
using GenderUnlocker.Patches.ViewPatches;
using HarmonyLib;

namespace GenderUnlocker.Patches;

[HarmonyPatch(typeof(HeroGenderSpecificFModEventRef))]
internal class HeroGenderSpecificFModEventRefPatches
{
    [HarmonyPatch("EventPath")]
    [HarmonyPatch(MethodType.Getter)]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions);

        matcher.MatchForward(false, new CodeMatch(inst => inst.opcode == OpCodes.Call && inst.operand is MethodInfo info && info.Name == nameof(WithBodyFeatureExtension.GetGender)));

        matcher.SetOperandAndAdvance(typeof(HeroGenderSpecificFModEventRefPatches).GetMethod(nameof(GetGenderTwoPatchedAlso), AccessTools.all));

        return matcher.InstructionEnumeration();
    }

    public static Gender GetGenderTwoPatchedAlso()
    {
        return Plugin.GenderData.baseGender.Value;
    }
}
