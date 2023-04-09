using Application.Common.Models.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IExcelService
    {
        List<ExcelCityDto> ReadCities(ExcelBase64Dto excelDto);
        List<ExcelCountryDto> ReadCountries(ExcelBase64Dto excelDto);
        //Task<List<ExcelCityDto>> ReadCitiesAsync(object excelDto, CancellationToken cancellationToken);
    }
}


