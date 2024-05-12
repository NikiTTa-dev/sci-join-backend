﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScientificWork.Domain.Professors;
using ScientificWork.Domain.Students;
using ScientificWork.Infrastructure.Abstractions.Interfaces;

namespace ScientificWork.UseCases.ScientificWorks.GetGeneralInformationById;

public class GetGeneralInformationByIdQueryHandler : IRequestHandler<GetGeneralInformationByIdQuery, GetGeneralInformationByIdResult>
{
    private readonly IMapper mapper;
    private readonly IAppDbContext dbContext;
    private readonly UserManager<Student> studentManager;
    private readonly UserManager<Professor> professorManager;
    private readonly ILoggedUserAccessor userAccessor;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GetGeneralInformationByIdQueryHandler(IMapper mapper, IAppDbContext dbContext,
        UserManager<Student> studentManager, UserManager<Professor> professorManager, ILoggedUserAccessor userAccessor)
    {
        this.mapper = mapper;
        this.dbContext = dbContext;
        this.studentManager = studentManager;
        this.professorManager = professorManager;
        this.userAccessor = userAccessor;
    }

    /// <inheritdoc />
    public async Task<GetGeneralInformationByIdResult> Handle(GetGeneralInformationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var scientificWorks = await dbContext.ScientificWorks
                .Where(x => x.Id == request.Id)
                .Include(x => x.ScientificInterests)
                .FirstOrDefaultAsync(cancellationToken);
        var result = mapper.Map<GetGeneralInformationByIdResult>(scientificWorks);
        result.IsFavorite = await CheckFavoritesAsync(request.Id);
        return result;
    }

    private async Task<bool> CheckFavoritesAsync(Guid swId)
    {
        var userId = userAccessor.GetCurrentUserId();
        var curUser = await studentManager.FindByIdAsync(userId.ToString());
        bool check;

        if (curUser == null)
        {
            check = professorManager.Users
                .Where(s => s.Id == userId)
                .Include(s => s.ProfessorFavoriteScientificWorks)
                .SelectMany(s => s.ProfessorFavoriteScientificWorks)
                .Any(s => s.ScientificWorkId == swId);
        }
        else
        {
            check = studentManager.Users
                .Where(s => s.Id == userId)
                .Include(s => s.StudentFavoriteScientificWorks)
                .SelectMany(s => s.StudentFavoriteScientificWorks)
                .Any(s => s.ScientificWorkId == swId);
        }

        return check;
    }
}
