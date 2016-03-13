using System.ComponentModel;

namespace MysensorListener.Settings
{
    public class NRF24State : BaseState
    {
        public NRF24State()
        {
            this.RequestUploadConfiguration = false;
        }

        [Description("Requests to upload new configuration")]
        public bool RequestUploadConfiguration { get; set; }
    }
}
