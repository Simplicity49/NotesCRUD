using System;
using NotesCRUD.Data.Models;
using NotesCRUD.Data.RequestModel;

namespace NotesCRUD.Data.Repository.Interface
{
	public interface IAuthRepo
	{
            string Authenticate(string username, string password);
            User Create(Register user);
    }
}

