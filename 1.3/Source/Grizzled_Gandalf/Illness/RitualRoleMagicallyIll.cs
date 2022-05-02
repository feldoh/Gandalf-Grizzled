using RimWorld;
using Verse;

namespace Grizzled_Gandalf.Illness
{
	public class RitualRoleMagicallyIll : RitualRole
	{
		public override bool AppliesToPawn(
			Pawn p,
			out string reason,
			LordJob_Ritual ritual = null,
			RitualRoleAssignments assignments = null,
			Precept_Ritual precept = null,
			bool skipReason = false)
		{
			reason = (string)null;

			if (HasMagicalHediff(p))
				return true;
			if (!skipReason)
				reason = (string)"MessageRitualRoleMustHaveMagicalIllness".Translate((NamedArgument)this.LabelCap);
			return false;
		}

		private bool HasMagicalHediff(Pawn pawn)
		{
			return pawn.health.hediffSet.hediffs.Exists(h => h.def is MagicalHediffDef mh && mh.isMagical);
		}

		public override bool AppliesToRole(
			Precept_Role role,
			out string reason,
			Precept_Ritual ritual = null,
			Pawn p = null,
			bool skipReason = false)
		{
			reason = (string)null;
			return false;
		}
	}
}
