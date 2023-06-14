using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotesCRUD.Data.DbContexts;
using NotesCRUD.Data.Models;
using NotesCRUD.Data.Repository.Interface;
using NotesCRUD.Data.RequestModel;
using static Azure.Core.HttpHeader;

namespace NotesCRUD.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly NotesCRUDDbContext _context;
    private readonly INoteRepo _noteRepo;

    public NotesController(NotesCRUDDbContext context, INoteRepo noteRepo)
    {
        _context = context;
        _noteRepo = noteRepo;
    }


    [HttpGet]
    public Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var notes = _noteRepo.GetAllNotes(userId);
        return Task.FromResult<ActionResult<IEnumerable<Note>>>(Ok(notes));
    }


    [HttpGet("{id}")]
    public Task<ActionResult<Note>> GetNoteById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var note = _noteRepo.GetNoteById(id, userId);
        return Task.FromResult<ActionResult<Note>>(Ok(note));
    }

    [HttpPost]
    public Task<ActionResult<Note>> Create(Note note)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _noteRepo.CreateNote(note, userId);
        return Task.FromResult<ActionResult<Note>>(Ok(note));
    }

    [HttpPut]
    public Task<ActionResult<Note>> Update(Note note)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _noteRepo.UpdateNote(note, userId);
        return Task.FromResult<ActionResult<Note>>(Ok(note));
    }


    //[HttpGet("{id}")]
    //[Authorize]
    //public ActionResult DeleteNote(int id)
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    _noteRepo.DeleteNote(id, userId);
    //    return Ok("note deleted");
    //}
}

