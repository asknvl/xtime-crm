using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace crm.Models.api.socket
{
    public class creativeChangedDTO
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
        [JsonPropertyName("filename")]
        public string filename { get; set; }
        [JsonPropertyName("filepath")]
        public string filepath { get; set; }
        [JsonPropertyName("uploaded")]
        public bool uploaded { get; set; }
        [JsonPropertyName("visibility")]
        public bool visibility { get; set; }
        [JsonPropertyName("enabled")]
        public bool enabled { get; set; }
    }
}


//{
//    "id": "1",
//    "name": "COL X CLEAR 1",
//    "filename": "test2",
//    "filepath": "/COL X Clear/videos/COL X Clear 1",
//    "uploaded": "true",
//    "visibility": "true",
//    "enabled": "true"
//}