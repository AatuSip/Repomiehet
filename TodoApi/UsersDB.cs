namespace UserDB
{
    public class UserRegister
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public decimal Money { get; set; }
    }

    public class UserLogin
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}


