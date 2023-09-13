namespace Par.CommandCenter.Domain.Model
{
    /// <summary>
    /// A class represent the Ping Cloud Controller REST response. 
    /// </summary>
    public class PingCloudControllerResponse
    {
        public bool PingSucceeded { get; set; }

        /// <summary>
        /// The ping reply round trip time in ms
        /// </summary>
        public int PingReplyRoundTripTime { get; set; }

        public bool TcpTestSucceeded { get; set; }

        public string RemoteAddress { get; set; }

        public string RemotePort { get; set; }


        public string Message { get; set; }
    }
}
