Initialize();

var name = "Mohammad";
var age = 26;
FormattableString s = $"Hello {name}. You are {age}.";
s.GetArgs().ForEach(arg => WriteLine($"{arg.Key} == {arg.Value}"));
