namespace QuoVadis.Common.Exceptions
{
    [GenerateSerializer]
    [Serializable]
	public class DomainException : Exception
	{
        public override string Message => base.Message;

        public DomainException() { }
	}
}
