using System.ComponentModel;

namespace MysensorListener.Settings
{
    public class BaseState
    {
        public BaseState()
        {
            this.Started = false;
            this.CountOfReceivedMessages = 0;
        }

        [Description("Tells if the capture has started")]
        public bool Started { get; set; }

        [Description("The total messages received since started")]
        public int CountOfReceivedMessages { get; set; }
    }
}
