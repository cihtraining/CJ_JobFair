using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.PointsToAnalysis;

namespace CJ_JobFair.Areas.Admin.Models
{
    public class RegistrationModel
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? mobile { get; set; }
        public string? whatsapp { get; set; }
        public string?   email { get; set; }
        public string? place { get; set; }
        public string? taluk { get; set; }
        public string? district { get; set; }
        public string? qualification { get; set; }
        public int? event_id { get; set; }
        public string? addedon { get; set; }
        public string? selectedid { get; set; }
        public string event_name { get; set; }
        public EventModel EventModel { get; set; }
    }
}
