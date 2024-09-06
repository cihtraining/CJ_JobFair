using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;
using System.Runtime.Intrinsics.Arm;
using dto = CJ_JobFair.Models;

namespace CJ_JobFair.Controllers
{
    public class RegistrationController : Controller
    {
        private MySqlDatabase MySqlDatabase { get; set; }

        public RegistrationController(MySqlDatabase mySqlDatabase)
        {

            this.MySqlDatabase = mySqlDatabase;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegistrationForm()
        {
            eventlist();
            return View();
        }
        public async Task<IActionResult> SaveCandidate(dto.RegistrationModel obj)
        {
                var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO registration(name,mobile,whatsapp,email,place,taluk,district,qualification,addedon,event_id) VALUES (@name,@mobile,@whatsapp,@email,@place,@taluk,@district,@qualification,now(),@event)";
                cmd.Parameters.AddWithValue("@name",obj.name);
                cmd.Parameters.AddWithValue("@mobile", obj.mobile);
                cmd.Parameters.AddWithValue("@whatsapp", obj.whatsapp);
                cmd.Parameters.AddWithValue("@place", obj.place);
                cmd.Parameters.AddWithValue("@email", obj.email);
                cmd.Parameters.AddWithValue("@taluk", obj.taluk);
                cmd.Parameters.AddWithValue("@district", obj.district);
                cmd.Parameters.AddWithValue("@qualification", obj.qualification);
                cmd.Parameters.AddWithValue("@event", obj.event_id);
            var recs = cmd.ExecuteNonQuery();
                return View("RegistrationForm");

        }

        public List<SelectListItem> eventlist()
        {
            List<SelectListItem> list = displayEvents().Result.Select(x => new SelectListItem
            {
                Text = x.event_name,
                Value = x.id.ToString()
            }).ToList();
            ViewBag.eventList = list;
            return list;

        }
        private async Task<List<dto.EventModel>> displayEvents()
        {
            var ret = new List<dto.EventModel>();

            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM event where date(date)>= CURRENT_DATE()";

            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.EventModel()
                    {

                        event_name = reader.GetFieldValue<string>(1),
                         id = reader.GetFieldValue<int>(0),

                    };

                    ret.Add(t);
                }
            return ret;

        }

    }
}
