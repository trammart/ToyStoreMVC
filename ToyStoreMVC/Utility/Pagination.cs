using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToyStoreMVC.Utility
{
    public static class Pagination
    {
        public static PagedData<T> PagedResult<T>(this IEnumerable<T> list, int PageNumber, int PageSize) where T : class
        { 
            var result = new PagedData<T>();
            PageSize = 8;
            result.Data = list.Skip(PageSize * (PageNumber - 1)).Take(PageSize).ToList();
            result.TotalPages = Convert.ToInt32(Math.Ceiling((double)list.Count() / PageSize));
            result.CurrentPage = PageNumber;
            return result;
        }
    }
}