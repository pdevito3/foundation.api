# Foundation.Api

## Description

This template uses .Net Core 3.1, to create a foundation for a standard CRUD API using the .Net Template Engine to create new projects on demand using `dotnet new`.

> Note that, while this package can be used as a stand alone template using the processes described below, it is optimized to be run using the [craftsman dotnet tool](https://github.com/pdevito3/craftsman). For the best development experience, please see the linked `craftsman` repo for operational details.

## Prerequisites

- VS2019 .Net Core Feature Set or .Net Core 3.1 SDK https://dotnet.microsoft.com/download/dotnet-core/3.1

## ToDo

✅ Basic 'Clean' Scaffold					✅ Repository Pattern					✅ DTOs with Automapper Profiles

✅ Basic CRUD Operations				✅ Add Fluent Validation				✅ Add Custom Pagination

✅ Add Sieve Filters and Sorting		✅ Healthchecks							✅ Add Unit Tests

✅ Add Integration Tests					 🔲 Swagger									🔲 Logging

🔲DbMigrations								  🔲 Auth										    🔲 Breakout Environments			 

🔲 Rookout										 🔲 CloudFormation Scripts			  🔲 CircleCi

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
cd "C:\Users\Paul\Documents\repos\Foundation.Api"

dotnet new -u "C:\Users\Paul\Documents\repos\Foundation.Api"

dotnet new -i .\

cd..

dotnet new foundation -n CarbonKitchen.Recipes.Api -e "Recipe" -en "recipe" -la "r"
```

## Project Structure
Instead of making you learn some custom directory structure, this template was built using the well known clean architecture format. If you are not familiar with it, the [Clean Architecture](#clean-architecture) section breakdown of the project structure.

### Clean Architecture

#### Core

The core layer is split into two projects, the `Application` and `Domain` layers. 

The `Domain` project is pretty simple and will capture all of the entities and items directly related to that. This layer should never depend on any other project.

The `Application` project is meant to abstract out our specific business rules for our application. It is dependent on the `Domain` layer, but has no dependencies on any other layer or project. This layer defines our interfaces, DTOs, Enums, Exceptions, Mapping Profiles, Validators, and Wrappers that can be used by our external layers.

#### Infrastructure

Our infrastructure layer is used for all of our external communication requirements (e.g. database communication). For more control, this layer is split into a `Persistence`  project as well as a `Shared` project. Additional layers can be added here if needed (e.g. `Auth`).

The `Persistence`  project will capture our application specific database configuration. The `Shared` project will capture any external service requirements that we may need across our infrastructure layer (e.g. DateTimeService).

#### API

Finally, we have our API layer. This is where our `WebApi` project will live to provide us access to our API endpoints. This layer depends on both the `Core` and `Infrastructure` layers,  however, the dependency on `Infrastructure` is only to support dependency  injection. Therefore only `Startup` classes should reference `Infrastructure`.

## What to Do with a Generated Project

If you're using this as a standalone template and not with `Craftsman`, this template with scaffold out the bones of your project, but there are a few things you'll need to do to have it operate with you entity.

1. Update the parameters in the Entity and the DTOs. It is recommended to do this with a global replace (`ctrl+shift+f`). For example, if you had a `Recipe` entity, you could replace `RecipeTextField1` with `Title`
2. Update the validators to suit your needs
3. Adjust Sieve Filters and Sorts on the Entity
4. Update the QueryString search in the Repository.GetList method to use whichever fields you'd like, or remove it all together. Note that modifications here might require you to update or remove some repository tests.
5. If needed, update repository tests for gets to accommodate filter and sorts
6. If needed, update any integration tests 
7. Add any additional tests  that you may want to run