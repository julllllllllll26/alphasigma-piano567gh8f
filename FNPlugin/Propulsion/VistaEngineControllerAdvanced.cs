using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FNPlugin
{
    enum EngineGenerationType { Mk1, Mk2, Mk3 }

    class VistaEngineControllerAdvanced : FNResourceSuppliableModule, IUpgradeableModule 
    {
        // Persistant
		[KSPField(isPersistant = true)]
		bool IsEnabled;
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Upgraded")]
        public bool isupgraded = false;
        [KSPField(isPersistant = true)]
        bool rad_safety_features = true;

        [KSPField(isPersistant = true, guiActive = true, guiName = "Power Ratio"), UI_FloatRange(stepIncrement = 20f, maxValue = 100f, minValue = 20f)]
        public float powerPercentageMk1 = 100;
        [KSPField(isPersistant = true, guiActive = true, guiName = "Power Ratio"), UI_FloatRange(stepIncrement = 10f, maxValue = 100f, minValue = 10f)]
        public float powerPercentageMk2 = 100;
        [KSPField(isPersistant = true, guiActive = true, guiName = "Power Ratio"), UI_FloatRange(stepIncrement = 5f, maxValue = 100f, minValue = 5f)]
        public float powerPercentageMk3 = 100;

        // None Persistant
		[KSPField(isPersistant = false, guiActive = true, guiName = "Radiation Hazard To")]
		public string radhazardstr = "";
        [KSPField(isPersistant = false, guiActive = true, guiName = "Temperature")]
        public string temperatureStr = "";

        [KSPField(isPersistant = false)]
        public float powerRequirement = 625;
        [KSPField(isPersistant = false)]
        public float powerRequirementUpgraded = 1250;
        [KSPField(isPersistant = false)]
        public float powerRequirementUpgraded2 = 2500;

        [KSPField(isPersistant = false)]
        public float maxThrust = 75;
        [KSPField(isPersistant = false)]
        public float maxThrustUpgraded = 300;
        [KSPField(isPersistant = false)]
        public float maxThrustUpgraded2 = 1200;

        [KSPField(isPersistant = false)]
        public float maxAtmosphereDensity = 0.001f;
        [KSPField(isPersistant = false)]
        public float leathalDistance = 2000;
        [KSPField(isPersistant = false)]
        public float killDivider = 50;

        [KSPField(isPersistant = false)]
        public float efficiency = 0.19f;
        [KSPField(isPersistant = false)]
        public float efficiencyUpgraded = 0.38f;
        [KSPField(isPersistant = false)]
        public float efficiencyUpgraded2 = 0.76f;

        [KSPField(isPersistant = false)]
        public float fusionWasteHeat = 625;
        [KSPField(isPersistant = false)]
        public float fusionWasteHeatUpgraded = 2500;
        [KSPField(isPersistant = false)]
        public float fusionWasteHeatUpgraded2 = 10000;

        // Use for SETI Mode
        [KSPField(isPersistant = false)]
        public float wasteHeatMultiplier = 1;
        [KSPField(isPersistant = false)]
        public float powerRequirementMultiplier = 1;

        [KSPField(isPersistant = false)]
        public float maxTemp = 2500;
        [KSPField(isPersistant = false)]
        public float upgradeCost = 100;
        [KSPField(isPersistant = false)]
        public string originalName = "Prototype DT Vista Engine";
        [KSPField(isPersistant = false)]
        public string upgradedName = "DT Vista Engine Mk2";
        [KSPField(isPersistant = false)]
        public string upgradedName2 = "DT Vista Engine Mk3";

        // Gui
        [KSPField(isPersistant = false, guiActive = true, guiName = "Type")]
        public string engineType = "";
        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = true, guiName= "upgrade tech 1")]
        public string upgradeTechReq = "advFusionReactions";
        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = true, guiName = "upgrade tech 2")]
        public string upgradeTechReq2 = "exoticReactions";

        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "Current Throtle", guiFormat = "F2")]
        public float throttle;
        [KSPField(isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "Fusion Ratio", guiFormat = "F2")]
        public float fusionRatio;
        [KSPField(isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "Power Requirement", guiFormat = "F2", guiUnits = " MW")]
        public float enginePowerRequirement;
        [KSPField(isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "Absorbed Wasteheat", guiFormat = "F2", guiUnits = " MW")]
        public float absorbedWasteheat;

        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "Radiator Temp")]
        public float coldBathTemp;
        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "Max Radiator Temp")]
        public float maxTempatureRadiators;
        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "Performance Radiators")]
        public float radiatorPerformance;
        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "Emisiveness")]
        public float partEmissiveConstant;

        protected bool hasrequiredupgrade = false;
		protected bool radhazard = false;
		protected double minISP = 0;
		protected double standard_megajoule_rate = 0;
		protected double standard_deuterium_rate = 0;
		protected double standard_tritium_rate = 0;
        protected ModuleEngines curEngineT;

        public EngineGenerationType EngineGenerationType { get; private set; }

		[KSPEvent(guiActive = true, guiName = "Disable Radiation Safety", active = true)]
		public void DeactivateRadSafety() 
        {
			rad_safety_features = false;
		}

		[KSPEvent(guiActive = true, guiName = "Activate Radiation Safety", active = false)]
		public void ActivateRadSafety() 
        {
			rad_safety_features = true;
		}

        [KSPEvent(guiActive = true, guiName = "Retrofit", active = true)]
        public void RetrofitEngine()
        {
            if (ResearchAndDevelopment.Instance == null || isupgraded || ResearchAndDevelopment.Instance.Science < upgradeCost) return;

            upgradePartModule();
            ResearchAndDevelopment.Instance.AddScience(-upgradeCost, TransactionReasons.RnDPartPurchase);
        }

        #region IUpgradeableModule

        public String UpgradeTechnology { get { return upgradeTechReq; } }

        public void upgradePartModule()
        {
            isupgraded = true;

            if (PluginHelper.upgradeAvailable(upgradeTechReq2))
            {
                engineType = upgradedName2 ;
                EngineGenerationType = EngineGenerationType.Mk3;
            }
            else
            {
                engineType = upgradedName;
                EngineGenerationType = EngineGenerationType.Mk2;
            }
        }

        #endregion

        public float MaximumThrust { get { return PowerRatio * FullTrustMaximum; } }
        
        public float FusionWasteHeat 
        { 
            get 
            {
                if (EngineGenerationType == EngineGenerationType.Mk1)
                    return fusionWasteHeat;
                else if (EngineGenerationType == EngineGenerationType.Mk2)
                    return fusionWasteHeatUpgraded;
                else
                    return fusionWasteHeatUpgraded2;
            } 
        }

        public float FullTrustMaximum
        {
            get
            {
                if (EngineGenerationType == EngineGenerationType.Mk1)
                    return maxThrust;
                else if (EngineGenerationType == EngineGenerationType.Mk2)
                    return maxThrustUpgraded;
                else
                    return maxThrustUpgraded2;
            }
        }

        public float LaserEfficiency
        {
            get
            {
                if (EngineGenerationType == EngineGenerationType.Mk1)
                    return efficiency;
                else if (EngineGenerationType == EngineGenerationType.Mk2)
                    return efficiencyUpgraded;
                else
                    return efficiencyUpgraded2;
            }
        }

        public float CurrentPowerRequirement
        {
            get
            {
                return PowerRequirementMaximum * PowerRatio * powerRequirementMultiplier;
            }
        }

        public float PowerRequirementMaximum
        {
            get
            {
                if (EngineGenerationType == EngineGenerationType.Mk1)
                    return powerRequirement;
                else if (EngineGenerationType == EngineGenerationType.Mk2)
                    return powerRequirementUpgraded;
                else
                    return powerRequirementUpgraded2;
            }
        }

        public float PowerRatio
        {
            get
            {
                if (EngineGenerationType == EngineGenerationType.Mk1)
                    return powerPercentageMk1 / 100;
                else if (EngineGenerationType == EngineGenerationType.Mk2)
                    return powerPercentageMk2 / 100;
                else
                    return powerPercentageMk3 / 100;
            }
        }
        

        public override void OnStart(PartModule.StartState state) 
        {
            part.maxTemp = maxTemp;
            part.thermalMass = 1;
            part.thermalMassModifier = 1;
            EngineGenerationType = EngineGenerationType.Mk1;

            engineType = originalName;
            curEngineT = this.part.FindModuleImplementing<ModuleEngines>();

            if (curEngineT == null) return;

            minISP = curEngineT.atmosphereCurve.Evaluate(0);

            standard_deuterium_rate = curEngineT.propellants.FirstOrDefault(pr => pr.name == InterstellarResourcesConfiguration.Instance.Deuterium).ratio;
            standard_tritium_rate = curEngineT.propellants.FirstOrDefault(pr => pr.name == InterstellarResourcesConfiguration.Instance.Tritium).ratio;

            // if we can upgrade, let's do so
            if (isupgraded)
                upgradePartModule();
            else if (this.HasTechsRequiredToUpgrade())
                hasrequiredupgrade = true;

            // calculate WasteHeat Capacity
            part.Resources[FNResourceManager.FNRESOURCE_WASTEHEAT].maxAmount = part.mass * 1.0e+5 * wasteHeatMultiplier;

            
            if (state == StartState.Editor && this.HasTechsRequiredToUpgrade())
            {
                isupgraded = true;
                upgradePartModule();
            }

            Fields["powerPercentageMk1"].guiActive = EngineGenerationType == EngineGenerationType.Mk1;
            Fields["powerPercentageMk2"].guiActive = EngineGenerationType == EngineGenerationType.Mk2;
            Fields["powerPercentageMk3"].guiActive = EngineGenerationType == EngineGenerationType.Mk3;
            
            if (state != StartState.Editor)
                part.emissiveConstant = maxTempatureRadiators > 0 ? 1 - coldBathTemp / maxTempatureRadiators : 0.01;
		}

		public override void OnUpdate() 
        {
            if (curEngineT == null) return;

            Fields["powerPercentageMk1"].guiActive = EngineGenerationType == EngineGenerationType.Mk1;
            Fields["powerPercentageMk2"].guiActive = EngineGenerationType == EngineGenerationType.Mk2;
            Fields["powerPercentageMk3"].guiActive = EngineGenerationType == EngineGenerationType.Mk3;

            Events["DeactivateRadSafety"].active = rad_safety_features;
            Events["ActivateRadSafety"].active = !rad_safety_features;
            Events["RetrofitEngine"].active = !isupgraded && ResearchAndDevelopment.Instance.Science >= upgradeCost && hasrequiredupgrade;

			if (curEngineT.isOperational && !IsEnabled) 
            {
				IsEnabled = true;
				part.force_activate ();
			}

			int kerbal_hazard_count = 0;
			foreach (Vessel vess in FlightGlobals.Vessels) 
            {
				float distance = (float)Vector3d.Distance (vessel.transform.position, vess.transform.position);
                if (distance < leathalDistance && vess != this.vessel)
					kerbal_hazard_count += vess.GetCrewCount ();
			}

			if (kerbal_hazard_count > 0) 
            {
				radhazard = true;
				if (kerbal_hazard_count > 1) 
					radhazardstr = kerbal_hazard_count.ToString () + " Kerbals.";
                else 
					radhazardstr = kerbal_hazard_count.ToString () + " Kerbal.";
				
				Fields["radhazardstr"].guiActive = true;
			} 
            else 
            {
				Fields["radhazardstr"].guiActive = false;
				radhazard = false;
				radhazardstr = "None.";
			}
		}

        private void ShutDown(string reason)
        {
            curEngineT.Events["Shutdown"].Invoke();
            curEngineT.currentThrottle = 0;
            curEngineT.requestedThrottle = 0;

            ScreenMessages.PostScreenMessage(reason, 5.0f, ScreenMessageStyle.UPPER_CENTER);
            foreach (FXGroup fx_group in part.fxGroups)
            {
                fx_group.setActive(false);
            }
        }

		public override void OnFixedUpdate()
        {
            temperatureStr = part.temperature.ToString("0.00") + "K / " + part.maxTemp.ToString("0.00") + "K";

            if (curEngineT == null) return;

            throttle = curEngineT.currentThrottle > 0 ? Mathf.Max(curEngineT.currentThrottle, 0.01f) : 0;

            if (throttle > 0)
            {
                if (vessel.atmDensity > maxAtmosphereDensity)
                    ShutDown("Inertial Fusion cannot operate in atmosphere!");

                if (radhazard && rad_safety_features)
                    ShutDown("Engines throttled down as they presently pose a radiation hazard");
            }

            KillKerbalsWithRadiation(throttle);

            if (throttle > 0)
            {
                // Calculate Fusion Ratio
                enginePowerRequirement = CurrentPowerRequirement;
                var requestedPowerFixed = enginePowerRequirement * TimeWarp.fixedDeltaTime;
                var recievedPowerFixed = consumeFNResource(requestedPowerFixed, FNResourceManager.FNRESOURCE_MEGAJOULES);
                var plasma_ratio = recievedPowerFixed / requestedPowerFixed;
                fusionRatio = plasma_ratio >= 1 ? 1 : plasma_ratio > 0.75f ? Mathf.Pow((float)plasma_ratio, 6) : 0;

                // Lasers produce Wasteheat
                supplyFNResource(recievedPowerFixed * (1 - LaserEfficiency), FNResourceManager.FNRESOURCE_WASTEHEAT);

                // The Aborbed wasteheat from Fusion
                absorbedWasteheat = PowerRatio * FusionWasteHeat * wasteHeatMultiplier * fusionRatio;
                supplyFNResource(absorbedWasteheat * TimeWarp.fixedDeltaTime, FNResourceManager.FNRESOURCE_WASTEHEAT);

                // change ratio propellants Hydrogen/Fusion
                curEngineT.propellants.FirstOrDefault(pr => pr.name == InterstellarResourcesConfiguration.Instance.Deuterium).ratio = (float)standard_deuterium_rate / throttle / throttle;
                curEngineT.propellants.FirstOrDefault(pr => pr.name == InterstellarResourcesConfiguration.Instance.Tritium).ratio = (float)standard_tritium_rate / throttle / throttle;

                // Update ISP
                var currentIsp = Math.Max(minISP * fusionRatio / throttle, minISP / 10);
                FloatCurve newISP = new FloatCurve();
                newISP.Add(0, (float)currentIsp);
                curEngineT.atmosphereCurve = newISP;

                // Update FuelFlow
                var maxFuelFlow = fusionRatio * MaximumThrust / currentIsp / PluginHelper.GravityConstant;
                curEngineT.maxFuelFlow = (float)maxFuelFlow;

                if (!curEngineT.getFlameoutState && plasma_ratio < 0.75 && recievedPowerFixed > 0)
                    curEngineT.status = "Insufficient Electricity";
            }
            else
            {
                enginePowerRequirement = 0;
                absorbedWasteheat = 0;
                fusionRatio = 0;

                var currentIsp = minISP * 100;
                FloatCurve newISP = new FloatCurve();
                newISP.Add(0, (float)currentIsp);
                curEngineT.atmosphereCurve = newISP;

                var maxFuelFlow = MaximumThrust / currentIsp / PluginHelper.GravityConstant;
                curEngineT.maxFuelFlow = (float)maxFuelFlow;

                curEngineT.propellants.FirstOrDefault(pr => pr.name == InterstellarResourcesConfiguration.Instance.Deuterium).ratio = (float)(standard_deuterium_rate);
                curEngineT.propellants.FirstOrDefault(pr => pr.name == InterstellarResourcesConfiguration.Instance.Tritium).ratio = (float)(standard_tritium_rate);
            }

            coldBathTemp = (float)FNRadiator.getAverageRadiatorTemperatureForVessel(vessel);
            maxTempatureRadiators = (float)FNRadiator.getAverageMaximumRadiatorTemperatureForVessel(vessel);
            radiatorPerformance = Mathf.Max(1 - (coldBathTemp / maxTempatureRadiators), 0.000001f);
            partEmissiveConstant = (float)part.emissiveConstant;
        }

        private void KillKerbalsWithRadiation(float throttle)
        {
            if (!radhazard || throttle <= 0 || rad_safety_features) return;

            System.Random rand = new System.Random(new System.DateTime().Millisecond);
            List<Vessel> vessels_to_remove = new List<Vessel>();
            List<ProtoCrewMember> crew_to_remove = new List<ProtoCrewMember>();
            double death_prob = TimeWarp.fixedDeltaTime;

            foreach (Vessel vess in FlightGlobals.Vessels)
            {
                float distance = (float)Vector3d.Distance(vessel.transform.position, vess.transform.position);

                if (distance >= leathalDistance || vess == this.vessel || vess.GetCrewCount() <= 0) continue;

                float inv_sq_dist = distance / killDivider;
                float inv_sq_mult = 1.0f / inv_sq_dist / inv_sq_dist;
                foreach (ProtoCrewMember crew_member in vess.GetVesselCrew())
                {
                    if (UnityEngine.Random.value < (1.0 - death_prob * inv_sq_mult)) continue;

                    if (!vess.isEVA)
                    {
                        ScreenMessages.PostScreenMessage(crew_member.name + " was killed by Neutron Radiation!", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                        crew_to_remove.Add(crew_member);
                    }
                    else
                    {
                        ScreenMessages.PostScreenMessage(crew_member.name + " was killed by Neutron Radiation!", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                        vessels_to_remove.Add(vess);
                    }
                }
            }

            foreach (Vessel vess in vessels_to_remove)
            {
                vess.rootPart.Die();
            }

            foreach (ProtoCrewMember crew_member in crew_to_remove)
            {
                Vessel vess = FlightGlobals.Vessels.Find(p => p.GetVesselCrew().Contains(crew_member));
                Part part = vess.Parts.Find(p => p.protoModuleCrew.Contains(crew_member));
                part.RemoveCrewmember(crew_member);
                crew_member.Die();
            }
        }

        public override string getResourceManagerDisplayName() 
        {
            return engineType;
        }

        public override int getPowerPriority() 
        {
            return 1;
        }
	}
}
