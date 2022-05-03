using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Grizzled_Gandalf.MagicEvents
{
	[HarmonyPatch(typeof(Building_Door), nameof(Building_Door.StartManualOpenBy))]
	internal static class DoorOpenHarmonyPatch
	{
		static void Postfix(Pawn opener)
		{
			if (Rand.Chance(0.01f) || opener == null) return;
			IntVec3 target = opener.OccupiedRect().CenterCell;
			SpawnRandomMagicalAnimalAt(target, opener.Map);
			MoteMaker.ThrowText(opener.DrawPos, opener.Map,
				"TextMote_MagicAnimalSpawn".Translate((NamedArgument)opener.Ideo.KeyDeityName), 6.5f);
		}

		private static bool SpawnRandomMagicalAnimalAt(IntVec3 loc, Map map)
		{
			PawnKindDef kindDef = PawnKindDef.Named(Rand.Bool ? "Duck" : "Snowhare");
			if (kindDef == null)
			{
				Log.Error("Cannot Spawn Magical Animal.");
				return false;
			}

			int randomInRange = Rand.RangeInclusive(1, Math.Max(3, kindDef.wildGroupSize.RandomInRange));
			for (int index = 0; index < randomInRange; ++index)
			{
				Pawn generatedAnimal = PawnGenerator.GeneratePawn(kindDef);
				GenSpawn.Spawn((Thing)generatedAnimal, loc, map);
				generatedAnimal.mindState.mentalStateHandler.TryStartMentalState(
					(Rand.Chance(0.15f)) ? MentalStateDefOf.Berserk : MentalStateDefOf.PanicFlee);
			}

			return true;
		}
	}
}
