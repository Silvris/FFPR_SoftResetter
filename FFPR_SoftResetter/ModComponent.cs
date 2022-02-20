using BepInEx.Logging;
using Il2CppSystem.Fade;
using Last.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FFPR_SoftResetter
{
    public sealed class ModComponent : MonoBehaviour
    {
        public static ModComponent Instance { get; private set; }
        public static Configuration Config { get; set; }
        public static ManualLogSource Log { get; private set; }
        public static FadeManager Fader { get; set; }
        public static SceneManager SceneManager { get; set; }
        private bool _IsHeld = false;
        public static KeyCode[] Input { get; set; }
        private Boolean _isDisabled;
        public ModComponent(IntPtr ptr) : base(ptr)
        {
        }
        public void Awake()
        {
            Log = BepInEx.Logging.Logger.CreateLogSource("FFPR_SoftResetter");
            Config = new Configuration(EntryPoint.Instance.Config);
            Input = new KeyCode[4];
            for(int i = 0; i< 4; i++)
            {
                Input[i] = KeyCode.None;
            }
            RefreshKeyCodes();
            try
            {
                Instance = this;
                Log.LogMessage($"[{nameof(ModComponent)}].{nameof(Awake)}: Processed successfully.");
            }
            catch (Exception ex)
            {
                _isDisabled = true;
                Log.LogError($"[{nameof(ModComponent)}].{nameof(Awake)}(): {ex}");
                throw;
            }

        }
        public static void RefreshKeyCodes()
        {
            Input[0] = Config.Input1.Value;
            Input[1] = Config.Input2.Value;
            Input[2] = Config.Input3.Value;
            Input[3] = Config.Input4.Value;
        }
        public static void ResetGame()
        {
            //Log.LogInfo("Reaching Resetting Code");
            SubSceneManager SubScene = SceneManager.GetCurrentSubSceneManager();
            MainGame MG = SubScene.GetParentScene().Cast<MainGame>();
            if (MG != null)
            {
                //Log.LogInfo("MG is not null");
                Fader.FadeOut(2f, Color.black);
                MG.ChangeState(SubSceneManagerMainGame.State.GotoTitle);
                //Fader.FadeIn(2f, Color.black);
            }
        }
        public void Update()
        {
            try
            {
                if (_isDisabled)
                {
                    return;
                }
                if(Fader is null)
                {
                    Fader = FadeManager.instance;
                    return;
                }
                if(SceneManager is null)
                {
                    SceneManager = SceneManager.Instance;
                    return;
                }
                if (Input[0] == KeyCode.None) return; //this lets us disable the soft-resetting easily
                foreach(KeyCode key in Input)
                {
                    if(key == KeyCode.None)
                    {
                        continue;
                    }
                    if (!InputManager.GetKey(key))
                    {
                        _IsHeld = false;
                        return;
                    }
                }
                //to get here, all keys that are not None must be held
                if(!_IsHeld) ResetGame();
                _IsHeld = true;
            }
            catch (Exception ex)
            {
                _isDisabled = true;
                Log.LogError($"[{nameof(ModComponent)}].{nameof(Update)}(): {ex}");
                throw;
            }

        }
    }
}
