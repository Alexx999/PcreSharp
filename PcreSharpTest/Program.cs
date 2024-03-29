﻿using System;
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

			int i;
			int testNum = 10;
			DateTime start;

			start = DateTime.Now;
			var regex = new PcreRegex("abcd", PcreOptions.NONE, PcreStudyOptions.NONE);
			Console.WriteLine("First creation time: " + (DateTime.Now - start).TotalMilliseconds);
			start = DateTime.Now;
			regex = new PcreRegex("\"id\\d+\"", PcreOptions.NONE, PcreStudyOptions.PCRE_STUDY_JIT_COMPILE);
			Console.WriteLine("JIT creation time: " + (DateTime.Now - start).TotalMilliseconds);
			/*
			var abc = "qwerty \"id5\" qwerty";
			var bca = regex.Replace(abc, "bca");
			*/
			start = DateTime.Now;
			for (i = 0; i < testNum; i++)
			{
				regex.Replace(str, "bca");
			}
			Console.WriteLine((DateTime.Now - start).TotalMilliseconds / testNum);

			start = DateTime.Now;
			for (i = 0; i < testNum; i++)
			{
				var a = regex.Matches(str).Count;
			}
			Console.WriteLine((DateTime.Now - start).TotalMilliseconds / testNum);

			start = DateTime.Now;
			for (i = 0; i < testNum; i++)
			{
				Test(regex, str);
			}
			Console.WriteLine((DateTime.Now - start).TotalMilliseconds / testNum);

			start = DateTime.Now;
			var regexOrig = new Regex("abcd", RegexOptions.None);
			Console.WriteLine("First creation time: " + (DateTime.Now - start).TotalMilliseconds);
			start = DateTime.Now;
			regexOrig = new Regex("\"id\\d+\"", RegexOptions.Compiled);
			Console.WriteLine("Second creation time: " + (DateTime.Now - start).TotalMilliseconds);

			start = DateTime.Now;
			for (i = 0; i < testNum; i++)
			{
				regexOrig.Replace(str, "bca");
			}
			Console.WriteLine((DateTime.Now - start).TotalMilliseconds / testNum);

			start = DateTime.Now;
			for (i = 0; i < testNum; i++)
			{
				//regexOrig.Replace(str,"str",)
				var a = regexOrig.Matches(str).Count;
			}
			Console.WriteLine((DateTime.Now - start).TotalMilliseconds / testNum);

			start = DateTime.Now;
			for (i = 0; i < testNum; i++)
			{
				TestOrig(regexOrig, str);
			}
			Console.WriteLine((DateTime.Now - start).TotalMilliseconds / testNum);
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

		private static void TestOrig(Regex regex, string str)
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
