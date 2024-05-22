using BusinessObjects.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Hosteland.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase {

        List<Room> listRoom = new List<Room>() {
                new Room {
                    Id=1,
                    Name= "Room1"
                },
                new Room {
                    Id=2,
                    Name= "Room2"
                },
                new Room {
                    Id=3,
                    Name= "Room3"
                },
                new Room {
                    Id=4,
                    Name= "Room4"
                },
        };

        public RoomController() {

        }


        [EnableQuery(PageSize = 3)]
        [HttpGet]
        public IQueryable<Room> Get() {
            return listRoom.AsQueryable();
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public SingleResult<Room> Get([FromODataUri] int key) {
            var data = listRoom.Where(a => a.Id == key).AsQueryable();
            return SingleResult.Create(data);
        }

    }
}
