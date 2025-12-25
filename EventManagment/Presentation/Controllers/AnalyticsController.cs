using Application.DTOs;
using Application.Services.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.ResponseModels;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        this.analyticsService = analyticsService;
    }

    /// <summary>
    /// Returns analytics dashboard data (KPIs, trends, distributions, etc.)
    /// </summary>
    /// <param name="filters">Analytics filters (date range, event types, locations)</param>
    [HttpGet]
    [ProducesResponseType(typeof(AnalyticsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnalyticsResponse>> GetAnalytics(
        [FromQuery] AnalyticsFilters filters,
        CancellationToken ct)
    {
        if (filters.From.HasValue && filters.To.HasValue && filters.From > filters.To)
        {
            return BadRequest("From date must be earlier than To date.");
        }

        var result = await analyticsService.GetAnalyticsDataAsync(filters, ct);

        return Ok(result.Adapt<AnalyticsResponse>());
    }
}
