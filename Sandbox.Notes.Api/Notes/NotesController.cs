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
using Sandbox.Notes.Api.Notes.JsonApi;
using Sandbox.Notes.Api.Notes.Resource;
using Sandbox.Notes.Api.Notes.Services;

namespace Sandbox.Notes.Api.Notes
{
    [Route("api/[controller]")]
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
            var notes = _readNoteService.Get();
            var notesList = notes
                .Filter(query, _noteMappingService)
                .Sort(query, _noteMappingService)
                .Paginate(query).ToList();

            var noteResources = notesList.Select(_noteMappingService.MapEntityToResource).ToList();

            var baseQueryLink = "/api/notes?";

            var nextLink = "";
            if (query.Page.Count > 0)
            {
                if (query.Page.Count == notesList.Count)
                {
                    nextLink = new Uri(Request.GetDisplayUrl()).GetLeftPart(UriPartial.Authority) + baseQueryLink + "page[count]=" + query.Page.Count + "&page[first]=" + (query.Page.Count + query.Page.First);
                    if (query.Filters.Any())
                    {
                        foreach (var filter in query.Filters)
                        {
                            foreach (var filterValue in filter.Values)
                            {
                                nextLink += "&filter[" + filter.Field + "]=" + filterValue;
                            }
                        }
                    }
                    if (query.Sorts.Any())
                    {
                        nextLink += "&sort=";
                        foreach (var sort in query.Sorts)
                        {
                            nextLink += sort;
                            if (!query.Sorts.Last().Equals(sort))
                            {
                                nextLink += ",";
                            }
                        }
                    }
                    if (query.Includes.Any())
                    {
                        nextLink += "&include=";
                        foreach (var include in query.Includes)
                        {
                            nextLink += include;
                            if (!query.Includes.Last().Equals(include))
                            {
                                nextLink += ",";
                            }
                        }
                    }
                }
            }

            var response = new ListOfNotesResponse
            {
                Links = new JsonApiDocumentLinks
                {
                    Self = Request.GetDisplayUrl(),
                    Next = nextLink
                },
                Data = noteResources
            };

            if (query.IsIncluded("noteList"))
            {
                var noteListIds = notesList.Select(note => note.NoteListId)
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
        public async Task<IActionResult> Post([FromBody] Note note)
        {
            _logger.LogDebug("Creating note");
            var response = await _createNoteService.Create(note);
            if (response.IsSuccess)
                return Ok(response.Data);
            else
                return BadRequest();
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