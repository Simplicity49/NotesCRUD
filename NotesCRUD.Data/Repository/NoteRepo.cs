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
            note.UserId = _context.User.FirstOrDefault(c => c.Username == userId).UserId;
            _context.Add(note);
            _context.SaveChanges();
            return note;
        }

        public Note GetNoteById(int id, string userId)
        {
            return _context.Note.Include(c => c.User).FirstOrDefault(c => c.User.Username == userId);
        }

        public List<Note> GetAllNotes(string userId)
        {
            return _context.Note.Include(c => c.User).Where(c => c.User.Username == userId).ToList();
        }

        public Note UpdateNote(Note note, string userId)
        {
            var prevNote = _context.Note.FirstOrDefault(c => c.NoteId == note.NoteId);
            if(prevNote.User.Username == userId)
            {
                _context.Update(note);
                _context.SaveChanges();
            }
            return note;
        }

        public void DeleteNote(int id, string userId)
        {
            var note = _context.Note.FirstOrDefault(c => c.NoteId == id && c.User.Username == userId);
            if (note != null)
            {
                _context.Remove(note);
                _context.SaveChanges();
            }
        }

        
    }
}
