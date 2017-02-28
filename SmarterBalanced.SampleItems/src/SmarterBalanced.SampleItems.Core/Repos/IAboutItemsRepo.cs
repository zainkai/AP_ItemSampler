namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public interface IAboutItemsRepo
    {
        AboutItemsViewModel GetAboutItemsViewModel();
        string GetItemViewerUrl(string interactionTypeCode);
    }
}
