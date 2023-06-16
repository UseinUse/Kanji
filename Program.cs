using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LeaningKanji
{
    internal class Program
    {
        static class Data
        {
            public static readonly string kanjiFilePath = Directory.GetCurrentDirectory() + "\\kanji.txt";
            public static readonly string koreanFilePath = Directory.GetCurrentDirectory() + "\\korean.txt";

            public static string[] allKanji;
            public static string[] allKorean;
            public static Dictionary<string, Kanji> kanjiInfo;
            public static Dictionary<string, string> korMeaningsInfo = new Dictionary<string, string>();

            public static int Length => allKanji.Length;

            public static void Init()
            {
                allKorean = File.ReadAllLines(koreanFilePath);
                allKanji = File.ReadAllLines(kanjiFilePath);
                kanjiInfo = new Dictionary<string, Kanji>(allKanji.Length);

                for (int i = 0; i < allKanji.Length; i++)
                {
                    korMeaningsInfo.Add(allKanji[i], allKorean[i]);
                    kanjiInfo.Add(allKanji[i], null);
                }
            }
        }

        public class Kanji
        {
            public string kanji;
            public int strokeCount;
            public int jlptLevel;
            public string[] korMeanings;
            public string[] kunReadings;
            public string[] onReadings;
            public string[] nameReadings;        

            public Kanji(string kanji, int strokeCount, int jlptLevel, string[] korMeanings, string[] kunReadings, string[] onReadings, string[] nameReadings)
            {
                this.kanji = kanji ?? "";
                this.strokeCount = strokeCount;
                this.jlptLevel = jlptLevel;
                this.korMeanings = korMeanings ?? new string[] { "" };
                this.kunReadings = kunReadings ?? new string[] { "" };
                this.onReadings = onReadings ?? new string[] { "" };
                this.nameReadings = nameReadings ?? new string[] { "" };
            }

            public void Print()
            {
                Console.WriteLine(
                    "\n[한자 정보]\n\n" +
                    $"  한자  :  {kanji} \n" +
                    $"  획수  :  {strokeCount}획 \n" +
                    $"  JLPT  :  {(jlptLevel < 1 ? "" : "N" + jlptLevel)} \n" +
                    $" 뜻, 음 :  {string.Join(", ", korMeanings)}  \n" +
                    $"  훈독  :  {string.Join(", ", kunReadings)}  \n" +
                    $"  음독  :  {string.Join(", ", onReadings)}  \n" +
                    $"  명독  :  {string.Join(", ", nameReadings)}");

            }           
        }

        public class KanjiApi
        {
            readonly string url = "https://kanjiapi.dev/v1/kanji/";

            public KanjiApi()
            {
                Data.Init();
                SetKanjiApi();
                Display();
            }

            string GetKanji(string kanji)
            {
                WebRequest request = WebRequest.Create(url + kanji);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string data = reader.ReadToEnd();

                return data;
            }

            void SetKanjiApi()
            {
                Task.Run(() =>
                {
                    Parallel.For(0, Data.Length, i =>
                    {
                        try
                        {
                            if (i == Data.Length)
                                return;

                            string kanji = Data.allKanji[i];
                            string data = GetKanji(kanji);
                            JObject json = JObject.Parse(data);
                            if (data == "" || json == null || Data.kanjiInfo[kanji] != null)
                                return;

                            if (!int.TryParse(json["stroke_count"].ToString(), out int strokeCount))
                                strokeCount = -1;
                            if (!int.TryParse(json["jlpt"].ToString(), out int jlptLevel))
                                jlptLevel = -1;

                            Data.kanjiInfo[kanji] = new Kanji(
                                json["kanji"].ToString(),
                                strokeCount,
                                jlptLevel,
                                Data.korMeaningsInfo[kanji].Split('/'),
                                json["kun_readings"].Select(x => x.ToString()).ToArray(),
                                json["on_readings"].Select(x => x.ToString()).ToArray(),
                                json["name_readings"].Select(x => x.ToString()).ToArray());

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }                       
                    });                 
                });
            }
     
            void Display()
            {
                Random r = new Random();
                int index = r.Next(0, Data.Length);
                string kanji = Data.allKanji[index];

                ConsoleKey escapeKey = GetKey();

                if (escapeKey == ConsoleKey.Escape)
                {
                    Console.WriteLine("시스템을 종료합니다...");
                    return;
                }

                while (Data.kanjiInfo[kanji] == null)
                {
                    index = r.Next(0, Data.Length);
                    kanji = Data.allKanji[index];
                }

                Data.kanjiInfo[kanji].Print();
                Display();

            }

            ConsoleKey GetKey()
            {
                while (true)
                    if (Console.KeyAvailable)
                        return Console.ReadKey(intercept: true).Key;
            }

        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            KanjiApi api = new KanjiApi();
            
        }
    }
}
