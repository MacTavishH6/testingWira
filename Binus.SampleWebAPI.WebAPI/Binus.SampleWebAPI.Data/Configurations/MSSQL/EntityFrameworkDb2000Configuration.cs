using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.SqlClient;
using System.Data.Common;

namespace Binus.SampleWebAPI.Data.Configurations
{
    internal sealed class EntityFrameworkDb2000Configuration : DbConfiguration
    {
        /// <summary>
        /// The provider manifest token to use for SQL Server.
        /// </summary>
        private const string SqlServerManifestToken = @"2000";

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDbConfiguration"/> class.
        /// </summary>
        public EntityFrameworkDb2000Configuration()
        {
            this.AddDependencyResolver(new SingletonDependencyResolver<IManifestTokenResolver>(new ManifestTokenService()));
        }

        /// <inheritdoc />
        private sealed class ManifestTokenService : IManifestTokenResolver
        {
            /// <summary>
            /// The default token resolver.
            /// </summary>
            private static readonly IManifestTokenResolver DefaultManifestTokenResolver = new DefaultManifestTokenResolver();

            /// <inheritdoc />
            public string ResolveManifestToken(DbConnection connection)
            {
                if (connection is SqlConnection)
                {
                    return SqlServerManifestToken;
                }

                return DefaultManifestTokenResolver.ResolveManifestToken(connection);
            }
        }
    }
}
