namespace Encryption_service.Helpers
{
    public static class KeyHelpers
    {
        public static string GenerateKey()
           => Guid.NewGuid().ToString();
    }
}
