namespace MoqContainerTests
{
    public class SomeClass
    {
        private readonly IDepencyA _a;
        private readonly IDepencyB _b;

        public SomeClass(IDepencyA a, IDepencyB b)
        {
            _a = a;
            _b = b;
        }

        public void CallA()
        {
            _a.Call();
        }

        public void CallB()
        {
            _b.Call();
        }
    }
}