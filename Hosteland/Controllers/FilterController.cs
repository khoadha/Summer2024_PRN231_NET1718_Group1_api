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

        [HttpGet("furniture")]
        public async Task<IActionResult> GetOFurniture([FromQuery] string? filter = "", [FromQuery] string? orderBy = "", [FromQuery] int top = 0, [FromQuery] int skip = 0) {
            var uri = GetODataRequestUrl(ODataRequestType.FURNITURE, filter, orderBy, top, skip);
            var request = GetHttpRequestMessage(uri);
            var response = await GetODataCollectionResponse(request);
            return Ok(response.Value);
        }

        [HttpGet("room")]
        public async Task<IActionResult> GetORooms([FromQuery] string? filter = "", [FromQuery] string? orderBy = "", [FromQuery] int top = 0, [FromQuery] int skip = 0, [FromQuery] string? selectedDropdownValue= "") {
            var uri = GetODataRequestUrl(ODataRequestType.ROOM, filter, orderBy, top, skip, selectedDropdownValue);
            var request = GetHttpRequestMessage(uri);
            var response = await GetODataCollectionResponse(request);
            return Ok(response.Value);
        }

        [HttpGet("room-display")]
        public async Task<IActionResult> GetORoomsDisplay([FromQuery] string? filter = "", [FromQuery] string? orderBy = "", [FromQuery] int top = 0, [FromQuery] int skip = 0, [FromQuery] string? selectedDropdownValue = "") {
            var uri = GetODataRequestUrl(ODataRequestType.ROOM_DISPLAY, filter, orderBy, top, skip, selectedDropdownValue, true);
            var request = GetHttpRequestMessage(uri);
            var oDataResponse = await GetODataCollectionResponse(request);

            var response = new ODataPaginationResponse() {
                Total = oDataResponse.Count,
                Data = oDataResponse.Value
            };

            return Ok(response);
        }

        [HttpGet("room-categories")]
        public async Task<IActionResult> GetORoomCategories([FromQuery] string? filter = "", [FromQuery] string? orderBy = "", [FromQuery] int top = 0, [FromQuery] int skip = 0) {
            var uri = GetODataRequestUrl(ODataRequestType.ROOM_CATEGORY, filter, orderBy, top, skip);
            var request = GetHttpRequestMessage(uri);
            var response = await GetODataCollectionResponse(request);
            return Ok(response.Value);
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetOServices([FromQuery] string? filter = "", [FromQuery] string? orderBy = "", [FromQuery] int top = 0, [FromQuery] int skip = 0) {
            var uri = GetODataRequestUrl(ODataRequestType.SERVICE, filter, orderBy, top, skip);
            var request = GetHttpRequestMessage(uri);
            var response = await GetODataCollectionResponse(request);
            return Ok(response.Value);
        }

        [HttpGet("global-rates")]
        public async Task<IActionResult> GetOGlobalRates([FromQuery] string? filter = "", [FromQuery] string? orderBy = "", [FromQuery] int top = 0, [FromQuery] int skip = 0) {
            var uri = GetODataRequestUrl(ODataRequestType.GLOBAL_RATE, filter, orderBy, top, skip);
            var request = GetHttpRequestMessage(uri);
            var response = await GetODataCollectionResponse(request);
            return Ok(response.Value);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOOrders([FromQuery] string? filter = "", [FromQuery] string? orderBy = "", [FromQuery] int top = 0, [FromQuery] int skip = 0) {
            var uri = GetODataRequestUrl(ODataRequestType.ORDER, filter, orderBy, top, skip);
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

        private Uri GetODataRequestUrl(ODataRequestType type, string? filter = "", string? orderBy = "", int top = 0, int skip = 0, string? selectedDropdownValue = "", bool isPaging = false) {
            var result = ODATA_SERVICE_URL;
            string query = "?";

            if (isPaging)
                query += "count=true&";

            var filters = new List<string>();

            if (!string.IsNullOrEmpty(filter)) {
                filters.Add($"contains(tolower({GetEntityFieldForFilter(type)}), tolower('{Uri.EscapeDataString(filter)}'))");
            }

            if (!string.IsNullOrEmpty(selectedDropdownValue)) {
                filters.Add($"{GetEntityFieldForDropdownSelect(type)} eq '{Uri.EscapeDataString(selectedDropdownValue)}'");
            }

            if (filters.Count > 0) {
                query += $"$filter={string.Join(" and ", filters)}";
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

        private string GetEntityFieldForDropdownSelect(ODataRequestType type) {
            return type switch {
                //ODataRequestType.FURNITURE => "name",
                //ODataRequestType.GLOBAL_RATE => "OGlobalRates",
                //ODataRequestType.SERVICE => "name",
                //ODataRequestType.ROOM_CATEGORY => "categoryName",
                ODataRequestType.ROOM => "categoryName",
                ODataRequestType.ROOM_DISPLAY => "categoryName",
                //ODataRequestType.ORDER => "OOrders",
                _ => "",
            };
        }

        private string GetEntityFieldForFilter(ODataRequestType type) {
            return type switch {
                ODataRequestType.FURNITURE => "name",
                //ODataRequestType.GLOBAL_RATE => "OGlobalRates",
                ODataRequestType.SERVICE => "name",
                ODataRequestType.ROOM_CATEGORY => "categoryName",
                ODataRequestType.ROOM => "name",
                ODataRequestType.ROOM_DISPLAY => "name",
                //ODataRequestType.ORDER => "OOrders",
                _ => "",
            };
        }

        private string GetEntityPath(ODataRequestType type) {
            return type switch {
                ODataRequestType.FURNITURE => "OFurnitures",
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
