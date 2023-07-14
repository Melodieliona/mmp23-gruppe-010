namespace Enemy.ShipVariants
{
    public class FastPirateShip : PirateShipController
    {
        public FastPirateShip() : base(10d, 4f, 20, 20)
        {
        }

        public void MultiplyHP(float factor)
        {
            SetHealth(GetHealth() * factor);
            SetMaxHealth(GetMaxHealth() * factor);
        }
    }
}