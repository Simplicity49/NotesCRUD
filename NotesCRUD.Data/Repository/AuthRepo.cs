using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NotesCRUD.Data.DbContexts;
using NotesCRUD.Data.Models;
using NotesCRUD.Data.Repository.Interface;
using NotesCRUD.Data.RequestModel;

namespace NotesCRUD.Data.Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly NotesCRUDDbContext _context;

        public AuthRepo(NotesCRUDDbContext context)
        {
            _context = context;
        }

        public string Authenticate(string username, string password)
        {
            var user = _context.User.FirstOrDefault(c => c.Username == username);
            if(user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return user.Username;
                }
            }
            return "invalid user";
        }

        public User Create(Register reg)
        {
            var user = new User
            {
                Username = reg.username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(reg.password)
            };
            _context.Add(user);
            _context.SaveChanges();
            return user;
        }

    }
}
