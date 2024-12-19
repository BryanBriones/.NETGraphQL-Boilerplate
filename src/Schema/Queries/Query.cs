using AutoMapper;
using Data;
using HotChocolate.Authorization;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using AuthCore.Services;

namespace Schema.Queries;


public class Query
{
    private readonly IMapper _mapper;
    private readonly PharmaservAdminContext _context;

    public Query(IMapper mapper, IDbContextFactory<PharmaservAdminContext> contextFactory)
    {
        _mapper = mapper;
        _context = contextFactory.CreateDbContext();
    }

    [Authorize(Policy = "Admin")]
    public async Task<UserLoginType?> GetUserLoginsAsync(CancellationToken cancellationToken)
    {
        return await _context.UserLogins
                .Select(s => _mapper.Map<UserLoginType>(s))
                .FirstOrDefaultAsync(cancellationToken);
    }

   
}

