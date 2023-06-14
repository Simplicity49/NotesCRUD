using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthRepo(NotesCRUDDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var signIn = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (signIn.Succeeded)
            {
                return username;
            }
            return "invalid user";
        }

        public async Task<AppUser> CreateAsync(Register reg)
        {
            var user = new AppUser
            {
                UserName = reg.username,
                Email = reg.Email
            };
            var createUser = await _userManager.CreateAsync(user, reg.password);
            if (createUser.Succeeded)
            {
                return user;
            }
            var errors = string.Join(", ", createUser.Errors.Select(e => e.Description));
            throw new Exception($"Failed to create user: {errors}");
        }

    }
}
