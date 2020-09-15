using System.Reflection;

[assembly: Obfuscation(Feature = "Apply to type * when public and enum: renaming", Exclude = false)]
[assembly: Obfuscation(Feature = "Apply to type * when internal: renaming", Exclude = true)]
[assembly: Obfuscation(Feature = "rename symbol names with printable characters", Exclude = true)]
[assembly: Obfuscation(Feature = "code control flow obfuscation", Exclude = false)]
[assembly: Obfuscation(Feature = "string encryption", Exclude = true)]