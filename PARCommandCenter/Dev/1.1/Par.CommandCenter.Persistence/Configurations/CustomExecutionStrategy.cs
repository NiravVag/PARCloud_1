using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.SqlClient;

namespace Par.CommandCenter.Persistence.Configurations
{
    public class CustomExecutionStrategy : ExecutionStrategy
    {
        public CustomExecutionStrategy(DbContext context) : base(context, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
        {

        }

        public CustomExecutionStrategy(ExecutionStrategyDependencies dependencies) : base(dependencies, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
        {

        }

        public CustomExecutionStrategy(DbContext context, int maxRetryCount, TimeSpan maxRetryDelay) :
            base(context, maxRetryCount, maxRetryDelay)
        {

        }

        protected override bool ShouldRetryOn(Exception ex)
        {
            if (ex is SqlException sqlex)
            {
                foreach (SqlError err in sqlex.Errors)
                {
                    // Error number found here https://github.com/dotnet/SqlClient/issues/617#issuecomment-649686544
                    if (err.Number == 18456)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }


}
