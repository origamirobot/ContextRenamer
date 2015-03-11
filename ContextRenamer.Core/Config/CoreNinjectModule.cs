using ContextRenamer.Core.Utilities;
using Ninject.Modules;

namespace ContextRenamer.Core.Config
{

	/// <summary>
	/// Responsible for wiring up the concrete implementations of the interface contracts in this assembly.
	/// </summary>
	public class CoreNinjectModule : NinjectModule
	{

		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			Kernel.Bind<IShellUtility>().To<ShellUtility>();
			Kernel.Bind<IFileUtility>().To<FileUtility>();

		}

	}

}
