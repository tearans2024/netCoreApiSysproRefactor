using System.Collections.Generic;

public class QueryRequest
{
    public string Query { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
}
