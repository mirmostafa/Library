namespace Mohammad.CqrsInfra.CommandInfra
{
    public class Nothing
    {
        public static Nothing Instance { get; } = new Nothing();

        private Nothing()
        {
        }

        public override string ToString() => string.Empty;
    }
}