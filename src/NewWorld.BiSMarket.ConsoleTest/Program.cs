﻿// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging;
using NewWorld.BiSMarket.Core;

Console.WriteLine("Hello, World!");

//PerkOcrExport.Export(@"C:\Users\kkass\OneDrive\Masaüstü\perks", @"C:\Users\kkass\OneDrive\Masaüstü\perks\export.txt");

var path = @"C:\Users\kkass\OneDrive\Masaüstü\NWITems\6.png";
var imageBytes = File.ReadAllBytes(path);
var ocr = ItemImageOcrV3.Create(imageBytes);
ocr.SaveAsImages(@"C:\Users\kkass\OneDrive\Masaüstü\outputOCR\");
var result = ocr.Read();

//var path2 = @"C:\Users\kkass\OneDrive\Masaüstü\NWITems\4.png";
//var imageBytes2 = File.ReadAllBytes(path2);
//var ocr2 = ItemImageOcrV3.Create(imageBytes2);
//var result2 = ocr.Read();

Console.ReadLine();

