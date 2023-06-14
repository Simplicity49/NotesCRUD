using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NotesCRUD.Data.Models
{
	public class BaseEntity
	{
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
		public string UserCreated { get; set; }
		public string UserUpdated { get; set; }
	}

	public class User : BaseEntity
	{
		[Key]
		public int UserId { get; set; }
		public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

	public class Note : BaseEntity
    {
        [Key]
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
		public int UserId { get; set; }
        public User User { get; set; }
    }
}

