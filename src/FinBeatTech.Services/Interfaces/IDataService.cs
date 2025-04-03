using FinBeatTech.DTO;

namespace FinBeatTech.Services.Interfaces;

public interface IDataService
{
    Task SaveData(IEnumerable<Dictionary<int, string>> inputData);
    Task<IReadOnlyCollection<DataEntry>> GetData(int? codeFilter = null);
}