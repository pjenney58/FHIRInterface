# Palisaid Practice Management

## Frontend

## Backend

* Primary API / Control Pod
   * Authentication
   * Aggregation
   * Query
* Data API
   * Patients
   * Practitioners
   * Encounters
   * Locations
   * Diagnoses
   * Diagnostics
   * Prescriptions
   * Treatments
* Vector API
    * Raw data
    * Statistical 
* Administrative API
    * System Management
        * Tenancy
        * Billing Management
        * Security Management
        * Reporting
    * Tenant Instance Management
        * User Management
        * Reporting
* Code API
    * SNOMED
    * ICD*
    * Billing Codes
 * Calendar
    * Schedule Events
    * Participants
    * Meeting Spaces
 * PobulationHealth
    * CDC
    * NIS
    * NHS (UK)
 * Collectors
    * HL7
    * X12
    * REST (Bespoke)
    * Socket (Bespoke)
    * HTTP/2 (Bespoke)       

# Deployment
Deployment is orchestrated by Kubernetes which manages pods for:

* Data Collection and Transformation
    * EMR/EHR Access
    * RESTful Access
    * TCP/IP TLS Access
    * gRPC/SignalR Access 
* Secure inter-pod communication
* Data Management 
* API Publishing
* Frontend/User Interface

# Code Best Practices
## Medium
https://levelup.gitconnected.com/12-bad-practices-to-avoid-in-asp-net-core-api-controllers-3ba52a10954e

# API Editing in OpenAPI 3
* https://editor.swagger.io/#/
* https://editor-next.swagger.io

# Good To Know
## How to fix git when .git ignore is ignored and filtered items are included

  git rm -r --cached .
  
  git add .
  
  git commit -m "fixed untracked files" 

## Drop and Recreate Database
  In pgAdmin4, select Palisaid, right click and select Delete(Force) -- Poof, its gone

  Rebuild it like this

`petejenney@bert PrimaryControllers % cd ../PalisaidMeta` 

`petejenney@bert PalisaidMeta % dotnet ef database update`

`Build started...`
`Build succeeded.`
`Running in Docker: False`

`Applying migration '20240118140201_RefreshedPrimary.1'.`
`Done.`
  
## How to repair a confused solution/ projects

  Open Nuget Package Manager
  
    Update-Package -reinstall

# Setting up the Palisaid Back End on a Local Machine

1. Install Postgres16, use pgpassword for the admin account
2. Install pgAdmin if it's not already there
3. Using pgAdmin, add role "palisaid"/"password"/SuperUser, CanLogin
4. Install .NET 8.01 or latest
5. Pull Palisaid Practice Management master branch from GitHub
8. Install xunit if it's not already there
10. Build and run Controllers/Primary -- This is important as it does additional database configuration/setup
11. Build and run DevTests/TFhirParser/ProcessPatients

This will work on Windows, Windows/WSL, Linux, and macOS and the overhead is low.

You can also run any of the other DevTests to add other types of data. 

