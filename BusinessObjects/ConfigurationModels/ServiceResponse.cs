using BusinessObjects.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ConfigurationModels {
    public class ServiceResponse<T> {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }

    public class ODataCollectionResponse {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }
        [JsonProperty("@odata.count")]
        public int Count { get; set; }
        public dynamic Value { get; set; }
    }

    public class ODataPaginationResponse {
        public int Total { get; set; }
        public dynamic Data { get; set; }
    }
}
