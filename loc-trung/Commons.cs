using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace loc_trung
{
    class Commons
    {

        public static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }


        public static List<UserLicense> GetLicenseKey()
        {
            string spreadSheet = "1sCWCCCQ01fycXOldoMGmfRFjZcO3PtPJJY6v3YvyvS4";
            string apikey = "AIzaSyCcx0bxkTkbGVQbnLgE4LU3u8jBAHwW8r4";
            string apiUrl = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadSheet}/values/SOFT_KEY!A1:E100?key={apikey}";

            List<UserLicense> list = new List<UserLicense>();
 
            WebRequest req = WebRequest.Create(apiUrl);
            using (WebResponse resp = req.GetResponse())
            using (StreamReader read = new StreamReader(resp.GetResponseStream()))
            {
                string response = read.ReadToEnd();

                JObject jdata = JObject.Parse(response);

                JArray valuesTexts = (JArray)jdata["values"];

                foreach (JArray item in valuesTexts) // <-- Note that here we used JObject instead of usual JProperty
                {
                    UserLicense user = new UserLicense();
                    user.Key = item[0].ToString();
                    user.ExpireDate = item[1].ToString();
                    list.Add(user);
                    Console.WriteLine(user);
               
                    // ...
                }

                Console.WriteLine(response);

            }

            return list;

        }

        public static List<HardDrive> GetAllDiskDrives()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            List<HardDrive> hdCollection = new List<HardDrive>();
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                HardDrive hd = new HardDrive();
                hd.Model = wmi_HD["Model"].ToString();
                hd.InterfaceType = wmi_HD["InterfaceType"].ToString();
                hd.Caption = wmi_HD["Caption"].ToString();

                hd.SerialNo = wmi_HD.GetPropertyValue("SerialNumber").ToString();//get the serailNumber of diskdrive

                hdCollection.Add(hd);
            }

            return hdCollection;

        }


        public class HardDrive
        {
            public string Model { get; set; }
            public string InterfaceType { get; set; }
            public string Caption { get; set; }
            public string SerialNo { get; set; }

            public String ToString()
            {
                return Model + SerialNo;
            }
        }

        public class UserLicense
        {
            public string Key { get; set; }
            public string ExpireDate { get; set; }

            public String ToString()
            {
                return Key + " " +ExpireDate;
            }
        }
    }
}
