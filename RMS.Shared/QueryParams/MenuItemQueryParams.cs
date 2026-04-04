using RMS.Shared.SortingOptionsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.QueryParams
{
    public class MenuItemQueryParams
    {
        public int? CategoryId { get; set; }
        public bool? IsAvailable { get; set; }
        public string? Search { get; set; }
        public MenuItemSortingOptions Sort { get; set; }


        #region Pagination
        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value <= 0 ? 1 : value;
        }

        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        private int _pageSize = DefaultPageSize;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value <= 0)
                {
                    _pageSize = DefaultPageSize;
                }
                else if (value > MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }
        #endregion
    }
}
