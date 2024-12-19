using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using Data;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using Azure.Storage.Blobs.Specialized;
using System.Data;
using System.Text.RegularExpressions;
using AuthCore.Services;

namespace Schema.Mutations;


public class Mutation
{
    private readonly IMapper _mapper;
    private readonly PharmaservAdminContext _context;
   // private readonly IAzureStorage _storage;

    public Mutation(IMapper mapper,
    IDbContextFactory<PharmaservAdminContext> contextFactory
    )
    {
        _mapper = mapper;
        _context = contextFactory.CreateDbContext();
        //_storage = storage;
    }

   
    public Task<string> GetToken(string email, string password, [Service] AuthService identityService){

         return identityService.Authenticate(email, password);
       
    }
            



}
