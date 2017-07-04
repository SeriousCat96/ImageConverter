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
		public const string RootImageFolderName  = "images";
		public const string RootOutputFolderName = "out";
		public const string OutputFileName       = "data.csv";
		
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

			var opencvMat = new Mat(fullFileName);
			var outputDir = Path.Combine(Directory.GetCurrentDirectory(), RootOutputFolderName);

			if(!Directory.Exists(outputDir))
			{
				Directory.CreateDirectory(outputDir);
			}

			var outputPath = Path.Combine(outputDir, OutputFileName);
			
			// Преобразование цветов в HSV модель
			Cv2.CvtColor(opencvMat, opencvMat, ColorConversion.BgrToHsv);

			var sb = new StringBuilder();

			for(int x = 0; x < opencvMat.Rows; x++)
			{
				for(int y = 0; y < opencvMat.Cols; y++)
				{
					var pixel = opencvMat.At<Vec3b>(x, y);
					// Интенсивность
					sb.Append($"{pixel.Item1},");
				}
			}

			sb.Append($"car{++ImageCounter}");

			AppendText(outputPath, sb.ToString());
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
