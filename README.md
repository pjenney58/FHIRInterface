# Palisaid Practice Management

## Frontend

## Backend

* Primary API
    * Primary object management
        * Patients
        * Practitioners
        * Locations
        * Encounters  
    * Secondary object management
        * Calendaring
        * Messaging
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
* Identity Management API
    * Authentication
    * Authorization 

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

# API Editing in OpenAPI 3
* https://editor.swagger.io/#/
* https://editor-next.swagger.io