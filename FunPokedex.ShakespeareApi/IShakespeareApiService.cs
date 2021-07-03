using System.Threading.Tasks;

namespace FunPokedex.ShakespeareApi
{
    public interface IShakespeareApiService
    {
        Task<ShakespeareApiResponse> TranslateToShakespeareSpeak(string textToTranslate);
    }
}