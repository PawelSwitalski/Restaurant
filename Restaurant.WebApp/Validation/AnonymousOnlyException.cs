namespace Restaurant.WebApp.Validation
{
    public class AnonymousOnlyException : Exception
    {
        public AnonymousOnlyException() { }

        public AnonymousOnlyException(string message) : base(message) { }

        public AnonymousOnlyException(string message, Exception innerException) : base(message, innerException) { }

    }
}
