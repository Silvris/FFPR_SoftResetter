using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FFPR_SoftResetter
{
    public class Configuration
    {
        public string SoftReset = "Soft-Reset";
        public ConfigEntry<KeyCode> Input1 { get; set; }
        public ConfigEntry<KeyCode> Input2 { get; set; }
        public ConfigEntry<KeyCode> Input3 { get; set; }
        public ConfigEntry<KeyCode> Input4 { get; set; }

        public Configuration(ConfigFile file)
        {
            Input1 = file.Bind(new ConfigDefinition(SoftReset, "Input 1"), KeyCode.None, new ConfigDescription("The key that needs to be held in order to soft-reset the game. \n If this input is set to KeyCode.None, soft-resetting will be disabled."));
            Input2 = file.Bind(new ConfigDefinition(SoftReset, "Input 2"), KeyCode.None, new ConfigDescription("The key that needs to be held in order to soft-reset the game."));
            Input3 = file.Bind(new ConfigDefinition(SoftReset, "Input 3"), KeyCode.None, new ConfigDescription("The key that needs to be held in order to soft-reset the game."));
            Input4 = file.Bind(new ConfigDefinition(SoftReset, "Input 4"), KeyCode.None, new ConfigDescription("The key that needs to be held in order to soft-reset the game."));

            Input1.SettingChanged += Input_SettingChanged;
            Input2.SettingChanged += Input_SettingChanged;
            Input3.SettingChanged += Input_SettingChanged;
            Input4.SettingChanged += Input_SettingChanged;
        }

        private void Input_SettingChanged(object sender, EventArgs e)
        {
            ModComponent.RefreshKeyCodes();
        }

    }
}
