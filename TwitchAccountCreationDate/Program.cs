using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TwitchAccountCreationDate
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isNetAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (isNetAvailable)
            {
                bool doAnother = true;
                while (doAnother)
                {
                    Program.fetchDateAndTime();

                    Console.Write("Do you want to do another (y/n): ");
                    ConsoleKeyInfo cKI = Console.ReadKey(false);
                    Console.Write('\n');
                    if (cKI.KeyChar != 'y')
                    { doAnother = false; }
                }
            }
        }

        static void fetchDateAndTime()
        {
            System.Net.WebClient wC = new WebClient();
            wC.Headers.Add("Accept: application/vnd.twitchtv.v2+json");
            StringBuilder url = new StringBuilder("https://api.twitch.tv/kraken/users/");
            Console.WriteLine("Please input the Username: ");
            string input = Console.ReadLine();
            url.Append(input);
            try
            {
                string str = wC.DownloadString(url.ToString());
                //Console.WriteLine(str);

                int keepingTrack = 0;
                int skip4Commas = 0;
                bool done = false;
                StringBuilder sB = new StringBuilder();
                foreach (var v in str)
                {
                    keepingTrack++;
                    if (v == ',') { skip4Commas++; }
                    if (skip4Commas == 4)
                    {
                        for (int i = keepingTrack; i <= 50000; i++)
                        {
                            if (str[i] == '"' || str[i] == ':') { } else { sB.Append(str[i]); }
                            if (str[i] == ',') { done = true; break; }
                        }
                    }
                    if (done == true) { break; }
                }
                Console.WriteLine("The Account was Created On Date: ");
                StringBuilder cD = new StringBuilder();
                for (int i = 0; i <= sB.Length; i++)
                {
                    if (i >= 10)
                    {
                        cD.Append(sB[i]);
                        if (sB[i] == 'T')
                        { break; }
                    }
                }
                cD.Remove(cD.Length - 1, 1);
                Console.WriteLine(cD);

                Console.WriteLine("And the Time was: ");
                StringBuilder cT = new StringBuilder();
                bool donedone = false;
                for (int i = 0; i <= sB.Length; i++)
                {
                    if (sB[i] == 'T')
                    {
                        for (int j = i; j <= sB.Length - 1; j++)
                        { cT.Append(sB[j]); }
                        donedone = true; break;
                    }
                    if (donedone == true) { break; }
                }
                cT.Remove(cT.Length - 2, 2);
                cT.Remove(0, 1);
                int dotdot = 0;
                foreach (var v in cT.ToString())
                {
                    dotdot++;
                    if (dotdot == 1 || dotdot == 3 || dotdot == 5)
                    { Console.Write(':'); }
                    Console.Write(v);
                }
                Console.Write('\n');
            }
            catch (System.Exception e)
            { Console.WriteLine("User Does Not Exist On Twitch.tv"); }
        }
    }
}
