namespace Enemy.ShipVariants
{
    public class NormalPirateShip : PirateShipController
    {
        public NormalPirateShip() : base(20d, 2f, 10, 10)
        {
        }
        public void MultiplyHP(float factor)
        {
            SetHealth(GetHealth() * factor);
            SetMaxHealth(GetMaxHealth() * factor);
        }
    }
}