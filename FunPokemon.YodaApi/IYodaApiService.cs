using System.Threading.Tasks;

namespace FunPokemon.YodaApi
{
    public interface IYodaApiService
    {
        Task<YodaApiResponse> TranslateToYodaSpeak(string textToTranslate);
    }
}