using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using File11Namespace;
using File1Namespace;
using File3Namespace;
using File5Namespace;
using File6Namespace;
using File7Namespace;

namespace TestCsvToXml
{
    class Driver
    {
        /*
         * Arguments for Main are:
         * 1. Source URI
         * 2. Download Location
         * 3. Scheduling DateTime (MM/DD/YYYY HH:MM:SS format)
         * 4. 
         */
        public static void Main(string[] args)
        {
            if (args[0].Contains("ec.europa.eu")) //matlab k file 1
            {
                File1XmltoXml.File1(args[1]);
            }//end if
            else if (args[0].Contains("downloads/consolidated/consolidated.xml")) //matlab k file 3
            {
                File3XmltoXml.File3(args[1]);
            }//end elseif
            else if (args[0].Contains("scsanctions.un.org/resources/xml/en/consolidated.xml")) //matlab k file 5
            {
                File5XmltoXml.File5(args[1]);
            }//end elseif
            else if (args[0].Contains("scsanctions.un.org/taliban")) //matlab k file 6
            {
                File6XmltoXml.File6(args[1]);
            }//end elseif
            else if (args[0].Contains("s3.amazonaws.com/sanctionsconlist.csv")) //matlab k file 7
            {
                File7CsvtoXml.File7(args[1]);
            }//end elseif
            else if (args[0].Contains("compliance/documents/debar.csv")) //matlab k file 11
            {
                File11CsvtoXml.File11(args[1]);
            }//end else if
            else //matlab k error
            {
                /*
                 * Some code about invalid file code goes 
                 * here and an erronous log is generated
                 */
            }//end else
        }//end Main
    }
}
