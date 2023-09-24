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

# API Editing in OpenAPI 3
* https://editor.swagger.io/#/
* https://editor-next.swagger.io

# Good To Know
  How to fix git when .git ignore is ignored and filtered items are included

  git rm -r --cached .
  
  git add .
  
  git commit -m "fixed untracked files" 
  
