namespace DAT_Lesson07_Model.DataModel
{
    public class DAT_Member
    {
        public string DAT_MemberID { get; set; }
        public string DAT_Username { get; set; }
        public string DAT_Password { get; set; }
        public string DAT_FullName { get; set; }
        public string DAT_Email { get; set; }

        // BẮT BUỘC: Constructor không tham số dành cho Model Binder
        public DAT_Member()
        {
        }

        // Constructor có tham số (nếu có)
        public DAT_Member(string id, string username, string password, string fullname, string email)
        {
            DAT_MemberID = id;
            DAT_Username = username;
            DAT_Password = password;
            DAT_FullName = fullname;
            DAT_Email = email;
        }
    }
}