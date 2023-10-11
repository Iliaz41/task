namespace task.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "404, sorry") : base(message)
        {

        }
    }
}
