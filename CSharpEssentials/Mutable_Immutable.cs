using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEssentials
{
    internal class Mutable_Immutable
    {
        public void immutable()
        {
            string s1 = "Hello";
            string s2 = s1;
            s1 = "World";
            Console.WriteLine(s2);
        }
        public void mutable()
        {
            StringBuilder s1 = new("Hello");
            StringBuilder s2 = s1;
            s1.Append("World");
            Console.WriteLine(s2);
        }
        //generate a program to check the difference between mutable and immutable
        public void mutable_Immutable()
        {
            immutable();
            mutable();
        }
        
        //generate a function to get the object count of mutable and immutable
        public void objectCount()
        {
            string s1 = "Hello";
            string s2 = s1;
            StringBuilder s3 = new("Hello");
            StringBuilder s4 = s3;
            Console.WriteLine($"String object count: {GC.GetGeneration(s2)}");
            Console.WriteLine($"StringBuilder object count: {GC.GetGeneration(s4)}");
        }
        //get a function to check the object count of string and stringbuilder

        //generate a function to get the object count of strin

    }
}
