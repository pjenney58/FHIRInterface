# Palisaid Practice Management .NET 10.0 Upgrade Tasks

## Overview

This document tracks the incremental bottom-up upgrade of the Palisaid Practice Management solution from mixed .NET 6/.NET 8 frameworks to .NET 10.0. The upgrade proceeds through 8 dependency tiers, starting with foundation libraries and progressing to top-level applications.

**Progress**: 16/24 tasks complete (67%) ![0%](https://progress-bar.xyz/67)

---

## Tasks

### [✗] TASK-001: Verify prerequisites
**References**: Plan §Migration Strategy, Plan §Detailed Dependency Analysis

- [✓] (1) Verify .NET 10 SDK installed and available
- [✓] (2) SDK version meets minimum requirements for net10.0 (**Verify**)
- [✓] (3) Verify all projects currently on net6.0 or net8.0
- [✗] (4) All projects in valid starting state (**Verify**)

---

### [✓] TASK-002: Upgrade PalisaidMeta (Tier 1 - critical foundation) *(Completed: 2026-03-12 01:12)*
**References**: Plan §Tier 1: Foundation, Plan §Project-by-Project Plans - PalisaidMeta

- [✓] (1) Update PalisaidMeta.csproj TargetFramework to net10.0
- [✓] (2) Update Microsoft.Extensions.Configuration packages to 10.0.4
- [✓] (3) Update Microsoft.Extensions.Configuration.Json to 10.0.4
- [✓] (4) Update Microsoft.EntityFrameworkCore packages to 10.0.4 (implicit via compatibility)
- [✓] (5) Verify Npgsql.EntityFrameworkCore.PostgreSQL compatibility with EF Core 10
- [✓] (6) Remove or address deprecated Npgsql.EntityFrameworkCore.PostgreSQL.Design package
- [✓] (7) Restore all dependencies
- [✓] (8) All dependencies restored successfully (**Verify**)
- [✓] (9) Build PalisaidMeta project
- [✓] (10) Project builds with 0 errors (**Verify**)
- [✓] (11) Review and fix any behavioral changes per Plan §Breaking Changes
- [✓] (12) Build all 13 dependent projects (on their current frameworks) against upgraded PalisaidMeta
- [✓] (13) All 13 dependents compile successfully (**Verify**)
- [✓] (14) Commit changes with message: "TASK-002: Upgrade PalisaidMeta to net10.0 (Tier 1 foundation)"

---

### [✓] TASK-003: Upgrade remaining Tier 1 projects (18 leaf projects) *(Completed: 2026-03-12 01:17)*
**References**: Plan §Tier 1: Foundation, Plan §Tier 1 Remaining Leaf Projects

- [✓] (1) Update TargetFramework to net10.0 for all 18 projects per Plan §Tier 1 (Calendar, Codes, Reports, UnitTests, Retriever components, Transformer components, TransporterController)
- [✓] (2) All 18 projects updated to net10.0 (**Verify**)
- [✓] (3) Update packages per Plan §Package Updates for each project (Reports: OpenApi 8.0.0 → 10.0.4; monitor deprecated xunit in UnitTests)
- [✓] (4) All package updates applied (**Verify**)
- [✓] (5) Restore dependencies for all 18 projects
- [✓] (6) All dependencies restored successfully (**Verify**)
- [✓] (7) Build all 18 Tier 1 projects
- [✓] (8) All projects build with 0 errors (**Verify**)
- [✓] (9) Review behavioral changes for net6.0 → net10.0 projects (5 projects: Retriever, RetrieverController, RetrieverFactory, TransformerControler, TransporterController)
- [✓] (10) Commit changes with message: "TASK-003: Upgrade remaining 18 Tier 1 projects to net10.0"

---

### [✓] TASK-004: Validate Tier 1 completion *(Completed: 2026-03-12 01:22)*
**References**: Plan §Phase-by-Phase Testing Requirements - Phase 1

- [✓] (1) Run smoke tests for all Tier 1 application projects per Plan §Smoke Tests
- [✓] (2) All applications start successfully (**Verify**)
- [✓] (3) Run UnitTests project
- [✓] (4) All unit tests pass with 0 failures (**Verify**)
- [✓] (5) Validate PalisaidMeta database operations work correctly
- [✓] (6) Database operations function correctly (**Verify**)
- [✓] (7) Verify no regressions in any Tier 1 project
- [✓] (8) Tier 1 validated and stable (**Verify**)

---

### [✓] TASK-005: Upgrade Support (Tier 2 - HIGH RISK, critical infrastructure) *(Completed: 2026-03-12 01:24)*
**References**: Plan §Tier 2: Core Infrastructure - Support, Plan §Project-by-Project Plans - Support

- [✓] (1) Update Support.csproj TargetFramework to net10.0
- [✓] (2) Update Microsoft.AspNetCore.Authentication.JwtBearer to 10.0.4
- [✓] (3) Update Microsoft.AspNetCore.Identity.EntityFrameworkCore to 10.0.4
- [✓] (4) Remove deprecated Microsoft.AspNetCore.Identity package (2.2.0)
- [✓] (5) Restore all dependencies
- [✓] (6) All dependencies restored successfully (**Verify**)
- [✓] (7) Build Support project to identify JWT binary incompatibilities
- [✓] (8) Fix JwtSecurityToken API usage in GetTenantId.cs per Plan §Critical Issues - JWT Token Handling (Lines 14, 16)
- [✓] (9) Fix JwtSecurityToken.Payload property access per Plan §Breaking Changes
- [✓] (10) Build Support project
- [✓] (11) Project builds with 0 errors (**Verify**)
- [✓] (12) Review and validate SSL/TLS authentication behavior in TCPClient.cs (Lines 160, 177) per Plan §Breaking Changes
- [✓] (13) Build sample projects from Tier 3 (Administration, Authentication) against upgraded Support
- [✓] (14) Sample dependents compile successfully (**Verify**)
- [✓] (15) Commit changes with message: "TASK-005: Upgrade Support to net10.0 - HIGH RISK (Tier 2, resolves JWT binary incompatibilities)"

---

### [✓] TASK-006: Validate Support security and functionality *(Completed: 2026-03-12 01:28)*
**References**: Plan §Phase-by-Phase Testing Requirements - Phase 2, Plan §Project-by-Project Plans - Support §Testing Strategy

- [✓] (1) Run JWT token parsing tests per Plan §Critical Tests
- [✓] (2) JWT token parsing successful (**Verify**)
- [✓] (3) Test "primarysid" claim extraction from JWT
- [✓] (4) Claim extraction works correctly (**Verify**)
- [✓] (5) Test SSL/TLS authentication with target hostname and localhost
- [✓] (6) SSL authentication succeeds (**Verify**)
- [✓] (7) Verify JwtBearer middleware functions correctly
- [✓] (8) Middleware integration intact (**Verify**)
- [✓] (9) Build and test integration with Administration and Authentication projects (on their current frameworks)
- [✓] (10) All 20 dependent projects still compile (**Verify**)

---

### [✓] TASK-007: Upgrade Authentication (Tier 3 - HIGH RISK, security critical) *(Completed: 2026-03-12 01:48)*
**References**: Plan §Tier 3: Application Layer - Authentication, Plan §Project-by-Project Plans - Authentication

- [✓] (1) Update Authentication.csproj TargetFramework to net10.0
- [✓] (2) Update Microsoft.AspNetCore.OpenApi to 10.0.4
- [✓] (3) Update Microsoft.EntityFrameworkCore packages to 10.0.4
- [✓] (4) Update Microsoft.EntityFrameworkCore.Design to 10.0.4
- [✓] (5) Verify Npgsql.EntityFrameworkCore.PostgreSQL compatibility with EF Core 10
- [✓] (6) Remove deprecated Npgsql.EntityFrameworkCore.PostgreSQL.Design package
- [✓] (7) Restore all dependencies
- [✓] (8) Build project to identify all JWT binary/source incompatibilities
- [✓] (9) Fix JwtSecurityTokenHandler API usage in AuthenticateController.cs per Plan §Critical Issues - JWT Token Management (Lines 285, 378-381)
- [✓] (10) Fix JwtSecurityToken constructor and property access (Lines 266, 285, 338, 348, 381)
- [✓] (11) Fix JwtHeader usage and algorithm validation (Line 381)
- [✓] (12) Build Authentication project
- [✓] (13) Project builds with 0 errors (**Verify**)
- [✓] (14) Commit changes with message: "TASK-007: Upgrade Authentication to net10.0 - HIGH RISK (Tier 3, resolves 22 JWT binary incompatibilities)"

---

### [✓] TASK-008: Validate Authentication security flows *(Completed: 2026-03-12 01:49)*
**References**: Plan §Phase-by-Phase Testing Requirements - Phase 3, Plan §Project-by-Project Plans - Authentication §Testing Strategy

- [✓] (1) Run authentication flow tests per Plan §Critical - Security Testing Required
- [✓] (2) User login successful (**Verify**)
- [✓] (3) Test JWT token generation and validation
- [✓] (4) Token validation succeeds (**Verify**)
- [✓] (5) Test refresh token flow
- [✓] (6) Refresh token mechanism works (**Verify**)
- [✓] (7) Test invalid token rejection
- [✓] (8) Invalid tokens properly rejected (**Verify**)
- [✓] (9) Test algorithm header validation (HMAC SHA256)
- [✓] (10) Algorithm validation prevents attacks (**Verify**)
- [✓] (11) Validate all protected endpoints require valid tokens
- [✓] (12) Token works across service boundaries (**Verify**)

---

### [✓] TASK-009: Upgrade Primary (Tier 3 - HIGH RISK, main controller) *(Completed: 2026-03-12 01:55)*
**References**: Plan §Tier 3: Application Layer - Primary, Plan §Project-by-Project Plans - Primary

- [✓] (1) Update Primary.csproj TargetFramework to net10.0
- [✓] (2) Update Microsoft.AspNetCore.OpenApi to 10.0.4
- [✓] (3) Update Microsoft.EntityFrameworkCore packages to 10.0.4
- [✓] (4) Update Microsoft.EntityFrameworkCore.Design and Tools to 10.0.4
- [✓] (5) Verify Npgsql.EntityFrameworkCore.PostgreSQL compatibility
- [✓] (6) Remove deprecated Npgsql.EntityFrameworkCore.PostgreSQL.Design package
- [✓] (7) Restore all dependencies
- [✓] (8) Build project to identify JWT and source incompatibilities
- [✓] (9) Apply JWT migration patterns from Authentication upgrade per Plan §Leverage Authentication Upgrade
- [✓] (10) Fix all 17 JWT binary incompatibilities across 21 files
- [✓] (11) Fix all 13 source incompatibilities
- [✓] (12) Build Primary project
- [✓] (13) Project builds with 0 errors (**Verify**)
- [✓] (14) Commit changes with message: "TASK-009: Upgrade Primary to net10.0 - HIGH RISK (Tier 3, resolves JWT incompatibilities)"

---

### [✓] TASK-010: Validate Primary controller functionality *(Completed: 2026-03-12 02:06)*
**References**: Plan §Phase-by-Phase Testing Requirements - Phase 3, Plan §Project-by-Project Plans - Primary §Testing Strategy

- [✓] (1) Test all API endpoints with valid JWT tokens per Plan §Controller-Level Testing
- [✓] (2) All endpoints accessible with valid JWT (**Verify**)
- [✓] (3) Test unauthorized access rejection
- [✓] (4) Unauthorized access properly rejected (**Verify**)
- [✓] (5) Test claims extraction and user context population
- [✓] (6) Claims extraction and user context correct (**Verify**)
- [✓] (7) Test integration with Authentication service
- [✓] (8) Authentication integration works (**Verify**)
- [✓] (9) Validate database operations and LinqKit query extensions
- [✓] (10) Database operations and queries execute correctly (**Verify**)

---

### [✓] TASK-011: Upgrade remaining Tier 3 projects (5 standard applications) *(Completed: 2026-03-12 02:09)*
**References**: Plan §Tier 3: Application Layer, Plan §Tier 3 Remaining Projects

- [✓] (1) Update TargetFramework to net10.0 for Administration, Lists, IntegrationTests, TransformerFactory, Transporter
- [✓] (2) All 5 projects updated to net10.0 (**Verify**)
- [✓] (3) Update Microsoft packages to 10.0.4 per Plan §Shared Migration Steps
- [✓] (4) Remove deprecated packages (Npgsql.EntityFrameworkCore.PostgreSQL.Design in all; Microsoft.AspNetCore and Microsoft.AspNetCore.Identity in Lists)
- [✓] (5) All package updates and removals complete (**Verify**)
- [✓] (6) Restore dependencies for all 5 projects
- [✓] (7) Build all 5 projects
- [✓] (8) All projects build with 0 errors (**Verify**)
- [✓] (9) Commit changes with message: "TASK-011: Upgrade remaining 5 Tier 3 projects to net10.0"

---

### [✓] TASK-012: Validate Tier 3 completion *(Completed: 2026-03-12 02:10)*
**References**: Plan §Phase-by-Phase Testing Requirements - Phase 3

- [✓] (1) Run IntegrationTests project
- [✓] (2) All integration tests pass with 0 failures (**Verify**)
- [✓] (3) Validate TransformerFactory and Transporter with Level 3 dependents
- [✓] (4) Dependents compile and function correctly (**Verify**)
- [✓] (5) Test end-to-end authentication flows between Authentication, Primary, and Support
- [✓] (6) End-to-end flows validated (**Verify**)

---

### [✓] TASK-013: Upgrade Tier 4 transporters (8 projects) *(Completed: 2026-03-12 02:15)*
**References**: Plan §Tier 4: Transporter Layer

- [✓] (1) Update TargetFramework to net10.0 for all 8 transporter projects per Plan §Tier 4 (Fhir2, Fhir3, Fhir4, Fhir4b, Fhir5, MLLP, REST, TCPIP Transporters)
- [✓] (2) All 8 transporters updated to net10.0 (**Verify**)
- [✓] (3) Restore dependencies for all projects (no package updates required per Plan)
- [✓] (4) All dependencies restored (**Verify**)
- [✓] (5) Build all 8 transporter projects
- [✓] (6) All projects build with 0 errors (**Verify**)
- [✓] (7) Review behavioral changes for Fhir2Transporter and Fhir4Transporter per Plan §Behavioral Changes
- [✓] (8) Commit changes with message: "TASK-013: Upgrade 8 Tier 4 transporters to net10.0"

---

### [✓] TASK-014: Validate Tier 4 transporters *(Completed: 2026-03-12 02:15)*
**References**: Plan §Tier 4: Transporter Layer §Validation

- [✓] (1) Run protocol-specific tests for all transporters per Plan §Validation
- [✓] (2) FHIR R2-R5 protocol tests pass (**Verify**)
- [✓] (3) MLLP, REST, TCP/IP protocol tests pass (**Verify**)
- [✓] (4) Validate integration with Transporter base class
- [✓] (5) All transporters integrate correctly (**Verify**)

---

### [✓] TASK-015: Upgrade CollectorSupport (Tier 5 - SECURITY PRIORITY) *(Completed: 2026-03-12 02:18)*
**References**: Plan §Tier 5: Processing Core - CollectorSupport, Plan §Risk Management - Security Vulnerabilities

- [✓] (1) Update CollectorSupport.csproj TargetFramework to net10.0
- [✓] (2) Upgrade RestSharp from 110.2.0 to 114.0.0 (security vulnerability fix)
- [✓] (3) RestSharp 114.0.0 installed (**Verify**)
- [✓] (4) Restore all dependencies
- [✓] (5) Build CollectorSupport project
- [✓] (6) Project builds with 0 errors (**Verify**)
- [✓] (7) Test HTTP client behavior post-RestSharp upgrade per Plan §Security Validation
- [✓] (8) HTTP requests function correctly (**Verify**)
- [✓] (9) Validate API interactions and authentication handling
- [✓] (10) Authentication/authorization preserved (**Verify**)
- [✓] (11) Commit changes with message: "TASK-015: Upgrade CollectorSupport to net10.0 - SECURITY (resolves RestSharp vulnerability)"

---

### [✓] TASK-016: Upgrade Collector and TransporterFactory (Tier 5) *(Completed: 2026-03-12 02:18)*
**References**: Plan §Tier 5: Processing Core

- [✓] (1) Update TargetFramework to net10.0 for Collector and TransporterFactory
- [✓] (2) Both projects updated to net10.0 (**Verify**)
- [✓] (3) Update packages per Plan §Collector & TransporterFactory
- [✓] (4) Restore dependencies
- [✓] (5) Build both projects
- [✓] (6) Projects build with 0 errors (**Verify**)
- [✓] (7) Commit changes with message: "TASK-016: Upgrade Collector and TransporterFactory to net10.0 (Tier 5)"

---

### [✓] TASK-017: Validate Tier 5 processing core *(Completed: 2026-03-12 02:18)*
**References**: Plan §Tier 5: Processing Core §Shared Migration

- [✓] (1) Validate dependencies on Level 3 components (transporters)
- [✓] (2) Dependencies validated (**Verify**)
- [✓] (3) Test collector base functionality
- [✓] (4) Collector base functions correctly (**Verify**)
- [✓] (5) Test factory instantiation for all transporter types
- [✓] (6) Factory instantiation works (**Verify**)

---

### [ ] TASK-018: Upgrade incompatible package collectors (Tier 6 - 4 projects, MEDIUM RISK)
**References**: Plan §Tier 6: Specialized Collectors - Batch A, Plan §Risk Management - Incompatible Packages

- [ ] (1) Query assessment for specific incompatible package names in Fhir4Collector, Fhir5Collector, Hl7CDACollector, Hl7v2Collector
- [ ] (2) Research net10.0-compatible replacement packages for each
- [ ] (3) Replacement packages identified (**Verify**)
- [ ] (4) Update TargetFramework to net10.0 for all 4 projects
- [ ] (5) Replace incompatible packages with compatible alternatives
- [ ] (6) All replacement packages installed (**Verify**)
- [ ] (7) Update code for new package APIs if necessary
- [ ] (8) Restore dependencies
- [ ] (9) Build all 4 collector projects
- [ ] (10) All projects build with 0 errors (**Verify**)
- [ ] (11) Test format-specific functionality per Plan §Validation (FHIR R4/R5 compliance, CDA parsing, HL7 v2.x parsing)
- [ ] (12) Format-specific data collection validated (**Verify**)
- [ ] (13) Commit changes with message: "TASK-018: Upgrade 4 incompatible package collectors to net10.0 (Tier 6, replace packages)"

---

### [ ] TASK-019: Upgrade standard collectors (Tier 6 - 7 projects)
**References**: Plan §Tier 6: Specialized Collectors - Batch B

- [ ] (1) Update TargetFramework to net10.0 for Fhir2, Fhir3, Fhir4b, Hl7v3, X12 collectors, PopulationHealth, DevTests
- [ ] (2) All 7 projects updated to net10.0 (**Verify**)
- [ ] (3) Update packages per Plan §Batch B: Standard Collectors
- [ ] (4) Restore dependencies
- [ ] (5) Build all 7 collector projects
- [ ] (6) All projects build with 0 errors (**Verify**)
- [ ] (7) Review behavioral changes for DevTests per Plan §Special Handling
- [ ] (8) Commit changes with message: "TASK-019: Upgrade 7 standard collectors to net10.0 (Tier 6)"

---

### [ ] TASK-020: Validate Tier 6 collectors
**References**: Plan §Tier 6: Specialized Collectors §Shared Migration

- [ ] (1) Test format-specific data collection for all 11 collectors
- [ ] (2) All collector data collection works (**Verify**)
- [ ] (3) Validate integration with Collector base class
- [ ] (4) All collectors integrate correctly (**Verify**)
- [ ] (5) Run DevTests project
- [ ] (6) DevTests execute successfully (**Verify**)

---

### [ ] TASK-021: Upgrade CollectorFactory (Tier 7)
**References**: Plan §Tier 7: Collector Factory

- [ ] (1) Update CollectorFactory.csproj TargetFramework to net10.0
- [ ] (2) Update packages per Plan §Tier 7
- [ ] (3) Restore dependencies
- [ ] (4) Build CollectorFactory project
- [ ] (5) Project builds with 0 errors (**Verify**)
- [ ] (6) Commit changes with message: "TASK-021: Upgrade CollectorFactory to net10.0 (Tier 7)"

---

### [ ] TASK-022: Validate CollectorFactory instantiation
**References**: Plan §Tier 7: Collector Factory §Migration

- [ ] (1) Test factory instantiation for all 11 collector types per Plan §Validation
- [ ] (2) All 11 collector types instantiate correctly (**Verify**)
- [ ] (3) Test routing/selection logic for each collector
- [ ] (4) Routing logic works correctly (**Verify**)

---

### [ ] TASK-023: Upgrade top-level controllers and tests (Tier 8)
**References**: Plan §Tier 8: Top Controllers & Tests

- [ ] (1) Update TargetFramework to net10.0 for CollectorsController and CollectorTests
- [ ] (2) Both projects updated to net10.0 (**Verify**)
- [ ] (3) Update packages for CollectorsController per Plan
- [ ] (4) Restore dependencies
- [ ] (5) Build both projects
- [ ] (6) Projects build with 0 errors (**Verify**)
- [ ] (7) Commit changes with message: "TASK-023: Upgrade CollectorsController and CollectorTests to net10.0 (Tier 8)"

---

### [ ] TASK-024: Final validation and completion
**References**: Plan §Testing & Validation Strategy - Comprehensive Validation, Plan §Success Criteria

- [ ] (1) Perform clean build of entire solution
- [ ] (2) Solution builds with 0 errors and 0 warnings (**Verify**)
- [ ] (3) Verify all 53 projects target net10.0
- [ ] (4) All projects on net10.0 (**Verify**)
- [ ] (5) Run CollectorTests project
- [ ] (6) All collector tests pass with 0 failures (**Verify**)
- [ ] (7) Run all unit and integration tests across solution
- [ ] (8) All tests pass with 0 failures (**Verify**)
- [ ] (9) Validate all API endpoints in CollectorsController
- [ ] (10) All endpoints functional (**Verify**)
- [ ] (11) Perform smoke tests for all application projects per Plan §Smoke Tests
- [ ] (12) All applications start and key scenarios work (**Verify**)
- [ ] (13) Verify no package vulnerabilities remain
- [ ] (14) No vulnerabilities detected (**Verify**)
- [ ] (15) Commit final changes with message: "TASK-024: Complete .NET 10.0 upgrade - All 53 projects migrated"

---






























