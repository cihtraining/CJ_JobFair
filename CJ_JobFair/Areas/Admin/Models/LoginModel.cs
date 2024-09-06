namespace CJ_JobFair.Areas.Admin.Models
{
    public class LoginModel
    {
        public int admin_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int rememberme { get; set; }
    }
}
