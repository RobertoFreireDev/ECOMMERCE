global using System.ComponentModel.DataAnnotations;

global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

global using Company.Ecommerce.Orders.Internal.Controllers.DataContracts.Requests;
global using Company.Ecommerce.Orders.Internal.Domain.Base;
global using Company.Ecommerce.Orders.Internal.Domain.Entities;
global using Company.Ecommerce.Orders.Internal.Domain.Repositories;
global using Company.Ecommerce.Orders.Internal.Services.Interfaces;
global using Company.Ecommerce.Orders.Internal.Services;

global using Company.Ecommerce.ShoppingCart.Public;