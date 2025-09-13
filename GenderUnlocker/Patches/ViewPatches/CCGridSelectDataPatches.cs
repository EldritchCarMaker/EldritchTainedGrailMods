using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Awaken.TG.Main.Fights.NPCs;
using Awaken.TG.Main.Heroes.CharacterCreators;
using Awaken.TG.Main.Heroes.CharacterCreators.Parts;
using HarmonyLib;

namespace GenderUnlocker.Patches.ViewPatches;

internal class CCGridSelectDataPatches
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions, MethodBase __originalMethod)
    {
        var matcher = new CodeMatcher(codeInstructions);

        matcher.MatchForward(false, new CodeMatch(inst => inst.opcode == OpCodes.Callvirt && inst.operand is MethodInfo info && info.Name == nameof(CharacterCreator.GetGender)));

        matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldstr, __originalMethod.Name));

        matcher.SetOperandAndAdvance(typeof(CCGridSelectDataPatches).GetMethod(nameof(GetGenderPatched)));

        return matcher.InstructionEnumeration();
    }

    public static Gender GetGenderPatched(CharacterCreator creator, string methodName)
    {
        return methodName switch//God I fucking hate this
        {
            "<Beards>b__0" => Plugin.GenderData.BeardGender.Value,
            "<BodyTattoo>b__0" => Plugin.GenderData.BodyTattooGender.Value,
            "<Eyebrow>b__0" => Plugin.GenderData.EyebrowGender.Value,
            "<Faces>b__0" => Plugin.GenderData.FacesGender.Value,
            "<FacesDetails>b__0" => Plugin.GenderData.FacesDetailsGender.Value,
            "<FaceTattoo>b__0" => Plugin.GenderData.FaceTattoosGender.Value,
            "<Hairs>b__0" => Plugin.GenderData.HairsGender.Value,
            "OnFullyInitialized" => Plugin.GenderData.BeardGender.Value,
            _ => throw new NotImplementedException()
        };
    }

    public static void Patch(Harmony harmony)
    {
        var transpileup = new HarmonyMethod(typeof(CCGridSelectDataPatches).GetMethod(nameof(Transpiler), AccessTools.all));

        var type = typeof(CCGridSelectData);
        var nested = type.GetNestedTypes(AccessTools.all);

        var beard = nested[3].GetMethod("<Beards>b__0", AccessTools.all);
        harmony.Patch(beard, null, null, transpileup);
        Plugin.Logger.LogMessage("found beard");

        var bodyTattoo = nested[11].GetMethod("<BodyTattoo>b__0", AccessTools.all);
        harmony.Patch(bodyTattoo, null, null, transpileup);
        Plugin.Logger.LogMessage("found bodytattoo");

        var eyebrow = nested[8].GetMethod("<Eyebrow>b__0", AccessTools.all);
        harmony.Patch(eyebrow, null, null, transpileup);
        Plugin.Logger.LogMessage("found eyebrow");

        var faces = nested[0].GetMethod("<Faces>b__0", AccessTools.all);
        harmony.Patch(faces, null, null, transpileup);
        Plugin.Logger.LogMessage("found faces");

        var facedetails = nested[1].GetMethod("<FacesDetails>b__0", AccessTools.all);
        harmony.Patch(facedetails, null, null, transpileup);
        Plugin.Logger.LogMessage("found facedetails");

        var facetattoo = nested[9].GetMethod("<FaceTattoo>b__0", AccessTools.all);
        harmony.Patch(facetattoo, null, null, transpileup);
        Plugin.Logger.LogMessage("found facetattoo");

        var hairs = nested[2].GetMethod("<Hairs>b__0", AccessTools.all);
        harmony.Patch(hairs, null, null, transpileup);
        Plugin.Logger.LogMessage("found hairs");
    }
}