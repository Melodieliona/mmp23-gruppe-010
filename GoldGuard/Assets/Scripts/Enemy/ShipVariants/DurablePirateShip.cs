namespace Enemy.ShipVariants
{
    public class DurablePirateShip : PirateShipController
    {
        public DurablePirateShip() : base(40d, 1.5f, 25, 25)
        {
        }

        public void MultiplyHP(float factor)
        {
            SetHealth(GetHealth() * factor);
            SetMaxHealth(GetMaxHealth() * factor);
        }
    }
}