namespace RankedSearch.LanguageModels
{
    using System.Collections.Generic;

    public interface ILanguageModel
    {
        IEnumerable<string> NGrams { get; }

        double Query(string phrase);
    }
}
