using Library.Helpers;
using Library.Helpers.ConsoleHelper;
using Library.Net;

namespace ConAppTest;

internal partial class Program
{
    private static void Main(string[] args)
    {
        var cards = Nic.GetAllNetworkInterfaceCards().Select(x => new
        {
            x.Name,
            x.Description,
            x.NetworkInterfaceType,
            Speed = x.Speed.SizeSuffixComputer(),
            x.OperationalStatus,
            PhysicalAddress = x.GetPhysicalAddress(),
            x.IsReceiveOnly,
        }).DumpLine();
    }
}