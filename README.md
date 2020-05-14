# Foundation.Api

## Description

This template uses .Net Core 3.1, to create a foundation for a standard CRUD API using the .Net Template Engine to create new projects on demand using `dotnet new`.

## Prerequisites

- VS2019 .Net Core Feature Set or .Net Core 3.1 SDK https://dotnet.microsoft.com/download/dotnet-core/3.1

## ToDo

✅ Basic Scaffold

✅ Basic CRUD Operations

✅ Add Fluent Validation

✅ Add Custom Pagination

✅ Add Sieve Filters and Sorting

✅ Add Unit Tests

✅ Add CQRS Config

✅ Mediatr Tests

🔲 Debug Integration Test Failures

🔲 Refactor Partial Update and Get List Mediatr Handlers to Not Use Controller

🔲 Breakout Startup Environments

🔲 Add Logging

🔲 Devops Pipeline

🔲 Config File (i.e. CloudFormation script)

## First Time Template Installation

For your first time using this template, follow these steps to get things set up.

1. Clone this repository to your computer in an accessible location: `git clone https://github.com/pdevito3/foundation.api`
2. Open a command prompt to the folder directory at the root of the api template repo
3. Run the following command: `dotnet new -i .\`
4. Confirm the installation by running `dotnet new -l` 

## Using the Template to Create an API
Once a successful installation has completed, you can use the `dotnet new` command along with a few different parameters to create your project. 

### Parameters
* **Name**: `-n|--name` This is the name of your project. Generally, I do the company or app name, then the plural entity, then Api as a period separated name in Pascal Case (e.g. CarbonKitchen.Recipes.Api)
* **EntityName**: `-e|--entity` This is the primary database entity that your API will be interacting with (e.g. Orders, OrderItems). **The parameter should be singular case and PascalCase (not camelCase)**.
* **EntityNameCamel**: `-en|--entitycamel` This should be the same as the EntityName parameter, but in camelCase and not PascalCase (e.g. orders, orderItems).
* **LambdaInitials**: `-la|--lambdainitials` This is the value that will be used in lambda expressions. Generally, this would be an all lowercase value that uses the first letter in each word of the entity name (e.g. o for orders, oi for OrderItems)

### Example Command for Creating an API for a Recipe Entity
```bash
λ dotnet new foundation -n CarbonKitchen.Recipes.Api -e "Recipe" -en "recipe" --la "r"
```

## Updating/Removing The Template
After your first use of the template, you'll want to make sure that you're using the most recent version. Unfortunately there isn't a smooth way to update the template after install. You must first manually remove the template and re-add it once again after retrieving the latest commit from this repository. This can be done using the `dotnet new -u [your repo dir]`, for example: `dotnet new -u "C:\Users\Paul\Documents\repos\Foundation.Api"` 

### Creating a New Project with an Updated Template
1. Pull the latest updates from the master remote.
2. Using the command prompt, `cd` to your your repository directory.
3. Run the uninstall script using `new`. Use `dotnet new -u [your repo dir]`. For example: `dotnet new -u "C:\Users\Paul\Documents\repos\Foundation.Api"`.
4. run `dotnet new -i .\` to reinstall the foundation template 
5. `cd` to the directory that you want to add your new repository
6. run `dotnet new foundation` as you normally would

#### Condensed Code For Using an Updated Template 
```bash
λ cd "C:\Users\Paul\Documents\repos\Foundation.Api"

λ dotnet new -u "C:\Users\Paul\Documents\repos\Foundation.Api"

λ dotnet new -i .\

λ cd..

λ dotnet new foundation -n CarbonKitchen.Recipes.Api -e "Recipe" -en "recipe" -la "r"
```

## What to Do with a Generated Project

This template will scaffold out the bones of your project, but there are a few things you'll need to do to have it operate with you entity.

1. Update the Entity and the DTOs with the correct properties. These are named so that you can easily do a global search and replace `CTRL+SHIFT+F` with whatever parameter you need to use. Don't forget to update your entity to use data annotations that match your database table and sieve attributes for any properties you want to have access to sorting and/or filtering.

   - The creation and update DTOs will inherit from the manipulation DTO to share configuration. Properties can be overridden on the update or creation dtos for operation specific logic.

2. Update the validators to perform whatever validation checks you'd like to use. There validators use [fluent validation](https://github.com/FluentValidation/FluentValidation).

3. Update the QueryString search in the Repository Get List method to use whichever fields you'd like

4. Update unit tests for gets to accommodate filter and sorts. 

   

## Project Organization

### The Mediator Pattern and CQRS

Generally, you will see projects where controllers do most of the heavy lifting. A request comes in, gets routed to the controller and that's that. You probably have a repository for your data access layer, but your controller still has a lot of logic built into it. 

With a mediator pattern, we still have a request come in that goes to the controller, but now instead of having all of our logic in the controller, **our controller will call mediator and it will route our request to the proper handler for that business logic**.

To do this, we're going to organize our project using CQRS which stands for <u>C</u>ommand <u>Q</u>uery <u>R</u>esponsibility <u>S</u>egregation. This means that we'll take the information we get in the request to our controller, along with any other information we may want, and package that info into a what's called a Command or a Query. These are just payloads to carry our data for us. Commands are used whenever we are mutating data (e.g. POST, PUT, PATCH, DELETE, etc.) and Queries are used when we aren't (e.g. GET).

Once we have all the info we need in our command or our query, mediator is going to send it to a Handler. This handler will perform all of the hard work and then return whatever we need back to the controller, which will then send a response back to the client. 

This project uses [Mediatr](https://github.com/jbogard/MediatR) to streamline this process. The process goes something like this:

1. The request comes in and gets routed to the controller

2. In the controller, we create a new instance of whatever query or command we want to use like so:

   ```csharp
   var query = new GetAllRecipesQuery(recipeParametersDto, this);
   //or
   var command = new CreateRecipeCommand(recipeForCreationDto, this);
   ```

3. Then, we can use mediator to send a request through like so:

   ```csharp
   var result = _mediator.Send(query);
   //or
   var result = _mediator.Send(command);
   ```

4. Mediator will then look at all of the handlers in our project that we've designated with `IRequestHandler<TRequest, TResponse>` and find the one that takes in the query or command in question. It knows this by looking at the `TRequest` parameter, with the `TResponse` parameter denoting what the Handler will return. For example:

   ```csharp
   public class GetAllValueToReplacesHandler : IRequestHandler<GetAllRecipesQuery, IActionResult> {}
   //or
   public class CreateValueToReplaceHandler : IRequestHandler<CreateRecipeCommand, ActionResult<RecipeDto>> {}
   ```

5. This handler will then perform whatever operations it needs to in order to get the job done and return whatever the `IRequestHandler` has designated to the controller. 

### Project Structure

There are three projects in this template. The below example illustrates an example for a `Recipe` entity in a project named `CarbonKitchen.Recipes.Api`

* **CarbonKitchen.Recipes.Api** 
  * Properties
    * **launchsettings.json**: the configurations available for launching the API from Visual Studio
  * Configuration
    * **RecipeProfile**: the mapping profiles for [automapper](https://github.com/AutoMapper/AutoMapper) to easily map data between objects
  * Controllers
    * **RecipeController**: the actual endpoints that the API will expose for consumption. These are kept intentionally dumb by design, with the logic in the [Mediatr](https://github.com/jbogard/MediatR) handlers.
  * Data
    * Entities
      * **Recipe**: the class that we will use to represent the actual database table
    * **RecipeDbContext**: provides a reference for the database and tables that we will be working using throughout the project
  * Mediator
    * **Commands**: each class in this folder is a [Mediatr](https://github.com/jbogard/MediatR) command used by our mutation endpoints in the controller (POST, PUT, PATCH, DELETE, etc). These commands are requests as opposed to notifications.
    * **Queries**: each class in this folder is a [Mediatr](https://github.com/jbogard/MediatR) query used by our non mutating endpoints in the controller (GET). These queries are requests as opposed to notifications.
    * **Handlers**: each class in this folder is a [Mediatr](https://github.com/jbogard/MediatR) handler that will perform the business logic needed in the mediator commands and queries. 
  * Services
    * **IRecipeRepository**: a list of all of the methods we can use in our data access layer (DAL)
    * **RecipeRepository**: the actual implementation of each method in IRecipeRepository
  * Validators
    * **RecipeForCreationDtoValidator**: validation rules using [fluent validation](https://github.com/FluentValidation/FluentValidation) when creating a new entity in the controller
    * **RecipeForUpdateDtoValidator**: validation rules using [fluent validation](https://github.com/FluentValidation/FluentValidation) when updating an existing entity in the controller
    * **RecipeForManipulationDtoValidator**: validation rules that are *shared* between both the creation and update validators
* **CarbonKitchen.Recipes.Api.Models**
  * Pagination
    * **PagedList**: a special type of `List` that captures pagination info with your collection (e.g. what page you are on, how big the page is, etc.)
    * **ResourceUriType**: minor enum used to capture the uri of the next and previous pages that our controller will use to provide additional pagination info when returning a list
    * **RecipePaginationParameters**: base parameters specific to pagination that our RecipeParametersDto can inherit when getting a list of entities
  * **RecipeDto**: the object we will use whenever we want to return a recipe externally
  * **RecipeForCreationDto**: the object we will expect to receive whenever someone is sending us a recipe to create
  * **RecipeForUpdateDto**: the object we will expect to use whenever someone wants to edit a recipe
  * **RecipeForManipulationDto**: the object that captures the shared parameters between both the creation and update DTOs
  * **RecipeParametersDto**: this object captures all of the parameters that we are able to receive when getting a list of entities (recipes)
* **CarbonKitchen.Recipes.Api.Tests**

