using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace File10Namespace
{
    class File10CsvtoXml
    {
        //same no change
        public static XDocument ConvertCsvToXML(string[] source)
        {
            //split the rows

            //Create the declaration
            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"));
            var responce = new XElement("Responce"); //Create the root
            var responceProperties = new XElement("ResponceProperties");
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
            for (int i = 1; i < source.Length; i++)
            {
                try

                {
                   if (i == 1517||i==1519||i==1518||i==2604||i==3212||i==3425||i==3426||i==3427||i==3889||i==3890||i==4128||i==4130||i==4252||i==4253||i==4254||i==5047||i==5324||i==5326||i==5578||i==5580||i==6277||i==6511||i==6512||i==6586||i==7160||i==7510) {
                      // System.Console.WriteLine(source[i]);
                       continue; }
                 //   var fields1 = source[i].Remove(0, 1).Split(',');
                   // if (source[i].EndsWith("#") || source[i].EndsWith("-"))
                        source[i] = source[i].Substring(0, source[i].Length - 1);
                    var fields = Regex.Split(source[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

                    //var result = Regex.Split(fields[i], "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

                    // tfp.Delimiters = new string[] { "," };
                    if (fields[5] == "Individual")
                    {
                        //XElement indivi = individualRowCreator(fields, source);
                        //if (!indivi.IsEmpty)
                        //{
                        //   Individuals.Add(indivi);
                        //}//end if
                    }
                    else if (fields[5] == "Entity")
                    {
                        XElement entit = entityRowCreator(fields, source);
                        if (!entit.IsEmpty)
                        {
                            Entities.Add(entit);
                        }//end if
                    }
                    System.Console.WriteLine(i);
                }
                catch (Exception ex) { System.Console.WriteLine(ex); }
            }

          
            Lists.Add(Individuals);
            Lists.Add(Entities);
            responce.Add(Lists);
            doc.Add(responce);
            return doc;
        }
        
        //To get first and last name 
        private static  string[] getFirstLastName(string field)
        {
            var fields1 = field.Split(',');
            string[] str = new string[2];
            //lastname
            str[0] = fields1[0];
            //firstname
            str[1] = fields1[1];
            return str;
        }
        private static string [][] getallDocInfo(string idField)
        {
            var singleDoc = idField.Split(';');
            string [][] arr = new string [singleDoc.Length][];
            for (int i=0;i<singleDoc.Length;i++)
            {
                var singleRec = singleDoc[i].Split(',');
                for (int j=0;j<singleRec.Length;j++)
                {
                    arr[i][j] = singleRec[j];
                }

            }
            return arr;
        }
        // Little change just call the above function to fill first and last name
        private static XElement individualRowCreator(string[] fields, string[] source)
        {
            if (fields[47] == "Individual")
            {

                 getallAddressAndAka(fields[57], source);
                List<string> addr = Address;
                //Getting all Aka
                List<string> aka = Aka;
                //
                string[] name = new string[2];

                name = getFirstLastName(fields[9]);
                //
                XElement Individual = new XElement("Individual");

                //IndividualReference, and childs
                XElement IndividualReference = new XElement("IndividualReference");
                XElement PersonName = new XElement("PersonName",
                                                        new XElement("FirstName", name[1]),
                                                        new XElement("LastName", name[0]));
                XElement PersonAKAs = new XElement("PersonAKAs");
                for (int i = 0; i < aka.Count; i++)
                {
                    PersonAKAs.Add(new XElement("PersonName",
                                        new XElement("FirstName", aka[i]),
                                        new XElement("LastName", aka[i+1])));
                }//end for i
                IndividualReference.Add(PersonName);
                IndividualReference.Add(PersonAKAs);
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

        // Same ma to issey charny ka soch bi nai sakhta 
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

            //else
            //{
            //    returningDate[0] = "EXACT";
            //    returningDate[1] = DOB[0] + "/" + DOB[1] + "/" + DOB[2];
            //    return returningDate;
            //}
            return null;
        }//end dobKind

        public static List<string> Address;
        public static List<string> Aka;

        //Under construction uptill now you can only get address for the individual
        //public static List<string> getAddresses(string  addresses,string type)
        //{

        //    Regex regex1 = new Regex(@"\s{2,}"); // matches at least 2 whitespaces
        //    var singleaddress = addresses.Split(';');
        //    List<string> lstaddress=new List<string>();
        //    if(type=="Individual")
        //    {
        //        for (int i = 0; i < singleaddress.Length; i++)
        //            lstaddress.Add(singleaddress[i]);
        //    }
        //    else if (type=="Entity")
        //    {
        //        Int32 intValue;
        //        string other = "";
        //        for (int i = 0; i < singleaddress.Length; i++)
        //        {
        //            var singleAddEntity = singleaddress[i].Split(',');
        //            for (int j=singleAddEntity.Length-1;j>=0; j--)
        //            {
        //                if (singleAddEntity.Length - 1 == j)
        //                {
        //                    //chk for country
        //                    lstaddress.Add(singleAddEntity[j]);
        //                    if (j==0)
        //                    {
        //                        for (int k = 0; k < 6; k++)
        //                            lstaddress.Add("");
        //                    }
        //                }
        //                else if(singleAddEntity.Length-2==j)
        //                {
        //                    //chk for zipcode
        //                    string str = singleAddEntity[j];
        //                   // Int32 intValue;

        //                    bool containsNum = Regex.IsMatch(singleAddEntity[j], @"\d");

        //                    if (containsNum)
        //                    {
        //                        // mystring is an integer
        //                        lstaddress.Add(singleAddEntity[j]);
        //                        if (j == 0)
        //                        {
        //                            for (int k = 0; k < 5; k++)
        //                                lstaddress.Add("");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        lstaddress.Add("");
        //                         //if not space its a state province
        //                        lstaddress.Add(singleAddEntity[j]);
        //                        if (j == 0)
        //                        {
        //                            for (int k = 0; k < 4; k++)
        //                                lstaddress.Add("");
        //                        }
        //                    }
                           
        //                }
        //                else if (singleAddEntity.Length - 3 == j)
        //                {
        //                    //chk for state province
        //                    if(lstaddress.Count!=3)
        //                    {
        //                        if(singleAddEntity[j].Contains("P.O. Box"))
        //                        {
        //                            //if its not state province and contain po box then it goes to other address info
        //                            for (int k = 0; k < 4; k++)
        //                                lstaddress.Add("");
        //                            //
        //                            other += singleAddEntity[j];
        //                            //lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                       //  Int32 intValue;

                             
        //                        else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
        //                        {
        //                            //for state province , city, street 
        //                            for (int k = 0; k < 3; k++)
        //                                lstaddress.Add("");
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if(j==0)
        //                            { lstaddress.Add(""); }
        //                        }
                                    
        //                  //  bool containsNum = Regex.IsMatch(singleAddEntity[j], @"\d");

        //                        else if (singleAddEntity[j].Contains("Street") || singleAddEntity[j].Contains("St."))//||  Regex.IsMatch(singleAddEntity[j], @"\d"))
        //                        {
        //                            //for state province
        //                            lstaddress.Add("");
        //                            //for city 
        //                            lstaddress.Add("");
        //                            //add in the street
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            {
        //                                //To fill the space of building and more information 
        //                                for (int k = 0; k < 2; k++)
        //                                    lstaddress.Add("");
        //                            }

        //                        }
        //                        else
        //                        {
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            {
        //                                //To fill the space of building and more information 
        //                                for (int k = 0; k < 4; k++)
        //                                    lstaddress.Add("");
        //                            }
        //                        }
        //                    }
        //                    else if (lstaddress.Count == 3)
        //                    {
        //                        if (singleAddEntity[j].Contains("P.O. Box"))
        //                        {
        //                            //if its not state province and contain po box then it goes to other address info
        //                            for (int k = 0; k < 3; k++)
        //                                lstaddress.Add("");
        //                           //

        //                            other += singleAddEntity[j];
        //                            //lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                        //  Int32 intValue;

                             
        //                        else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
        //                        {
        //                            //for state province , city, street 
        //                            for (int k = 0; k < 2; k++)
        //                                lstaddress.Add("");
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            { lstaddress.Add(""); }
        //                        }

        //                        else if (singleAddEntity[j].Contains("Street") || singleAddEntity[j].Contains("St."))// || Regex.IsMatch(singleAddEntity[j], @"\d"))
        //                        {

        //                            //for city 
        //                            lstaddress.Add("");
        //                            //add in the street
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            {
        //                                //To fill the space of building and more information 
        //                                for (int k = 0; k < 2; k++)
        //                                    lstaddress.Add("");
        //                            }

        //                        }
        //                        else
        //                        {
        //                            for (int k = 0; k < 3; k++)
        //                                lstaddress.Add("");
        //                            //
        //                            other += singleAddEntity[j];
        //                           // lstaddress.Add(singleAddEntity[j]);
        //                        }

        //                    }
        //                }
        //                else if (singleAddEntity.Length - 4 == j)
        //                {
        //                    //for city
        //                    if(lstaddress.Count<4)
        //                    {
        //                        if (singleAddEntity[j].Contains("P.O. Box"))
        //                        {
        //                            //if its not state province and contain po box then it goes to other address info
        //                            for (int k = 0; k < 3; k++)
        //                                lstaddress.Add("");
        //                            //
        //                            other += singleAddEntity[j];
        //                           // lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                        //  Int32 intValue;

                              
        //                        else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
        //                        {
        //                            //for state province , city, street 
        //                            for (int k = 0; k < 2; k++)
        //                                lstaddress.Add("");
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            { lstaddress.Add(""); }
        //                        }

        //                        else if (singleAddEntity[j].Contains("Street") || singleAddEntity[j].Contains("St."))// || Regex.IsMatch(singleAddEntity[j], @"\d"))
        //                        {

        //                            //for city 
        //                            lstaddress.Add("");
        //                            //add in the street
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            {
        //                                //To fill the space of building and more information 
        //                                for (int k = 0; k < 2; k++)
        //                                    lstaddress.Add("");
        //                            }

        //                        }
        //                        else if (!regex1.IsMatch(singleAddEntity[j]))
        //                        {
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            {
        //                                //To fill the space of building and more information 
        //                                for (int k = 0; k < 3; k++)
        //                                    lstaddress.Add("");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //else its an other information
        //                            for (int k = 0; k < 3; k++)
        //                                lstaddress.Add("");
        //                            //
        //                            other += singleAddEntity[j];
        //                            //lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                    }
        //                    else if(lstaddress.Count>=4)
        //                    {
        //                        if (singleAddEntity[j].Contains("P.O. Box"))
        //                        {
        //                            //if its not state province and contain po box then it goes to other address info
        //                            for (int k = 0; k < 2; k++)
        //                                lstaddress.Add("");

        //                            other += singleAddEntity[j];
        //                           // lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                        //  Int32 intValue;

                              
        //                        else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
        //                        {
        //                            //for  street 
        //                                lstaddress.Add("");
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            { lstaddress.Add(""); }
        //                        }
        //                        else if (singleAddEntity[j].Contains("Street") || singleAddEntity[j].Contains("St."))//|| Regex.IsMatch(singleAddEntity[j], @"\d"))
        //                        {

        //                            //add in the street
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            {
        //                                //To fill the space of building and more information 
        //                                for (int k = 0; k < 2; k++)
        //                                    lstaddress.Add("");
        //                            }

        //                        }
        //                        else
        //                        {
        //                            //else its an other information
        //                            for (int k = 0; k < 2; k++)
        //                                lstaddress.Add("");
        //                            //
        //                            other += singleAddEntity[j];
        //                            //lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                    }
        //                }
        //                else if (singleAddEntity.Length - 5 == j)
        //                {
        //                    //for street
        //                    if (lstaddress.Count < 5)
        //                    {
        //                        if (singleAddEntity[j].Contains("P.O. Box"))
        //                        {
        //                            //if its not street and contain po.box then it goes to other address info
        //                            for (int k = 0; k < 2; k++)
        //                                lstaddress.Add("");
        //                            //
        //                            other += singleAddEntity[j];
        //                            //lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                        //  Int32 intValue;

                               
        //                        else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
        //                        {
        //                            //for  street 
        //                                 lstaddress.Add("");
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            { lstaddress.Add(""); }
        //                        }
        //                        else                                 {
        //                                 //add in the street
        //                                    lstaddress.Add(singleAddEntity[j]);
        //                                    if (j == 0)
        //                                    {
        //                                //To fill the space of building and more information 
        //                                for (int k = 0; k < 2; k++)
        //                                    lstaddress.Add("");
        //                            }

        //                        }
                               
        //                    }
        //                        //galti 
        //                    else if (lstaddress.Count >= 5)
        //                    {
        //                        if (singleAddEntity[j].Contains("P.O. Box"))
        //                        {
        //                            //if its not state province and contain po box then it goes to other address info
        //                            for (int k = 0; k < 1; k++)
        //                                lstaddress.Add("");
        //                            //
        //                            other += singleAddEntity[j];
        //                          //  lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                        //  Int32 intValue;

                              
        //                        else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
        //                        {
        //                            //for  street 
        //                           // lstaddress.Add("");
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            { lstaddress.Add(""); }
        //                        }
        //                        else  
        //                        {

        //                            //add in the building
        //                            lstaddress.Add("");
        //                           //
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            //if (j == 0)
        //                            //{
        //                            //    //To fill the space of building and more information 
        //                            //    for (int k = 0; k < 2; k++)
        //                            //        lstaddress.Add("");
        //                            //}

        //                        }
                                
        //                    }
        //                }
        //                else if (singleAddEntity.Length -6 == j)
        //                {
        //                    //for building
        //                    if (lstaddress.Count < 6)
        //                    {
        //                        if (singleAddEntity[j].Contains("P.O. Box"))
        //                        {
        //                            //if its  and contain po.box then it goes to other address info
        //                             lstaddress.Add("");
        //                             //
        //                             other += singleAddEntity[j];
        //                            //lstaddress.Add(singleAddEntity[j]);
        //                        }
        //                        //  Int32 intValue;


        //                        else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
        //                        {
        //                            //for  street 
        //                            //lstaddress.Add("");
        //                            lstaddress.Add(singleAddEntity[j]);
        //                            if (j == 0)
        //                            { lstaddress.Add(""); }
        //                        }
        //                        else
        //                        {
        //                            //add in the other
        //                            lstaddress.Add("");
        //                            //
        //                            other += singleAddEntity[j];
        //                            //  lstaddress.Add(singleAddEntity[j]);
                                    

        //                        }

        //                    }
        //                    else if (lstaddress.Count >= 6)
        //                    {

        //                        other += singleAddEntity[j];
        //                     //       lstaddress.Add(singleAddEntity[j]);
                                
        //                    }
        //                }
        //                //
        //                else
        //                {

        //                    other += singleAddEntity[j];
        //                    //lstaddress.Add(singleAddEntity[j]);
        //                }
        //                if (j == 0)
        //                {
        //                    if (lstaddress.Count % 6 == 0)
        //                    {
        //                        lstaddress.Add(other);
        //                    }
        //                    else
        //                    {
        //                        int factor = lstaddress.Count / 6;
        //                        int num = (lstaddress.Count % 6)-(factor-1);
        //                        for (int k = num-1; k >=0; k--)
        //                            lstaddress.RemoveAt((6*factor)+k);
        //                        if(((lstaddress.Count+1)%(7*factor)) > 0)
        //                        {
        //                            int cloop = (-1*((lstaddress.Count % 7) - 7))-1;
        //                            for (int m = 0; m < cloop; m++) 
        //                            { lstaddress.Add(""); }
        //                        }
        //                        lstaddress.Add(other);
        //                    }
        //                }
        //            }
        //        }
        //    }

           
        //    return lstaddress;
            
        //}

        public static List<string> getAddresses(string addresses, string type)
        {

            Regex regex1 = new Regex(@"\s{2,}"); // matches at least 2 whitespaces
            var singleaddress = addresses.Split(';');
            List<string> lstaddress = new List<string>();
            if (type == "Individual")
            {
                for (int i = 0; i < singleaddress.Length; i++)
                    lstaddress.Add(singleaddress[i]);
            }
            else if (type == "Entity")
            {
                for (int i = 0; i < singleaddress.Length; i++)
                {

                    var singleAddEntity = singleaddress[i].Split(',');
                    string cout = "", zip = "", state = "", city = "", stret = "", buld = "", oth = "";

                    for (int j = singleAddEntity.Length - 1; j >= 0; j--)
                    {
                        if (singleAddEntity.Length - 1 == j)
                        {
                            //chk for country
                            cout+=singleAddEntity[j];
                          
                        }
                        else if (singleAddEntity.Length - 2 == j)
                        {
                            //chk for zipcode
                            string str = singleAddEntity[j];
                            // Int32 intValue;

                            bool containsNum = Regex.IsMatch(singleAddEntity[j], @"\d");

                            if (containsNum)
                            {
                                // mystring is an integer
                               zip+=singleAddEntity[j];
                            }
                            else
                            {
                                //if not space its a state province
                                state+=singleAddEntity[j];
                            }

                        }
                        else if (singleAddEntity.Length - 3 == j)
                        {
                            //chk for state province
                            if (state=="")
                            {
                                if (singleAddEntity[j].Contains("P.O. Box"))
                                {
                                    //if its not state province and contain po box then it goes to other address info
                                   
                                    oth += singleAddEntity[j];
                                }
                                //  Int32 intValue;

                                else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
                                {
                                    
                                    buld+=singleAddEntity[j];
                                   
                                }

                                else if (singleAddEntity[j].Contains("Street") || singleAddEntity[j].Contains("St."))//||  Regex.IsMatch(singleAddEntity[j], @"\d"))
                                {
                                 
                                    //add in the street
                                    stret+=singleAddEntity[j];
                                  
                                }
                                else
                                {
                                    state+=singleAddEntity[j];
                                   
                                }
                            }
                            else if (state!="")
                            {
                                if (singleAddEntity[j].Contains("P.O. Box"))
                                {
                                    //if its not state province and contain po box then it goes to other address info
                                   
                                    oth += singleAddEntity[j];
                                    //lstaddress.Add(singleAddEntity[j]);
                                }
                                //  Int32 intValue;

                                    
                                else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
                                {
                                    
                                   buld+=singleAddEntity[j];
                                   
                                }

                                else if (singleAddEntity[j].Contains("Street") || singleAddEntity[j].Contains("St."))// || Regex.IsMatch(singleAddEntity[j], @"\d"))
                                {

                                    //add in the street
                                    stret+=singleAddEntity[j];
                                   

                                }
                                else
                                {
                                    oth += singleAddEntity[j];
                                    // lstaddress.Add(singleAddEntity[j]);
                                }

                            }
                        }
                        else if (singleAddEntity.Length - 4 == j)
                        {
                            //for city
                            if (city=="")
                            {
                                if (singleAddEntity[j].Contains("P.O. Box"))
                                {
                                    
                                    oth += singleAddEntity[j];
                                    // lstaddress.Add(singleAddEntity[j]);
                                }
                                //  Int32 intValue;


                                else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
                                {
                                    //for state province , city, street 
                                    buld+=singleAddEntity[j];
                                    
                                }

                                else if (singleAddEntity[j].Contains("Street") || singleAddEntity[j].Contains("St."))// || Regex.IsMatch(singleAddEntity[j], @"\d"))
                                {

                                   
                                    //add in the street
                                    stret+=singleAddEntity[j];
                                   

                                }

                                else if (!regex1.IsMatch(singleAddEntity[j]) && !(Regex.IsMatch(singleAddEntity[j], @"\d")))
                                {
                                    city+=singleAddEntity[j];
                                   
                                }
                                else
                                {
                                    //else its an other information
                                   
                                    oth += singleAddEntity[j];
                                    //lstaddress.Add(singleAddEntity[j]);
                                }
                            }
                            else if (city!="")
                            {
                                if (singleAddEntity[j].Contains("P.O. Box"))
                                {
                                   

                                    oth += singleAddEntity[j];
                                    // lstaddress.Add(singleAddEntity[j]);
                                }
                                //  Int32 intValue;


                                else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
                                {
                                    
                                    buld+=singleAddEntity[j];
                                   
                                }
                                else if (singleAddEntity[j].Contains("Street") || singleAddEntity[j].Contains("St."))//|| Regex.IsMatch(singleAddEntity[j], @"\d"))
                                {

                                    //add in the street
                                   stret+=singleAddEntity[j];
                                  
                                }
                                else
                                {
                                    //else its an other information
                                    
                                    //
                                    oth += singleAddEntity[j];
                                    //lstaddress.Add(singleAddEntity[j]);
                                }
                            }
                        }
                        else if (singleAddEntity.Length - 5 == j)
                        {
                            //for street
                            if (stret=="")
                            {
                                if (singleAddEntity[j].Contains("P.O. Box"))
                                {
                                    
                                    //
                                    oth += singleAddEntity[j];
                                    //lstaddress.Add(singleAddEntity[j]);
                                }
                                //  Int32 intValue;


                                else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
                                {
                                    
                                    
                                    buld+=singleAddEntity[j];
                                   
                                }
                                else
                                {
                                    //add in the street
                                   stret+=singleAddEntity[j];
                                   

                                }

                            }
                            //galti 
                            else if (stret!="")
                            {
                                if (singleAddEntity[j].Contains("P.O. Box"))
                                {
                                  
                                    //
                                    oth += singleAddEntity[j];
                                    //  lstaddress.Add(singleAddEntity[j]);
                                }
                                //  Int32 intValue;


                                else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
                                {
                                    //for  street 
                                    // lstaddress.Add("");
                                    buld+=singleAddEntity[j];
                                   
                                }
                                else
                                {  
                                    //
                                    oth+=singleAddEntity[j];
                                 
                                }

                            }
                        }
                        else if (singleAddEntity.Length - 6 == j)
                        {
                            //for building
                            if (buld=="")
                            {
                                if (singleAddEntity[j].Contains("P.O. Box"))
                                {
                                  
                                    oth += singleAddEntity[j];
                                    //lstaddress.Add(singleAddEntity[j]);
                                }
                                //  Int32 intValue;


                                else if (singleAddEntity[j].Contains("Building") || singleAddEntity[j].Contains("Bldg"))
                                {
                                   
                                    buld+=singleAddEntity[j];
                                  
                                }
                                else
                                {
                                    //add in the other
                                   
                                    oth += singleAddEntity[j];
                                    //  lstaddress.Add(singleAddEntity[j]);


                                }

                            }
                            else if (buld!="")
                            {

                                oth += singleAddEntity[j];
                                //       lstaddress.Add(singleAddEntity[j]);

                            }
                        }
                        //
                        else
                        {

                            oth += singleAddEntity[j];
                            //lstaddress.Add(singleAddEntity[j]);
                        }
                        if(j==0)
                        {
                            lstaddress.Add(cout);
                            lstaddress.Add(zip);
                            lstaddress.Add(state);
                            lstaddress.Add(city);
                            lstaddress.Add(stret);
                            lstaddress.Add(buld);
                            lstaddress.Add(oth);
                        }
                       
                    }
                }
            }


            return lstaddress;

        }
        public static List<string> getAka(string aka,string type)
        {
            List<string> str = new List<string>();

            if (type == "Individual")
            {
                var singleAka = aka.Split(';');
              
                for (int i = 0; i < singleAka.Length; i++)
                {
                    string[] nameFL = new string[2];
                    nameFL = getFirstLastName(singleAka[i]);
                    //lastname
                    str.Add(nameFL[0]);
                    //firstName
                    str.Add(nameFL[1]);

                }
            }
            else if(type=="Entity")
            {
                var singleAka = aka.Split(';');
                
                for (int i = 0; i < singleAka.Length; i++)
                {
                    str.Add(singleAka[i]);
                    
                }
            }
            return str;
        }
        //

        //Not used just for motivation 
        public static void getallAddressAndAka(string gid, string[] source)
        {
            Address = new List<string>();
            Aka = new List<string>();
            int count = -2, count1 = -2;

            // string[] address = new string[59]; int j = 0;
            for (int i = 1; i < source.Length; i++)
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

       // Same isko maat chona 
        private static XElement entityRowCreator(string[] fields, string[] source)
        {
            XElement Entity = new XElement("Entity");

            if (fields[5] == "Entity" )
            {
                //Getting all address
                //getallAddressAka();
                List<string> addr = getAddresses(fields[13],fields[5]);
                //Getting all Aka
                List<string> aka = getAka(fields[43],fields[5]);

                //

                XElement EntRef = new XElement("EntityReference");
                XElement EntName = new XElement("EntityName", fields[9]);
               //adding name
                EntRef.Add(EntName);
                if (aka.Count > 0)
                {
                    if (aka[0] != "\"\"")
                    {
                        XElement EntSynonyms = new XElement("EntitySynonmys");
                        for (int i = 0; i < aka.Count; i++)
                        {
                            EntSynonyms.Add(new XElement("Synonmys", aka[i].Trim('"')));
                        }

                        //adding entity synonyms
                        EntRef.Add(EntSynonyms);

                    }
                }
                //Not required for this document 
                //XElement EntAbbre = new XElement("EntityAbbreviations",
                //                            new XElement("Abbreviations"));//ask client
                
                //    EntRef.Add(EntAbbre);
                
                //adding in Entity
                Entity.Add(EntRef);
                ///
                

                if (addr.Count>0)
                {
                    XElement EntReg = new XElement("EntityRegistrations",
                                        new XElement("EntityRegistration",
                                            new XElement("Country", addr[0]))
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

                    for (int i = 0; i < addr.Count; i += 7)
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
                        if (addr[6 + i].Length != 0 )
                        {
                            XElement Entother = new XElement("AddressOtherDetails",
                                 new XElement("Detail",
                                     new XElement("Name", "Address"),
                                new XElement("Value", addr[6 + i])) );
                            EntAddr.Add(Entother);
                        }


                    }
                    EntAddrs.Add(EntAddr);

                    //adding in the entity list
                    Entity.Add(EntAddrs);
                    //
                }
                //
                //Entity other detail
                if (fields[3].Length != 0)
                {
                    XElement EntotherDet = new XElement("EntityOtherDetails");

                    XElement Entdet1 = new XElement("Detail",
                                                new XElement("Name", "id"),
                                                new XElement("Value", fields[3]));
                    XElement Entdet2 = new XElement("Detail",
                                                new XElement("Name", "entity_number"),
                                                new XElement("Value", fields[3]));
                    EntotherDet.Add(Entdet1);
                    EntotherDet.Add(Entdet2);     
                    Entity.Add(EntotherDet);

                }
                
                //
                //Not req in this doc.

                //if (fields[53].Length != 0)
                //{
                //    XElement Entlist = new XElement("Listed", fields[53]);
                //    Entity.Add(Entlist);
                //}
                //XElement EntDelist = new XElement("DeListed");

                if (fields[7].Length != 0 || fields[1].Length != 0 || fields[53].Length != 0 || fields[41].Length != 0)
                {
                    int[] otherDetailsIndexes = { 7, 1, 53, 41 };
                    string[] fieldNames = { "programs", "source", "source_information_url", "source_list_url" };
                    string[] otherDetailsValues = new string[fieldNames.Length];

                    XElement EntResn = new XElement("ReasonDisclosures");
                    
                    for (int x = 0; x < fieldNames.Length; x++)
                    {
                        otherDetailsValues[x] = fields[otherDetailsIndexes[x]];
                    }//end for x
                    //
                    for (int a = 0; a < otherDetailsValues.Length; a++)
                    {
                        if (otherDetailsValues[a].Length > 1) //please dont hate me for this shit
                        {
                          XElement EntRes= ( new XElement("ReasonDisclosure",
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
            }
            //  }//end for i
            return Entity;
        } //end rowCreator

        // Main ha bahi pichla wala comment kar dena dusri file mey

        //private static void Main()
        //{

        //    var watch = System.Diagnostics.Stopwatch.StartNew();

        //    string[] source = File.ReadAllLines("C:\\Users\\Uzair Tahir\\Downloads\\screening_list-consolidated_2017-05-25.csv");

        //    XDocument doc = ConvertCsvToXML(source);
        //    doc.Save("outputxmlfile10Ent.xml");

        //    // the code that you want to measure comes here
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;

        //}
    }
}
