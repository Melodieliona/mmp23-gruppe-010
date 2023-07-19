namespace Defence
{
    public class LongCannonController : CannonController
    {
        protected override CannonStats GetBaseStats()
        {
            return new CannonStats(1,75, 7f, 1f, 400f);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            return new CannonStats(2,150, 7.5f, 1f, 400f);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            return new CannonStats(3,200, 8f, 1f, 400f);
        }
    }
}