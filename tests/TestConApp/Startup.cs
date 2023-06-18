using Library.Net;

var ip = IpAddress.MinValue;
var count = 0;
while (ip < IpAddress.MaxValue)
{
    ip = ip.Add(1);
    //Console.WriteLine(ip = ip.Add(1));
    count++;
}

//var ip = IpAddress.MinValue;
//while (ip < IpAddress.MaxValue)
//{
//    Console.WriteLine(ip = ip.Add(1));
//}