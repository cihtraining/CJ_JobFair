using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using dto = CJ_JobFair.Areas.Admin.Models;
namespace CJ_JobFair.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/Event/[action]/{id?}")]
    public class EventController : Controller
    {
        private MySqlDatabase MySqlDatabase { get; set; }

        public EventController(MySqlDatabase mySqlDatabase)
        {

            this.MySqlDatabase = mySqlDatabase;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EventRegisration()
        {
            return View();

        }

        public async Task<IActionResult> SaveEvent(dto.EventModel obj)
        {
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            if (obj.id > 0)
            {
                cmd.CommandText = @"update event set event_name='"+ obj.event_name + "',date='"+ obj.date + "',location='"+ obj.location + "' where id='"+ obj.id + "'";
                var recs = cmd.ExecuteNonQuery();
            }
            else
            {
                cmd.CommandText = @"INSERT INTO event(event_name,date,location) VALUES ('"+ obj.event_name + "','"+ obj.date + "','"+ obj.location + "')";
                var recs = cmd.ExecuteNonQuery();
                ModelState.Clear();
            }
                    
            return View("EventRegisration");

        }
        public async Task<IActionResult> DisplayEventList()
        {
            return View("DisplayEventList", await this.GetEventList());
        }

        private async Task<List<dto.EventModel>> GetEventList()

        {
            var ret = new List<dto.EventModel>();

            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT *   FROM event ";

            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.EventModel()
                    {
                        id = reader.GetFieldValue<int>(0),
                        event_name = reader.IsDBNull(1) ? "NA" : reader.GetFieldValue<string>(1),
                        date = reader.IsDBNull(2) ? "NA" : reader.GetFieldValue<string>(2),
                        location = reader.IsDBNull(3) ? "NA" : reader.GetFieldValue<string>(3),
                    };

                    ret.Add(t);
                }
            return ret;
        }
        //[HttpGet]
        //[Route("admin/Event/EventDetailsToEdit/{id?}")]
        public async Task<IActionResult> EventDetailsToEdit(int id)
        {
            var ret = new dto.EventModel();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT *   FROM event  where id=" + id;

            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {

                    ret.id = reader.GetFieldValue<int>(0);
                    ret.event_name = reader.GetFieldValue<string>(1);
                    ret.date = reader.GetFieldValue<string>(2);
                    ret.location = reader.GetFieldValue<string>(3);
                }
            return View("EventRegisration", ret);
          

        }
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ret = new dto.EventModel();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"delete from event  where id=" + id;
            var recs = cmd.ExecuteNonQuery();
            return View("DisplayEventList", await this.GetEventList());

        }
    }
}
