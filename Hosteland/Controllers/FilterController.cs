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

        private Uri GetODataRequestUrl(ODataRequestType type) {

            //Not filter yet
            var result = ODATA_SERVICE_URL;
            return type switch {
                ODataRequestType.PRODUCT => new Uri($"{result}/OFurnitures"),
                ODataRequestType.GLOBAL_RATE => new Uri($"{result}/OGlobalRates"),
                ODataRequestType.SERVICE => new Uri($"{result}/OServices"),
                ODataRequestType.ROOM_CATEGORY => new Uri($"{result}/ORoomCategories"),
                ODataRequestType.ROOM => new Uri($"{result}/ORooms"),
                ODataRequestType.ROOM_DISPLAY => new Uri($"{result}/ORoomDisplays"),
                ODataRequestType.ORDER => new Uri($"{result}/OOrders"),
                _ => new Uri(""),
            };
        }
    }
}
