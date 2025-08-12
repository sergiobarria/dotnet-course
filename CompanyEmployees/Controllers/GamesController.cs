using CompanyEmployees.Models;
using CompanyEmployees.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController([FromKeyedServices("player")] IPlayerGenerator playerGenerator) : ControllerBase
{
    [HttpGet]
    public Player Get()
    {
        var newPlayer = playerGenerator.CreateNewPlayer();

        return newPlayer;
    }
}