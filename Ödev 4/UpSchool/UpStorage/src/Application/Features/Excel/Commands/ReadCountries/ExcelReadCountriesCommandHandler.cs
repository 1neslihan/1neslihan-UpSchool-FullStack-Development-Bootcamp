using Application.Common.Interfaces;
using Application.Common.Models.Excel;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Excel.Comments.ReadCountries
{
    public class ExcelReadCountriesCommandHandler : IRequestHandler<ExcelReadCountriesCommand, Response<int>>
    {
        private readonly IExcelService _excelService;
        private readonly IApplicationDbContext _applicationDbContext;

        public ExcelReadCountriesCommandHandler(IExcelService excelService, IApplicationDbContext applicationDbContext)
        {
            _excelService = excelService;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<int>> Handle(ExcelReadCountriesCommand request, CancellationToken cancellationToken)
        {
            var countryDtos = _excelService.ReadCountries(new ExcelBase64Dto(file: request.ExcelBase64File));

            var countries = countryDtos
                .Select(x => x.MapToCountry())
                .ToList();

            await _applicationDbContext.Countries.AddRangeAsync(countries, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<int>($"{countries.Count} countries were added to the db successfully.", countries.Count);
        }
    }
}
