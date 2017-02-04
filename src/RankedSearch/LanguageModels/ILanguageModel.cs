namespace RankedSearch.LanguageModels
{
    public interface ILanguageModel
    {
        double Query(string phrase);
    }
}
