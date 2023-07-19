namespace Defence
{
    public class CannonStats
    {
        private readonly int level;
        private readonly int cost;
        private readonly float range;
        private readonly float bps;
        private readonly float rotationSpeed;

        public CannonStats(int level, int cost, float range, float bps, float rotationSpeed)
        {
            this.level = level;
            this.cost = cost;
            this.range = range;
            this.bps = bps;
            this.rotationSpeed = rotationSpeed;
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetCost()
        {
            return cost;
        }

        public float GetRange()
        {
            return range;
        }

        public float GetBps()
        {
            return bps;
        }

        public float GetRotationSpeed()
        {
            return rotationSpeed;
        }

        public override string ToString()
        {
            return "Level: " + level + ", Range: " + range + ", BPS: " + bps + ", RotationSpeed: " + rotationSpeed;
        }
    }
}