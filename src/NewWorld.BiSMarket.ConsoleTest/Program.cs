// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging;
using NewWorld.BiSMarket.Core;

Console.WriteLine("Hello, World!");

//PerkOcrExport.Export(@"C:\Users\kkass\OneDrive\Masaüstü\perks", @"C:\Users\kkass\OneDrive\Masaüstü\perks\export.txt");

var path = @"C:\Users\kkass\OneDrive\Masaüstü\NWITems\  (3).png";
var imageBytes = File.ReadAllBytes(path);
var ocr = ItemImageOcrV2.Create(imageBytes);

var result = ocr.Read();

Console.ReadLine();