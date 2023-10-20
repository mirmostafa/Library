﻿using Library.CodeGeneration;
using Library.Helpers.CodeGen;

Initialize();

var setAgeMethodBody = @"
     this._age = age;
     return this;
     ";
var ctorBody = @$"
     this.Name = name;
     this.{TypeMemberNameHelper.ToFieldName("age")} = age;
     ";
var ns = RoslynHelper.CreateNamespace("Test.Dtos");
var personType = new TypePath("PersonDto");
var personDto = RoslynHelper.CreateType(personType)
    .AddField(new(TypeMemberNameHelper.ToFieldName("lastName"), TypePath.New<string>()), out var lastNameField)
    .AddConstructor(new[] { (TypePath.New<string>(), "name"), (TypePath.New<int>(), "age") }, ctorBody)
    .AddProperty<string>("Name")
    .AddPropertyWithBackingField(new("LastName", TypePath.New<string>()), lastNameField)
    .AddPropertyWithBackingField(new("Age", TypePath.New<int>(), setAccessor: (false, null)))
    .AddMethod(new MethodInfo(null, personType, "SetAge", new[] { (TypePath.New<int>(), "age") }, setAgeMethodBody))
    ;

ns = ns.AddType(personDto);

var root = RoslynHelper.CreateRoot()
    .AddUsingNameSpace("System")
    .AddUsingNameSpace("System.Linq")
    .AddUsingNameSpace("System.Threading")
    .AddNameSpace(ns);
WriteLine(root.GenerateCode());

internal class RoslynTest
{
    public static string GenerateDtoCode(string className, params (TypePath Type, string Name)[] props)
    {
        var result = RoslynHelper.CreateType(className);
        foreach (var prop in props)
        {
            result = result.AddProperty(prop.Name, prop.Type);
        }
        return result.GenerateCode();
    }
}