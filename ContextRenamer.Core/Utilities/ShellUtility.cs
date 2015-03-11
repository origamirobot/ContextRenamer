using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ContextRenamer.Core.Utilities
{

	/// <summary>
	/// 
	/// </summary>
	public class ShellUtility : IShellUtility
	{

		/// <summary>
		/// Register a simple shell context menu.
		/// </summary>
		/// <param name="shellKeyName">Name that appears in the registry.</param>
		/// <param name="menuText">Text that appears in the context menu.</param>
		/// <param name="menuCommand">Command line that is executed.</param>
		public static void Register(String shellKeyName, String menuText, String menuCommand)
		{
			if (shellKeyName == null)
				throw new ArgumentNullException("shellKeyName");

			if (menuText == null)
				throw new ArgumentNullException("menuText");

			if (menuCommand == null)
				throw new ArgumentNullException("menuCommand");


			Debug.Assert(!String.IsNullOrEmpty(shellKeyName) &&
				!String.IsNullOrEmpty(menuText) &&
				!String.IsNullOrEmpty(menuCommand));

			// create full path to registry location
			var regPath = string.Format(@"Folder\shell\{0}", shellKeyName);

			// add context menu to the registry
			using (var key = Registry.ClassesRoot.CreateSubKey(regPath))
			{
				key.SetValue(null, menuText);
			}

			// add command that is invoked to the registry
			using (var key = Registry.ClassesRoot.CreateSubKey(String.Format(@"{0}\command", regPath)))
			{
				key.SetValue(null, menuCommand);
			}
		}

		/// <summary>
		/// Unregister a simple shell context menu.
		/// </summary>
		/// <param name="fileType">The file type to unregister.</param>
		/// <param name="shellKeyName">Name that was registered in the registry.</param>
		public static void Unregister(String shellKeyName)
		{
			Debug.Assert(!String.IsNullOrEmpty(shellKeyName));

			// full path to the registry location			
			var regPath = String.Format(@"Folder\shell\{0}", shellKeyName);

			// remove context menu from the registry
			Registry.ClassesRoot.DeleteSubKeyTree(regPath);
		}

	}

}
