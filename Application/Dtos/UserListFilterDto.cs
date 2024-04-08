using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserListFilterDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SearchParam { get; set; }

        public string? SortProp { get; set; }

        public bool SortByDescending { get; set; }
    }
}
