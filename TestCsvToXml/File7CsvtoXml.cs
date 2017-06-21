
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Text;

namespace File7Namespace
{
    internal class File7CsvtoXml
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
            DateTime lastUpdatedDate = Convert.ToDateTime(properties[1]);
            var propertyValue = new XElement("Value", ((DateTime)lastUpdatedDate).ToString("dd/MM/yyyy"));

            property.Add(propertyName);
            property.Add(propertyValue);
            responceProperties.Add(property);
            responce.Add(responceProperties);

            var Lists = new XElement("Lists");
            var Individuals = new XElement("Individuals");
            var Entities = new XElement("Entities");
            for (int i = 2; i < source.Length; i++)
            {
                var fields1 = source[i].Remove(0, 1).Split(',');
                var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

                //var result = Regex.Split(fields[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

                // tfp.Delimiters = new string[] { "," };
                if (fields[47] == "Individual")
                {
                    XElement indivi = individualRowCreator(fields, source);
                    if (!indivi.IsEmpty)
                    {
                        Individuals.Add(indivi);
                    }//end if
                }
                else if (fields[47] == "Entity")
                {
                    XElement entit = entityRowCreator(fields, source);
                    if (!entit.IsEmpty)
                    {
                        Entities.Add(entit);
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
                                                        new XElement("FirstName", fields[3].Trim()),
                                                        new XElement("LastName", fields[1].Trim()));
                XElement PersonAKAs = new XElement("PersonAKAs");
                if (aka.Count != 0)
                {
                    for (int i = 0; i < aka.Count; i += 2)
                    {
                        PersonAKAs.Add(new XElement("PersonName",
                                            new XElement("FirstName", aka[i].Trim()),
                                            new XElement("LastName", aka[i + 1].Trim())));
                    }//end for i
                }//end if
                IndividualReference.Add(PersonName);
                if (aka.Count != 0)
                {
                    IndividualReference.Add(PersonAKAs);
                }//end if
                //documents section
                XElement IndividualDocuments = new XElement("IndividualDocuments");
                if (fields[23].Length > 0)
                {
                    XElement PersonDocument = new XElement("PersonDocument");
                    XElement PersonDucomentOtherDetails = new XElement("PersonDocumentOtherDetails");
                    XElement PersonDocumentOtherDetail = new XElement("PersonDocumentOtherDetail",
                                                            new XElement("Name", "Passport"),
                                                            new XElement("Detail", fields[23]));
                    PersonDucomentOtherDetails.Add(PersonDocumentOtherDetail);
                    PersonDocument.Add(PersonDucomentOtherDetails);
                    IndividualDocuments.Add(PersonDocument);
                }//end if
                //other details secion
                XElement IndividualOtherDetails = new XElement("IndividualOtherDetails");
                //declaring an int array with oterDetailsIndexes
                int[] otherDetailsIndexes = { 13, 17, 19, 25, 27, 41, 45, 55, 57 };
                string[] fieldNames = { "Title", "TownOfBirth", "CountryOfBirth", "NINumber", "Position", "PostalZipCode", "OtherInfo", "LastUpdated", "GroupID" };
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
                        if (fieldNames[a] == "CountryOfBirth")
                        {
                            IndividualOtherDetails.Add(new XElement("IndividualOtherDetail",
                                                        new XElement("Detail",
                                                            new XElement("Name", fieldNames[a]),
                                                            new XElement("Value", RemoveSpecialCharacters(otherDetailsValues[a]).Trim()))
                                                               )
                                                   );
                        }//end inner if
                        else if (fieldNames[a] == "LastUpdated")
                        {
                            IndividualOtherDetails.Add(new XElement("IndividualOtherDetail",
                                                        new XElement("Detail",
                                                            new XElement("Name", fieldNames[a]),
                                                            new XElement("Value", reformatDate(otherDetailsValues[a])))
                                                               )
                                                   );
                        }//end inner elseif
                        else
                        {
                            IndividualOtherDetails.Add(new XElement("IndividualOtherDetail",
                                                            new XElement("Detail",
                                                                new XElement("Name", fieldNames[a]),
                                                                new XElement("Value", otherDetailsValues[a]))
                                                                   )
                                                       );
                        }//end inner else
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
                if (fields[23].Length > 0)
                    Individual.Add(IndividualDocuments);
                return Individual;
            }
            return null;
        } //end individualRowCreator

        public static string reformatDate(string date)
        {
            //the incoming date is of the format dd/MM/yyyy (can also be d/M/yyyy)
            string[] dateChunks = date.Split('/');
            if (dateChunks[0].Length == 1)
            {
                dateChunks[0] = "0" + dateChunks[0];
            }//end if
            if (dateChunks[1].Length == 1)
            {
                dateChunks[1] = "0" + dateChunks[1];
            }//end if
            return dateChunks[0] + "/" + dateChunks[1] + "/" + dateChunks[2];
        }//end reformatDate

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }


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
                string street = "", buldg = "", province = "", other = "", city = "";
                if (count == -1 && count1 == -1)
                {
                    break;
                }
                var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
                if (fields[49] == "Prime Alias" && fields[57] == gid)
                {
                    //country
                    Address.Add(fields[43]);
                    //ZipCode
                    Address.Add(fields[41]);
                    //state province field[39]
                    if (fields[39].Contains("PO Box")) { other += fields[39]; }
                    else if (fields[39].Contains("Street"))
                    { street += fields[39]; }
                    else if (fields[39].Contains("Building") || fields[39].Contains("Bldg"))
                    { buldg += fields[39]; }
                    else { province += fields[39]; }
                    //City field[37]
                    if (fields[37].Contains("PO Box")) { other += fields[37]; }
                    else if (fields[37].Contains("Street"))
                    { street += fields[37]; }
                    else if (fields[37].Contains("Building") || fields[37].Contains("Bldg"))
                    { buldg += fields[37]; }
                    else { city += fields[37]; }

                    //For field[29]
                    if (fields[29].Contains("Street"))
                    { street += fields[29]; }
                    else if (fields[29].Contains("Building") || fields[29].Contains("Bldg"))
                    { buldg += fields[29]; }
                    else if (fields[29].Contains("Province"))
                    { province += fields[29]; }
                    else { other += fields[29]; }
                    //For field[31]
                    if (fields[31].Contains("Street"))
                    { street += fields[31]; }
                    else if (fields[31].Contains("Building") || fields[31].Contains("Bldg"))
                    { buldg += fields[31]; }
                    else if (fields[31].Contains("Province"))
                    { province += fields[31]; }
                    else { other += fields[31]; }
                    //For field[33]
                    if (fields[33].Contains("Street"))
                    { street += fields[33]; }
                    else if (fields[33].Contains("Building") || fields[33].Contains("Bldg"))
                    { buldg += fields[33]; }
                    else if (fields[33].Contains("Province"))
                    { province += fields[33]; }
                    else { other += fields[33]; }
                    //for field [35]
                    if (fields[35].Contains("Street"))
                    { street += fields[35]; }
                    else if (fields[35].Contains("Building") || fields[35].Contains("Bldg"))
                    { buldg += fields[35]; }
                    else if (fields[35].Contains("Province"))
                    { province += fields[35]; }
                    else { other += fields[35]; }
                    //
                    Address.Add(province);
                    Address.Add(city);
                    Address.Add(street);
                    Address.Add(buldg);
                    Address.Add(other);
                    Address.Add("");
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
                    EntSynonyms.Add(new XElement("Synonmys", aka[i].Trim('"')));
                }
                //Not required for this document 
                //XElement EntAbbre = new XElement("EntityAbbreviations",
                //                            new XElement("Abbreviations"));//ask client

                EntRef.Add(EntName);
                EntRef.Add(EntSynonyms);
                //    EntRef.Add(EntAbbre);
                //adding in Entity
                Entity.Add(EntRef);
                ///

                if (fields[43].Length != 0)
                {
                    XElement EntReg = new XElement("EntityRegistrations",
                                        new XElement("EntityRegistration",
                                            new XElement("Country", fields[43]))
                        //new XElement("Date"),//ask client
                        //new XElement("Number"))//ask client

                                            );
                    //adding in Entity
                    Entity.Add(EntReg);
                    //
                }

                //changes here to enter multiple addresses

                if (addr.Count > 0)
                {
                    XElement EntAddrs = new XElement("EntityAddresses");
                    XElement EntAddr = new XElement("EntityAddress");

                    for (int i = 0; i < addr.Count; i += 8)
                    {
                        //Ali don't hate me plz
                        if (addr[0 + i].Length != 0)
                        {
                            XElement Entcon = new XElement("Country", addr[0 + i]);
                            EntAddr.Add(Entcon);
                        }
                        if (addr[1 + i].Length != 0)
                        {
                            XElement Entzip = new XElement("ZipCode", addr[1 + i]);
                            EntAddr.Add(Entzip);
                        }
                        if (addr[2 + i].Length != 0)
                        {
                            XElement Entstat = new XElement("StateProvince", addr[2 + i]);
                            EntAddr.Add(Entstat);
                        }
                        if (addr[3 + i].Length != 0)
                        {
                            XElement Entcity = new XElement("City", addr[3 + i]);
                            EntAddr.Add(Entcity);
                        }
                        if (addr[4 + i].Length != 0)
                        {
                            XElement Entstret = new XElement("Street", addr[4 + i]);
                            EntAddr.Add(Entstret);
                        }
                        if (addr[5 + i].Length != 0)
                        {

                            XElement Entbui = new XElement("Building", addr[5 + i]);
                            EntAddr.Add(Entbui);
                        }
                        if (addr[6 + i].Length != 0 || addr[7 + i].Length != 0)
                        {
                            XElement Entother = new XElement("AddressOtherDetails",
                                 new XElement("Detail",
                                     new XElement("Name", "Address"),
                                new XElement("Value", addr[6 + i] + addr[7 + i])));
                            EntAddr.Add(Entother);
                        }


                    }
                    EntAddrs.Add(EntAddr);

                    //adding in the entity list
                    Entity.Add(EntAddrs);
                    //
                }

                if (fields[53].Length != 0)
                {
                    XElement Entlist = new XElement("Listed", fields[53]);
                    Entity.Add(Entlist);
                }
                //XElement EntDelist = new XElement("DeListed");//ask client
                if (fields[51].Length != 0)
                {
                    XElement EntResn = new XElement("ReasonDisclosures",
                                            new XElement("ReasonDisclosure",
                                                new XElement("Name", "Regime"),
                                                new XElement("Value", fields[51]))
                                            );
                    Entity.Add(EntResn);

                }




                //  Entity.Add(EntDelist);

                //
                //After using global Address and Aka
                Address = null;
                Aka = null;
            }
            //  }//end for i
            return Entity;
        } //end rowCreator

        //

        ////
        public static void File7(string filename, string datetime)
        {

            /*
             * All the below lines are to be enclosed in an action function
             * and are to be executed at a specific time using TaskScheduler
             * Please refer to: https://msdn.microsoft.com/en-us/library/system.threading.tasks.taskscheduler(v=vs.110).aspx
             * for more details
             */

            var watch = System.Diagnostics.Stopwatch.StartNew();

            string[] source = File.ReadAllLines(filename);

            XDocument doc = ConvertCsvToXML(source);
            doc.Save("outputxml.xml");

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            //System.Console.ReadKey();

        }
    }
}


