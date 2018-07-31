using Microsoft.AspNetCore.Mvc.Formatters;

public class HtmlOutputFormatter : StringOutputFormatter
{
    public HtmlOutputFormatter()
    {
        //You can add more than one ContentType
        SupportedMediaTypes.Add("text/html");
    }
}