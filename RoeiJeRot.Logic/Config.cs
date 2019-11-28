using Microsoft.Extensions.Configuration;

namespace RoeiJeRot.Logic.Config
{
    public interface IConfig
    {
        /// <summary>
        ///     Connection string to the database endpoint.
        /// </summary>
        string ConnectionString { get; }
    }

    public class Config : IConfig
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public string ConnectionString => _configuration["connectionString"];
    }
}