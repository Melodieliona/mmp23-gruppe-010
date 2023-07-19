namespace Defence
{
    public class HeavyCannonController : CannonController
    {
        protected override CannonStats GetBaseStats()
        {
            return new CannonStats(1, 125, 4f, 0.8f, 350);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            return new CannonStats(2, 175, 4f, 0.9f, 375);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            return new CannonStats(3, 250, 4.5f, 1f, 400);
        }
    }
}