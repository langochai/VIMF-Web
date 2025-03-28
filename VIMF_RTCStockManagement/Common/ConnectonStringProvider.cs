namespace VIMF_RTCStockManagement.Common
{
    public sealed class ConnectionStringProvider
    {
        private readonly IConfiguration _configuration;
        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string DefaultConnectionString => _configuration.GetConnectionString("DefaultConnection") ?? "";
    }
}
