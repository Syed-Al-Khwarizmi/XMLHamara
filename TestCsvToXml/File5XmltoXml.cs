using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace File5Namespace
{
    class File5XmltoXml
    {

        private static List<string> getakaindi(List<string> lst)
        {
            List<string> lstaka = new List<string>();
            if (lst.Contains("ALIAS_NAME"))
            {
                var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == "ALIAS_NAME").Select(p => p.Index);
                foreach (var i in Indexes)
                {
                    if(lst[i]!="")
                    lstaka.Add(lst[i + 1]);
                }
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


            if (fields.Contains("FIRST_NAME") || fields.Contains("SECOND_NAME"))
            {
                int fnindex = fields.IndexOf("FIRST_NAME");

                int snindex = fields.IndexOf("SECOND_NAME");

                XElement PersonName = new XElement("PersonName",
                                                        new XElement("FirstName", fields[fnindex+1].Trim()),
                                                        new XElement("LastName", fields[snindex+1].Trim()));

                if (fields.Contains("THIRD_NAME") || fields.Contains("FOURTH_NAME"))
                {
                    XElement othnameprt = new XElement("OtherNameParts");
                    //middle name
                    if (fields.Contains("THIRD_NAME"))
                    {

                        int tnindex = fields.IndexOf("THIRD_NAME");
                        if (fields[tnindex+1] != "")
                        {
                            XElement nameprtM = new XElement("NamePart",
                                new XElement("Name", "THIRD_NAME"),
                                new XElement("Value", fields[tnindex + 1]));
                            othnameprt.Add(nameprtM);
                        }
                    }
                    if (fields.Contains("FOURTH_NAME"))
                    {

                        int frnindex = fields.IndexOf("FOURTH_NAME");
                        if (fields[frnindex+1] != "")
                        {
                            XElement nameprtW = new XElement("NamePart",
                                new XElement("Name", "FOURTH_NAME"),
                                new XElement("Value", fields[frnindex + 1]));
                            othnameprt.Add(nameprtW);
                        }
                    }
                    PersonName.Add(othnameprt);

                }
                //
                if (fields.Contains("NAME_ORIGINAL_SCRIPT") )
                {
                    XElement othnamedet = new XElement("OtherNameDetails");
                    //middle name
                    if (fields.Contains("NAME_ORIGINAL_SCRIPT"))
                    {
                        int nnindex = fields.IndexOf("NAME_ORIGINAL_SCRIPT");
                        if (fields[nnindex+1] != "")
                        {
                            XElement nameprtN = new XElement("OtherNameDetail",
                                new XElement("Name", "NAME_ORIGINAL_SCRIPT"),
                                new XElement("Value", fields[nnindex + 1]));
                            othnamedet.Add(nameprtN);
                        }
                    }
                    
                    PersonName.Add(othnamedet);

                }
                //
                IndividualReference.Add(PersonName);
            }
            //
            //Individual AKA
            XElement PersonAKAs = new XElement("PersonAKAs");
            if (aka.Count != 0)
            {
                for (int i = 0; i < aka.Count; i ++)
                {

                    //addition
                    if (aka[i] != "")
                    {
                        XElement othnameprtAka = new XElement("OtherNameParts");
                        //middle name
                        
                            //int tnindex = fields.IndexOf("THIRD_NAME");
                            XElement nameprtaka = new XElement("NamePart",
                                new XElement("Name", "ALIAS_NAME"),
                                new XElement("Value", aka[i]));
                            othnameprtAka.Add(nameprtaka);
                        

                        PersonAKAs.Add(othnameprtAka);
                    }
                    //end addition
                }//end for i
            }//end if

            if (aka.Count != 0)
            {
                if(aka[0]!="")
                IndividualReference.Add(PersonAKAs);
            }//end if

            Individual.Add(IndividualReference);
            //
            //DateBirth
            //Ali correct it
            if (fields.Contains("YEAR") || fields.Contains("DATE"))
            {

                int birIndex=0;
                // XElement individualBirth = new XElement("DateBirth");
                 if (fields.Contains("YEAR"))
                 birIndex = fields.IndexOf("YEAR");
                if (fields.Contains("DATE"))
                    birIndex = fields.IndexOf("DATE");
               

                XElement DOB = new XElement("DateBirth");
                //yahan sey
                if (fields.Contains("TYPE_OF_DATE"))
                {
                    int kindex = fields.IndexOf("TYPE_OF_DATE");

                    XElement kind = new XElement("Kind", fields[kindex+1]);
                    DOB.Add(kind);

                }

                XElement date = new XElement("DateValue", fields[birIndex + 1]);
                
                DOB.Add(date);
                Individual.Add(DOB);
                //  Individual.Add(individualBirth);
            }


            //

            //documents section
            //

           // XElement IndividualDocuments = new XElement("IndividualDocuments");
         //   XElement PersonDocument = new XElement("PersonDocument");
          //  PersonDocument.Add(personCon);
         //   IndividualDocuments.Add(PersonDocument);
            //
            List<int> indexadd = getaddInd(fields, "INDIVIDUAL_DOCUMENT");

            if (indexadd.Count > 0)
            {
               // XElement EntAddrs = new XElement("EntityAddresses");
                XElement IndividualDocuments = new XElement("IndividualDocuments");
           
                for (int k = 0; k < indexadd.Count - 1; k++)
                {
                    //  int times = Convert.ToInt32(fields[indexadd[k] + 1]);
                   // XElement EntAddr = new XElement("EntityAddress");
                    XElement PersonDocument = new XElement("PersonDocument");
                    XElement PersonDocumentOther = new XElement("PersonDocumentOtherDetails");
                    
                    for (int i = indexadd[k]; i < indexadd[k + 1]; i++)
                    {
                        if (i == fields.Count || fields[i] == "SORT_KEY")
                        {
                            break;
                        }
                        //Ali don't hate me plz
                        if (fields[i] == "COUNTRY_OF_ISSUE")
                        {
                            XElement personCon = new XElement("Country", fields[i + 1]);
                            PersonDocument.Add(personCon);
          
                        }
                        if (fields[i] == "NUMBER")
                        {
                            XElement indnum = new XElement("Number", fields[i + 1]);
                            PersonDocument.Add(indnum);
          
                        }
                        if (fields[i] == "DATE_OF_ISSUE")
                        {
                            XElement IndIssue = new XElement("DATE_OF_ISSUE", fields[i + 1]);
                            PersonDocument.Add(IndIssue);
                        }

                        if (fields[i] == "TYPE_OF_DOCUMENT")
                        {
                            XElement Indother = new XElement("PersonDocumentOtherDetail",
                                 new XElement("Detail",
                                     new XElement("Name", "TYPE_OF_DOCUMENT"),
                                new XElement("Value", fields[i + 1])));
                            PersonDocumentOther.Add(Indother);
                        }
                        if (fields[i] == "TYPE_OF_DOCUMENT2")
                        {
                            XElement Indother = new XElement("PersonDocumentOtherDetail",
                                 new XElement("Detail",
                                     new XElement("Name", "TYPE_OF_DOCUMENT2"),
                                new XElement("Value", fields[i + 1])));
                            PersonDocumentOther.Add(Indother);
                        }

                        if (fields[i] == "CITY_OF_ISSUE")
                        {
                            XElement Indother = new XElement("PersonDocumentOtherDetail",
                                 new XElement("Detail",
                                     new XElement("Name", "CITY_OF_ISSUE"),
                                new XElement("Value", fields[i + 1])));
                            PersonDocumentOther.Add(Indother);
                        }
                        if (fields[i] == "NOTE")
                        {
                            XElement Indother = new XElement("PersonDocumentOtherDetail",
                                 new XElement("Detail",
                                     new XElement("Name", "NOTE"),
                                new XElement("Value", fields[i + 1])));
                            PersonDocumentOther.Add(Indother);
                        }


                    }
                    //adding in the entity list
                  //  EntAddrs.Add(EntAddr);
                    IndividualDocuments.Add(PersonDocument);
                    IndividualDocuments.Add(PersonDocumentOther);
                    //
                }
               // Entity.Add(EntAddrs);
                Individual.Add(IndividualDocuments);
            }

          
            //

            //if (fields.Contains("Passport"))
            //{
            //    XElement PersonDocument = new XElement("PersonDocument");

            //    int conindex = fields.IndexOf("Passport");
            //    //country
            //    if (fields[conindex + 4] != "")
            //    {
            //        XElement personCon = new XElement("Country", fields[conindex + 4]);
            //        PersonDocument.Add(personCon);
            //    }

            //    //Number
            //    if (fields[conindex + 2] != "")
            //    {
            //        XElement personCon = new XElement("Number", fields[conindex + 2]);
            //        PersonDocument.Add(personCon);
            //    }

            //    IndividualDocuments.Add(PersonDocument);
            //    //  Individual.Add(IndividualDocuments);

            //}//end if
            ////
            //if (fields.Contains("Registration ID"))
            //{
            //    XElement PersonDocument = new XElement("PersonDocument");
            //    string idnumber = "", idcountry = "", iddate = "";
            //    int conindex = fields.IndexOf("Registration ID");
            //    //getting values
            //    {
            //        if (fields[conindex + 1] == "idNumber")
            //        { idnumber = fields[conindex + 1]; }
            //        if (fields[conindex + 3] == "idCountry")
            //        { idcountry = fields[conindex + 3]; }
            //        if (fields[conindex + 5] == "issueDate")
            //        { iddate = fields[conindex + 5]; }
            //    }
            //    //country
            //    if (idcountry != "")
            //    {
            //        XElement personCon = new XElement("Country", idcountry);
            //        PersonDocument.Add(personCon);
            //    }

            //    //Number
            //    if (idnumber != "")
            //    {
            //        XElement personCon = new XElement("Number", idnumber);
            //        PersonDocument.Add(personCon);
            //    }
            //    //issueDate
            //    if (iddate != "")
            //    {
            //        XElement personCon = new XElement("DateIssue", iddate);
            //        PersonDocument.Add(personCon);
            //    }

            //    IndividualDocuments.Add(PersonDocument);


            //}//end if
            //if (fields.Contains("INDIVIDUAL_DOCUMENT") )
            //{
            //    Individual.Add(IndividualDocuments);
            //}
            //
            //other details secion
            XElement IndividualOtherDetails = new XElement("IndividualOtherDetails");

            string[] fieldNamesOther = { "DATAID", "VERSIONNUM", "GENDER", "SUBMITTED_BY", "COUNTRY" };


            for (int a = 0; a < fieldNamesOther.Length; a++)
            {
                if (fields.Contains(fieldNamesOther[a]))
                {
                    int indexNum = fields.IndexOf(fieldNamesOther[a]);
                    if (fields[indexNum + 1].Length > 1)
                    {

                        IndividualOtherDetails.Add(new XElement("ReasonDisclosure",
                                                        new XElement("Name", fieldNamesOther[a]),
                                                            new XElement("Value", fields[indexNum+1]))
                                                               );
                    }
                }
            }

                Individual.Add(IndividualOtherDetails);

            
    





            //Listed
            if (fields.Contains("LISTED_ON"))
            {
                int lindex = fields.IndexOf("LISTED_ON");
                XElement listed = new XElement("Listed", fields[lindex+1]);
                Individual.Add(listed);
            }
           

            //
            XElement reasonDisclosures = new XElement("ReasonDisclosures");
           
            //declaring an int array with oterDetailsIndexes
            //int[] reasonIndexes = { 1, 2, 3 };
            string[] fieldNamesReason = { "COMMENTS1", "UN_LIST_TYPE", "REFERENCE_NUMBER", "LIST_TYPE" };
         //   string[] reasonIndexesValues = new string[fieldNamesReason.Length];
            //for (int x = 0; x < fieldNamesReason.Length; x++)
            //{
            //    reasonIndexesValues[x] = fields[reasonIndexes[x]];
            //}//end for x

            for (int a = 0; a < fieldNamesReason.Length; a++)
            {
                if (fields.Contains(fieldNamesReason[a]))
                {
                    int indexNum = fields.IndexOf(fieldNamesReason[a]);
                    if (fields[indexNum + 1].Length > 1)
                    {
                        int k = 1;
                        if (fieldNamesReason[a] == "LIST_TYPE")
                            k = 2;

                        reasonDisclosures.Add(new XElement("ReasonDisclosure",
                                                        new XElement("Name", fieldNamesReason[a]),
                                                            new XElement("Value", fields[indexNum+k]))
                                                               );
                    }
                }
            }
            Individual.Add(reasonDisclosures);
            //Empty global Address and Aka

            //Nationality
            if (fields.Contains("NATIONALITY"))
            {
                int citIndex = fields.IndexOf("NATIONALITY");

                if (fields[citIndex + 1] != "")
                {
                    XElement nation = new XElement("Nationality", fields[citIndex + 2]);
                    Individual.Add(nation);
                }
            }




            return Individual;

        } //end individualRowCreator

        public static List<int> getaddInd(List<string> lst,string str)
        {
            var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == str).Select(p => p.Index);
            List<int> countlst = new List<int>();
            foreach (var i in Indexes)
            {
                countlst.Add(i);

            }
            if (countlst.Count > 0)
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

        private static XElement entityRowCreator(List<string> fields)
        {
            XElement Entity = new XElement("Entity");


            //Getting all address
            //getallAddressAka();
            // getallAddressAndAka(fields[57], source);
            //  List<string> addr = getEntAddr(fields);
            //Getting all Aka
            List<string> aka = getakaindi(fields);

            //

            XElement EntRef = new XElement("EntityReference");

            int lstindex = fields.IndexOf("FIRST_NAME");
            XElement EntName = new XElement("EntityName", fields[lstindex + 1]);

            EntRef.Add(EntName);
            if (aka.Count > 0)
            {
                XElement EntSynonyms = new XElement("EntitySynonmys");
                for (int i = 0; i < aka.Count; i++)
                {
                    if(aka[i]!="")
                    EntSynonyms.Add(new XElement("Synonmys", aka[i].Trim('"')));
                }
                if(aka[0]!="")
                EntRef.Add(EntSynonyms);
            }
            //Not required for this document 
            //XElement EntAbbre = new XElement("EntityAbbreviations",
            //                            new XElement("Abbreviations"));//ask client

            
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
            List<int> indexadd = getaddInd(fields, "ENTITY_ADDRESS");

            if (indexadd.Count > 0)
            {
                XElement EntAddrs = new XElement("EntityAddresses");

                for (int k = 0; k < indexadd.Count - 1; k++)
                {
                    //  int times = Convert.ToInt32(fields[indexadd[k] + 1]);
                    XElement EntAddr = new XElement("EntityAddress");

                    for (int i = indexadd[k]; i < indexadd[k + 1]; i++)
                    {
                        if (i == fields.Count || fields[i] == "SORT_KEY")
                        {
                            break;
                        }
                        //Ali don't hate me plz
                        if (fields[i] == "COUNTRY")
                        {
                            XElement Entcon = new XElement("Country", fields[i + 1]);
                            EntAddr.Add(Entcon);
                        }
                        if (fields[i] == "ZIP_CODE")
                        {
                            XElement Entzip = new XElement("ZipCode", fields[i + 1]);
                            EntAddr.Add(Entzip);
                        }
                        if (fields[i] == "STATE_PROVINCE")
                        {
                            XElement Entstat = new XElement("StateProvince", fields[i + 1]);
                            EntAddr.Add(Entstat);
                        }
                        if (fields[i] == "COUNTRY")
                        {
                            XElement Entcity = new XElement("City", fields[i + 1]);
                            EntAddr.Add(Entcity);
                        }

                        if (fields[i] == "STREET")
                        {
                            XElement Entstrt = new XElement("Street", fields[i + 1]);
                            EntAddr.Add(Entstrt);
                        }
                        if (fields[i] == "NOTE")
                        {
                            XElement Entother = new XElement("AddressOtherDetails",
                                 new XElement("Detail",
                                     new XElement("Name", "NOTE"),
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
            if (fields.Contains("LISTED_ON"))
            {
                int lindex = fields.IndexOf("LISTED_ON");
                XElement Entlist = new XElement("Listed", fields[lindex + 1]);
                Entity.Add(Entlist);
            }
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

            //Entity Other Details
            XElement EntityOtherDetails = new XElement("IndividualOtherDetails");

            string[] fieldNamesOther = { "DATAID", "VERSIONNUM", "SUBMITTED_BY" };


            for (int a = 0; a < fieldNamesOther.Length; a++)
            {
                if (fields.Contains(fieldNamesOther[a]))
                {
                    int indexNum = fields.IndexOf(fieldNamesOther[a]);
                    if (fields[indexNum + 1].Length > 1)
                    {

                        EntityOtherDetails.Add(new XElement("ReasonDisclosure",
                                                        new XElement("Name", fieldNamesOther[a]),
                                                            new XElement("Value", fields[indexNum + 1]))
                                                               );
                    }
                }
            }

            Entity.Add(EntityOtherDetails);

            //
            //Reason Disclouser
            XElement reasonDisclosures = new XElement("ReasonDisclosures");
            //
            string[] fieldNamesReason = { "COMMENTS1", "UN_LIST_TYPE", "REFERENCE_NUMBER", "LIST_TYPE" };
            
            for (int a = 0; a < fieldNamesReason.Length; a++)
            {
                if (fields.Contains(fieldNamesReason[a]))
                {
                    int indexNum = fields.IndexOf(fieldNamesReason[a]);
                    if (fields[indexNum + 1].Length > 1)
                    {
                        int k = 1;
                        if (fieldNamesReason[a] == "LIST_TYPE")
                            k = 2;

                        reasonDisclosures.Add(new XElement("ReasonDisclosure",
                                                        new XElement("Name", fieldNamesReason[a]),
                                                            new XElement("Value", fields[indexNum + k]))
                                                               );
                    }
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
            string pubdate, numrows;
            //Create the declaration
            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"));
            var responce = new XElement("Response"); //Create the root
            string[] responceAttributes = { "Source", "CountryReferences", "TimeStart" };
            string[] responceAttributesValues = { "SCSanctionsUN", "Free Country Name", (DateTime.Now).ToString(new CultureInfo("en-GB")) };

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
                    if (rdr.Name == "INDIVIDUAL")// || rdr.Name == "ENTITY")
                    {
                        if (record.Contains("INDIVIDUAL"))
                        {

                            XElement indivi = individualRowCreator(record);
                            Individuals.Add(indivi);
                            record.Clear();

                            record.Add(rdr.Name);
                            //call individual row 
                        }
                        else
                        {
                            record.Add("INDIVIDUAL");
                        }
                    }//end individual

                    else if (rdr.Name == "ENTITY") 
                    {
                        if (record.Contains("ENTITY"))
                        {

                            XElement ent = entityRowCreator(record);
                            Entities.Add(ent);
                            record.Clear();
                            record.Add(rdr.Name);
                        }
                        else
                        {
                            record.Add("ENTITY");
                        }

                        //string id = rdr.GetAttribute("Id");
                    }



                        //
                    else if (rdr.Name == "CONSOLIDATED_LIST")
                    {
                       // record.Add(rdr.Name);
                        string Dg = rdr.GetAttribute("dateGenerated");
                        var propertyName = new XElement("Name", "dateGenerated");
                        var propertyValue = new XElement("Value", Dg);
                        property.Add(propertyName);
                        property.Add(propertyValue);

                    }


                    else if (rdr.Name == "INDIVIDUAL_PLACE_OF_BIRTH")
                    {
                        record.Add(rdr.Name);
                    }

                    else if (rdr.Name == "INDIVIDUAL_DOCUMENT")
                    {
                        record.Add(rdr.Name);
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
        public static void File5(string filename, string output, string datetime)
        {

            /*
             * All the below lines are to be enclosed in an action function
             * and are to be executed at a specific time using TaskScheduler
             * Please refer to: https://msdn.microsoft.com/en-us/library/system.threading.tasks.taskscheduler(v=vs.110).aspx
             * for more details
             */

            var watch = System.Diagnostics.Stopwatch.StartNew();
            XmlReader rdr = XmlReader.Create(filename);
            XDocument doc = ConvertXMLToXML(rdr);
            doc.Save(output);
            //
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
        }


    }
}
