using BusinessObjects.ConfigurationModels;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hosteland.Controllers {

    [Route("api/v1/filter")]
    [ApiController]
    public class FilterController : ControllerBase {

        private readonly IHttpClientFactory _clientFactory;
        private static readonly IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        private readonly string ODATA_SERVICE_URL = config["ODataService:BaseUrl"];

        public FilterController(IHttpClientFactory clientFactory) {
            _clientFactory = clientFactory;
        }


        [HttpGet("product")]
        public async Task<IActionResult> GetOProducts() {

            var uri = GetODataRequestUrl(ODataRequestType.PRODUCT);

            var request = GetHttpRequestMessage(uri);

            var response = await GetODataCollectionResponse(request);

            return Ok(response.Value);
        }

        [HttpGet("room")]
        public async Task<IActionResult> GetORooms() {

            var uri = GetODataRequestUrl(ODataRequestType.ROOM);

            var request = GetHttpRequestMessage(uri);

            var response = await GetODataCollectionResponse(request);

            return Ok(response.Value);
        }

        [HttpGet("room-display")]
        public async Task<IActionResult> GetORoomsDisplay() {

            var uri = GetODataRequestUrl(ODataRequestType.ROOM_DISPLAY);

            var request = GetHttpRequestMessage(uri);

            var response = await GetODataCollectionResponse(request);

            return Ok(response.Value);
        }

        [HttpGet("room-categories")]
        public async Task<IActionResult> GetORoomCategories() {

            var uri = GetODataRequestUrl(ODataRequestType.ROOM_CATEGORY);

            var request = GetHttpRequestMessage(uri);

            var response = await GetODataCollectionResponse(request);

            return Ok(response.Value);
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetOServices() {

            var uri = GetODataRequestUrl(ODataRequestType.SERVICE);

            var request = GetHttpRequestMessage(uri);

            var response = await GetODataCollectionResponse(request);

            return Ok(response.Value);
        }

        [HttpGet("global-rates")]
        public async Task<IActionResult> GetOGlobalRates() {

            var uri = GetODataRequestUrl(ODataRequestType.GLOBAL_RATE);

            var request = GetHttpRequestMessage(uri);

            var response = await GetODataCollectionResponse(request);

            return Ok(response.Value);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOOrders() {

            var uri = GetODataRequestUrl(ODataRequestType.ORDER);

            var request = GetHttpRequestMessage(uri);

            var response = await GetODataCollectionResponse(request);

            return Ok(response.Value);
        }

        private HttpRequestMessage GetHttpRequestMessage(Uri requestUri) {
            return new HttpRequestMessage {
                RequestUri = requestUri,
                Method = HttpMethod.Get,
            };
        }

        private async Task<ODataCollectionResponse> GetODataCollectionResponse(HttpRequestMessage request) {
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            string result = await response.Content.ReadAsStringAsync();
            var searchResponse = JsonConvert.DeserializeObject<ODataCollectionResponse>(result);
            return searchResponse;
        }

        private Uri GetODataRequestUrl(ODataRequestType type, string filter = "", string orderBy = "", int top = 0, int skip = 0) {
            var result = ODATA_SERVICE_URL;
            string query = "";

            if (!string.IsNullOrEmpty(filter)) {
                query += $"?$filter={Uri.EscapeDataString(filter)}";
            }

            if (!string.IsNullOrEmpty(orderBy)) {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += $"$orderby={Uri.EscapeDataString(orderBy)}";
            }

            if (top > 0) {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += $"$top={top}";
            }

            if (skip > 0) {
                query += string.IsNullOrEmpty(query) ? "?" : "&";
                query += $"$skip={skip}";
            }

            return new Uri($"{result}/{GetEntityPath(type)}{query}");
        }
        private string GetEntityPath(ODataRequestType type) {
            return type switch {
                ODataRequestType.PRODUCT => "OFurnitures",
                ODataRequestType.GLOBAL_RATE => "OGlobalRates",
                ODataRequestType.SERVICE => "OServices",
                ODataRequestType.ROOM_CATEGORY => "ORoomCategories",
                ODataRequestType.ROOM => "ORooms",
                ODataRequestType.ROOM_DISPLAY => "ORoomDisplays",
                ODataRequestType.ORDER => "OOrders",
                _ => "",
            };
        }

    }
}
