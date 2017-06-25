using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace File1Namespace
{
    class File1XmltoXml
    {
        private static List<string> getaka(List<string>lst)
        {
            List<string> lstaka = new List<string>();
            var Indexes = lst.Select((t, i) => new { Index = i, Text = t }).Where(p => p.Text == "NAME").Select(p => p.Index);
            foreach (var i in Indexes)
            {
                if(i!=4)
                {
                    lstaka.Add(lst[i + 1]);
                    lstaka.Add(lst[i + 2]);
                    lstaka.Add(lst[i + 3]);
                    lstaka.Add(lst[i + 4]);

                }
            }
            return lstaka;
        }

        private static XElement individualRowCreator(List<string> fields,string listedDate)
        {
                //
                List<string> addr = null;
                //Getting all Aka
                List<string> aka = getaka(fields);
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

                if (fields[5] != "\n" || fields[6] != "\n")
                {
                    XElement PersonName = new XElement("PersonName",
                                                            new XElement("FirstName", fields[6].Trim()),
                                                            new XElement("LastName", fields[5].Trim()));

                    if(fields[7]!="\n"||fields[8]!="\n")
                    {
                        XElement othnameprt= new XElement ("OtherNameParts");
                        //middle name
                        if (fields[7] != "\n")
                        {
                            XElement nameprtM = new XElement("NamePart",
                                new XElement("Name", "MIDDLENAME"),
                                new XElement("Value", fields[7]));
                            othnameprt.Add(nameprtM);
                        }
                        if (fields[8] != "\n")
                        {
                            XElement nameprtW = new XElement("NamePart",
                                new XElement("Name", "WHOLENAME"),
                                new XElement("Value", fields[8]));
                            othnameprt.Add(nameprtW);
                        }
                        PersonName.Add(othnameprt);
 
                    }
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
                                                                    new XElement("FirstName", aka[1].Trim()),
                                                                    new XElement("LastName", aka[0].Trim()));

                            if (aka[2] != "\n" || aka[3] != "\n")
                            {
                                XElement othnameprt = new XElement("OtherNameParts");
                                //middle name
                                if (aka[2] != "\n")
                                {
                                    XElement nameprtM = new XElement("NamePart",
                                        new XElement("Name", "MIDDLENAME"),
                                        new XElement("Value", aka[2]));
                                    othnameprt.Add(nameprtM);
                                }
                                if (aka[3] != "\n")
                                {
                                    XElement nameprtW = new XElement("NamePart",
                                        new XElement("Name", "WHOLENAME"),
                                        new XElement("Value", aka[3]));
                                    othnameprt.Add(nameprtW);
                                }
                                Personakaprt.Add(othnameprt);

                            }

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
                if (fields.Contains("BIRTH"))
                {

                    // XElement individualBirth = new XElement("DateBirth");
                    int birIndex = fields.IndexOf("BIRTH");
                    XElement DOB = new XElement("DateBirth");
                    //yahan sey
               //     XElement kind = new XElement("Kind", dobKind(fields[birIndex + 1])[0]);
                //    XElement date = new XElement("DateValue", dobKind(fields[birIndex + 1])[1]);
                //    DOB.Add(kind);
                  //  DOB.Add(date);
                    Individual.Add(DOB);
                    //  Individual.Add(individualBirth);
                }


            //

                //documents section
                XElement IndividualDocuments = new XElement("IndividualDocuments");
                if (fields.Contains("PASSPORT"))
                {
                    XElement PersonDocument = new XElement("PersonDocument");

                    int conindex = fields.IndexOf("PASSPORT");
                    //country
                    if (fields[conindex + 2] != "")
                    {
                        XElement personCon = new XElement("Country", fields[conindex + 2]);
                        PersonDocument.Add(personCon);
                    }

                    //Number
                    if(fields[conindex+1]!="")
                    {
                        var numSp = fields[conindex + 1].Split(' ');
                        XElement personCon = new XElement("Number", numSp[0]);
                        PersonDocument.Add(personCon);
                    }
                    try
                    {
                        var numSp1 = fields[conindex + 1].Split(' ');
                        //for passport comment
                        string comment="";
                        for (int i = 1; i < numSp1.Length; i++)
                            comment += numSp1[i];
                        //

                        XElement PersonDucomentOtherDetails = new XElement("PersonDocumentOtherDetails");
                        XElement PersonDocumentOtherDetail = new XElement("PersonDocumentOtherDetail",
                                                                new XElement("Name", "PASSPORT NUMBER COMMENTS"),
                                                                new XElement("Value", comment));
                        PersonDucomentOtherDetails.Add(PersonDocumentOtherDetail);
                        PersonDocument.Add(PersonDucomentOtherDetails);
                    }
                    catch { }
                    IndividualDocuments.Add(PersonDocument);
                    Individual.Add(IndividualDocuments);
                    
                }//end if

                //other details secion
                XElement IndividualOtherDetails = new XElement("IndividualOtherDetails");
                //declaring an int array with oterDetailsIndexes
                int[] otherDetailsIndexes = { 0, 9, 10, 11, 12};
                string[] fieldNames = { "Id", "GENDER", "TITLE", "FUNCTION", "LANGUAGE" };
                string[] otherDetailsValues = new string[fieldNames.Length];
                for (int x = 0; x < fieldNames.Length; x++)
                {
                    otherDetailsValues[x] = fields[otherDetailsIndexes[x]];
                }//end for x
                
                for (int a = 0; a < otherDetailsValues.Length; a++)
                {
                    if (otherDetailsValues[a].Length > 0 && otherDetailsValues[a]!="\n") //please dont hate me for this shit
                    {
                        
                            IndividualOtherDetails.Add(new XElement("IndividualOtherDetail",
                                                            new XElement("Detail",
                                                                new XElement("Name", fieldNames[a]),
                                                                new XElement("Value", otherDetailsValues[a]))
                                                                   ));
                    }
                }
                Individual.Add(IndividualOtherDetails);

                
                
            
            


            //Nationality
            if (listedDate!="" || listedDate!=null)
            {
                XElement listed = new XElement("Listed", listedDate);
                Individual.Add(listed);
            }
            //Reason Disclousres  pdf , programme , remark

                
            //
                XElement reasonDisclosures = new XElement("ReasonDisclosures");
                //declaring an int array with oterDetailsIndexes
                int[] reasonIndexes = { 1, 2, 3};
                string[] fieldNamesReason = { "pdf_link", "programme", "remark"};
                string[] reasonIndexesValues = new string[fieldNamesReason.Length];
                for (int x = 0; x < fieldNamesReason.Length; x++)
                {
                    reasonIndexesValues[x] = fields[reasonIndexes[x]];
                }//end for x

                for (int a = 0; a < reasonIndexesValues.Length; a++)
                {
                    if (reasonIndexesValues[a].Length >1) 
                    {

                        reasonDisclosures.Add(new XElement("ReasonDisclosure",
                                                        new XElement("Name", fieldNamesReason[a]),
                                                            new XElement("Value", reasonIndexesValues[a]))
                                                               );
                    }
                }
                Individual.Add(reasonDisclosures);
                //Empty global Address and Aka

                //Nationality
                if (fields.Contains("CITIZEN"))
                {
                    int citIndex = fields.IndexOf("CITIZEN");
                    XElement nation = new XElement("Nationality", fields[citIndex + 1]);
                    Individual.Add(nation);
                }
    

                
                
                return Individual;
            
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
            string[] DOB = dob.Split('-'); //--/--/1980 becomes {--, --, 1980}
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
        private static string minDate(string date1,string date2)
        {
            if (date1 == "")
            {
                return date2;
            }
            else if (date2 == "")
                return date1;
            else
            {
                var date1Split = date1.Split('-');
                var date2Split = date2.Split('-');
                int intdate1 = Convert.ToInt32(date1Split[0]);
                int intdate2 = Convert.ToInt32(date2Split[0]);

                if (intdate1 > intdate2)
                {
                    return date1;
                }
                else if (intdate2 > intdate1)
                    return date2;
                else if (intdate1 == intdate2)
                {
                    int intdate11 = Convert.ToInt32(date1Split[1]);
                    int intdate12 = Convert.ToInt32(date2Split[1]);


                    if (intdate11 > intdate12)
                    {
                        return date1;
                    }
                    else if (intdate12 > intdate11)
                        return date2;
                    else if (intdate11 == intdate12)
                    {
                        int intdate21 = Convert.ToInt32(date1Split[2]);
                        int intdate22 = Convert.ToInt32(date2Split[2]);

                        if (intdate21 > intdate22)
                        {
                            return date1;
                        }
                        else if (intdate22 > intdate21)
                            return date2;
                        else if (intdate21 == intdate22)
                        {
                            return date1;

                        }

                    }
                }
            }
            return null;
        }
        public static XDocument ConvertXMLToXML(XmlReader rdr)
        {
            //split the rows

            //Create the declaration
            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"));
            var responce = new XElement("Response"); //Create the root
            string[] responceAttributes = { "Source", "CountryReferences","TimeStart" };
            string[] responceAttributesValues = { "EUSanctionsList", "ISO 3166-1 3 Letter Code", (DateTime.Now).ToString(new CultureInfo("en-GB")) };

            for (int i = 0; i < responceAttributes.Length; i++)
            {
                responce.SetAttributeValue(responceAttributes[i], responceAttributesValues[i]);
            }//end for i

            // 
            var responceProperties = new XElement("ResponceProperties");
            var property = new XElement("Property");

            //string[] properties = source[0].Split(',');
            var propertyName = new XElement("Name", "Date");
            var propertyValue = new XElement("Value", System.DateTime.Now.ToString("dd/MM/yyyy") );

            property.Add(propertyName);
            property.Add(propertyValue);
            responceProperties.Add(property);
            responce.Add(responceProperties);
            //
            int j = 0;
            var Lists = new XElement("Lists");
            var Individuals = new XElement("Individuals");
            //var Entities = new XElement("Entities");
          //
            //declarations
            List<string> record = new List<string>();
            string reg_date="";
                //
            
            

            while (rdr.Read())
            {
                if (rdr.NodeType == XmlNodeType.Element)
                {
                    if (rdr.Name == "ENTITY")
                    {
                        if (j != 0)
                        {
                            XElement indivi = individualRowCreator(record,reg_date);
                            Individuals.Add(indivi);
                            record.Clear();
                            reg_date = "";
                            //call individual row 
                        }

                        string id = rdr.GetAttribute("Id");
                        string link = rdr.GetAttribute("pdf_link");
                        string pro = rdr.GetAttribute("programme");
                        string remk = rdr.GetAttribute("remark");
                        //adding in the lst
                        record.Add(id);
                        record.Add(link);
                        record.Add(pro);
                        record.Add(remk);
                        j++;

                    }//end if ENTITY

                    else if (rdr.Name == "NAME")
                    {
                        record.Add("NAME");

                        string date = rdr.GetAttribute("reg_date");
                        reg_date = minDate(reg_date, date);

                    }
                    else if (rdr.Name == "FIRSTNAME")
                    {
                        rdr.Read();
                        var fname = rdr.Value;
                        record.Add(fname);

                    }
                    else if (rdr.Name == "LASTNAME")
                    {
                        rdr.Read();
                        var lname = rdr.Value;
                        record.Add(lname);
                    }
                    else if (rdr.Name == "MIDDLENAME")
                    {
                        rdr.Read();
                        var mname = rdr.Value;
                        record.Add(mname);
                    }
                    else if (rdr.Name == "WHOLENAME")
                    {
                        rdr.Read();
                        var wname = rdr.Value;
                        record.Add(wname);
                    }
                    else if (rdr.Name == "GENDER")
                    {
                        rdr.Read();
                        var gender = rdr.Value;
                        record.Add(gender);
                    }
                    else if (rdr.Name == "TITLE")
                    {
                        rdr.Read();
                        var title = rdr.Value;
                        record.Add(title);
                    }
                    else if (rdr.Name == "FUNCTION")
                    {
                        rdr.Read();
                        var fun = rdr.Value;
                        record.Add(fun);
                    }
                    else if (rdr.Name == "LANGUAGE")
                    {
                        rdr.Read();
                        var lan = rdr.Value;
                        record.Add(lan);
                    }
                    else if (rdr.Name == "DATE")
                    {
                        if(!record.Contains("BIRTH"))
                        {
                            record.Add("BIRTH");
                            rdr.Read();
                            var date = rdr.Value;
                            record.Add(date);
                        }
                    }
                    
                    else if (rdr.Name == "COUNTRY")
                    {
                        rdr.Read();
                        var con = rdr.Value;
                        record.Add(con);
                    }
                    
                    else if (rdr.Name == "PASSPORT")
                    {
                       
                        record.Add("PASSPORT");
                    }
                    else if (rdr.Name == "NUMBER")
                    {
                        rdr.Read();
                        var num = rdr.Value;
                        record.Add(num);
                    }
                    else if (rdr.Name == "CITIZEN")
                    {
                        record.Add("CITIZEN");
                    }

                }
            }
            //
            Lists.Add(Individuals);
        //    Lists.Add(Entities);
            responce.SetAttributeValue("TimeAnswer", (DateTime.Now).ToString(new CultureInfo("en-GB")));
            responce.Add(Lists);
            doc.Add(responce);
            return doc;
        }
        public static void File1(string filename, string output, string datetime)
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
