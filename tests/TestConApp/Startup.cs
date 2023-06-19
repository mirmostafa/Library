using Library.Net;

var localIp = IpAddress.GetLocalHost();
IpAddress.GetRange(localIp, localIp.Add(50)).WriteLine();