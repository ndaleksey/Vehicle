namespace Swsu.BattleFieldMonitor.Protocol
{
    public class InvalidPayloadSizeException : ProtocolException
    {
        #region Constructors
        public InvalidPayloadSizeException(int payloadSize)
        {
            PayloadSize = payloadSize;
        }
        #endregion

        #region Properties
        public int PayloadSize
        {
            get;
        }
        #endregion
    }
}
