using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Method
{
    public class Method
    {
        /// <summary>
        /// Constructor for methods
        /// If Zip true then we need to call zip algorithm else unzip.
        /// </summary>
        /// <param name="InputFileStream"> File stream with data </param>
        /// <param name="OutputFileStream"> Clear stream for compressed/decompressed data </param>
        /// <param name="ZipFlag"> Flag of method. If ZipFlag = true then need to call zip method </param>
        public Method(System.IO.Stream InputFileStream, System.IO.Stream OutputFileStream, bool ZipFlag)
        {
            this.BinFileReader = new System.IO.BinaryReader(InputFileStream);
            this.BinFileWriter = new System.IO.BinaryWriter(OutputFileStream);

            if (ZipFlag)
            {
                Compress();
            }
            else
            {
                DeCompress();
            }
        }

        private void Compress()
        {
            StringBuilder str = new StringBuilder();
            while (this.BinFileReader.BaseStream.Position != this.BinFileReader.BaseStream.Length)
            {
                str.Append(this.BinFileReader.ReadChar().ToString());
            }
            Dictionary<string, int> dictionary = DictionaryIni(str.ToString());
            List<int> lst = Arc(str.ToString());

            this.BinFileWriter.Write(dictionary.Count);
            this.BinFileWriter.Write(lst.Count);

            foreach (var item in dictionary.Keys)
            {
                this.BinFileWriter.Write(item);
            }

            foreach (var item in lst)
            {
                this.BinFileWriter.Write(item);
            }
            Console.WriteLine("lst");
            int j = 0;
            foreach (var item in lst)
            {
                Console.WriteLine("{0}:{1}", j, item);
                j++;
            }
            Console.WriteLine("Info\n Dictionary Count:{0}\nList Count:{1}", dictionary.Count, lst.Count);
        }
        private void DeCompress()
        {
            int dicCount = this.BinFileReader.ReadInt32();
            int lstCOunt = this.BinFileReader.ReadInt32();
            Console.WriteLine("ReadInfo\n Dictionary Count:{0}\nList Count:{1}", dicCount, lstCOunt);
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < dicCount; i++)
            {
                str.Append(this.BinFileReader.ReadString());
            }
            Dictionary<string, int> dictionary = DictionaryIni(str.ToString());
            List<int> lst = new List<int>();
            for (int i = 0; i <= lstCOunt; i++)
            {
                lst.Add(this.BinFileReader.ReadInt32());
            }

            string decompress = Dar(lst, dictionary);

            foreach (var item in decompress)
            {
                this.BinFileWriter.Write(item);
            }

            Console.WriteLine("lst1");
            int j = 0;
            foreach (var item in lst)
            {
                Console.WriteLine("{0}:{1}",j,item);
                j++;
            }

            
        }
        private System.IO.BinaryReader BinFileReader
        {
            get;
            set;
        }
        private System.IO.BinaryWriter BinFileWriter
        {
            get;
            set;
        }
        //-------------------------------------------------------------------------------------
        private static string Dar(List<int> compressed, Dictionary<string, int> dic)
        {

            Dictionary<int, string> dictionary = DictionaryIni(dic);


            string w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);


                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }
        private static List<int> Arc(string uncompressed)
        {
            Dictionary<string, int> dictionary = DictionaryIni(uncompressed);

            string w = string.Empty;

            List<int> compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;

                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {


                    compressed.Add(dictionary[w]);
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }


            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }
        private static Dictionary<string, int> DictionaryIni(string str)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string w = string.Empty;

            foreach (var c in str)
            {
                string wc = w + c;
                if (dic.ContainsKey(wc))
                {
                    continue;
                }
                else
                {
                    dic.Add(wc, dic.Count);
                }

            }

            return dic;
        }
        private static Dictionary<int, string> DictionaryIni(Dictionary<string, int> dic)
        {
            Dictionary<int, string> tmp = new Dictionary<int, string>();

            List<string> stringlist = new List<string>();

            foreach (var item in dic.Keys)
            {
                stringlist.Add(item);
            }


            int i = 0;
            foreach (var e in dic)
            {

                int tmp1 = dic[stringlist[i]];


                tmp.Add(tmp1, stringlist[i]);
                i++;

            }

            return tmp;
        }

    }
}