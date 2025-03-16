using System.Collections.Generic;

public class ResponseModel
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public PaginationInfo Pagination { get; set; }
}

public class PaginationInfo
{
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (PageSize == 0) ? 0 : (int)System.Math.Ceiling((double)TotalItems / PageSize);
}
