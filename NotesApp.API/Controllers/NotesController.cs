using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.API.Data;
using NotesApp.API.Models.DomainModels;
using NotesApp.API.Models.DTO;

namespace NotesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesDbContext dbContext;
        public NotesController(NotesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult AddNote(AddNoteRequest addNoteRequest)
        {
            var note = new Models.DomainModels.Note
            {
                Title = addNoteRequest.Title,
                Description = addNoteRequest.Description,
                ColorHex = addNoteRequest.ColorHex,
                DateCreated = DateTime.Now
            };

            dbContext.Notes.Add(note);
            dbContext.SaveChanges();

            return Ok(note);
        }

        [HttpGet]
        public IActionResult GetAllNote()
        {
            var notes = dbContext.Notes.ToList();

            var notesDTO = new List<Models.DTO.Note>();

            foreach (var note in notes)
            {
                notesDTO.Add(new Models.DTO.Note
                {
                    Id = note.Id,
                    Title = note.Title,
                    Description = note.Description,
                    ColorHex = note.ColorHex,
                    DateCreated = note.DateCreated,
                });
            }

            return Ok(notesDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetNotesById(Guid id)
        {
            var noteDomainObject = dbContext.Notes.Find(id);

            if (noteDomainObject != null)
            {
                var noteDto = new Models.DTO.Note
                {
                    Id = noteDomainObject.Id,
                    Title = noteDomainObject.Title,
                    Description = noteDomainObject.Description,
                    ColorHex = noteDomainObject.ColorHex,
                    DateCreated = noteDomainObject.DateCreated,
                };

                return Ok(noteDto);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult UpdateNote(Guid id, UpdateNoteRequest updateNoteRequest)
        {
            var existingNote = dbContext.Notes.Find(id);

            if (existingNote != null)
            {
                existingNote.Title = updateNoteRequest.Title;
                existingNote.Description = updateNoteRequest.Description;
                existingNote.ColorHex = updateNoteRequest.ColorHex;

                dbContext.SaveChanges();

                return Ok(existingNote);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteNote(Guid id)
        {
            var existingNote = dbContext.Notes.Find(id);

            if(existingNote != null)
            {
                dbContext.Notes.Remove(existingNote);
                dbContext.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }

    }
}
