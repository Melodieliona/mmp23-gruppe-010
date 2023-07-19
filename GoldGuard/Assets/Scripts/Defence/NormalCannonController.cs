namespace Defence
{
    public class NormalCannonController : CannonController
    {
        protected override CannonStats GetBaseStats()
        {
            return new CannonStats(1, 50, 5f, 1f, 400f);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            return new CannonStats(2, 100, 5f, 1.5f, 450f);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            return new CannonStats(3, 150, 5f, 2f, 500f);
        }
    }
}