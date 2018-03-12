using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRiver.JsonApi;
using AppRiver.JsonApi.AspNetCore;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sandbox.Notes.Api.Infrastructure.Configuration;
using Sandbox.Notes.Api.Infrastructure.Validation;
using Sandbox.Notes.Api.Notes.JsonApi.Extensions;
using Sandbox.Notes.Api.Notes.JsonApi.Requests;
using Sandbox.Notes.Api.Notes.JsonApi.Responses;
using Sandbox.Notes.Api.Notes.JsonApi.Services;
using Sandbox.Notes.Api.Notes.JsonApi.Utility;
using Sandbox.Notes.Api.Notes.Resource;
using Sandbox.Notes.Api.Notes.Services;

namespace Sandbox.Notes.Api.Notes
{
    [Route("api/[controller]"), JsonApiErrorHandling]
    public class NotesController : ControllerBase
    {
        private readonly IReadNoteService _readNoteService;
        private readonly ICreateNoteService _createNoteService;
        private readonly IUpdateNoteService _updateNoteService;
        private readonly IDeleteNoteService _deleteNoteService;
        private readonly IReadNoteListService _readNoteListService;
        private readonly ILogger<NotesController> _logger;
        private readonly IOptions<AppConfiguration> _appOptions;
        private readonly NoteMappingService _noteMappingService;
        private readonly NoteListMappingService _noteListMappingService;


        public NotesController(IReadNoteService readNoteService, ICreateNoteService createNoteService,
            IUpdateNoteService updateNoteService, IDeleteNoteService deleteNoteService, IReadNoteListService readNoteListService, ILogger<NotesController> logger,
            IOptions<AppConfiguration> appOptions)
        {
            _readNoteService = readNoteService;
            _createNoteService = createNoteService;
            _updateNoteService = updateNoteService;
            _deleteNoteService = deleteNoteService;
            _readNoteListService = readNoteListService;
            _logger = logger;
            _appOptions = appOptions;
            _noteMappingService = new NoteMappingService();
            _noteListMappingService = new NoteListMappingService();
        }

        [HttpGet]
        [Produces(typeof(SingleNoteResponse))]
        public IActionResult Get(JsonApiQueryModel query)
        {
            _logger.LogDebug($"Getting notes from app: {_appOptions.Value.ApplicationName}");

            var filteredNotes = _readNoteService.Get()
                .Filter(query, _noteMappingService)
                .Sort(query, _noteMappingService);

            var notes = filteredNotes.Paginate(query).ToList();

            var noteResources = notes.Select(_noteMappingService.MapEntityToResource).ToList();

            var nextLink = "";
            var lastLink = "";
            if (query.Page.Count > 0)
            {
                const string baseQueryLink = "/api/notes?";

                var totalNumberOfNotes = filteredNotes.Count();
                var requestDisplayUrl = Request.GetDisplayUrl();

                nextLink = JsonApiLinkUtility.BuildNextLink(query, totalNumberOfNotes, baseQueryLink, requestDisplayUrl);

                lastLink = JsonApiLinkUtility.BuildLastLink(query, totalNumberOfNotes, baseQueryLink, requestDisplayUrl);
            }

            var response = new ListOfNotesResponse
            {
                Links = new JsonApiDocumentLinks
                {
                    Self = Request.GetDisplayUrl(),
                    Next = nextLink,
                    Last = lastLink
                },
                Data = noteResources
            };

            if (query.IsIncluded("noteList"))
            {
                var noteListIds = notes.Select(note => note.NoteListId)
                    .Distinct()
                    .ToList();

                var noteListResources = noteListIds.Select(_readNoteListService.Get)
                    .Select(_noteListMappingService.MapEntityToResource)
                    .ToList();

                response.Included = new List<JsonApiResource>(noteListResources);
            }

            return Ok(response);
        }


        [HttpGet("{noteId}")]
        public async Task<IActionResult> Get(int noteId)
        {
            _logger.LogDebug($"Getting notes by id: {noteId}");
            var response = await _readNoteService.Get(noteId);
            if (response.IsSuccess)
                return Ok(response.Data);
            else
                return BadRequest();
        }

        [HttpPost, ValidateModelState]
        [Produces(typeof(SingleNoteResponse))]
        public async Task<IActionResult> Post([FromBody] NoteCreationRequest request)
        {
            _logger.LogDebug("Creating note");

            var newNoteResource = request.Data;
            var resourceValidator = new JsonApiResourceValidator("notes", true, false);
            resourceValidator.ValidateResource(newNoteResource);


            var createdNote =
                await _createNoteService.Create(_noteMappingService.MapResourceToEntity(newNoteResource));
            var noteResource = _noteMappingService.MapEntityToResource(createdNote.Data);

            var response = new SingleNoteResponse
            {
                Links = new JsonApiDocumentLinks
                {
                    Self = new Uri(Request.GetDisplayUrl()).GetLeftPart(UriPartial.Authority) + "/api/notes/" +
                           createdNote.Data.Id
                },
                Data = noteResource
            };

            return Ok(response);
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> Put([FromBody] Note note, int noteId)
        {
            _logger.LogDebug($"Updating note by: {noteId}");
            var response = await _updateNoteService.Update(noteId, note);
            if (response.IsSuccess)
                return Ok(response.Data);
            else
                return BadRequest();
        }

        [HttpDelete("{noteId}")]
        [Produces(typeof(NoDataResponse))]
        public async Task<IActionResult> Delete(int noteId)
        {
            _logger.LogDebug($"Deleting notes by id: {noteId}");
            var response = await _deleteNoteService.Delete(noteId);
            if (response.IsSuccess)
                return Ok();
            else
                return BadRequest();
        }
    }
}