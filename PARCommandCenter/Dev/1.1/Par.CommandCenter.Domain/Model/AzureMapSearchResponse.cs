// This class was copied as it is from Par.Command
using System.Collections.Generic;

namespace Par.CommandCenter.Domain.Model
{
    public class AzureMapSearchResponse
    {
        public IEnumerable<Result> Results { get; set; }

        public object Summary { get; set; }

        public class Result
        {
            public Position Position { get; set; }
        }

        public class Position
        {
            public double Lat { get; set; }

            public double Lon { get; set; }
        }
    }
}
