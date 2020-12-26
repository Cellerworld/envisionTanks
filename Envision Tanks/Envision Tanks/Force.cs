
namespace Envision.Tanks.Math
{
    public class Force
    {
        public Vector2 dir { get; set; }
        public float magnitude { get; set; }
        public float decline { get; private set; }

        public Force(Vector2 dir, float magnitude, float decline)
        {
            this.dir = dir.Normalized;
            this.magnitude = magnitude;
            this.decline = System.Math.Min(decline, 0.99f); ;
        }

        //every time the force is applied it needs to decline because energy is used. Simplified
        public Vector2 UseForce()
        {
            Vector2 currentForce = dir * magnitude;
            UpdateForce();
            return currentForce;
        }

        private void UpdateForce()
        {
            magnitude = magnitude * decline;
        }
    }
}