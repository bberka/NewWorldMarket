using IronOcr;

namespace NewWorld.BiSMarket.Core;

public static class PerkOcrExport
{
    public static void Export(string folderPath, string exportTextFilePath)
    {
        var list = new List<string>();
        var fileList = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);
        foreach (var file in fileList)
        {
            var ocr = new IronTesseract();
            var input = new OcrInput();
            input.AddImage(file);
            input.Scale(200);
            input.Contrast(1.6F);
            input.DeNoise();
            ocr.Language = OcrLanguage.English;
            var result = ocr.Read(file);
            var lines = result.Lines.Select(x => x.Text);
            list.AddRange(lines);
        }

        var distinct = list.Distinct().ToList();
        var text = string.Join(Environment.NewLine, distinct);
        File.WriteAllText(exportTextFilePath, text);
    }
}