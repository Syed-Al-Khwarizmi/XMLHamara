//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.Xml.Linq;

//namespace TestCsvToXml
//{
//    class XMLManipulation
//    {
//        public static void Main(string[] args)
//        {
//            XDocument doc = XDocument.Load("C:\\Users\\Ali_H\\Desktop\\TestCsvToXml\\XMLManipulation.xml");
//            //printing the complete document
//            Console.WriteLine(doc.Element("person"));

//            //getting the students
//            Console.WriteLine("\nGetting the students\n");
//            XElement students = doc.Element("person").Element("students");
//            Console.WriteLine(students);

//            Console.WriteLine("\nGetting the teacher courses\n");
//            XElement courses = doc.Element("person").Element("teachers").Element("teacher").Element("courses");
//            //We'll be needing LINQ queries as well
//            Console.WriteLine(courses);
//            Console.ReadKey();
//        }//end main
//    }
//}