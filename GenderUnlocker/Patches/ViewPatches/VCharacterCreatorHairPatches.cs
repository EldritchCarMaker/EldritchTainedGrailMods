using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Awaken.TG.Main.Heroes.CharacterCreators;
using HarmonyLib;

namespace GenderUnlocker.Patches.ViewPatches;

[HarmonyPatch(typeof(VCharacterCreatorHair))]
internal class VCharacterCreatorHairPatches
{
    [HarmonyPatch("OnFullyInitialized")]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase __originalMethod)
    {
        var matcher = new CodeMatcher(instructions);

        matcher.MatchForward(false, new CodeMatch(inst => inst.opcode == OpCodes.Callvirt && inst.operand is MethodInfo info && info.Name == nameof(CharacterCreator.GetGender)));

        matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldstr, __originalMethod.Name));

        matcher.SetOperandAndAdvance(typeof(CCGridSelectDataPatches).GetMethod(nameof(CCGridSelectDataPatches.GetGenderPatched)));

        return matcher.InstructionEnumeration();
    }
}
