using System.ComponentModel;

namespace MysensorListener.Settings
{
    public class NRF24State : BaseState
    {
        public NRF24State()
        {
            this.Listening = false;
        }

        [Description("Tells if the 'Listening...' string has been received")]
        public bool Listening { get; set; }
    }
}
