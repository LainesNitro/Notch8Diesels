using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using DV.CabControls.Spec;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using LocoSim.Definitions;

namespace Notch8Diesels;

[EnableReloading]
public static class Main
{
	public static Settings? settings;

	private static bool Load(UnityModManager.ModEntry modEntry)
	{
		settings = Settings.Load<Settings>(modEntry);
		modEntry.OnGUI = OnGUI;
		modEntry.OnSaveGUI = OnSaveGUI;

		SetAllLevers();

		return true;
	}

	static void OnGUI(UnityModManager.ModEntry modEntry)
	{
		GUILayout.Label("Customize the amount of lever notches. \nWARNING: It is strongly advised to make changes from main menu or throttle behaviour may be unexpected.");
		settings?.Draw(modEntry);
	}

	static void OnSaveGUI(UnityModManager.ModEntry modEntry)
	{
		settings?.Save(modEntry);
	}

	static void SetLeverNotches(TrainCarType __loco, string __path, int __notches)
	{
		__loco.ToV2().interiorPrefab.transform.Find(__path).GetComponent<Lever>().notches = __notches;
	}
	static void SetThrottleGammaNotches(TrainCarType __loco, string __path, int __notches)
	{
		__loco.ToV2().prefab.transform.Find(__path).GetComponent<ThrottleGammaPowerConversionDefinition>().numberOfNotches = __notches;
    }

	public static void SetAllLevers()
	{
		if (settings == null)
			return;

		if (settings.DE6Enabled)
		{
			SetLeverNotches(TrainCarType.LocoDiesel, "RightColumn/Throttle/C_Throttle", settings.DE6ThrottleNotches + 1);
            SetThrottleGammaNotches(TrainCarType.LocoDiesel, "[sim]/throttlePower", settings.DE6ThrottleNotches + 1);
            SetLeverNotches(TrainCarType.LocoDiesel, "RightColumn/BrakeDynamic/C_BrakeDynamic", settings.DE6DynamicNotches + 1);
			SetLeverNotches(TrainCarType.LocoDiesel, "LeftColumn/BrakeTrain/C_BrakeTrain", settings.DE6BrakeNotches + 1);
			SetLeverNotches(TrainCarType.LocoDiesel, "LeftColumn/BrakeIndependent/C_BrakeIndependent", settings.DE6IndependentNotches + 1);

		}
		if (settings.DH4Enabled)
		{
			SetLeverNotches(TrainCarType.LocoDH4, "LeftCluster/Throttle/C_Throttle", settings.DH4ThrottleNotches + 1);
			SetLeverNotches(TrainCarType.LocoDH4, "LeftCluster/BrakeDynamic/C_BrakeDynamic", settings.DH4DynamicNotches + 1);
			SetLeverNotches(TrainCarType.LocoDH4, "RightCluster/BrakeTrain/C_BrakeTrain", settings.DH4BrakeNotches + 1);
			SetLeverNotches(TrainCarType.LocoDH4, "RightCluster/BrakeIndependent/C_BrakeIndependent", settings.DH4IndependentNotches + 1);
		}
		if (settings.DM3Enabled)
		{
			SetLeverNotches(TrainCarType.LocoDM3, "DashCluster/Throttle/C_Throttle", settings.DM3ThrottleNotches + 1);
			SetLeverNotches(TrainCarType.LocoDM3, "DashCluster/BrakeDynamic/C_BrakeDynamic", settings.DM3DynamicNotches + 1);
			SetLeverNotches(TrainCarType.LocoDM3, "DashCluster/BrakeIndependent/C_BrakeIndependent", settings.DM3IndependentNotches + 1);
		}
		if (settings.DE2Enabled)
		{
			SetLeverNotches(TrainCarType.LocoShunter, "DashCluster/Throttle/C_Throttle", settings.DE2ThrottleNotches + 1);
			SetThrottleGammaNotches(TrainCarType.LocoShunter, "[sim]/throttlePower", settings.DE2ThrottleNotches + 1);
			SetLeverNotches(TrainCarType.LocoShunter, "RightCluster/BrakeTrain/C_BrakeTrain", settings.DE2BrakeNotches + 1);
			SetLeverNotches(TrainCarType.LocoShunter, "RightCluster/BrakeIndependent/C_BrakeIndependent", settings.DE2IndependentNotches + 1);
		}
		if (settings.DM1UEnabled)
		{
            SetLeverNotches(TrainCarType.LocoDM1U, "LeftCluster/Throttle/C_Throttle", settings.DM1UThrottleNotches + 1);
            SetLeverNotches(TrainCarType.LocoDM1U, "RightCluster/TrainBrake/C_TrainBrake", settings.DM1UBrakeNotches + 1);
        }
		if (settings.BE2Enabled)
		{
            SetLeverNotches(TrainCarType.LocoMicroshunter, "DashCluster/Throttle/C_Throttle", settings.BE2ThrottleNotches + 1);
            SetLeverNotches(TrainCarType.LocoMicroshunter, "DashCluster/BrakeTrain/C_BrakeTrain", settings.BE2BrakeNotches + 1);
        }
    }
}

public class Settings : UnityModManager.ModSettings, IDrawable
{
	[Draw("DE6")]
	public bool DE6Enabled = true;
	[Draw("Throttle\t\t", VisibleOn = "DE6Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DE6ThrottleNotches = 8;
	[Draw("Dynamic Brake\t", VisibleOn = "DE6Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DE6DynamicNotches = 8;
	[Draw("Train Brake\t", VisibleOn = "DE6Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DE6BrakeNotches = 10;
	[Draw("Independent Brake\t", VisibleOn = "DE6Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DE6IndependentNotches = 7;
	[Space(5)]
	[Draw("DH4")]
	public bool DH4Enabled = false;
	[Draw("Throttle\t\t", VisibleOn = "DH4Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DH4ThrottleNotches = 7;
	[Draw("Dynamic Brake\t", VisibleOn = "DH4Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DH4DynamicNotches = 7;
	[Draw("Train Brake\t", VisibleOn = "DH4Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DH4BrakeNotches = 11;
	[Draw("Independent Brake\t", VisibleOn = "DH4Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DH4IndependentNotches = 7;
	[Space(5)]
	[Draw("DM3")]
	public bool DM3Enabled = false;
	[Draw("Throttle\t\t", VisibleOn = "DM3Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DM3ThrottleNotches = 7;
	[Draw("Dynamic Brake\t", VisibleOn = "DM3Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DM3DynamicNotches = 7;
	[Draw("Independent Brake\t", VisibleOn = "DM3Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DM3IndependentNotches = 7;
	[Space(5)]
	[Draw("DE2")]
	public bool DE2Enabled = false;
	[Draw("Throttle\t\t", VisibleOn = "DE2Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DE2ThrottleNotches = 11;
	[Draw("Train Brake\t", VisibleOn = "DE2Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DE2BrakeNotches = 11;
	[Draw("Independent Brake\t", VisibleOn = "DE2Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
	public int DE2IndependentNotches = 7;
	[Space(5)]
	[Draw("DM1U")]
	public bool DM1UEnabled = false;
    [Draw("Throttle\t\t", VisibleOn = "DM1UEnabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
    public int DM1UThrottleNotches = 5;
    [Draw("Train Brake\t", VisibleOn = "DM1UEnabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
    public int DM1UBrakeNotches = 7;
    [Space(5)]
    [Draw("BE2")]
    public bool BE2Enabled = false;
    [Draw("Throttle\t\t", VisibleOn = "BE2Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
    public int BE2ThrottleNotches = 9;
    [Draw("Train Brake\t", VisibleOn = "BE2Enabled|true", Type = DrawType.Slider, Min = 1, Max = 50, Width = 300)]
    public int BE2BrakeNotches = 11;


    public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}

	public void OnChange()
	{
		Main.SetAllLevers();
	}

}
