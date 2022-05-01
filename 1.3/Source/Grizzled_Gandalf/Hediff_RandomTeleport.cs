using System;
using RimWorld;
using Verse;
using Verse.Sound;

namespace Grizzled_Gandalf
{
	public class Hediff_RandomTeleport : HediffWithComps
	{
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			UpdateTarget();
		}
		
		public override void Tick()
		{
			base.Tick();
			if (!this.pawn.IsHashIntervalTick(250))
			{
				UpdateTarget();
			}
		
		}

		private void UpdateTarget()
		{
			Pawn pawn = this.pawn;
			if (pawn.Spawned)
			{
				var pawnMap = pawn.Map;
				IntVec3 approxDest = pawn.OccupiedRect().ExpandedBy(50).RandomCell.ClampInsideMap(pawnMap);
				if (this.FindFreeCell(approxDest, pawnMap, out var result)) {
					pawn.teleporting = true;
					pawn.Position = result;
					FleckMaker.ThrowDustPuffThick(result.ToVector3(), pawnMap, Rand.Range(1.5f, 3f), CompAbilityEffect_Chunkskip.DustColor);
					pawn.teleporting = false;
					pawn.stances.stunner.StunFor(new IntRange(50, 150).RandomInRange, (Thing)pawn, false);
					pawn.Notify_Teleported();
					SoundDefOf.Psycast_Skip_Pulse.PlayOneShot((SoundInfo) new TargetInfo(approxDest, pawnMap));
					pawn.health?.RemoveHediff(this);
				}
			}
		}
		
		private bool FindFreeCell(IntVec3 target, Map map, out IntVec3 result) => CellFinder.TryFindRandomSpawnCellForPawnNear(target, map, out result, 10, (Predicate<IntVec3>) (cell => CompAbilityEffect_WithDest.CanTeleportThingTo((LocalTargetInfo) cell, map)));

	}
	
}