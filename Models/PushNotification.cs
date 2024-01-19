using Newtonsoft.Json;

namespace Models
{
    public class PushNotification
    {
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";

        [JsonProperty("data")]
        public DataPayload Data { get; set; }

        [JsonProperty("notification")]
        public DataPayload Notification { get; set; }
    }

    public class DataPayload
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}