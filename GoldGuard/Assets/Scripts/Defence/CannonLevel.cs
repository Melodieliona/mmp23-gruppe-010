using System;

namespace Defence
{
    public class CannonLevel
    {
        private readonly int level;
        private readonly int cost;
        private readonly float range;
        private readonly float bps;
        private readonly float damage;

        public CannonLevel(int level, int cost, float range, float bps, float damage)
        {
            this.level = level;
            this.cost = cost;
            this.range = range;
            this.bps = bps;
            this.damage = damage;
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

        public float GetDamage()
        {
            return damage;
        }
        
        public string GetLevelText(CannonLevel currentLevel)
        {
            return $"{currentLevel.GetLevel()}<sprite name=\"arrow\"> {level}";
        }
        
        public string GetUpgradeRangeText(CannonLevel currentLevel)
        {
            return $"Range: {currentLevel.GetRange()}<sprite name=\"arrow\"> {range}";
        }
        
        public string GetUpgradeDamageText(CannonLevel currentLevel)
        {
            return $"Damage: {currentLevel.GetDamage()}<sprite name=\"arrow\"> {damage}";
        }
        
        public string GetBpsDamageText(CannonLevel currentLevel)
        {
            return $"BPS: {currentLevel.GetBps()}<sprite name=\"arrow\"> {bps}";
        }
        
        public override string ToString()
        {
            return "Level: " + level + ", Range: " + range + ", BPS: " + bps + ", Damage: " + damage;
        }
    }
}