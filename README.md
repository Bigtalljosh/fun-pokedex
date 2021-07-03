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

Once you have that installed you should be able to navigate into the folder in this repo which contains the `.sln` file (the root), open up a terminal and run

`dotnet run --project .\FunPokedex.Api\FunPokedex.Api.csproj`

If you navigate to the `/swagger` path in the url you should be presented with a Swagger screen where you can easily test this application.
