using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Grizzled_Gandalf
{
	public class Hediff_RandomTeleport : HediffWithComps
	{
		private int teleportInterval;
		private int teleportRange;
		private float teleportFactor;
		private const int OneDayTicks = 60000;
		private const int TeleportBaseRange = 50;

		public override void PostMake()
		{
			base.PostMake();
			teleportFactor = Rand.Range(0.8f, 2.0f);
			UpdateTeleportSeverity();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref teleportFactor, "teleportFactor");
		}

		private void UpdateTeleportSeverity()
		{
			teleportInterval = Mathf.Clamp((int)((1.0f - Severity) * teleportFactor) * OneDayTicks, 250,
				(int)(OneDayTicks * teleportFactor));
			teleportRange = Mathf.Clamp((int)(Severity * teleportFactor * TeleportBaseRange), 10, pawn.Map.Size.x);
		}

		private bool ShouldTeleport()
		{
			return !pawn.IsHashIntervalTick(teleportInterval);
		}

		public override void Tick()
		{
			base.Tick();
			if (!ShouldTeleport())
				return;
			UpdateTarget();
			UpdateTeleportSeverity();
		}

		private void UpdateTarget()
		{
			if (!pawn.Spawned) return;
			var pawnMap = pawn.Map;
			IntVec3 approxDest = pawn.OccupiedRect().ExpandedBy(teleportRange).RandomCell.ClampInsideMap(pawnMap);
			if (!this.FindFreeCell(approxDest, pawnMap, out var result)) return;
			pawn.teleporting = true;
			pawn.Position = result;
			FleckMaker.ThrowDustPuffThick(result.ToVector3(), pawnMap, Rand.Range(1.5f, 3f),
				CompAbilityEffect_Chunkskip.DustColor);
			pawn.teleporting = false;
			pawn.stances.stunner.StunFor(new IntRange(50, 150).RandomInRange, pawn, false);
			pawn.Notify_Teleported();
			SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(approxDest, pawnMap));
		}

		private bool FindFreeCell(IntVec3 target, Map map, out IntVec3 result) =>
			CellFinder.TryFindRandomSpawnCellForPawnNear(target, map, out result, 10,
				(Predicate<IntVec3>)(cell => CompAbilityEffect_WithDest.CanTeleportThingTo((LocalTargetInfo)cell, map)));
	}
}