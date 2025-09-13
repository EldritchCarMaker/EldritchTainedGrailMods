using System.Reflection;
using Awaken.TG.Main.Fights.NPCs;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GenderUnlocker.Patches.ViewPatches;
using HarmonyLib;

namespace GenderUnlocker
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        public static GenderData GenderData = new();

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            CCGridSelectDataPatches.Patch(harmony);

            SetUpConfig();

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
        private void SetUpConfig()
        {
            GenderData.baseGender = Config.Bind("Test", "baseGender", Gender.Male, "Honestly idk");
            GenderData.localizationGender = Config.Bind("Test", "localizationGender", Gender.Male, "The gender that localization (such as character dialogue) treats you as");

            GenderData.BeardGender = Config.Bind("Hair", "beardGender", Gender.Male, "Females have no beards listed, may as well keep this as male");
            GenderData.HairsGender = Config.Bind("Hair", "HairsGender", Gender.Male, "The hair options listed");

            GenderData.FacesGender = Config.Bind("Faces", "FacesGender", Gender.Male, "Gender of faces listed");
            GenderData.FacesDetailsGender = Config.Bind("Faces", "FacesDetailsGender", Gender.Male, "Gender of face details listed. I don't know if there's a difference between them, I assume best to just keep matched to the FacesGender");
            GenderData.EyebrowGender = Config.Bind("Faces", "EyebrowGender", Gender.Male, "Gender of eyebrows listed");

            GenderData.BodyTattooGender = Config.Bind("Tattoos", "bodyTattooGender", Gender.Male, "Honestly I don't know if this changes anything, the tattoo options seemed identical but I'm offering the option anyway");
            GenderData.FaceTattoosGender = Config.Bind("Tattoos", "FaceTattoosGender", Gender.Male, "Gender of face tattoos listed. Same as body tattoos");
        }
    }

    public class GenderData//Funny name
    {
        public ConfigEntry<Gender> baseGender;
        public ConfigEntry<Gender> localizationGender;
        public ConfigEntry<Gender> BeardGender;
        public ConfigEntry<Gender> BodyTattooGender;
        public ConfigEntry<Gender> EyebrowGender;
        public ConfigEntry<Gender> FacesGender;
        public ConfigEntry<Gender> FacesDetailsGender;
        public ConfigEntry<Gender> FaceTattoosGender;
        public ConfigEntry<Gender> HairsGender;
    }
}
