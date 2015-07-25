﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FNPlugin 
{
    class ReactorFuelMode 
	{
        protected int _reactor_type;
        protected string _mode_gui_name;
        protected string _tech_requirement;
        protected List<ReactorFuel> _fuels;
        protected List<ReactorProduct> _products;
        protected double _normreactionrate;
        protected double _normpowerrequirements;
        protected double _charged_power_ratio;
        protected double _mev_per_charged_product;
        protected double _neutrons_ratio;
        protected double _fuel_efficency_multiplier;

        public ReactorFuelMode(ConfigNode node) 
        {
            _reactor_type = Convert.ToInt32(node.GetValue("ReactorType"));
            _mode_gui_name = node.GetValue("GUIName");
            _tech_requirement = node.HasValue("TechRequirement") ? node.GetValue("TechRequirement") : String.Empty;

            _normreactionrate = Double.Parse(node.GetValue("NormalisedReactionRate"));
            _normpowerrequirements = Double.Parse(node.GetValue("NormalisedPowerConsumption"));
            _charged_power_ratio = Double.Parse(node.GetValue("ChargedParticleRatio"));

            _mev_per_charged_product = node.HasValue("MeVPerChargedProduct") ? Double.Parse(node.GetValue("MeVPerChargedProduct")) : 0;
            _neutrons_ratio = node.HasValue("NeutronsRatio") ? Double.Parse(node.GetValue("NeutronsRatio")) : 1;
            _fuel_efficency_multiplier = node.HasValue("FuelEfficiencyMultiplier") ? Double.Parse(node.GetValue("FuelEfficiencyMultiplier")) : 1;

            ConfigNode[] fuel_nodes = node.GetNodes("FUEL");
            _fuels = fuel_nodes.Select(nd => new ReactorFuel(nd)).ToList();

            ConfigNode[] products_nodes = node.GetNodes("PRODUCT");
            _products = products_nodes.Select(nd => new ReactorProduct(nd)).ToList();
        }

        public int SupportedReactorTypes { get { return _reactor_type; } }

        public string ModeGUIName { get { return _mode_gui_name; } }

        public string TechRequirement  { get { return _tech_requirement; } }

        public IList<ReactorFuel> ReactorFuels { get { return _fuels; } }

        public IList<ReactorProduct> ReactorProducts { get { return _products; } }

        public bool Aneutronic { get { return _neutrons_ratio == 0; } }

        public double ChargedPowerRatio { get { return _charged_power_ratio; } }

        public double MeVPerChargedProduct { get { return _mev_per_charged_product; } }

        public double NormalisedReactionRate { get { return _normreactionrate; } }

        public double NormalisedPowerRequirements { get { return _normpowerrequirements; } }

        public double NeutronsRatio { get { return _neutrons_ratio; } }

        public double FuelEfficencyMultiplier { get { return _fuel_efficency_multiplier; } }
    }
}
