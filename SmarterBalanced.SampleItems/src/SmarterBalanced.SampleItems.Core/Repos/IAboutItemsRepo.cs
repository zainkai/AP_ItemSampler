namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public interface IAboutItemsRepo : IItemViewRepo
    {
        AboutItemsViewModel GetAboutItemsViewModel();
        string GetItemViewerUrl(string interactionTypeCode);
    }
}
