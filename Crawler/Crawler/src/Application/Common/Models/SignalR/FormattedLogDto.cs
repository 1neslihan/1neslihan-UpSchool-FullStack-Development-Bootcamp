using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.SignalR
{
    public class FormattedLogDto
    {
        public string product_Name { get; set; }
        public string product_isDiscounted { get; set; }
        public string product_discountedPrice {get; set;}
        public string product_originalPrice { get; set;}
        public string product_imageURL { get; set;}

    
    }
}
