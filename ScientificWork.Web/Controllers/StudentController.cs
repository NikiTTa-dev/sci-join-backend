﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScientificWork.Domain.Admins;
using ScientificWork.UseCases.Professors.AddScientificWorksToFavorites;
using ScientificWork.UseCases.Requests.GetProfessorRequestsStudent;
using ScientificWork.UseCases.Requests.GetStudentRequestsStudent;
using ScientificWork.UseCases.Students.AddProfessorToFavorites;
using ScientificWork.UseCases.Students.AddStudentToFavorites;
using ScientificWork.UseCases.Students.GetStudentProfileById;
using ScientificWork.UseCases.Students.GetStudents;
using ScientificWork.UseCases.Students.UploadStudents;
using ScientificWork.Web.Infrastructure.Web;

namespace ScientificWork.Web.Controllers;

/// <summary>
/// Student controller.
/// </summary>
[ApiController]
[Route("api/student")]
[ApiExplorerSettings(GroupName = "student")]
[Authorize(Policy = "RegistrationComplete")]
public class StudentController : ControllerBase
{
    private readonly IMediator mediator;

    /// <summary>
    /// Constructor.
    /// </summary>
    public StudentController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Student profile by id.
    /// </summary>
    [HttpGet("student-profile-by-id")]
    public async Task<ActionResult> GetStudentProfile([FromQuery] GetStudentProfileByIdQuery query)
    {
        var res = await mediator.Send(query);
        return Ok(res);
    }

    /// <summary>
    /// List students.
    /// </summary>
    [HttpGet("list-students")]
    public async Task<ActionResult> GetStudents([FromQuery] GetStudentsQuery query)
    {
        HttpContext.Items.Add("userId", User.GetCurrentUserId());
        var res = await mediator.Send(query);
        return Ok(res);
    }

    [HttpPost("upload-students")]
    [Authorize(Roles = nameof(SystemAdmin))]
    public async Task UploadStudents([FromForm] UploadStudentsCommand command)
    {
        await mediator.Send(command);
    }

    [HttpPost("add-student-to-favorites")]
    public async Task AddStudentToFavorites([FromQuery] AddStudentToFavoritesCommand command)
    {
        HttpContext.Items.Add("userId", User.GetCurrentUserId());
        await mediator.Send(command);
    }

    [HttpPost("add-professor-to-favorites")]
    public async Task AddProfessorToFavorites([FromQuery] AddProfessorToFavoritesCommand command)
    {
        HttpContext.Items.Add("userId", User.GetCurrentUserId());
        await mediator.Send(command);
    }

    [HttpPost("add-scientific-work-to-favorites")]
    public async Task AddScientificWorksToFavorites([FromQuery] AddScientificWorksToFavoritesCommand command)
    {
        HttpContext.Items.Add("userId", User.GetCurrentUserId());
        await mediator.Send(command);
    }

    /// <summary>
    /// List request from student to student .
    /// </summary>
    [HttpGet("list-request-from-student")]
    public async Task<ActionResult> GetStudentRequestStudent([FromQuery] GetStudentRequestsStudentQuery query)
    {
        HttpContext.Items.Add("userId", User.GetCurrentUserId());
        var res = await mediator.Send(query);
        return Ok(res);
    }

    /// <summary>
    /// List request from professor to student .
    /// </summary>
    [HttpGet("list-request-from-professor")]
    public async Task<ActionResult> GetProfessorRequestStudent([FromQuery] GetProfessorRequestsStudentQuery query)
    {
        HttpContext.Items.Add("userId", User.GetCurrentUserId());
        var res = await mediator.Send(query);
        return Ok(res);
    }
}
