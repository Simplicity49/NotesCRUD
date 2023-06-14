using System;
namespace NotesCRUD.Data.RequestModel
{
	public class Login
	{
		public string username { get; set; }
		public string password { get; set; }
	}

    public class Register
    {
        public string username { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
    }
}

