using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using Library.CodeGeneration;
using Library.CodeGeneration.v2;
using Library.CodeGeneration.v2.Back;
using Library.CodeGeneration.v2.Front;
using Library.CodeGeneration.v2.Front.HtmlGeneration;
using Library.DesignPatterns.StateMachine;
using Library.Helpers.CodeGen;
using Library.IO;
using Library.Logging;

namespace TestConApp;

internal partial class Obsoletes
{
    private static readonly ILogger _logger = ConsoleServices.Logger;

    public static void CodeGenTest() => INamespace.New("GSTech.Dtos")
    .AddType(IClass.New("PersonDto")
        .AddField("_age", typeof(int), AccessModifier.Private)
        .AddProperty("Name", TypePath.New(typeof(string).FullName!, isNullable: true))
        .AddProperty("Age", typeof(int), "_age")
        .AddMethod("CalcBirthYear", "return DateTime.Now.Year - this.Age;", null, typeof(int))
        )
    .GenerateCode<RoslynCodeGenerator>()
    .WriteLine();

    public static void ConstantExpectedTest([ConstantExpected(Max = 10)] int a) => Console.WriteLine(a);

    public static IEnumerable<long> Fibonacci(long count)
    {
        return FibonacciRecursiveIterator(count, 0L, 1L);

        static IEnumerable<long> FibonacciRecursiveIterator(long count, long a, long b)
        {
            if (count <= 0)
            {
                yield break;
            }

            yield return a;

            foreach (var number in FibonacciRecursiveIterator(count - 1, b, a + b))
            {
                yield return number;
            }
        }
    }

    public static void HtmlCodeGenerationTest()
    {
        var div = new DivElement()
                    .AddAttribute("row")
                    .AddChild(new InputElement("text"))
                    .AddChild(new DivElement().AddAttribute("class", "col")
                            .AddChild(new DivElement().AddAttribute("class", "col")
                                .AddChild(new InputElement("checkbox").AddAttribute("class", "text-box")))
                            .AddChild(new InputElement("password")))
                    .AddChild(new DivElement().AddAttribute("class", "col")
                            .AddChild(new DivElement().AddAttribute("class", "col")
                                .AddChild(new InputElement("checkbox").AddAttribute("class", "text-box")))
                            .AddChild(new InputElement("password")))
                    .AddChild(new DivElement().AddAttribute("class", "col")
                            .AddChild(new DivElement().AddAttribute("class", "col")
                                .AddChild(new InputElement("checkbox").AddAttribute("class", "text-box")))
                            .AddChild(new InputElement("password")))
                    .AddChild(new InputElement("text"));
        WriteLine(div.ToHtml());
    }

    public static void ListAsFluent()
    {
        List<int> list = new List<int>().AsFluent().Add(5).Add(2).Add(3);
        _ = DumpListLine(list);
    }

    public static IEnumerable<TResult> SimDActor<T1, T2, TResult>(
                IEnumerable<T1> left, IEnumerable<T2> right, Func<Vector<T1>, Vector<T2>, Vector<TResult>> actor, long count)
        where T1 : struct
        where T2 : struct
        where TResult : struct
    {
        var leftArray = left.ToArray();
        var rightArray = right.ToArray();
        var result = new TResult[count];
        var offset = Vector<TResult>.Count;
        for (var i = 0; i < count - offset; i += offset)
        {
            var v1 = new Vector<T1>(leftArray, i);
            var v2 = new Vector<T2>(rightArray, i);
            actor(v1, v2).CopyTo(result, i);
        }
        return result;
    }

    public static async Task StateMachineTest()
    {
        _ = await StateMachineManager.Dispatch(
                        () => Task.FromResult((0, MoveDirection.Forward)),
                        flow => Task.FromResult(move(flow)),
                        flow => Task.FromResult(move(flow)),
                        display,
                        display);
        WriteLine($"End.");

        static (int Current, MoveDirection Direction) move((int Current, IEnumerable<(int State, MoveDirection Direction)>) flow)
        {
            WriteLine($"Current: {flow.Current}. Press Up or Down to move foreword or backward. Or press any other key to done.");
            var response = ReadKey().Key;
            var direction = MoveDirection.Ended;
            switch (response)
            {
                case ConsoleKey.UpArrow:
                    flow.Current++;
                    direction = MoveDirection.Forward;
                    break;

                case ConsoleKey.DownArrow:
                    flow.Current--;
                    direction = MoveDirection.Backward;
                    break;

                case ConsoleKey.None:
                    break;

                case ConsoleKey.Backspace:
                    break;

                case ConsoleKey.Tab:
                    break;

                case ConsoleKey.Clear:
                    break;

                case ConsoleKey.Enter:
                    break;

                case ConsoleKey.Pause:
                    break;

                case ConsoleKey.Escape:
                    break;

                case ConsoleKey.Spacebar:
                    break;

                case ConsoleKey.PageUp:
                    break;

                case ConsoleKey.PageDown:
                    break;

                case ConsoleKey.End:
                    break;

                case ConsoleKey.Home:
                    break;

                case ConsoleKey.LeftArrow:
                    break;

                case ConsoleKey.RightArrow:
                    break;

                case ConsoleKey.Select:
                    break;

                case ConsoleKey.Print:
                    break;

                case ConsoleKey.Execute:
                    break;

                case ConsoleKey.PrintScreen:
                    break;

                case ConsoleKey.Insert:
                    break;

                case ConsoleKey.Delete:
                    break;

                case ConsoleKey.Help:
                    break;

                case ConsoleKey.D0:
                    break;

                case ConsoleKey.D1:
                    break;

                case ConsoleKey.D2:
                    break;

                case ConsoleKey.D3:
                    break;

                case ConsoleKey.D4:
                    break;

                case ConsoleKey.D5:
                    break;

                case ConsoleKey.D6:
                    break;

                case ConsoleKey.D7:
                    break;

                case ConsoleKey.D8:
                    break;

                case ConsoleKey.D9:
                    break;

                case ConsoleKey.A:
                    break;

                case ConsoleKey.B:
                    break;

                case ConsoleKey.C:
                    break;

                case ConsoleKey.D:
                    break;

                case ConsoleKey.E:
                    break;

                case ConsoleKey.F:
                    break;

                case ConsoleKey.G:
                    break;

                case ConsoleKey.H:
                    break;

                case ConsoleKey.I:
                    break;

                case ConsoleKey.J:
                    break;

                case ConsoleKey.K:
                    break;

                case ConsoleKey.L:
                    break;

                case ConsoleKey.M:
                    break;

                case ConsoleKey.N:
                    break;

                case ConsoleKey.O:
                    break;

                case ConsoleKey.P:
                    break;

                case ConsoleKey.Q:
                    break;

                case ConsoleKey.R:
                    break;

                case ConsoleKey.S:
                    break;

                case ConsoleKey.T:
                    break;

                case ConsoleKey.U:
                    break;

                case ConsoleKey.V:
                    break;

                case ConsoleKey.W:
                    break;

                case ConsoleKey.X:
                    break;

                case ConsoleKey.Y:
                    break;

                case ConsoleKey.Z:
                    break;

                case ConsoleKey.LeftWindows:
                    break;

                case ConsoleKey.RightWindows:
                    break;

                case ConsoleKey.Applications:
                    break;

                case ConsoleKey.Sleep:
                    break;

                case ConsoleKey.NumPad0:
                    break;

                case ConsoleKey.NumPad1:
                    break;

                case ConsoleKey.NumPad2:
                    break;

                case ConsoleKey.NumPad3:
                    break;

                case ConsoleKey.NumPad4:
                    break;

                case ConsoleKey.NumPad5:
                    break;

                case ConsoleKey.NumPad6:
                    break;

                case ConsoleKey.NumPad7:
                    break;

                case ConsoleKey.NumPad8:
                    break;

                case ConsoleKey.NumPad9:
                    break;

                case ConsoleKey.Multiply:
                    break;

                case ConsoleKey.Add:
                    break;

                case ConsoleKey.Separator:
                    break;

                case ConsoleKey.Subtract:
                    break;

                case ConsoleKey.Decimal:
                    break;

                case ConsoleKey.Divide:
                    break;

                case ConsoleKey.F1:
                    break;

                case ConsoleKey.F2:
                    break;

                case ConsoleKey.F3:
                    break;

                case ConsoleKey.F4:
                    break;

                case ConsoleKey.F5:
                    break;

                case ConsoleKey.F6:
                    break;

                case ConsoleKey.F7:
                    break;

                case ConsoleKey.F8:
                    break;

                case ConsoleKey.F9:
                    break;

                case ConsoleKey.F10:
                    break;

                case ConsoleKey.F11:
                    break;

                case ConsoleKey.F12:
                    break;

                case ConsoleKey.F13:
                    break;

                case ConsoleKey.F14:
                    break;

                case ConsoleKey.F15:
                    break;

                case ConsoleKey.F16:
                    break;

                case ConsoleKey.F17:
                    break;

                case ConsoleKey.F18:
                    break;

                case ConsoleKey.F19:
                    break;

                case ConsoleKey.F20:
                    break;

                case ConsoleKey.F21:
                    break;

                case ConsoleKey.F22:
                    break;

                case ConsoleKey.F23:
                    break;

                case ConsoleKey.F24:
                    break;

                case ConsoleKey.BrowserBack:
                    break;

                case ConsoleKey.BrowserForward:
                    break;

                case ConsoleKey.BrowserRefresh:
                    break;

                case ConsoleKey.BrowserStop:
                    break;

                case ConsoleKey.BrowserSearch:
                    break;

                case ConsoleKey.BrowserFavorites:
                    break;

                case ConsoleKey.BrowserHome:
                    break;

                case ConsoleKey.VolumeMute:
                    break;

                case ConsoleKey.VolumeDown:
                    break;

                case ConsoleKey.VolumeUp:
                    break;

                case ConsoleKey.MediaNext:
                    break;

                case ConsoleKey.MediaPrevious:
                    break;

                case ConsoleKey.MediaStop:
                    break;

                case ConsoleKey.MediaPlay:
                    break;

                case ConsoleKey.LaunchMail:
                    break;

                case ConsoleKey.LaunchMediaSelect:
                    break;

                case ConsoleKey.LaunchApp1:
                    break;

                case ConsoleKey.LaunchApp2:
                    break;

                case ConsoleKey.Oem1:
                    break;

                case ConsoleKey.OemPlus:
                    break;

                case ConsoleKey.OemComma:
                    break;

                case ConsoleKey.OemMinus:
                    break;

                case ConsoleKey.OemPeriod:
                    break;

                case ConsoleKey.Oem2:
                    break;

                case ConsoleKey.Oem3:
                    break;

                case ConsoleKey.Oem4:
                    break;

                case ConsoleKey.Oem5:
                    break;

                case ConsoleKey.Oem6:
                    break;

                case ConsoleKey.Oem7:
                    break;

                case ConsoleKey.Oem8:
                    break;

                case ConsoleKey.Oem102:
                    break;

                case ConsoleKey.Process:
                    break;

                case ConsoleKey.Packet:
                    break;

                case ConsoleKey.Attention:
                    break;

                case ConsoleKey.CrSel:
                    break;

                case ConsoleKey.ExSel:
                    break;

                case ConsoleKey.EraseEndOfFile:
                    break;

                case ConsoleKey.Play:
                    break;

                case ConsoleKey.Zoom:
                    break;

                case ConsoleKey.NoName:
                    break;

                case ConsoleKey.Pa1:
                    break;

                case ConsoleKey.OemClear:
                    break;

                default:
                    direction = MoveDirection.Ended;
                    break;
            }
            return (flow.Current, direction);
        }

        static Task display((int Current, IEnumerable<(int State, MoveDirection Direction)> History) flow)
        {
            WriteLine(flow.Current);
            flow.History.ForEach(x => Write(x));
            WriteLine();
            WriteLine("==================");
            return Task.CompletedTask;
        }
    }

    public static void TestRoslynCodeGenerator()
    {
        var setAgeMethodBody = @"
     this._age = age;
     return this;
     ";
        var ctorBody = @$"
     this.Name = name;
     this.{TypeMemberNameHelper.ToFieldName("age")} = age;
     ";
        var personType = new TypePath("PersonDto");
        var personDto = RoslynHelper.CreateType(personType)
            .AddField(new(TypeMemberNameHelper.ToFieldName("lastName"), TypePath.New<string>()), out var lastNameField)
            .AddConstructor([(TypePath.New<string>(), "name"), (TypePath.New<int>(), "age")], ctorBody)
            .AddProperty<string>("Name")
            .AddPropertyWithBackingField(new("LastName", TypePath.New<string>()), lastNameField)
            .AddPropertyWithBackingField(new("Age", TypePath.New<int>(), setAccessor: (false, null)))
            .AddMethod(new RosMethodInfo(null, personType, "SetAge", [(TypePath.New<int>(), "age")], setAgeMethodBody))
            ;

        var ns = RoslynHelper.CreateNamespace("Test.Dtos")
            .AddType(personDto);

        var root = RoslynHelper.CreateRoot()
            .AddUsingNameSpace(nameof(System))
            .AddUsingNameSpace(nameof(System.Linq))
            .AddUsingNameSpace(nameof(System.Threading))
            .AddNameSpace(ns);
        WriteLine(root.GenerateCode());
    }

    public static void WatchHardDisk()
    {
        var watchers = Drive.GetDrives().Select(watch).Enumerate(x => WriteLine($"Watching {x.Path}")).ToList();
        WriteLine("Ready");

        While(() => ReadKey().Key != ConsoleKey.X);
        WriteLine("Closing...");
        watchers.ForEach(x => x.Dispose());

        static Library.IO.FileSystemWatcher watch(Drive drive)
            => Library.IO.FileSystemWatcher.New(drive, includeSubdirectories: true)
                .OnCreated((_, e) => _logger.Log($"{e.Item.FullName} created.", format: LogFormat.FORMAT_SHORT))
                .OnChanged((_, e) => _logger.Log($"{e.Item.FullName} changed.", format: LogFormat.FORMAT_SHORT))
                .OnRenamed((_, e) => _logger.Log($"in {e.Item.FullPath} : {e.Item.OldName} renamed to {e.Item.NewName}.", format: LogFormat.FORMAT_SHORT))
                .OnDeleted((_, e) => _logger.Log($"{e.Item.FullName} deleted.", format: LogFormat.FORMAT_SHORT))
                .Start();
    }
}