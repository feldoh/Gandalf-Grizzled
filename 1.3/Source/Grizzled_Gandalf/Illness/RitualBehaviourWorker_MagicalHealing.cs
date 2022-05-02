using RimWorld;
using Verse;

namespace Grizzled_Gandalf.Illness
{
	public class RitualBehaviourWorker_MagicalHealing: RitualBehaviorWorker
	{
		private int ticksSinceLastInteraction = -1;
    public const int SocialInteractionIntervalTicks = 700;

    public RitualBehaviourWorker_MagicalHealing()
    {
    }

    public RitualBehaviourWorker_MagicalHealing(RitualBehaviorDef def)
      : base(def)
    {
    }

    public override void Tick(LordJob_Ritual ritual)
    {
      base.Tick(ritual);
      if (ritual.StageIndex == 0)
        return;
      if (this.ticksSinceLastInteraction == -1 || this.ticksSinceLastInteraction > 700)
      {
        this.ticksSinceLastInteraction = 0;
        Pawn focus = ritual.PawnWithRole("focus");
        Pawn patient = ritual.PawnWithRole("patient");
        focus.interactions.TryInteractWith(patient, InteractionDefOf.Reassure);
      }
      else
        ++this.ticksSinceLastInteraction;
    }

    public override void ExposeData()
    {
      base.ExposeData();
      Scribe_Values.Look<int>(ref this.ticksSinceLastInteraction, "ticksSinceLastInteraction", -1);
    }
  }
}


