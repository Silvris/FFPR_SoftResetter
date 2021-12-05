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
        public static ManualLogSource Log { get; private set; }
        public static FadeManager Fader { get; set; }
        public static SceneManager SceneManager { get; set; }
        public static KeyCode[] Input { get; set; }
        private Boolean _isDisabled;
        public ModComponent(IntPtr ptr) : base(ptr)
        {
        }
        public void Awake()
        {
            Log = BepInEx.Logging.Logger.CreateLogSource("FFPR_SoftResetter");
            Input = new KeyCode[4];
            for(int i = 0; i< 4; i++)
            {
                Input[i] = KeyCode.None;
            }
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
                foreach(KeyCode key in Input)
                {
                    if(key == KeyCode.None)
                    {
                        continue;
                    }
                    if (!InputManager.GetKey(key))
                    {
                        return;
                    }
                }
                //to get here, all keys that are not None must be held
                //Fader.FadeOut(2f, Color.black);
                //Fader.FadeIn(2f, Color.black);
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
