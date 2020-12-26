using System.Collections.Generic;

namespace Envision.Tanks
{
    public class FakeTankEffect : ImpactEffect
    {
        private List<FakeTank> fakeTanks;

        public FakeTankEffect()
        {
            fakeTanks = new List<FakeTank>();
        }

        public override void TriggerEffect(GameObject sender)
        {
            fakeTanks.Add(new FakeTank(sender.position, sender.tag));
        }
    }
}