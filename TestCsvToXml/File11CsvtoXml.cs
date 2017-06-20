using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Text;
using System;
using System.Collections.Generic;

namespace File11Namespace
{
    class File11CsvtoXml
    {
        public static XDocument ConvertCsvToXML(string[] source)
        {
            //split the rows

            //Create the declaration
            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"));
            var responce = new XElement("Response"); //Create the root
            string[] responceAttributes = { "Source", "CountryReferences" }; 
            string[] responceAttributesValues = { "DefenseTradeControlsDebarredParties", "ISO 3166-1 Common Name" };

            for (int i = 0; i < responceAttributes.Length; i++)
            {
                responce.SetAttributeValue(responceAttributes[i], responceAttributesValues[i]);
            }//end for i
            
            var Lists = new XElement("Lists");
            var Individuals = new XElement("Individuals");
            var Entities = new XElement("Entities");
            for (int i = 3; i < source.Length; i++)
            {
               // var fields1 = source[i].Remove(0, 1).Split(',');
                var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

                //Checker Function
                string chk = checker(fields[1]);
                if (chk == "Individual")
                {
                    XElement indivi = individualRowCreator(fields, source);
                    if (!indivi.IsEmpty)
                    {
                        Individuals.Add(indivi);
                    }//end if
                }
                else if (chk == "Entity")
                {
                    XElement entit = entityRowCreator(fields, source);
                    if (!entit.IsEmpty)
                    {
                        Entities.Add(entit);
                    }//end if
                }
                 System.Console.WriteLine(i);
            }

            Lists.Add(Individuals);
            Lists.Add(Entities);
            responce.Add(Lists);
            doc.Add(responce);
            return doc;
        }

        private static string  checker(string field)
        {
            if(field.Contains("Inc.")||field.Contains("Ltd.")||field.Contains("Corp."))
            {
                return "Entity";
            }
            else
            {
                var field1 = field.Split(',');
                if (field1.Length == 1)
                { return "Entity"; }
                else if (field1.Length > 1)
                    return "Individual";
            }
            return null;
        }

        private static string [] getFirstLastName(string str)
        {
            string[] FLname = new string[2];
            var name = str.Split(',');
            if (name.Length == 2)
            {
                FLname[0] = name[0].Remove(0,1);
                FLname[1] = name[1];
                
            }
            else if (name.Length == 3)
            {
                FLname[0] = name[0].Remove(0,1);
                FLname[1] = name[2];
            }
            return FLname;
        }
        private static string[] getnameIndividual(string str)
        {
            string[] FLname = new string[2];
            if (str.Contains("a.k.a."))
            {
                var name = str.Split('(');
                FLname = getFirstLastName(name[0].Trim('"'));

            }
            else
            {
                FLname = getFirstLastName(str);
            }
            return FLname;
        }
        private static List<string> getAkasIndividual(string str)
        {
            List<string> Allaka = new List<string>();
            List<string> aka = getakas(str);
            for (int i=0;i<aka.Count;i++)
            {
                var subaka= aka[i].Split(' ');
                if (subaka.Length == 3)
                {
                    Allaka.Add(subaka[1]);
                    Allaka.Add(subaka[2]);
                }
                else if(subaka.Length==2)
                {
                    Allaka.Add(subaka[1]);
                    Allaka.Add(subaka[0]);
                }
                else if (subaka.Length==4)
                {
                    Allaka.Add(subaka[1] + " " + subaka[2]);
                    Allaka.Add(subaka[3]);
                }
                else if(subaka.Length==5)
                {
                    string fn = "";
                    for (int o=1;o<subaka.Length-1;o++)
                    {
                        fn += subaka[o]+" ";
                    }
                    Allaka.Add(fn);
                    Allaka.Add(subaka[subaka.Length-1]);
                }
            }
            return Allaka;
        }
        private static XElement individualRowCreator(string[] fields, string[] source)
        {
           
               
                List<string> aka = getAkasIndividual(fields[1]);
              
                //
                XElement Individual = new XElement("Individual");
                string[] name = new string[2];
                name = getnameIndividual(fields[1]);
                
                //IndividualReference, and childs
                XElement IndividualReference = new XElement("IndividualReference");
                XElement PersonName = new XElement("PersonName",
                                                        new XElement("FirstName", name[1].Trim('"').Remove(0,1)),
                                                        new XElement("LastName", name[0].Trim('"')));
                XElement PersonAKAs = new XElement("PersonAKAs");
                if (aka.Count != 0)
                {
                    for (int i = 0; i < aka.Count; i += 2)
                    {
                        XElement pn= (new XElement("PersonName"));
                        if(aka[i].Length>0)
                        pn.Add(new XElement("FirstName", aka[i].Trim('"')));
                        if(aka[i+1].Length>0)
                        pn.Add(new XElement("LastName", aka[i + 1].Trim('"')));

                        PersonAKAs.Add(pn);

                    }//end for i
                }//end if
                IndividualReference.Add(PersonName);
                if (aka.Count != 0)
                {
                    IndividualReference.Add(PersonAKAs);
                }//end if

            Individual.Add(IndividualReference);
            
               
                //Empty global Address and Aka
            //start
               string reDate = reArrangeDate(fields[7]);
                if (reDate.Length != 0)
                {

                    XElement indlist = new XElement("Listed", reDate);
                    Individual.Add(indlist);
                }

                if (fields[7].Length != 0 || fields[5].Length != 0 || fields[11].Length != 0 || fields[9].Length != 0)
                {
                    string fornotice;
                    string forcorrectednotice;
                    //notice
                    string year = getyear(fields[7]);
                    string code = removespace(fields[5]);
                    if (year != null && code != null)
                    {
                        fornotice = "www.pmddtc.state.gov/FR/" + year + "/" + code + ".pdf";
                    }
                    else
                        fornotice = "";
                    //correctednotice
                    string year1 = getyear(fields[11]);
                    string code1 = removespace(fields[9]);
                    if (year1 != null && code1 != null)
                    {
                        forcorrectednotice = "www.pmddtc.state.gov/FR/" + year1 + "/" + code1 + ".pdf";
                    }
                    else
                        forcorrectednotice = "";
                    //

                    string[] fieldNames = { "notice", "corrected notice" };
                    string[] otherDetailsValues = new string[fieldNames.Length];

                    XElement indResn = new XElement("ReasonDisclosures");


                    otherDetailsValues[0] = fornotice;
                    otherDetailsValues[1] = forcorrectednotice;

                    //
                    for (int a = 0; a < otherDetailsValues.Length; a++)
                    {
                        if (otherDetailsValues[a].Length > 1) //please dont hate me for this shit
                        {
                            XElement indRes = (new XElement("ReasonDisclosure",
                                                                  new XElement("Name", fieldNames[a]),
                                                                  new XElement("Value", otherDetailsValues[a]))
                                                               );
                            indResn.Add(indRes);
                        }//end if
                    }//end for a
                    //end
                    Individual.Add(indResn);
                }
                Address = null;
                Aka = null;


                
                //if (fields[23].Length > 0)
                //    Individual.Add(IndividualDocuments);
                return Individual;
            
            
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


        private static string getyear(string date)
        {

            if (date.Contains("/"))
            {
                var dateChunks = date.Split('/');

                return dateChunks[2];
            }
            else if (date.Contains("-"))
            {
                var dateChunks = date.Split('-');
                return dateChunks[2];
            }
            else
                return null;
        }
        private static string removespace(string str)
        {
           str= Regex.Replace(str, @"\s+", "");
            return str;
        }
        private static string reArrangeDate(string date)
        {
            if (date.Contains("/"))
            {
                var dateChunks = date.Split('/');
                string comDate = dateChunks[1] +"/"+ dateChunks[0] +"/"+ dateChunks[2];
                return comDate;
            }
            else if (date.Contains("-"))
            {
                var dateChunks = date.Split('-');
                string comDate = dateChunks[1] +"/"+ dateChunks[0]+"/"+ dateChunks[2];
                return comDate;
            }
            return null;
        }
        private static string getname (string str)
        {
            if (str.Contains("a.k.a."))
            {
                var name = str.Split('(');
                return name[0];
            }
            else
                return str;
        }
        private static List <string> getakas (string str)
        {
            List<string> akas = new List<string>();
            if (str.Contains("a.k.a."))
            {
                var name = str.Split('(');
                string subname = name[1].Substring(6, name[1].Length - 8);
                var eachname = subname.Split(';');
                for (int i = 0; i < eachname.Length; i++)
                    akas.Add(eachname[i]);

                return akas;
            }
            else
                return akas;
        }
        private static XElement entityRowCreator(string[] fields, string[] source)
        {
            XElement Entity = new XElement("Entity");

            
                //Getting all address
                //getallAddressAka();
               // getallAddressAndAka(fields[57], source);
                List<string> addr = Address;
                //Getting all Aka
                List<string> aka = getakas(fields[1]);

                //
                string name = getname(fields[1]);
                XElement EntRef = new XElement("EntityReference");
                XElement EntName = new XElement("EntityName", name.Trim('"'));
                XElement EntSynonyms = new XElement("EntitySynonmys");
                for (int i = 0; i < aka.Count; i++)
                {
                    EntSynonyms.Add(new XElement("Synonmys", aka[i].Trim('"')));
                }
            //    Not required for this document 
                //XElement EntAbbre = new XElement("EntityAbbreviations",
                //                            new XElement("Abbreviations"));//ask client

                EntRef.Add(EntName);
                EntRef.Add(EntSynonyms);
                //    EntRef.Add(EntAbbre);
                //adding in Entity
                Entity.Add(EntRef);
                ///

                string reDate = reArrangeDate(fields[7]);
                if (reDate.Length != 0)
                {

                    XElement Entlist = new XElement("Listed", reDate);
                    Entity.Add(Entlist);
                }
                //XElement EntDelist = new XElement("DeListed");//ask client
                if (fields[7].Length != 0 || fields[5].Length != 0 || fields[11].Length != 0 || fields[9].Length != 0)
                {
                    string fornotice;
                    string forcorrectednotice;
                    //notice
                    string year = getyear(fields[7]);
                    string code = removespace(fields[5]);
                    if (year != null && code != null)
                    {
                        fornotice = "www.pmddtc.state.gov/FR/" + year + "/" + code + ".pdf";
                    }
                    else
                        fornotice = "";   
                    //correctednotice
                    string year1 = getyear(fields[11]);
                    string code1 = removespace(fields[9]);
                    if (year1 != null && code1 != null)
                    {
                        forcorrectednotice = "www.pmddtc.state.gov/FR/" + year1 + "/" + code1 + ".pdf";
                    }
                    else
                        forcorrectednotice = "";
                    //

                    string[] fieldNames = { "notice", "corrected notice" };
                    string[] otherDetailsValues = new string[fieldNames.Length];

                    XElement EntResn = new XElement("ReasonDisclosures");

                    
                        otherDetailsValues[0] =fornotice ;
                        otherDetailsValues[1] = forcorrectednotice;
                   
                    //
                    for (int a = 0; a < otherDetailsValues.Length; a++)
                    {
                        if (otherDetailsValues[a].Length > 1) //please dont hate me for this shit
                        {
                            XElement EntRes = (new XElement("ReasonDisclosure",
                                                                  new XElement("Name", fieldNames[a]),
                                                                  new XElement("Value", otherDetailsValues[a]))
                                                               );
                            EntResn.Add(EntRes);
                        }//end if
                    }//end for a

                    //
                    Entity.Add(EntResn);

                }




                //  Entity.Add(EntDelist);

                //
                //After using global Address and Aka
                Address = null;
                Aka = null;
            
            //  }//end for i
            return Entity;
        } //end rowCreator

        //

        //
        public static void File11(string filename)
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
            doc.Save("File11output.xml");

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            //System.Console.ReadKey();

        }
    }
}
