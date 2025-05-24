using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Frontend.Components.Alert;
using ChapsDotNET.Frontend.Components.ListFilter;
using ChapsDotNET.Frontend.Components.Pagination;
using ChapsDotNET.Frontend.Components.Table;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    public class IndexController<T> : Controller
    {
        public string ToggleSortOrder(string currentSortOrder, string column)
        {
            if (currentSortOrder == column)
            {
                return column + "_desc";
            }
            return column;
        }
        public SortOptions GetSort(string currentSortOrder, string column)
        {
            if (currentSortOrder == column)
            {
                return SortOptions.ascending;
            }
            else if (currentSortOrder == $"{column}_desc")
            {
                return SortOptions.descending;
            }
            return SortOptions.none;
        }
        public AlertModel? GetAlerts()
        {
            if (TempData["alertStatus"] == null || TempData["alertContent"] == null || TempData["alertSummary"] == null)
            {
                return null;
            }
            return new AlertModel
            {
                Type = AlertModel.GetAlertTypeFromStatus(TempData["alertStatus"] as string),
                Content = TempData["alertContent"] as string,
                Summary = TempData["alertSummary"] as string
            };
        }
        public PaginationModel GetPagination(PagedResult<List<T>> resultsData)
        {
            return new PaginationModel
            {
                CurrentPage = resultsData.CurrentPage,
                TotalCount = resultsData.RowCount,
                PageSize = resultsData.PageSize,
                PageCount = resultsData.PageCount
            };
        }

        public TableModel GetTable(string Caption, List<HeaderCell> Headers, List<Row> Rows, PagedResult<List<T>> resultsData, int resultsCount)
        {
            var resultsFrom = PaginationModel.GetResultsFrom(resultsData.CurrentPage, resultsData.PageSize).ToString();
            var resultsTo = PaginationModel.GetResultsTo(resultsData.CurrentPage, resultsData.PageSize, resultsCount).ToString();
            return new TableModel {
                Caption = Caption, 
                HeaderData = Headers, 
                BodyData = Rows, 
                ResultsTotal = resultsData.RowCount.ToString(), 
                ResultsFrom = resultsFrom, 
                ResultsTo = resultsTo 
            };

        }
        public ListFilterModel GetListFilter(PagedResult<List<T>> resultsData, TableModel tableModel, List<IFormFieldModel>? filters = null, string? resetUrl = null, List<SelectedFilter>? selectedFilters = null)
        {
            return new ListFilterModel
            {
                Table = tableModel,
                Filters = filters != null ? filters : null,
                SelectedFilters = selectedFilters?.Count > 0 ? selectedFilters : null,
                Pagination = GetPagination(resultsData),
                ResetUrl = resetUrl != null ? resetUrl : null,
            };
        }
    }
}
