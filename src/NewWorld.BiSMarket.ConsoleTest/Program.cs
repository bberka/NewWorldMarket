// See https://aka.ms/new-console-template for more information

using NewWorld.BiSMarket.Core;

Console.WriteLine("Hello, World!");

//PerkOcrExport.Export(@"C:\Users\kkass\OneDrive\Masaüstü\perks", @"C:\Users\kkass\OneDrive\Masaüstü\perks\export.txt");

var ocr = ItemImageOcr.Create(@"C:\Users\kkass\OneDrive\Masaüstü\NewWorld_InTJAoxKXS.png");
var result = ocr.Read();