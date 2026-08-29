namespace DAT_Lesson04_Lab.Models
{
    public class DATAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Bio { get; set; }
        public int Gender { get; set; } // 1: Nam, 0: Nữ
        public DateTime Birthday { get; set; }
    }
}
