using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace RoeiJeRot.Logic.Config
{
    public interface IConfig
    {
        string ConnectionString { get; }
    }

    public class Config : IConfig
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration["connectionString"];
    }
}
