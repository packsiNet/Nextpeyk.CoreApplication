using CoreApplication.Application.Features.Admin.Reports.Common;
using MediatR;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierFinancial;

public record GetCourierFinancialQuery(
    int? CourierId = null,
    DateOnly? From = null,
    DateOnly? To = null) : IRequest<List<CourierFinancialDto>>;
