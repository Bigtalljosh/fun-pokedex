using FunPokedex.YodaApi.Mocks;
using FunPokemon.YodaApi;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunPokedex.YodaApi.UnitTests
{
    public class YodaApiServiceTests
    {
        [Fact]
        public async Task TranslateToYodaSpeak_ShouldReturnObject_WhenApiReturnsSuccessfully()
        {
            var testString = "this is a test";
            var mockedResponseJson = File.ReadAllText("MockedYodaResponse.json");
            var mockHttpHandler = new MockHttpMessageHandler(mockedResponseJson, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockHttpHandler)
            {
                BaseAddress = new Uri("https://testbase.com/")
            };
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new YodaApiService(httpClient, cache);
            var yoda = await service.TranslateToYodaSpeak(testString);

            Assert.NotNull(yoda);
        }

        [Fact]
        public async Task TranslateToYodaSpeak_ShouldCacheResponse_WhenApiReturnsSuccessfullyAndObjectDoesNotExistInCache()
        {
            var testString = "this is a test";
            var mockedResponseJson = File.ReadAllText("MockedYodaResponse.json");
            var mockHttpHandler = new MockHttpMessageHandler(mockedResponseJson, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockHttpHandler)
            {
                BaseAddress = new Uri("https://testbase.com/")
            };
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new YodaApiService(httpClient, cache);
            var yoda = await service.TranslateToYodaSpeak(testString);

            Assert.Equal(1, cache.Count);
        }

        [Fact]
        public async Task TranslateToYodaSpeak_ShouldNotCacheResponse_WhenApiReturnsFailure()
        {
            var testString = "this is a test";
            var mockedFailedResponseJson = File.ReadAllText("MockedFailedYodaResponse.json");
            var mockHttpHandler = new MockHttpMessageHandler(mockedFailedResponseJson, HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(mockHttpHandler)
            {
                BaseAddress = new Uri("https://testbase.com/")
            };
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new YodaApiService(httpClient, cache);
            var yoda = await service.TranslateToYodaSpeak(testString);

            Assert.Null(yoda);
            Assert.Equal(0, cache.Count);
        }

        [Fact]
        public async Task TranslateToYodaSpeak_ShouldNotCacheResponse_WhenObjectAlreadyExistsInCache()
        {
            var testString = "this is a test";
            var mockedResponseJson = File.ReadAllText("MockedYodaResponse.json");
            var mockHttpHandler = new MockHttpMessageHandler(mockedResponseJson, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockHttpHandler)
            {
                BaseAddress = new Uri("https://testbase.com/")
            };
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new YodaApiService(httpClient, cache);

            await service.TranslateToYodaSpeak(testString);
            Assert.Equal(1, cache.Count);

            await service.TranslateToYodaSpeak(testString);
            Assert.Equal(1, cache.Count);
        }
    }
}
