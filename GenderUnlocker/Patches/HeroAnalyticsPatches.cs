using Awaken.TG.Main.Analytics;
using Awaken.TG.Main.Heroes.CharacterCreators;
using Awaken.TG.Main.Localization;
using Awaken.TG.Main.Settings.Gameplay;
using Awaken.TG.MVC;
using Awaken.Utility.Extensions;
using HarmonyLib;

namespace GenderUnlocker.Patches;

[HarmonyPatch(typeof(HeroAnalytics))]
internal class HeroAnalyticsPatches
{
    [HarmonyPatch("OnHeroCreated")]
    public static bool Prefix(HeroAnalytics __instance, CharacterCreator creator)//this is bad but I am far too lazy so fuck you
    {

        string enumName = World.Only<DifficultySetting>().Difficulty.EnumName;
        AnalyticsUtils.TrySendDesignEvent("HeroCreated:Difficulty:" + enumName);
        string text = "None";
        LocString name = creator.BuildPreset.name;
        if (name != null)
        {
            text = name.IdOverride;
            if (text.IsNullOrWhitespace())
            {
                text = name.ID;
            }
            text = AnalyticsUtils.EventName(name);
        }
        AnalyticsUtils.TrySendDesignEvent("HeroCreated:StartingTemplate:" + text);
        int presetIndex = creator.GetPresetIndex();
        AnalyticsUtils.TrySendDesignEvent(string.Format("HeroCreated:{0}:StartingPreset:{1}", Plugin.GenderData.baseGender, (presetIndex == -1) ? "Custom" : presetIndex));//Honestly I don't know what any of these do
        AnalyticsUtils.TrySendDesignEvent(string.Format("HeroCreated:{0}:HeadShape:{1}", Plugin.GenderData.FacesGender, creator.GetHeadShapeIndex()));//Maybe nothing
        AnalyticsUtils.TrySendDesignEvent(string.Format("HeroCreated:{0}:SkinColour:{1}", Plugin.GenderData.baseGender, creator.GetSkinColorIndex()));//Mbbye everything
        AnalyticsUtils.TrySendDesignEvent(string.Format("HeroCreated:{0}:Hair:{1}:{2}", Plugin.GenderData.HairsGender, creator.GetHairIndex(), creator.GetHairColorIndex()));
        AnalyticsUtils.TrySendDesignEvent(string.Format("HeroCreated:{0}:Beard:{1}:{2}", Plugin.GenderData.BeardGender, creator.GetBeardIndex(), creator.GetBeardColorIndex()));
        AnalyticsUtils.TrySendDesignEvent(string.Format("HeroCreated:{0}:BodyNormals:{1}", Plugin.GenderData.baseGender, creator.GetBodyNormalsIndex()));



        return false;
    }
}
