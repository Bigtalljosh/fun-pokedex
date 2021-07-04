# fun-pokedex

## The Challenge

Write 2 API endpoints.
The first one will take a Pokemon name(or id) as the route param and return the details for that Pokemon.
Details are:

* Name
* Description
* Habitat
* IsLegendary

The second one will modify the description.
If the Pokemon is Legendary or the Pokemon's habitat is "Cave" then translate the Description to Yoda-Speak.
Otherwise translate the description into Shakespearean.
If for any reason you're unable to translate, then the description should remain as the normal version.

Now, I know there are .NET API Clients that exist for this already, whilst the task doesn't forbid me from using those, I am assuming I'm not supposed to use them otherwise this wouldn't be much of a test 😋

## Running the solution

Pre-reqs are .NET 5.
You can install this by downloading from [here and running the installer](https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-5.0.301-windows-x64-installer).

### Dotnet

Once you have that installed you should be able to navigate into the folder in this repo which contains the `.sln` file (the root), open up a terminal and run

`dotnet run --project .\FunPokedex.Api\FunPokedex.Api.csproj`

If you navigate to the `/swagger` path in the url you should be presented with a Swagger screen where you can easily test this application.

### Docker

This project has Docker support, you can get docker from [here and run the installer](https://www.docker.com/products/docker-desktop). Once that's installed, in the root dir of this project run the following command:

`docker build -f .\FunPokedex.Api\Dockerfile -t fun-pokedex:1.0 .`

and then

`docker run -it --rm -p 5000:80 --name fun_pokedex fun-pokedex:1.0`

and finally, navigate to the page in your browser by visiting the following:

`http://localhost:5000/swagger`

## Potential issues + What I'd do for a production service

This is not a perfect solution, there's a couple of things I'd change were this for a real production service. Some of the things I'd change are:

* Swagger is enabled in release mode
* The tests can easily be broken with a simple return Ok(new Pokemon {.....}); We should be checking the content too
* Given the rate limiting on the third party API(s) and the brittle behaviour this causes, I'd be considering if we could store the responses in our own DB not just in a cache
* I'd question if the validation logic belongs in the business layer - I put it there for now with the train of thought that multiple controllers might end up using this same service and end up duplicating code and possibly having different Sanitisation logic. Didn't want to overthink and tried to keep it simple for this demo though
* I would swap out the Base64 encoding for SHA256 and take the first 30 or so characters only. SHA256 has a much better colission rate and given there's < 1000 Pokemon I think the first 30 characters would be plenty enough to presume a unique key whilst also making it much harder for us to cause an issue where the cache memory is filled
