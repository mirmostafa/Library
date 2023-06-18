using Library.Net;

var ip = IpAddress.MaxValue;
while (ip > IpAddress.MinValue)
{
    Console.WriteLine(ip = ip.Add(-1));
}

//var ip = IpAddress.MinValue;
//while (ip < IpAddress.MaxValue)
//{
//    Console.WriteLine(ip = ip.Add(1));
//}