using System.Collections;
using System.Reflection;
using Awaken.TG.Main.Character;
using Awaken.TG.Main.Heroes;
using Awaken.TG.Main.Heroes.Development.Talents;
using Awaken.TG.Main.Skills;
using Awaken.TG.MVC;
using Awaken.TG.MVC.Events;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace BowZoomFallSpeed
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        internal static float termVel = 0;
        public const float BowTermVel = -1.5f;

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");
        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => World.EventSystem != null);

            World.EventSystem.ListenTo("*", Talent.Events.TalentConfirmed, (talent) => Debug.Log($"talent confirmed {talent}"));
            World.EventSystem.ListenTo("*", Talent.Events.TalentChanged, (talent) => Debug.Log($"talent changed {talent}"));


            World.EventSystem.ListenTo("*", ICharacter.Events.OnBowZoomStart, OnBowZoomStart);
            World.EventSystem.ListenTo("*", ICharacter.Events.OnBowZoomEnd, OnBowZoomEnd);
        }
        private void OnBowZoomStart(ICharacter chararacter)
        {
            if (chararacter is not Hero hero)
                return;


            var newTermVel = hero.Data.terminalVelocity;
            if (Mathf.Approximately(newTermVel, BowTermVel))
            {
                //Somehow the terminal velocity got stuck
                //So try just reset to the base
                if (termVel == 0)//We don't have any terminal velocity saved yet
                    newTermVel = -120;//Default game's terminal velocity, hardcoding here because we don't have access to the base terminal velocity otherwise
                else
                    newTermVel = termVel;//We have an old non-zero terminal velocity, we can probably assume its safe
            }

            //We save the terminal velocity rather than always using the base -120
            //So that this mod doesn't fuck with terminal velocity if the game updates
            //Or if a mod decides to change the terminal velocity itself
            //We just temporarily overwrite it with a new value
            //Not perfect but plenty good enough
            termVel = newTermVel;

            hero.Data.terminalVelocity = BowTermVel;
        }
        private void OnBowZoomEnd(ICharacter chararacter)
        {
            if (chararacter is Hero hero) hero.Data.terminalVelocity = termVel;
        }
    }
}
