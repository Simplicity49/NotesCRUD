using System;
using NotesCRUD.Data.Models;
using NotesCRUD.Data.RequestModel;

namespace NotesCRUD.Data.Repository.Interface
{
	public interface IAuthRepo
	{
            Task<string> Authenticate(string username, string password);
            Task<AppUser> CreateAsync(Register user);
    }
}

