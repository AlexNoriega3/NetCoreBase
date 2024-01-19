using Newtonsoft.Json;

namespace Models.DTOs
{
    public class PushNotificationDto
    {
        [JsonProperty("tocken")]
        public string Token { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("isSingleAndroiodDevice")]
        public bool IsSingleAndroiodDevice { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}