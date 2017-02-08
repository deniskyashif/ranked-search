namespace RankedSearch.LanguageModels
{
    using System.Collections.Generic;

    public interface ILanguageModel
    {
        IEnumerable<string> Terms { get; }

        double Query(string phrase);
        
    }
}
