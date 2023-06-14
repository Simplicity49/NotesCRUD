using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NotesCRUD.Data.DbContexts;
using NotesCRUD.Data.Models;
using NotesCRUD.Data.Repository.Interface;

namespace NotesCRUD.Data.Repository
{
    public class NoteRepo : INoteRepo
    {
        private readonly NotesCRUDDbContext _context;

        public NoteRepo(NotesCRUDDbContext context)
        {
            _context = context;
        }

        public Note CreateNote(Note note, string userId)
        {
            note.UserId = userId;
            _context.Add(note);
            _context.SaveChanges();
            return note;
        }

        public Note GetNoteById(int id, string userId)
        {
            return _context.Note.FirstOrDefault(c => c.UserId == userId && c.NoteId == id);
        }

        public List<Note> GetAllNotes(string userId)
        {
            return _context.Note.Where(c => c.UserId == userId).ToList();
        }

        public Note UpdateNote(Note note, string userId)
        {
            note.UserId = userId;
            _context.Update(note);
            _context.SaveChanges();
            return note;
        }

        public void DeleteNote(int id, string userId)
        {
            var note = _context.Note.FirstOrDefault(c => c.NoteId == id && c.UserId == userId);
            if (note != null)
            {
                _context.Remove(note);
                _context.SaveChanges();
            }
        }

        
    }
}
