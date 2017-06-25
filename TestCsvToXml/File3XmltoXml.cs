using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace File3Namespace
{
    class File3XmltoXml
    {
       
        private static List<string> getakaindi(List<string> lst)
        {
            List<string> lstaka = new List<string>();
            var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == "aka").Select(p => p.Index);
            foreach (var i in Indexes)
            {
                
                    lstaka.Add(lst[i + 9]);
                    lstaka.Add(lst[i + 11]);
                
            }
            return lstaka;
        }
        private static List<string> getResname(List<string> lst)
        {
            List<string> lstres = new List<string>();
            var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == "idType").Select(p => p.Index);
            foreach (var i in Indexes)
            {
                if (lst[i + 1] != "Passport" && lst[i + 1] != "Registration ID")
                {
                    lstres.Add(lst[i + 1]);
                   // lstres.Add(lst[i + 10]);
                }
            }
            return lstres;
        }
        private static List<string> getResval(List<string> lst)
        {
            List<string> lstres = new List<string>();
            var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == "idType").Select(p => p.Index);
            foreach (var i in Indexes)
            {
                if (lst[i + 1] != "Passport" && lst[i + 1] != "Registration ID")
                {
                    lstres.Add(lst[i + 3]);
                    // lstres.Add(lst[i + 10]);
                }
            }
            return lstres;
        }
        private static XElement individualRowCreator(List<string> fields)
        {
            //
            List<string> addr = null;
            //Getting all Aka
            List<string> aka = getakaindi(fields);
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

            if (fields[3] != "\n" || fields[5] != "\n")
            {
                XElement PersonName = new XElement("PersonName",
                                                        new XElement("FirstName", fields[3].Trim()),
                                                        new XElement("LastName", fields[5].Trim()));
                IndividualReference.Add(PersonName);
            }
            //
            //Individual AKA
            XElement PersonAKAs = new XElement("PersonAKAs");
            if (aka.Count != 0)
            {
                for (int i = 0; i < aka.Count; i += 2)
                {

                    //addition
                    if (aka[0] != "\n" || aka[1] != "\n")
                    {
                        XElement Personakaprt = new XElement("PersonName",
                                                                new XElement("FirstName", aka[0].Trim()),
                                                                new XElement("LastName", aka[1].Trim()));


                        PersonAKAs.Add(Personakaprt);
                    }
                    //end addition
                }//end for i
            }//end if

            if (aka.Count != 0)
            {
                IndividualReference.Add(PersonAKAs);
            }//end if

            Individual.Add(IndividualReference);
            //
            //DateBirth
            //Ali correct it
            if (fields.Contains("dateOfBirth"))
            {
               
                // XElement individualBirth = new XElement("DateBirth");
                int birIndex = fields.IndexOf("dateOfBirth");
                XElement DOB = new XElement("DateBirth");
                //yahan sey
                XElement kind = new XElement("Kind", "PART");
                XElement date = new XElement("DateValue", fields[birIndex + 1]);
                DOB.Add(kind);
                DOB.Add(date);
                Individual.Add(DOB);
                //  Individual.Add(individualBirth);
            }


            //

            //documents section
            XElement IndividualDocuments = new XElement("IndividualDocuments");
            if (fields.Contains("Passport"))
            {
                XElement PersonDocument = new XElement("PersonDocument");

                int conindex = fields.IndexOf("Passport");
                //country
                if (fields[conindex + 4] != "" )
                {
                    XElement personCon = new XElement("Country", fields[conindex + 4]);
                    PersonDocument.Add(personCon);
                }

                //Number
                if (fields[conindex + 2] != "")
                {
                    XElement personCon = new XElement("Number", fields[conindex + 2]);
                    PersonDocument.Add(personCon);
                }

                IndividualDocuments.Add(PersonDocument);
              //  Individual.Add(IndividualDocuments);

            }//end if
            //
            if (fields.Contains("Registration ID"))
            {
                XElement PersonDocument = new XElement("PersonDocument");
                string idnumber="", idcountry="", iddate="";
                int conindex = fields.IndexOf("Registration ID");
                //getting values
                { 
                    if(fields[conindex+1]=="idNumber")
                    { idnumber = fields[conindex + 1]; }
                    if (fields[conindex + 3] == "idCountry")
                    { idcountry = fields[conindex + 3]; }
                    if (fields[conindex + 5] == "issueDate")
                    { iddate = fields[conindex + 5]; }
                }
                //country
                if (idcountry!= "")
                {
                    XElement personCon = new XElement("Country", idcountry);
                    PersonDocument.Add(personCon);
                }

                //Number
                if (idnumber != "")
                {
                    XElement personCon = new XElement("Number", idnumber);
                    PersonDocument.Add(personCon);
                }
                //issueDate
                if (iddate != "")
                {
                    XElement personCon = new XElement("DateIssue", iddate);
                    PersonDocument.Add(personCon);
                }

                IndividualDocuments.Add(PersonDocument);
                

            }//end if
            if (fields.Contains("Registration ID") || fields.Contains("Passport"))
            {
                Individual.Add(IndividualDocuments);
            }
            //
            //other details secion
            XElement IndividualOtherDetails = new XElement("IndividualOtherDetails");
            //declaring an int array with oterDetailsIndexes
            if (fields.Contains("title"))
            {
                int indTitle = fields.IndexOf("title");
                int[] otherDetailsIndexes = { indTitle+1 };
                string[] fieldNames = { "TITLE" };
                string[] otherDetailsValues = new string[fieldNames.Length];
                for (int x = 0; x < fieldNames.Length; x++)
                {
                    otherDetailsValues[x] = fields[otherDetailsIndexes[x]];
                }//end for x

                for (int a = 0; a < otherDetailsValues.Length; a++)
                {
                    if (otherDetailsValues[a].Length > 0 && otherDetailsValues[a] != "\n") //please dont hate me for this shit
                    {

                        IndividualOtherDetails.Add(new XElement("IndividualOtherDetail",
                                                        new XElement("Detail",
                                                            new XElement("Name", fieldNames[a]),
                                                            new XElement("Value", otherDetailsValues[a]))
                                                               ));
                    }
                }
                Individual.Add(IndividualOtherDetails);

            }





            //Listed
            //if (listedDate != "" || listedDate != null)
            //{
            //    XElement listed = new XElement("Listed", listedDate);
            //    Individual.Add(listed);
            //}
            //Reason Disclousres  pdf , programme , remark


            //
            XElement reasonDisclosures = new XElement("ReasonDisclosures");
            //
            //getting index name
            List<string> resname = getResname(fields);
            List<string> resval = getResval(fields);
            if (fields.Contains("program"))
            {
                int proindex = fields.IndexOf("program");
                resname.Add("program");
                resval.Add(fields[proindex+1]);
            }
            //
            //
            //declaring an int array with oterDetailsIndexes
            //int[] reasonIndexes = { 1, 2, 3 };
            //string[] fieldNamesReason = { "pdf_link", "programme", "remark" };
            //string[] reasonIndexesValues = new string[fieldNamesReason.Length];
            //for (int x = 0; x < fieldNamesReason.Length; x++)
            //{
            //    reasonIndexesValues[x] = fields[reasonIndexes[x]];
            //}//end for x

            for (int a = 0; a < resname.Count ; a++)
            {
                if (resval[a].Length > 1)
                {

                    reasonDisclosures.Add(new XElement("ReasonDisclosure",
                                                    new XElement("Name", resname[a]),
                                                        new XElement("Value", resval[a]))
                                                           );
                }
            }
            Individual.Add(reasonDisclosures);
            //Empty global Address and Aka

            //Nationality
            if (fields.Contains("nationality"))
            {
                int citIndex = fields.IndexOf("nationality");
                XElement nation = new XElement("Nationality", fields[citIndex + 5]);
                Individual.Add(nation);
            }




            return Individual;

        } //end individualRowCreator
        
        public static List<int> getaddInd(List<string> lst)
        {
            var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == "address").Select(p => p.Index);
            List<int> countlst = new List<int>();
            foreach (var i in Indexes)
            {
                countlst.Add(i);

            }
            if(countlst.Count>0)
            {
                countlst.Add(countlst[countlst.Count - 1] + 12);
            }
            return countlst;
        }
     //public static List<string> getEntAddr(List<string> lst)
     //   {
     //       List<string> lstadd = new List<string>();
     //       string con = "", zip = "", state = "", city = "",addr1="",addr2="",addr3="";
     //       var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == "address").Select(p => p.Index);
     //       List<int> indexes = new List<int>();
     //       int counter = 0;
     //       foreach (var i in Indexes)
     //       {
     //           if (lst[i + 1] != "Passport" && lst[i + 1] != "Registration ID")
     //           {
     //              // lstres.Add(lst[i + 3]);
     //               indexes.Add(i);
     //               // lstres.Add(lst[i + 10]);
     //           }
     //           if(indexes.Count==1)
     //           {

     //               if(lst.Contains("country"))
     //               {
     //                   int ind = lst.IndexOf("country");
     //                   lstadd.Add("country");
     //                   lstadd.Add(lst[ind + 1]);
     //               }
     //               else if (lst.Contains("stateOrProvince"))
     //               {
     //                   int ind = lst.IndexOf("stateOrProvince");
     //                   lstadd.Add("stateOrProvince");
     //                   lstadd.Add(lst[ind + 1]);
     //               }
     //               else if (lst.Contains("stateOrProvince"))
     //               {
     //                   int ind = lst.IndexOf("stateOrProvince");
     //                   lstadd.Add("stateOrProvince");
     //                   lstadd.Add(lst[ind + 1]);
     //               }
     //               else if (lst.Contains("city"))
     //               {
     //                   int ind = lst.IndexOf("city");
     //                   lstadd.Add("city");
     //                   lstadd.Add(lst[ind + 1]);
     //               }
     //               else if (lst.Contains("postalCode"))
     //               {
     //                   int ind = lst.IndexOf("postalCode");
     //                   lstadd.Add("postalCode");
     //                   lstadd.Add(lst[ind + 1]);
     //               }

     //               else if (lst.Contains("address1"))
     //               {
     //                   int ind = lst.IndexOf("address1");
     //                   lstadd.Add("address1");
     //                   lstadd.Add(lst[ind + 1]);
     //               }

     //               else if (lst.Contains("address2"))
     //               {
     //                   int ind = lst.IndexOf("address2");
     //                   lstadd.Add("address2");
     //                   lstadd.Add(lst[ind + 1]);
     //               }

     //               else if (lst.Contains("address3"))
     //               {
     //                   int ind = lst.IndexOf("address3");
     //                   lstadd.Add("address3");
     //                   lstadd.Add(lst[ind + 1]);
     //               }
                    
     //           }
     //           else if (indexes.Count>1)
     //           {

     //           }
     //       }
     //       return lstadd;
     //   }

        private static XElement entityRowCreator(List<string>fields)
        {
            XElement Entity = new XElement("Entity");

            
                //Getting all address
                //getallAddressAka();
               // getallAddressAndAka(fields[57], source);
              //  List<string> addr = getEntAddr(fields);
                //Getting all Aka
                List<string> aka = getakaEntity(fields);

                //

                XElement EntRef = new XElement("EntityReference");

                int lstindex = fields.IndexOf("lastName");
                XElement EntName = new XElement("EntityName", fields[lstindex+1]);
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

                //if (fields[43].Length != 0)
                //{
                //    XElement EntReg = new XElement("EntityRegistrations",
                //                        new XElement("EntityRegistration",
                //                            new XElement("Country", fields[43]))
                //        //new XElement("Date"),//ask client
                //        //new XElement("Number"))//ask client

                //                            );
                //    //adding in Entity
                //    Entity.Add(EntReg);
                //    //
                //}

                //changes here to enter multiple addresses
                //counter for address in the entity
                List<int> indexadd = getaddInd(fields);

                if (indexadd.Count > 0)
                {
                    XElement EntAddrs = new XElement("EntityAddresses");

                    for (int k = 0; k < indexadd.Count-1; k++)
                    {
                      //  int times = Convert.ToInt32(fields[indexadd[k] + 1]);
                        XElement EntAddr = new XElement("EntityAddress");

                        for (int i = indexadd[k]; i < indexadd[k + 1]; i++)
                        {
                            if(i==fields.Count)
                            {
                                break;
                            }
                            //Ali don't hate me plz
                            if (fields[i] == "country")
                            {
                                XElement Entcon = new XElement("Country", fields[i + 1]);
                                EntAddr.Add(Entcon);
                            }
                            if (fields[i] == "postalCode")
                            {
                                XElement Entzip = new XElement("ZipCode", fields[i + 1]);
                                EntAddr.Add(Entzip);
                            }
                            if (fields[i] == "stateOrProvince")
                            {
                                XElement Entstat = new XElement("StateProvince", fields[i + 1]);
                                EntAddr.Add(Entstat);
                            }
                            if (fields[i] == "city")
                            {
                                XElement Entcity = new XElement("City", fields[i + 1]);
                                EntAddr.Add(Entcity);
                            }

                            if (fields[i] == "address1")
                            {
                                XElement Entother = new XElement("AddressOtherDetails",
                                     new XElement("Detail",
                                         new XElement("Name", "Address1"),
                                    new XElement("Value", fields[i + 1])));
                                EntAddr.Add(Entother);
                            }



                            if (fields[i] == "address2")
                            {
                                XElement Entother = new XElement("AddressOtherDetails",
                                     new XElement("Detail",
                                         new XElement("Name", "Address2"),
                                    new XElement("Value", fields[i + 1])));
                                EntAddr.Add(Entother);
                            }


                            if (fields[i] == "address3")
                            {
                                XElement Entother = new XElement("AddressOtherDetails",
                                     new XElement("Detail",
                                         new XElement("Name", "Address3"),
                                    new XElement("Value", fields[i + 1])));
                                EntAddr.Add(Entother);
                            }


                        }
                        //adding in the entity list
                        EntAddrs.Add(EntAddr);

                        //
                    }
                    Entity.Add(EntAddrs);
                }

                ////if (fields[53].Length != 0)
                //{
                //    XElement Entlist = new XElement("Listed", fields[53]);
                //    Entity.Add(Entlist);
                //}
                //XElement EntDelist = new XElement("DeListed");//ask client
                //if (fields[51].Length != 0)
                //{
                //    XElement EntResn = new XElement("ReasonDisclosures",
                //                            new XElement("ReasonDisclosure",
                //                                new XElement("Name", "Regime"),
                //                                new XElement("Value", fields[51]))
                //                            );
                //    Entity.Add(EntResn);

                //}

            //Reason Disclouser
                XElement reasonDisclosures = new XElement("ReasonDisclosures");
                //
                //getting index name
                List<string> resname = getResname(fields);
                List<string> resval = getResval(fields);
                if (fields.Contains("program"))
                {
                    int proindex = fields.IndexOf("program");
                    resname.Add("program");
                    resval.Add(fields[proindex + 1]);
                }
               

                for (int a = 0; a < resname.Count; a++)
                {
                    if (resval[a].Length > 1)
                    {

                        reasonDisclosures.Add(new XElement("ReasonDisclosure",
                                                        new XElement("Name", resname[a]),
                                                            new XElement("Value", resval[a]))
                                                               );
                    }
                }
                Entity.Add(reasonDisclosures);
           
            //


                //  Entity.Add(EntDelist);

                //
                //After using global Address and Aka
                
            //  }//end for i
            return Entity;
        }

        private static List<string> getakaEntity(List<string> lst)
        {
            List<string> lstaka = new List<string>();
            var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == "aka").Select(p => p.Index);
            foreach (var i in Indexes)
            {

                lstaka.Add(lst[i + 9]);

            }
            return lstaka;
        } //end rowCreator

        public static string[] dobKind(string dob) //returns {kind, data}
        {
            string[] returningDate = new string[2];
            if (dob == "")
            {
                returningDate[0] = "";
                returningDate[1] = "";
                return returningDate;
            }
            string[] DOB = dob.Split('/'); //--/--/1980 becomes {--, --, 1980}
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
       
        public static XDocument ConvertXMLToXML(XmlReader rdr)
        {
            //split the rows
            string pubdate,numrows;
            //Create the declaration
            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"));
            var responce = new XElement("Response"); //Create the root
            string[] responceAttributes = { "Source", "CountryReferences", "TimeStart" };
            string[] responceAttributesValues = { "ofac", "Free Country Name", (DateTime.Now).ToString(new CultureInfo("en-GB")) };

            for (int i = 0; i < responceAttributes.Length; i++)
            {
                responce.SetAttributeValue(responceAttributes[i], responceAttributesValues[i]);
            }//end for i

            // 
            var responceProperties = new XElement("ResponceProperties");
            var property = new XElement("Property");

            //string[] properties = source[0].Split(',');
           
            //
            int j = 0;
            var Lists = new XElement("Lists");
            var Individuals = new XElement("Individuals");
            var Entities = new XElement("Entities");
            //
            //declarations
            List<string> record = new List<string>();
            string reg_date = "";
            //



            while (rdr.Read())
            {
                if (rdr.NodeType == XmlNodeType.Element)
                {
                    if (rdr.Name == "sdnEntry")
                    {
                        if (record.Contains("Individual"))
                        {

                            XElement indivi = individualRowCreator(record);
                            Individuals.Add(indivi);
                            record.Clear();
                           
                            //call individual row 
                        }
                        else if (record.Contains("Entity"))
                        {

                            XElement ent = entityRowCreator(record);
                            Entities.Add(ent);
                            record.Clear();
                        }
                        else if (record.Contains("Vessel"))
                        {
                            //XElement ent = individualRowCreator(record);
                            //Entities.Add(ent);
                            record.Clear();
                        }
                        //string id = rdr.GetAttribute("Id");
                        
                        j++;

                    }//end if ENTITY
                   
                        //
                    else if (rdr.Name == "Publish_Date")
                    {
                        
                        record.Add(rdr.Name);
                       
                        rdr.Read();
                        var res = rdr.Value;
                        record.Add(res);
                        pubdate = res;
                        var propertyName = new XElement("Name", "Publish_Date");
                        var propertyValue = new XElement("Value", pubdate);
                        property.Add(propertyName);
                        property.Add(propertyValue);
                    }

                    else if (rdr.Name == "Record_Count")
                    {

                        record.Add(rdr.Name);

                        rdr.Read();
                        var res = rdr.Value;
                        record.Add(res);
                        numrows = res;

                        var propertyName1 = new XElement("Name", "Record_Count");
                        var propertyValue1 = new XElement("Value", numrows);
                        property.Add(propertyName1);
                        property.Add(propertyValue1);

                    }
                        //
                    else 
                    {
                        
                        record.Add(rdr.Name);
                       
                        rdr.Read();
                        var res = rdr.Value;
                        record.Add(res);
                        

                    }

                }
            }
            //
           


            
            responceProperties.Add(property);
            responce.Add(responceProperties);

            //
            Lists.Add(Individuals);
            Lists.Add(Entities);
            responce.SetAttributeValue("TimeAnswer", (DateTime.Now).ToString(new CultureInfo("en-GB")));
            responce.Add(Lists);
            doc.Add(responce);
            return doc;
        }
        public static void File3(string filename, string output, string datetime)
        {
            /*
             * All the below lines are to be enclosed in an action function
             * and are to be executed at a specific time using TaskScheduler
             * Please refer to: https://msdn.microsoft.com/en-us/library/system.threading.tasks.taskscheduler(v=vs.110).aspx
             * for more details
             */
            var watch = System.Diagnostics.Stopwatch.StartNew();
            XmlReader rdr = XmlReader.Create(filename);
            int i = 0;

            //
            XDocument doc = ConvertXMLToXML(rdr);
            doc.Save(output);
            //
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
        }


    }
}
