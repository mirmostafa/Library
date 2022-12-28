using Library.Globalization;
using Library.Helpers;
using Library.Helpers.ConsoleHelper;
using Library.Net;
using Library.Results;

namespace ConAppTest;

internal partial class Program
{
    private static void Main(string[] args)
    {
        //var result = Div(5, 0);
        //if(result)
        //{
        //    Console.WriteLine(result.Value);
        //}

        //double div = Div(5, 1);
        //WriteLine(div);
        
        //DateTime date = PersianDateTime.Now;
        //PersianDateTime d = DateTime.Now;
    }

    public static Result<double> Div(double x, double y)
    {
        try
        {
            var result = x / y;
            return Result<double>.CreateSuccess(result);
        }
        catch (Exception ex)
        {
            return Result<double>.CreateFail(error: ex);
        }
    }
}