using System;
using Microsoft.Xna.Framework;
using Celeste.Mod.Entities;
using Monocle;
using System.Collections.Generic;
using FMOD.Studio;
using MonoMod.Utils;

namespace Celeste.Mod.ettessaCHelper {
    public class ettessaCHelperModule : EverestModule {
        public static ettessaCHelperModule Instance { get; private set; }

        private Level level;
        private int numSyncs;

        private bool isPaused;

        private float sixnote;
        private EventInstance sfx;

        bool dontRepeatSixteenTimesPlease = true;

        CassetteBlockManager current;
        DynamicData cassetteManagerData;
        private String music;

        public override Type SettingsType => typeof(ettessaCHelperModuleSettings);
        public static ettessaCHelperModuleSettings Settings => (ettessaCHelperModuleSettings) Instance._Settings;

        public override Type SessionType => typeof(ettessaCHelperModuleSession);
        public static ettessaCHelperModuleSession Session => (ettessaCHelperModuleSession) Instance._Session;

        public ettessaCHelperModule() {
            Instance = this;
#if DEBUG
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(ettessaCHelperModule), LogLevel.Verbose);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(ettessaCHelperModule), LogLevel.Info);
#endif
        }

        public override void Load() {
            
            Logger.SetLogLevel("ettessaCHelper", LogLevel.Debug);
            On.Celeste.CassetteBlockManager.AdvanceMusic += BetterAdvance;
            Everest.Events.Level.OnTransitionTo +=  GetMusic;
            Everest.Events.Level.OnPause += StopMusicOnPause;
        }

        public override void Unload() {
            On.Celeste.CassetteBlockManager.AdvanceMusic -= BetterAdvance;
            Everest.Events.Level.OnTransitionTo -=  GetMusic;
            Everest.Events.Level.OnPause -= StopMusicOnPause;
        }


        public void BetterAdvance(On.Celeste.CassetteBlockManager.orig_AdvanceMusic orig, CassetteBlockManager s, float t) {
            if (numSyncs % 2 == 0) { orig(s, t); }
            if (sfx == null) {
                current = Celeste.Scene.Tracker.GetEntity<CassetteBlockManager>();
                cassetteManagerData = new DynamicData(current);
                sfx = cassetteManagerData.Get<EventInstance>("sfx");
            }
            if (current.GetSixteenthNote() % 256 == 0 && dontRepeatSixteenTimesPlease || isPaused && dontRepeatSixteenTimesPlease) {
                // sfxTemp = sfx;
                //float currSixteenth;
                music = Audio.GetEventName(sfx);
                Audio.BusStopAll("bus:/music/tunes/cassette");
                sfx.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, cassetteManagerData.Get<int>("beatIndexMax"));
                //sfx.getParameterValue("autosync", out currSixteenth, out temp);
                //Logger.Log(LogLevel.Debug, "ettessaCHelper", "sixteenthNote on sfx before set: " + currSixteenth);
                //Logger.Log(LogLevel.Debug, "ettessaCHelper", "sixteenthNote on CassetteBlockManager before: " + current.GetSixteenthNote());
                sixnote = current.GetSixteenthNote();
                sfx.setParameterValue("autosync", sixnote);
                sfx.start();
                //Logger.Log(LogLevel.Debug, "ettessaCHelper", "maxBeats: " + cassetteManagerData.Get<int>("beatIndexMax"));
                numSyncs++;
                //sfx.getParameterValue("autosync", out currSixteenth, out temp);
                //Logger.Log(LogLevel.Debug, "ettessaCHelper", "sixteenthNote on sfx: " + currSixteenth);
                //Logger.Log(LogLevel.Debug, "ettessaCHelper", "sixteenthNote on CassetteBlockManager: " + current.GetSixteenthNote());
                dontRepeatSixteenTimesPlease = false;
                isPaused = false;
            } else if (current.GetSixteenthNote() % 64 != 0) {
                dontRepeatSixteenTimesPlease = true;
            }
            if (numSyncs % 2 == 1) { orig(s, t); }
        }
        public void GetMusic(Level level, LevelData next, Vector2 direction) {
            music = next.AltMusic;
            this.level = level;
        }

        public void StopMusicOnPause(Level level, int startIndex, bool minimal, bool quickReset) {
            if (sfx != null) {
                Audio.BusStopAll("bus:/music/tunes/cassette");
            }
            isPaused = true;
        }
    }
}