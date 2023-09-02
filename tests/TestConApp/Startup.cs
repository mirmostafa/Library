Configure();

var typeName1 = "Library.Net.IpAddress";
var typeName2 = "HanyCo.Infra.Cqrs.IQueryResult<System.Linq.IEnumerable<Test.Hr.Dtos.GetAllPeopleResult>>";
var typeName3 = "HanyCo.Infra.Cqrs.IQueryResult<IEnumerable<Test.Hr.Dtos.GetAllPeopleResult>>";
GetName(typeName1).WriteLine();
GetName(typeName2).WriteLine();
GetName(typeName3).WriteLine();

static string GetName(string fullName)
{
    string result;
    if (!fullName.Contains('.'))
    {
        result = fullName;
    }
    else if (!fullName.Contains('<'))
    {
        result = fullName.Split('.').Last();
    }
    else
    {
        var generics = fullName.Split('<');
        result = null!;
        foreach (var generic in generics)
        {
            if (result.IsNullOrEmpty())
            {
                result = generic.Split(".").Last();
            }
            else
            {
                result += $"<{generic.Split(".").Last()}";
            }
        }
    }
    return result;
}