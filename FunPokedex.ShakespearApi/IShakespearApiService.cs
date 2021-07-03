using System.Threading.Tasks;

namespace FunPokedex.ShakespearApi
{
    public interface IShakespearApiService
    {
        Task<ShakespearApiResponse> TranslateToShakespearSpeak(string textToTranslate);
    }
}