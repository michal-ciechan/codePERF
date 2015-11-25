namespace RxRequestResponse
{
    public struct Request
    {
        public int Value;

        public Request(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}