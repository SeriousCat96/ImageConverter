using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace ImageConverter
{
	public class CsvFileConverter
	{
		public const string RootImageFolderName = "images";
		public const string RootOutputFolderName = "out";
		
		public CsvFileConverter(string fileExtension)
		{
			ImageCounter = 0;
			FileExtension = fileExtension;
		}

		public string FileExtension { get; }
		public int ImageCounter { get; private set; }

		public void Convert(string fullFileName)
		{
			var fileInfo = new FileInfo(fullFileName);

			if(fileInfo.Extension != FileExtension) return;

			var bitmap = new Bitmap(fullFileName);

			var folders = Regex
				.Match(fullFileName, $@"{RootImageFolderName}.*")
				.Value;

			var dirName = folders.Substring(RootImageFolderName.Length + 1);

			var d = Path.Combine(Environment.CurrentDirectory, RootOutputFolderName, dirName);
			var outputDir = Directory
				.GetParent(d)
				.FullName;

			if(!Directory.Exists(outputDir))
			{
				Directory.CreateDirectory(outputDir);
			}

			var outputPath = GetCsvFilePath(outputDir);

			if(File.Exists(outputPath)) return;

			AppendText(outputPath, "X,Y,R,G,B");

			for(int x = 0; x < bitmap.Width; ++x)
			{
				for(int y = 0; y < bitmap.Height; ++y)
				{
					var pixel = bitmap.GetPixel(x, y);

					AppendText(outputPath, $"{x},{y},{pixel.R},{pixel.G},{pixel.B}");
				}
			}
		}

		public string GetCsvFilePath(string directory)
		{
			return Path.Combine(directory, (ImageCounter++).ToString() + ".csv");
		}

		public void AppendText(string filename, string text)
		{
			using(var writer = new StreamWriter(filename, true, Encoding.Default))
			{
				writer.WriteLine(text);
				writer.Flush();
			}
		}
	}
}
