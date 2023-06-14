using System;
using NotesCRUD.Data.Models;

namespace NotesCRUD.Data.Repository.Interface
{
    public interface INoteRepo
    {
        Note CreateNote(Note note, string userId);
        Note GetNoteById(int id, string userId);
        List<Note> GetAllNotes(string userId);
        Note UpdateNote(Note note, string userId);
        void DeleteNote(int id, string userId);
    }
}