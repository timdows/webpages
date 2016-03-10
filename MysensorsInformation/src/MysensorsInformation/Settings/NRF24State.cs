using System.ComponentModel;

namespace MysensorListener.Settings
{
    public class NRF24State : BaseState
    {
        public NRF24State()
        {
            this.Listening = false;
            this.RequestUploadConfiguration = false;
            this.RawReceviedDebug = string.Empty;
        }

        [Description("Tells if the 'Listening...' string has been received")]
        public bool Listening { get; set; }

        [Description("Requests to upload new configuration")]
        public bool RequestUploadConfiguration { get; set; }

        public string RawReceviedDebug { get; set; }
    }
}
