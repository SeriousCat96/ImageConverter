using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace ImageConverter
{
	public partial class MainForm : Form
	{
		private const string FileExtension = ".jpeg";

		private CsvFileConverter _csvFileConverter;
		private ImageResizer _imageResizer;

		public MainForm()
		{
			InitializeComponent();
			_csvFileConverter = new CsvFileConverter(FileExtension);
			_imageResizer = new ImageResizer();
		}

		private void ConvertFiles()
		{
			var rootDir = Path.Combine(Directory.GetCurrentDirectory(), CsvFileConverter.RootImageFolderName);
			Convert(rootDir);
		}

		private void Convert(string path)
		{
			var directories = Directory.EnumerateDirectories(path);

			if(directories.Count() != 0)
			{
				foreach(var directory in directories)
				{
					Convert(directory);
				}
			}
			else
			{
				var fullFileNames = Directory
					.EnumerateFiles(path)
					.Where(e => e.EndsWith(FileExtension));


				foreach(var fullFileName in fullFileNames)
				{
					
					_imageResizer.Resize(fullFileName);
					_csvFileConverter.Convert(fullFileName);
				}
			}
		}

		private void OnButtonConvertClick(object sender, EventArgs e)
		{
			ConvertFiles();
		}
	}
}
