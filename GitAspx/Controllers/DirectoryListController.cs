namespace GitAspx.Controllers {
	using System.Web.Mvc;
	using GitAspx.Lib;
	using GitAspx.ViewModels;

	public class DirectoryListController : Controller {

		RepositoryService repositories;

		public DirectoryListController(RepositoryService repositories) {
			this.repositories = repositories;
		}

		public ActionResult Index() {
			return View(new DirectoryListViewModel {
				RepositoriesDirectory = repositories.GetRepositoriesDirectory().FullName,
				Directories = repositories.GetAllRepositories()
			});
		}
	}
}