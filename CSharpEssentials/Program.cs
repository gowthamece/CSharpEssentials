// See https://aka.ms/new-console-template for more information
using CSharpEssentials;
using System.Reflection;
using System.Text;


//RegularExp exp = new CSharpEssentials.RegularExp();
//exp.RegularExpfn();

//Mutable_Immutable mi = new CSharpEssentials.Mutable_Immutable();
//mi.objectCount();

//String s1 = "Hello";
//Console.WriteLine($"String object count: {GC.GetGeneration(s1)}");
//String s2 = s1;
//StringBuilder s3 = new("Hello");
//StringBuilder s4 = s3;
//Console.WriteLine($"StringBuilder object count: {GC.GetGeneration(s3)}");
//Console.ReadKey();

string s1 = "Hello" + "test";
Console.WriteLine("Initial string: " + s1);

// Concatenation operation

// Check if s1 and s2 refer to the same object
bool areSameReference = Object.ReferenceEquals(s1, s1);
Console.WriteLine("Are s1 and s2 the same reference? " + areSameReference);

// Print the result
int numberOfStringObjects = areSameReference ? 1 : 2;
Console.WriteLine("Number of string objects created: " + numberOfStringObjects);


StringBuilder sb1 = new StringBuilder("Hello");
sb1.Append("test");

Console.WriteLine("Initial string: " + sb1);

// Check if s1 and s2 refer to the same object
//bool areSameReference = Object.ReferenceEquals(s1, s2);
//Console.WriteLine("Are s1 and s2 the same reference? " + areSameReference);

//// Print the result
//int numberOfStringObjects = areSameReference ? 1 : 2;
//Console.WriteLine("Number of string objects created: " + numberOfStringObjects);