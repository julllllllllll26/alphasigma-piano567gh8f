﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FNPlugin
{
    public class InterstellarResourcesConfiguration
    {
        private static InterstellarResourcesConfiguration _instance = null;

        private const String _ANTIMATTER = "Antimatter";
        private const String _INTAKE_ATMOSPHERE = "IntakeAtm";
        private const String _URANIUM_TETRAFLOURIDE = "EnrichedUranium";
        private const String _THORIUM_TETRAFLOURIDE = "ThF4";
        private const String _URANIUM_NITRIDE = "UraniumNitride";
        private const String _ACTINIDES = "Actinides";
        private const String _DEPLETED_FUEL = "DepletedFuel";
        private const String _VACUUM_PLASMA = "VacuumPlasma";
        private const String _EXOTIC_MATTER = "ExoticMatter";
        private const String _LITHIUM = "Lithium";
        private const String _PLUTONIUM_238 = "Plutonium-238";
        private const String _ALUMINA = "Alumina";
        private const String _HELIUM = "LqdHelium";
        private const String _HELIUM3 = "LqdHe3";
        private const String _DEUTERIUM = "LqdDeuterium";

        private readonly String _uranium_TerraFloride = _URANIUM_TETRAFLOURIDE;
        private readonly String _aluminium;
        private readonly String _ammonia = "LqdAmmonia";
        private readonly String _argon;
        private readonly String _carbonDioxide = "LqdCO2";
        private readonly String _carbonMoxoxide = "LqdCO";
        private readonly String _hydrogen = "LqdHydrogen";
        private readonly String _hydrogen_peroxide;
        private readonly String _methane = "LqdMethane";
        private readonly String _nitrogen = "LqdNitrogen";
        private readonly String _oxygen = "LqdOxygen";
        private readonly String _water = "Water";
        private readonly String _tritium = "LqdTritium";

        public String Actinides { get { return _ACTINIDES; } }
        public String Alumina { get { return _ALUMINA; } }
        public String Aluminium { get { return _aluminium; } }
        public String Ammonia { get { return _ammonia; } }
        public String Antimatter { get { return _ANTIMATTER; } }
        public String Argon { get { return _argon; } }
        public String CarbonDioxide { get { return _carbonDioxide; } }
        public String CarbonMoxoxide { get { return _carbonMoxoxide; } }
        public String DepletedFuel { get { return _DEPLETED_FUEL; } }
        public String Deuterium { get { return _DEUTERIUM; } }
        public String ExoticMatter { get { return _EXOTIC_MATTER; } }
        public String Helium { get { return _HELIUM; } }
        public String Helium3 { get { return _HELIUM3; } }
        public String Hydrogen { get { return _hydrogen; } }
        public String HydrogenPeroxide { get { return _hydrogen_peroxide; } }
        public String IntakeAtmosphere { get { return _INTAKE_ATMOSPHERE; } }
        public String Lithium { get { return _LITHIUM; } }
        public String Methane { get { return _methane; } }
        public String Nitrogen { get { return _nitrogen; } }
        public String Oxygen { get { return _oxygen; } }
        public String Plutonium238 { get { return _PLUTONIUM_238; } }
        public String ThoriumTetraflouride { get { return _THORIUM_TETRAFLOURIDE; } }
        public String Tritium { get { return _tritium; } }
        public String UraniumTetraflouride { get { return _uranium_TerraFloride; } }
        public String UraniumNitride { get { return _URANIUM_NITRIDE; } }
        public String VacuumPlasma { get { return _VACUUM_PLASMA; } }
        public String Water { get { return _water; } }
        

        public InterstellarResourcesConfiguration(ConfigNode plugin_settings)
        {
            if (plugin_settings != null)
            {
                if (plugin_settings.HasValue("TritiumResourceName"))
                {
                    _tritium = plugin_settings.GetValue("TritiumResourceName");
                    Debug.Log("[KSP Interstellar] Tritium resource name set to " + _tritium);
                }

                if (plugin_settings.HasValue("CarbonDioxideResourceName"))
                {
                    _carbonDioxide = plugin_settings.GetValue("CarbonDioxideResourceName");
                    Debug.Log("[KSP Interstellar] CarbonDioxide resource name set to " + _carbonDioxide);
                }
                if (plugin_settings.HasValue("CarbonMonoxideResourceName"))
                {
                    _carbonMoxoxide = plugin_settings.GetValue("CarbonMonoxideResourceName");
                    Debug.Log("[KSP Interstellar] CarbonMonoxide resource name set to " + _carbonMoxoxide);
                }

                if (plugin_settings.HasValue("UraniumTetraflourideName"))
                {
                    _uranium_TerraFloride = plugin_settings.GetValue("UraniumTetraflourideName");
                    Debug.Log("[KSP Interstellar] UraniumTetraflouride resource name set to " + _uranium_TerraFloride);
                }
                if (plugin_settings.HasValue("HydrogenResourceName"))
                {
                    _hydrogen = plugin_settings.GetValue("HydrogenResourceName");
                    Debug.Log("[KSP Interstellar] Hydrogen resource name set to " + Hydrogen);
                }
                if (plugin_settings.HasValue("OxygenResourceName"))
                {
                    _oxygen = plugin_settings.GetValue("OxygenResourceName");
                    Debug.Log("[KSP Interstellar] Oxygen resource name set to " + Oxygen);
                }
                if (plugin_settings.HasValue("AluminiumResourceName"))
                {
                    _aluminium = plugin_settings.GetValue("AluminiumResourceName");
                    Debug.Log("[KSP Interstellar] Aluminium resource name set to " + Aluminium);
                }
                if (plugin_settings.HasValue("MethaneResourceName"))
                {
                    _methane = plugin_settings.GetValue("MethaneResourceName");
                    Debug.Log("[KSP Interstellar] Methane resource name set to " + Methane);
                }
                if (plugin_settings.HasValue("ArgonResourceName"))
                {
                    _argon = plugin_settings.GetValue("ArgonResourceName");
                    Debug.Log("[KSP Interstellar] Argon resource name set to " + Argon);
                }
                if (plugin_settings.HasValue("WaterResourceName"))
                {
                    _water = plugin_settings.GetValue("WaterResourceName");
                    Debug.Log("[KSP Interstellar] Water resource name set to " + Water);
                }
                if (plugin_settings.HasValue("HydrogenPeroxideResourceName"))
                {
                    _hydrogen_peroxide = plugin_settings.GetValue("HydrogenPeroxideResourceName");
                    Debug.Log("[KSP Interstellar] Hydrogen Peroxide resource name set to " + HydrogenPeroxide);
                }
                if (plugin_settings.HasValue("AmmoniaResourceName"))
                {
                    _ammonia = plugin_settings.GetValue("AmmoniaResourceName");
                    Debug.Log("[KSP Interstellar] Ammonia resource name set to " + Ammonia);
                }
                if (plugin_settings.HasValue("NitrogenResourceName"))
                {
                    _nitrogen = plugin_settings.GetValue("NitrogenResourceName");
                    Debug.Log("[KSP Interstellar] Nitrogen resource name set to " + Nitrogen);
                }
            } else
            {
                PluginHelper.showInstallationErrorMessage();
            }
        }

        public static InterstellarResourcesConfiguration Instance { get { return _instance ?? (_instance = new InterstellarResourcesConfiguration(PluginHelper.PluginSettingsConfig)); } }
    }
}
