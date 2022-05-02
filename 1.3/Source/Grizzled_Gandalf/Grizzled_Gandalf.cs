using System.Reflection;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;
using HarmonyLib;

namespace Grizzled_Gandalf
{
	public class Mod : Verse.Mod
	{
	public static Settings settings;
		public Mod(ModContentPack content) : base(content)
		{
			Log.Message("Hello world from Grizzled Gandalf");

			// initialize settings
			settings = GetSettings<Settings>();

#if DEBUG
			Harmony.DEBUG = true;
#endif

			Harmony harmony = new Harmony("Taggerung.rimworld.Grizzled_Gandalf.main");	
			harmony.PatchAll();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			base.DoSettingsWindowContents(inRect);
			settings.DoWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "Grizzled Gandalf";
		}
	}
}
