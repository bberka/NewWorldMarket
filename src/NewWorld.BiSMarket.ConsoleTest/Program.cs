// See https://aka.ms/new-console-template for more information

using NewWorld.BiSMarket.Core;

Console.WriteLine("Hello, World!");

//PerkOcrExport.Export(@"C:\Users\kkass\OneDrive\Masaüstü\perks", @"C:\Users\kkass\OneDrive\Masaüstü\perks\export.txt");

var ocr = ItemImageOcr.Create(@"C:\Users\kkass\OneDrive\Masaüstü\NWITems\1.png");

var result = ocr.Read();

Console.WriteLine();