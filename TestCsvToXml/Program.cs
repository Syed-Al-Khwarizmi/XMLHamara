////using System;
////using System.IO;
////using System.Xml.Linq;
////using System.Collections.Generic;

////namespace TestCsvToXml
////{
////    internal class Program
////    {
////        /// <summary>
////        /// Simple test conversion
////        /// </summary>
////        /// 

////        private static void Sort<T>(T[][] data, int col)
////        {
////            Comparer<T> comparer = Comparer<T>.Default;
////            Array.Sort<T[]>(data, (x, y) => comparer.Compare(x[col], y[col]));
////        }

////        public static XDocument ConvertCsvToXML(string[] source) {
////            //split the rows

////            //Create the declaration
////            var doc = new XDocument(
////                new XDeclaration("1.0", "UTF-8", "yes"));
////            var lists = new XElement("Lists"); //Create the root
////            for (int i = 2; i < 200; i++)
////            {
////                var fields = source[i].Split(',');
////                    lists.Add(individualRowCreator(fields));
////            }
////            doc.Add(lists);
////            return doc;
////        }


////        private static XElement individualRowCreator(string[] fields) {
////            XElement Individuals = new XElement("Individuals");
////            XElement Individual = new XElement("Individual",
////                                        new XElement("IndividualReference"),
////                                            new XElement("PersonName",
////                                                new XElement("FirstName", fields[1]),
////                                                new XElement("LastName", fields[0]),
////                                                new XElement("OtherNamePart",
////                                                    new XElement("namePart", fields[2]),
////                                                    new XElement("namePart", fields[3]),
////                                                    new XElement("namePart", fields[4]),
////                                                    new XElement("namePart", fields[5])
////                                                    )
////                                                ),
////                                                    new XElement("OtherNameDetails",
////                                                        new XElement("OtherNameDetail", fields[6])
////                                                        )
////                                            );
////                XElement dob = new XElement("DateBirth",
////                                    new XElement("kind", "DD/MM/YYYY"),
////                                    new XElement("DateValue", fields[7]));
////                Individuals.Add(Individual);
////                Individuals.Add(dob);
////            return Individuals;
////        } //end individualRowCreator
////        private static void Main()
////        {
////            //string csv = File.ReadAllText("C:\\Users\\Ali_H\\Desktop\\TestCsvToXml\\sanctionsconlist.csv");
////            //XDocument doc = ConversorCsvXml.ConvertCsvToXML(csv, new[] {","});
////            //doc.Save("bundPaar2.xml");
////            ////Console.WriteLine(doc.Declaration);
////            ////foreach (XElement c in doc.Elements())
////            ////{
////            ////    Console.WriteLine(c);
////            ////}
////            ////Console.ReadLine();

////            string[] source = File.ReadAllLines("C:\\Users\\Ali_H\\Desktop\\TestCsvToXml\\sanctionsconlist.csv");
////            string[][] source2 = new string[200-2][];
////            for (int i = 2; i < 200; i++) {
////                source2[i-2] = new string[source[i].Length];
////                source2[i-2] = source[i].Split(',');
////            }
////            Sort<string>(source2, 23);
////            XDocument doc = ConvertCsvToXML(source);
////            doc.Save("outputxml.xml");


////            //XElement Individuals = new XElement("Individuals");
////            //for (int i = 2; i < 200; i++) { // i = 2, because discarding the first two rows
////            //    string[] fields = source[i].Split(',');
////            //    Individuals.Add(rowCreator(fields));
////            //}
////            //doc.Add(Individuals);
////            //doc.Save("bundPaar2.xml");
////        }
////    }
////}





/////////////////////////////#################################///////////////////////////////////////////




////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using System.IO;
////using System.Xml.Linq;

////namespace UzairKaProject
////{
////    class Program
////    {
////        static void Main(string[] args)
////        {
////            string[] source = File.ReadAllLines("C:\\Users\\Uzair Tahir\\Downloads\\sanctionsconlist.csv");
////            XElement individual = new XElement("Individuals",
////                from str in source
////                let fields = str.Split(',')
////            //    from type in fields[23]
////                where fields[23] == "Individual"

////                select new XElement("Individual",
////                    new XElement("IndividualReference",
////                        new XElement("PersonName",
////                            new XElement("FirstName", fields[1]),
////                            new XElement("LastName", fields[0]),
////                            new XElement("otherNameParts",
////                                new XElement("namePart2", fields[2]),
////                                new XElement("namePart3", fields[3]),
////                                new XElement("namePart4", fields[4]),
////                                new XElement("namePart5", fields[5])
////                                )
////                            )
////                        ),
////                    new XElement("CompanyName", fields[1]),
////                    new XElement("ContactName", fields[2]),
////                    new XElement("ContactTitle", fields[3]),
////                    new XElement("Phone", fields[4]),
////                    new XElement("FullAddress",
////                        new XElement("Address", fields[5]),
////                        new XElement("City", fields[6]),
////                        new XElement("Region", fields[7]),
////                        new XElement("PostalCode", fields[8]),
////                        new XElement("Country", fields[9])
////                    )
////                )
////            );
////            using (StreamWriter sw = File.CreateText("bundPaar.xml"))
////            {
////                sw.WriteLine(individual);
////            }
////        }//end main
////    }//end class
////}//end namespace
///////////////////////////////

////using System;
////using System.IO;
////using System.Text.RegularExpressions;
////using System.Xml;
////using System.Xml.Linq;
//////using Microsoft.Xml;


////namespace TestCsvToXml
////{
////    internal class Program
////    {
////        /// <summary>
////        /// Simple test conversion
////        /// </summary>
////        /// 


////        public static XDocument ConvertCsvToXML(string[] source)
////        {
////            //split the rows

////            //Create the declaration
////            var doc = new XDocument(
////                new XDeclaration("1.0", "UTF-8", "yes"));
////            var responce = new XElement("Entities"); //Create the root
////            for (int i = 1; i < 200; i++)
////            {
////                var fields1 = source[i].Remove(0, 1).Split(',');
////                var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

////                //var result = Regex.Split(fields[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

////                // tfp.Delimiters = new string[] { "," };
////                responce.Add(rowCreator(fields));
////            }
////            doc.Add(responce);
////            return doc;
////        }


////        private static XElement rowCreator(string[] fields)
////        {
////            XElement Entity = new XElement("Entity");
////            //for (int i = 0; i < fields[i].Length; i++)
////            //{
////            if (fields[47] != "Entity")
////            {
////                //this means it's an entity
////                //  continue;
////            }
////            else
////            {
////                XElement EntRef = new XElement("EntityReference",
////                                        new XElement("EntityName", fields[1]),
////                                        new XElement("Entity Synonmys",
////                                           new XElement("Synonmys", fields[3])),//ask client
////                                        new XElement("EntityAbbreviations",
////                                            new XElement("Abbreviations", fields[4]))//ask client
////                                    );
////                XElement EntReg = new XElement("EntityRegistrations",
////                                    new XElement("EntityRegistration",
////                                        new XElement("Country",fields[0]),
////                                        new XElement("Date", fields[0]),//ask client
////                                        new XElement("Number", fields[0]))//ask client

////                                        );
////                XElement EntAddr = new XElement("EntityAddresses",
////                                        new XElement("EntityAddress",
////                                            new XElement("Country", fields[0]),
////                                            new XElement("ZipCode", fields[0]),
////                                            new XElement("StateProvince", fields[0]),
////                                            new XElement("City", fields[0]),
////                                            new XElement("Street", fields[0]),
////                                            new XElement("Building", fields[0]),
////                                            new XElement("AddressOtherDetails",
////                                                new XElement("Detail", fields[0])))
////                                                );

////                XElement Entlist = new XElement("Listed", fields[0]);

////                XElement EntDelist = new XElement("DeListed", fields[0]);//ask client

////                XElement EntResn = new XElement("ReasonDisclousers",
////                                        new XElement("ReasonDisclouser", fields[0])
////                                        );



////                Entity.Add(EntRef);
////                Entity.Add(EntReg);
////                Entity.Add(EntAddr);
////                Entity.Add(Entlist);
////                Entity.Add(EntDelist);
////                Entity.Add(EntResn);
////            }
////            //  }//end for i
////            return Entity;
////        } //end rowCreator

////        //

////        //
////        private static void Main()
////        {
////            //string csv = File.ReadAllText("C:\\Users\\Uzair Tahir\\Downloads\\sanctionsconlist.csv");
////            //XDocument doc = ConversorCsvXml.ConvertCsvToXML(csv, new[] {","});
////            //doc.Save("bundPaar2.xml");
////            ////Console.WriteLine(doc.Declaration);
////            ////foreach (XElement c in doc.Elements())
////            ////{
////            ////    Console.WriteLine(c);
////            ////}
////            ////Console.ReadLine();

////            string[] source = File.ReadAllLines("C:\\Users\\Ali_H\\Desktop\\TestCsvToXml\\sanctionsconlist.csv");

////            XDocument doc = ConvertCsvToXML(source);
////            doc.Save("outputxml.xml");

////            //
////            //XmlDocument doc1 = new XmlDocument();
////            //XmlCsvReader reader = new XmlCsvReader(new Uri("file:///C:/My%20Download%20Files/XmlCsvReader/XmlCsvReader/input.csv"), doc1.NameTable);
////            //reader.FirstRowHasColumnNames = true;
////            //reader.RootName = "customers";
////            //reader.RowName = "customer";

////            //doc1.Load(reader);
////            //Console.WriteLine(doc1.OuterXml);
////            //doc1.Save("output.xml"); 
////            ////
////            //XElement Individuals = new XElement("Individuals");
////            //for (int i = 2; i < 200; i++) { // i = 2, because discarding the first two rows
////            //    string[] fields = source[i].Split(',');
////            //    Individuals.Add(rowCreator(fields));
////            //}
////            //doc.Add(Individuals);
////            //doc.Save("bundPaar2.xml");
////        }
////    }
////}


////C:\\Users\\Ali_H\\Desktop\\TestCsvToXml\\sanctionsconlist.csv

//////////////////////////////////////////##############################//////////////////////////////
//using System;
//using System.IO;
//using System.Text.RegularExpressions;
//using System.Xml;
//using System.Xml.Linq;
////using Microsoft.Xml;


//namespace TestCsvToXml
//{
//    internal class Program
//    {
//        /// <summary>
//        /// Simple test conversion
//        /// </summary>
//        /// 


//        public static XDocument ConvertCsvToXML(string[] source)
//        {
//            //split the rows

//            //Create the declaration
//            var doc = new XDocument(
//                new XDeclaration("1.0", "UTF-8", "yes"));
//            var responce = new XElement("Lists"); //Create the root
//            var Individuals = new XElement("Individuals");
//            var Entities = new XElement("Entities");
//            for (int i = 2; i < 200; i++)
//            {
//                var fields1 = source[i].Remove(0, 1).Split(',');
//                var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
//                for (int j = 0; i < fields.Length; i++)
//                {
//                    Console.WriteLine(i + " " + fields[i]);
//                }

//                //var result = Regex.Split(fields[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

//                // tfp.Delimiters = new string[] { "," };
//                if (individualRowCreator(fields) != null)
//                {
//                    Individuals.Add(individualRowCreator(fields));
//                }//end if
//            }

//            for (int i = 2; i < 200; i++)
//            {
//                var fields1 = source[i].Remove(0, 1).Split(',');
//                var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

//                //var result = Regex.Split(fields[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

//                // tfp.Delimiters = new string[] { "," };
//                if (entityRowCreator(fields) != null)
//                {
//                    Entities.Add(entityRowCreator(fields));
//                }//end if
//            }
//            responce.Add(Individuals);
//            responce.Add(Entities);
//            doc.Add(responce);
//            return doc;
//        }



//        private static XElement individualRowCreator(string[] fields)
//        {
//            if (fields[47] == "Individual")
//            {
//                //XElement Individuals = new XElement("Individuals");
//                XElement Individual = new XElement("Individual");
//                            XElement IndivRef = new XElement("IndividualReference",
//                                                new XElement("PersonName",
//                                                    new XElement("FirstName", fields[1]),
//                                                    new XElement("LastName", fields[0]),
//                                                    new XElement("OtherNamePart",
//                                                        new XElement("namePart", fields[2]),
//                                                        new XElement("namePart", fields[3]),
//                                                        new XElement("namePart", fields[4]),
//                                                        new XElement("namePart", fields[5])
//                                                        )
//                                                    ),
//                                                        new XElement("OtherNameDetails",
//                                                            new XElement("OtherNameDetail", fields[6])
//                                                            ),
//                                                        new XElement("Nationality", fields[43]),
//                                                        new XElement("IndividualDocument", 
//                                                            new XElement("PersonDocument", 
//                                                                new XElement("Country", fields[43]),
//                                                                new XElement("Number", 25),
//                                                                new XElement("DateIssue", fields[53])),
//                                                                new XElement("PersonDocumentOtherDetails",
//                                                                    new XElement("PersonDocumentOtherDetail",
//                                                                        new XElement("Detail", fields[45])))),
//                                                        new XElement("IndividualOtherDetails",
//                                                            new XElement("IndividualOtherDetail",
//                                                                new XElement("Detail", fields[45]))),
//                                                        new XElement("Listed", fields[53]),
//                                                        new XElement("Delisted",fields[55]),
//                                                        new XElement("ReasonDisclosures",
//                                                            new XElement("ReasonDisclosure", fields[51]))

//                                                );
//                        XElement dob = new XElement("DateBirth",
//                                    new XElement("kind", "DD/MM/YYYY"),
//                                    new XElement("DateValue", fields[7]));
//                Individual.Add(IndivRef);
//                Individual.Add(dob);
//                return Individual;
//            }
//            return null;
//        } //end individualRowCreator



//        private static XElement entityRowCreator(string[] fields)
//        {
//            //for (int i = 0; i < fields[i].Length; i++)
//            //{
//            if (fields[47] == "Entity")
//            {
//                //XElement Entities = new XElement("Entities");
//                XElement Entity = new XElement("Entity");
//                      XElement EntRef =  new XElement("EntityReference",
//                                            new XElement("EntityName", fields[1]),
//                                            new XElement("EntitySynonmys",
//                                                new XElement("Synonmys", fields[3])),//ask client
//                                            new XElement("EntityAbbreviations",
//                                                new XElement("Abbreviations", fields[3])),
//                                        new XElement("EntityRegistrations",
//                                            new XElement("EntityRegistration",
//                                            new XElement("Country", fields[43]),
//                                            new XElement("Date", fields[45]),//ask client
//                                            new XElement("Number", fields[45]))//ask client
//                                        ),
//                                        new XElement("EntityAddresses",
//                                        new XElement("EntityAddress",
//                                            new XElement("Country", fields[43]),
//                                            new XElement("ZipCode", fields[41]),
//                                            new XElement("StateProvince", fields[39]),
//                                            new XElement("City", fields[37]),
//                                            new XElement("Street", fields[35]),
//                                            new XElement("Building", fields[33]),
//                                            new XElement("AddressOtherDetails",
//                                                new XElement("Detail", fields[31])))
//                                            ),
//                                        new XElement("Listed", fields[53]),
//                                        new XElement("DeListed", fields[55]),
//                                        new XElement("ReasonDisclousers",
//                                            new XElement("ReasonDisclouser", fields[57])
//                                        )

//                    );
//                Entity.Add(EntRef);
//                return Entity;
//            }
//            return null;
//        } //end rowCreator

//        //

//        //
//        private static void Main()
//        {
//            //string csv = File.ReadAllText("C:\\Users\\Uzair Tahir\\Downloads\\sanctionsconlist.csv");
//            //XDocument doc = ConversorCsvXml.ConvertCsvToXML(csv, new[] {","});
//            //doc.Save("bundPaar2.xml");
//            ////Console.WriteLine(doc.Declaration);
//            ////foreach (XElement c in doc.Elements())
//            ////{
//            ////    Console.WriteLine(c);
//            ////}
//            ////Console.ReadLine();

//            string[] source = File.ReadAllLines("C:\\Users\\Ali_H\\Desktop\\TestCsvToXml\\sanctionsconlist.csv");

//            XDocument doc = ConvertCsvToXML(source);
//            doc.Save("outputxml.xml");

//            //
//            //XmlDocument doc1 = new XmlDocument();
//            //XmlCsvReader reader = new XmlCsvReader(new Uri("file:///C:/My%20Download%20Files/XmlCsvReader/XmlCsvReader/input.csv"), doc1.NameTable);
//            //reader.FirstRowHasColumnNames = true;
//            //reader.RootName = "customers";
//            //reader.RowName = "customer";

//            //doc1.Load(reader);
//            //Console.WriteLine(doc1.OuterXml);
//            //doc1.Save("output.xml"); 
//            ////
//            //XElement Individuals = new XElement("Individuals");
//            //for (int i = 2; i < 200; i++) { // i = 2, because discarding the first two rows
//            //    string[] fields = source[i].Split(',');
//            //    Individuals.Add(rowCreator(fields));
//            //}
//            //doc.Add(Individuals);
//            //doc.Save("bundPaar2.xml");
//        }
//    }
//}





///////////////////////////////////###############################////////////////////////////////////


using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace TestCsvToXml
{
    internal class Program
    {
        /// <summary>
        /// Simple test conversion
        /// </summary>
        /// 
        //public static int 200 = 150; //replacing this over 200 in the code for the sake of testing the output

        public static XDocument ConvertCsvToXML(string[] source)
        {
            //split the rows

            //Create the declaration
            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"));
            var responce = new XElement("Response"); //Create the root
            string[] responceAttributes = { "Source", "CountryReferences", "TimeStart" }; //TimeAnswer will be added later
            string[] responceAttributesValues = { "HMTreasury", "Free Country Name", (DateTime.Now).ToString(new CultureInfo("en-GB")) };

            for (int i = 0; i < responceAttributes.Length; i++)
            {
                responce.SetAttributeValue(responceAttributes[i], responceAttributesValues[i]);
            }//end for i
            var responceProperties = new XElement("ResponseProperties");
            var property = new XElement("Property");

            string[] properties = source[0].Split(',');
            var propertyName = new XElement("Name", properties[0]);
            var propertyValue = new XElement("Value", properties[1]);

            property.Add(propertyName);
            property.Add(propertyValue);
            responceProperties.Add(property);
            responce.Add(responceProperties);

            var Lists = new XElement("Lists");
            var Individuals = new XElement("Individuals");
            var Entities = new XElement("Entities");
            for (int i = 2; i < 200; i++)
            {
                var fields1 = source[i].Remove(0, 1).Split(',');
                var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

                //var result = Regex.Split(fields[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

                // tfp.Delimiters = new string[] { "," };
                if (fields[47] == "Individual")
                {
                    if (!individualRowCreator(fields, source).IsEmpty)
                    {
                        Individuals.Add(individualRowCreator(fields, source));
                    }//end if
                }
                else if (fields[47] == "Entity")
                {
                    if (!entityRowCreator(fields, source).IsEmpty)
                    {
                        Entities.Add(entityRowCreator(fields, source));
                    }//end if
                }
                // System.Console.WriteLine(Individuals);
            }

            //for (int i = 2; i < 200; i++)
            //{
            //    var fields1 = source[i].Remove(0, 1).Split(',');
            //    var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

            //    //var result = Regex.Split(fields[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

            //    // tfp.Delimiters = new string[] { "," };
            //    if (!entityRowCreator(fields, source).IsEmpty)
            //    {
            //        Entities.Add(entityRowCreator(fields, source));
            //    }//end if
            //}
            Lists.Add(Individuals);
            Lists.Add(Entities);
            responce.Add(Lists);
            responce.SetAttributeValue("TimeAnswer", (DateTime.Now).ToString(new CultureInfo("en-GB")));
            doc.Add(responce);
            return doc;
        }



        private static XElement individualRowCreator(string[] fields, string[] source)
        {
            if (fields[47] == "Individual")
            {

                //Getting all address
                //   List<string> addr = getallAddresses(fields[57], source);
                //Getting all Aka
                //  List<string> Aka = getallAKA(fields[57], source);
                //
                //XElement Individuals = new XElement("Individuals");

                //new added 5-june-17 
                getallAddressAndAka(fields[57], source);
                List<string> addr = Address;
                //Getting all Aka
                List<string> aka = Aka;
                ////TESTING CODE STARTS
                ////CHECKING THE LENGTHS(COUNTS) OF "aka"(WHETHER ALL ARE EVEN, OR NOT)
                //if (aka.Count % 2 != 0)
                //{
                //    System.Console.WriteLine("Odd length aka found on entry number {0}", fields[57]);
                //}//end if
                ////TESTING CODE ENDS
                //
                XElement Individual = new XElement("Individual");

                //IndividualReference, and childs
                XElement IndividualReference = new XElement("IndividualReference");
                XElement PersonName = new XElement("PersonName",
                                                        new XElement("FirstName", fields[3]),
                                                        new XElement("LastName", fields[1]));
                XElement PersonAKAs = new XElement("PersonAKAs");
                if (aka.Count != 0)
                {
                    for (int i = 0; i < aka.Count; i+=2)
                    {
                        PersonAKAs.Add(new XElement("PersonName",
                                            new XElement("FirstName", aka[i]),
                                            new XElement("LastName", aka[i + 1])));
                    }//end for i
                }//end if
                IndividualReference.Add(PersonName);
                if (aka.Count != 0)
                {
                    IndividualReference.Add(PersonAKAs);
                }//end if
                //other details secion
                XElement IndividualOtherDetails = new XElement("IndividualOtherDetails");
                //declaring an int array with oterDetailsIndexes
                int[] otherDetailsIndexes = { 13, 17, 19, 23, 25, 27, 41, 45, 55, 57 };
                string[] fieldNames = { "Title", "TownOfBirth", "CountryOfBirth", "PassportDetails", "NINumber", "Position", "PostalZipCode", "OtherInfo", "LastUpdated", "GroupID" };
                string[] otherDetailsValues = new string[fieldNames.Length];

                for (int x = 0; x < fieldNames.Length; x++)
                {
                    otherDetailsValues[x] = fields[otherDetailsIndexes[x]];
                }//end for x
                /*
                 * Title: 13, TownOfBirth: 17, CountryOfBirth: 19, PassportDetails: 23
                 * NI Number: 25, Position: 27, PostalZipCode: 41, OtherInfo: 45
                 * LastUpdated: 55, GroupID: 57
                 */
                for (int a = 0; a < otherDetailsValues.Length; a++)
                {
                    if (otherDetailsValues[a].Length > 1) //please dont hate me for this shit
                    {
                        IndividualOtherDetails.Add(new XElement("IndividualOtherDetail",
                                                        new XElement("Detail",
                                                            new XElement("Name", fieldNames[a]),
                                                            new XElement("Value", otherDetailsValues[a]))
                                                               )
                                                   );
                    }//end if
                }//end for a

                XElement DOB = new XElement("DateBirth");
                XElement kind = new XElement("Kind", dobKind(fields[15])[0]);
                XElement date = new XElement("DateValue", dobKind(fields[15])[1]);
                DOB.Add(kind);
                DOB.Add(date);
                IndividualReference.Add(DOB);
                IndividualReference.Add(IndividualOtherDetails);
                XElement listed = new XElement("Listed", fields[53]);
                XElement reasonDisclosures = new XElement("ReasonDisclosures",
                                                new XElement("ReasonDisclosure",
                                                    new XElement("Name", "REGIME"),
                                                    new XElement("Value", fields[51])));
                //Empty global Address and Aka
                Address = null;
                Aka = null;


                Individual.Add(IndividualReference);
                return Individual;
            }
            return null;
        } //end individualRowCreator


        public static string[] dobKind(string dob) //returns {kind, data}
        {
            string[] returningDate = new string[2];
            if (dob == "")
            {
                returningDate[0] = "";
                returningDate[1] = "";
                return returningDate;
            }
            string[] DOB = dob.Split('/'); //00/00/1980 becomes {00, 00, 1980}
            if (DOB[0] == "00")
            {
                if (DOB[1] == "00")
                {
                    returningDate[0] = "PART";
                    returningDate[1] = DOB[2];
                    return returningDate;
                }//end if
                else if (DOB[1] != "00")
                {
                    returningDate[0] = "PART";
                    returningDate[1] = DOB[1] + "/" + DOB[2];
                    return returningDate;
                }//end else if
            }//end if
            else
            {
                returningDate[0] = "EXACT";
                returningDate[1] = DOB[0] + "/" + DOB[1] + "/" + DOB[2];
                return returningDate;
            }
            return null;
        }//end dobKind

        public static List<string> Address;
        public static List<string> Aka;

        //To get all addresses
        //public static List<string> getallAddresses(string gid, string[] source)
        //{
        //    List<string> address = new List<string>();

        //    // string[] address = new string[59]; int j = 0;
        //    for (int i = 1; i < 200; i++)
        //    {

        //        var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
        //        if (fields[49] == "Prime Alias" && fields[57] == gid)
        //        {
        //            address.Add(fields[43]);
        //            address.Add(fields[41]);
        //            address.Add(fields[39]);
        //            address.Add(fields[37]);
        //            address.Add(fields[35]);
        //            address.Add(fields[33]);
        //            address.Add(fields[31]);
        //            address.Add(fields[29]);
        //        }


        //    }
        //    return address;

        //}
        //new update 5-june-17 6:45pm
        public static void getallAddressAndAka(string gid, string[] source)
        {
            Address = new List<string>();
            Aka = new List<string>();
            int count = -2, count1 = -2;

            // string[] address = new string[59]; int j = 0;
            for (int i = 1; i < 200; i++)
            {
                if (count == -1 && count1 == -1)
                {
                    break;
                }
                var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
                if (fields[49] == "Prime Alias" && fields[57] == gid)
                {
                    Address.Add(fields[43]);
                    Address.Add(fields[41]);
                    Address.Add(fields[39]);
                    Address.Add(fields[37]);
                    Address.Add(fields[35]);
                    Address.Add(fields[33]);
                    Address.Add(fields[31]);
                    Address.Add(fields[29]);
                    if (count == -2)
                        count = 0;
                    count++;
                }
                else if (fields[49] == "AKA" && fields[57] == gid)
                {
                    if (fields[47] == "Entity")
                    {
                        if (!Aka.Contains(fields[1]))
                            Aka.Add(fields[1]);
                    }
                    else if (fields[47] == "Individual")
                    {
                        if (!Aka.Contains(fields[1]) && !Aka.Contains(fields[0]))
                        {
                            //here you will get list which contain lastname0 firstname0 lastname1 firstname1 .....

                            Aka.Add(fields[1]);
                            Aka.Add(fields[3]);
                        }
                    }
                    if (count1 == -2)
                        count1 = 0;
                    count1++;
                }
                else
                {
                    if (count != -2)
                        count--;
                    if (count1 != -2)
                        count1--;


                }

            }

        }

        //

        //To get AKA
        //public static List<string> getallAKA(string gid, string[] source)
        //{
        //    List<string> aka = new List<string>();

        //    // string[] address = new string[59]; int j = 0;
        //    for (int i = 1; i < 200; i++)
        //    {

        //        var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
        //        if (fields[49] == "AKA" && fields[57] == gid)
        //        {
        //            if (!aka.Contains(fields[1]))
        //                aka.Add(fields[1]);

        //        }

        //    }
        //    return aka;

        //}
        private static XElement entityRowCreator(string[] fields, string[] source)
        {
            XElement Entity = new XElement("Entity");

            if (fields[47] == "Entity" && fields[49] == "Prime Alias")
            {
                //Getting all address
                //getallAddressAka();
                getallAddressAndAka(fields[57], source);
                List<string> addr = Address;
                //Getting all Aka
                List<string> aka = Aka;

                //

                XElement EntRef = new XElement("EntityReference");
                XElement EntName = new XElement("EntityName", fields[1]);
                XElement EntSynonyms = new XElement("EntitySynonmys");
                for (int i = 0; i < aka.Count; i++)
                {
                    EntSynonyms.Add(new XElement("Synonmys", aka[i]));
                }

                XElement EntAbbre = new XElement("EntityAbbreviations",
                                            new XElement("Abbreviations"));//ask client

                EntRef.Add(EntName);
                EntRef.Add(EntSynonyms);
                EntRef.Add(EntAbbre);

                XElement EntReg = new XElement("EntityRegistrations",
                                    new XElement("EntityRegistration",
                                        new XElement("Country", fields[43]),
                                        new XElement("Date"),//ask client
                                        new XElement("Number"))//ask client

                                        );
                //changes here to enter multiple addresses

                XElement EntAddrs = new XElement("EntityAddresses");
                XElement EntAddr = new XElement("EntityAddress");
                for (int i = 0; i < addr.Count; i += 8)
                {


                    EntAddr.Add(new XElement("Country", addr[0 + i]),
                     new XElement("ZipCode", addr[1 + i]),
                     new XElement("StateProvince", addr[2 + i]),
                     new XElement("City", addr[3 + i]),
                     new XElement("Street", addr[4 + i]),
                     new XElement("Building", addr[5 + i]),
                     new XElement("AddressOtherDetails",
                         new XElement("Detail", addr[6 + i] + addr[7 + i]))
                         );
                }
                EntAddrs.Add(EntAddr);

                XElement Entlist = new XElement("Listed", fields[53]);

                XElement EntDelist = new XElement("DeListed");//ask client

                XElement EntResn = new XElement("ReasonDisclousers",
                                        new XElement("ReasonDisclouser", fields[51])
                                        );



                Entity.Add(EntRef);
                Entity.Add(EntReg);
                Entity.Add(EntAddrs);
                Entity.Add(Entlist);
                Entity.Add(EntDelist);
                Entity.Add(EntResn);
                //
                //After using global Address and Aka
                Address = null;
                Aka = null;
            }
            //  }//end for i
            return Entity;
        } //end rowCreator

        //

        //
        private static void Main()
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();

            string[] source = File.ReadAllLines("C:\\Users\\Ali_H\\Desktop\\TestCsvToXml\\sanctionsconlist.csv");

            XDocument doc = ConvertCsvToXML(source);
            doc.Save("outputxml.xml");

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            System.Console.ReadKey();

        }
    }
}


