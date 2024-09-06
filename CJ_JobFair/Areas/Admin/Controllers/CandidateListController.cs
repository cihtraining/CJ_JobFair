using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;
using dto = CJ_JobFair.Areas.Admin.Models;
namespace CJ_JobFair.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CandidateListController : Controller
    {
        private MySqlDatabase MySqlDatabase { get; set; }

        public CandidateListController(MySqlDatabase mySqlDatabase)
        {

            this.MySqlDatabase = mySqlDatabase;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DisplayCadidateList()
        {

            return View("DisplayCadidateList", await this.SelectCadidateList());
        }
        private async Task<List<dto.RegistrationModel>> SelectCadidateList()
        {
            var ret = new List<dto.RegistrationModel>();

            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT registration.*, event.event_name  FROM registration inner join event on registration.event_id=event.id ";
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.RegistrationModel()
                    {
                        id = reader.GetFieldValue<int>(0),
                        name = reader.IsDBNull(1) ? "NA" : reader.GetFieldValue<string>(1),
                        mobile = reader.IsDBNull(2) ? "NA" : reader.GetFieldValue<string>(2),
                        whatsapp = reader.IsDBNull(3) ? "NA" : reader.GetFieldValue<string>(3),
                        email = reader.IsDBNull(4) ? "NA" : reader.GetFieldValue<string>(4),
                        place = reader.IsDBNull(5) ? "NA" : reader.GetFieldValue<string>(5),
                        taluk = reader.IsDBNull(6) ? "NA" : reader.GetFieldValue<string>(6),
                        district = reader.IsDBNull(7) ? "NA" : reader.GetFieldValue<string>(7),
                        qualification = reader.IsDBNull(8) ? "NA" : reader.GetFieldValue<string>(8),
                        event_name = reader.IsDBNull(9) ? "NA" : reader.GetFieldValue<string>(11),
                    };

                    ret.Add(t);
                }
            return ret;
        }
        public IActionResult ViewCandidateDetails()
        {
            eventlist();
            return View();

        }

        public async Task<IActionResult> GetCandidateDetails(int id)
        {
            var ret = new dto.RegistrationModel();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT *   FROM registration  where id=" + id;
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    ret.id = reader.GetFieldValue<int>(0);
                    ret.name = reader.GetFieldValue<string>(1);
                    ret.mobile = reader.GetFieldValue<string>(2);
                    ret.whatsapp = reader.GetFieldValue<string>(3);
                    ret.email = reader.GetFieldValue<string>(4);
                    ret.place = reader.GetFieldValue<string>(5);
                    ret.taluk = reader.GetFieldValue<string>(6);
                    ret.district = reader.GetFieldValue<string>(7);
                    ret.qualification = reader.GetFieldValue<string>(8);
                    ret.event_id = reader.GetFieldValue<int>(9);
                }
            eventlist();
            return View("ViewCandidateDetails", ret);
        }
        public async Task<IActionResult> UpdateCandidateDetails(dto.RegistrationModel obj)
        {
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            if (obj.id > 0)
            {
                cmd.CommandText = @"update registration set name=@name,mobile=@mobile,whatsapp=@whatsapp,email=@email,place=@place,taluk=@taluk,district=@district,qualification=@qualification,addedon=now(),event_id=@event where id=@id";
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@mobile", obj.mobile);
                cmd.Parameters.AddWithValue("@whatsapp", obj.whatsapp);
                cmd.Parameters.AddWithValue("@email", obj.email);
                cmd.Parameters.AddWithValue("@place", obj.place);
                cmd.Parameters.AddWithValue("@taluk", obj.taluk);
                cmd.Parameters.AddWithValue("@district", obj.district);
                cmd.Parameters.AddWithValue("@qualification", obj.qualification);
                cmd.Parameters.AddWithValue("@event", obj.event_id);
                cmd.Parameters.AddWithValue("@id", obj.id);
                var recs = cmd.ExecuteNonQuery();
            }
            eventlist();
            return View("ViewCandidateDetails");
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

        public IActionResult PrintCandidateData()
        {
            return View();
        }
        public IActionResult Printpage()
        {
            return View();
        }
        [HttpPost]

        private async Task<List<dto.RegistrationModel>> GetCandidateDetailToPrint(int id)
        {
            var ret = new List<dto.RegistrationModel>();

            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT *   FROM registration where id=" + id;
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.RegistrationModel()
                    {
                        id = reader.GetFieldValue<int>(0),
                        name = reader.IsDBNull(1) ? "NA" : reader.GetFieldValue<string>(1),
                        mobile = reader.IsDBNull(2) ? "NA" : reader.GetFieldValue<string>(2),
                        whatsapp = reader.IsDBNull(3) ? "NA" : reader.GetFieldValue<string>(3),
                        email = reader.IsDBNull(4) ? "NA" : reader.GetFieldValue<string>(4),
                        place = reader.IsDBNull(5) ? "NA" : reader.GetFieldValue<string>(5),
                        taluk = reader.IsDBNull(6) ? "NA" : reader.GetFieldValue<string>(6),
                        district = reader.IsDBNull(7) ? "NA" : reader.GetFieldValue<string>(7),
                        qualification = reader.IsDBNull(8) ? "NA" : reader.GetFieldValue<string>(8),
                    };

                    ret.Add(t);
                }
            return ret;
        }
        public async Task<IActionResult> PrintedCadidateList(int id)
        {

            return View("PrintedCadidateList", await this.GetCandidateDetailToPrint(id));
        }

        public async Task<IActionResult> SelectedlistToPrint(string list)
        {

            return View("PrintedCadidateList", await this.SelectedlistToPrints(list));
        }

        private async Task<List<dto.RegistrationModel>> SelectedlistToPrints(string list)
        {
            var ret = new List<dto.RegistrationModel>();
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT *   FROM registration where FIND_IN_SET(id, '" + list + "')";
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.RegistrationModel()
                    {
                        id = reader.GetFieldValue<int>(0),
                        name = reader.IsDBNull(1) ? "NA" : reader.GetFieldValue<string>(1),
                        mobile = reader.IsDBNull(2) ? "NA" : reader.GetFieldValue<string>(2),
                        whatsapp = reader.IsDBNull(3) ? "NA" : reader.GetFieldValue<string>(3),
                        email = reader.IsDBNull(4) ? "NA" : reader.GetFieldValue<string>(4),
                        place = reader.IsDBNull(5) ? "NA" : reader.GetFieldValue<string>(5),
                        taluk = reader.IsDBNull(6) ? "NA" : reader.GetFieldValue<string>(6),
                        district = reader.IsDBNull(7) ? "NA" : reader.GetFieldValue<string>(7),
                        qualification = reader.IsDBNull(8) ? "NA" : reader.GetFieldValue<string>(8),
                    };

                    ret.Add(t);
                }
            return ret;
        }
    }
}
