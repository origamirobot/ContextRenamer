using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: CLSCompliant(true)]
namespace ContextRenamer
{
	class Program
	{
		private const String KeyName = "Renamer";
		private const String MenuText = "Rename Movie";
		private const String YearDetector = @"((19|20)\d{2})";

		[STAThread]
		static void Main(String[] args)
		{
			// process register or unregister commands
			if (!ProcessCommand(args))
			{
				RenameMovie(args[0]);
			}
		}

		/// <summary>
		/// Renames the movie.
		/// </summary>
		/// <param name="path">Name of the movie.</param>
		static void RenameMovie(String path)
		{
			var directory = new DirectoryInfo(path);
			var name = directory.Name;

			var regex = new Regex(YearDetector);
			var match = regex.Match(name);

			var index = match.Index + 4;
			if (index > name.Length)
				return;

			name = name.Substring(0, index);
			name = name.Replace(".", " ").Trim();
			var year = match.Value;
			name = name.Substring(0, name.Length - 4);
			name = name + String.Format(" ({0})", year);

			var parent = directory.FullName.Replace(directory.Name, "");
			var newPath = Path.Combine(parent, name);

			if (Directory.Exists(newPath))
			{
				MessageBox.Show("Folder already exists");
				return;
			}

			directory.MoveTo(newPath);

		}


		/// <summary>
		/// Process command line actions (register or unregister).
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		/// <returns>True if processed an action in the command line.</returns>
		static bool ProcessCommand(String[] args)
		{
			// register
			if (args.Length == 0 || String.Compare(args[0], "-register", true) == 0)
			{
				// full path to self, %L is placeholder for selected file
				var menuCommand = string.Format("\"{0}\" \"%L\"", Application.ExecutablePath);
				FileShellExtension.Register(KeyName, MenuText, menuCommand);
				MessageBox.Show(String.Format("The {0} shell extension was registered.", KeyName), KeyName);
				return true;
			}

			// unregister		
			if (String.Compare(args[0], "-unregister", StringComparison.OrdinalIgnoreCase) == 0)
			{
				// unregister the context menu
				FileShellExtension.Unregister(KeyName);
				MessageBox.Show(String.Format("The {0} shell extension was unregistered.", KeyName), KeyName);
				return true;
			}

			// command line did not contain an action
			return false;
		}


	}
}
