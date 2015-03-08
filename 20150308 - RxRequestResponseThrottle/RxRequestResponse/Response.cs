namespace RxRequestResponse
{
    public struct Response
    {
        public int Value;
        public Response(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}