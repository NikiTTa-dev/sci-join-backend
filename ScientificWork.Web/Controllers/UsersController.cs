﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScientificWork.UseCases.Users.UpdateProfileInfo;
using ScientificWork.UseCases.Users.UpdateStatusCommand;
using ScientificWork.UseCases.Users.UpdateStudentScientificPortfolio;
using ScientificWork.UseCases.Users.UpdateUserPassword;

namespace ScientificWork.Web.Controllers;

[ApiController]
[Route("api/users")]
[ApiExplorerSettings(GroupName = "users")]
[Authorize(Policy = "RegistrationComplete")]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    /// <summary>
    /// Constructor.
    /// </summary>
    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPut("update-profile-info")]
    public async Task UpdateProfileInfoAsync(UpdateStudentProfileInfoCommand command)
    {
        await mediator.Send(command);
    }

    [HttpPut("update-scientific-portfolio")]
    public async Task UpdateScientificPortfolioAsync(UpdateStudentScientificPortfolioCommand command)
    {
        await mediator.Send(command);
    }

    [HttpPut("update-status")]
    public async Task UpdateStatusAsync(UpdateStatusCommand command)
    {
        await mediator.Send(command);
    }

    /// <summary>
    /// Update password.
    /// </summary>
    [HttpPut("update-user-password")]
    public async Task UpdateUserPassword([FromBody] UpdateUserPasswordCommand command)
    {
        await mediator.Send(command);
    }
}
