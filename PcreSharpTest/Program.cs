using System;
using System.Text;
using System.Text.RegularExpressions;
using PcreSharp;

namespace PcreSharpTest
{
	class Program
	{
		private static Random rand = new Random();
		private const int StrLen = 1000000;
		static void Main(string[] args)
		{
			string str = GenerateRandomDataString(StrLen);

		    MatchCollection c;
            

            var regex = new PcreRegex("\"id\\d+\"", PcreOptions.NONE, PcreStudyOptions.PCRE_STUDY_JIT_COMPILE);
            var start = DateTime.Now;
			Console.WriteLine(regex.MatchCount(str));
            Console.WriteLine((DateTime.Now - start).TotalMilliseconds);

            start = DateTime.Now;
            Test(regex, str);
            Console.WriteLine((DateTime.Now - start).TotalMilliseconds);

			if (args.Length > 0) Console.ReadKey();
		}

        private static void Test(PcreRegex regex, string str)
        {
            var match = regex.Match(str);

            while (match.Success)
            {
                //Console.WriteLine(match.Value);
                match = match.NextMatch();
            }
        }

		private static string GenerateRandomDataString(int len)
		{
			var builder = new StringBuilder();

			for (int i = 0; i < len; i++)
			{
				builder.Append("\"");
				GenerateRandomString(builder, 12);
				builder.Append("\":\"id");
				builder.Append(i);
				builder.Append("\",");
			}

			return builder.ToString();
		}

		private static void GenerateRandomString(StringBuilder builder, int len)
		{
			char[] chars = {'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у',
			               	'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			               	'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
			               	'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7',
			               	'8', '9'
			               };

			int count = chars.Length;
			for (int i = 0; i < len; i++)
			{
				builder.Append(chars[rand.Next(count)]);
			}
		}
	}
}
