

using Newtonsoft.Json;

namespace door_watcher
{
    public class StatusMessage
    {
        public StatusMessage(string message)
        {
            this.Message = message;
        }

        [JsonProperty(PropertyName="message")]
        public string Message { get; set; }
    }
}
