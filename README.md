# Foundation.Api

## Prerequisites
- VS2019 .Net Core Feature Set or .Net Core 3.1 SDK https://dotnet.microsoft.com/download/dotnet-core/3.1

----

## Description
This template uses .Net Core 3.1, to create a foundation for a standard CRUD API using the .Net Template Engine to create new projects on demand using `dotnet new`.

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

λ dotnet new foundation -n CarbonKitchen.Recipes.Api -e "Recipe" -en "recipe" --la "r"
```

## Project Structure
TBD

## What to Do with a Generated Project

This template with scaffold out the bones of your project, but there are a few things you'll need to do to have it operate with you entity.

1. Update the parameters in the Entity and the DTOs
2. Update the validators to suit your needs
3. Adjust Sieve Filters and Sorts on the Entity
4. Update the QueryString search in the Repository Get List method to use whichever fields you'd like
5. TBD