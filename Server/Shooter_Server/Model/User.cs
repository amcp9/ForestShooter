using System;
namespace War_Server.Model
{
    public class User
    {
        public User(int id,string name,string pwd)
        {
            this.Id = id;
            this.Username = name;
            this.Password = pwd;
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
