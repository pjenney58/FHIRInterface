# .NET 10.0 Upgrade Plan

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Migration Strategy](#migration-strategy)
3. [Detailed Dependency Analysis](#detailed-dependency-analysis)
4. [Project-by-Project Plans](#project-by-project-plans)
5. [Risk Management](#risk-management)
6. [Testing & Validation Strategy](#testing--validation-strategy)
7. [Complexity & Effort Assessment](#complexity--effort-assessment)
8. [Source Control Strategy](#source-control-strategy)
9. [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description
Upgrade the Palisaid Practice Management solution from mixed .NET 6/.NET 8 target frameworks to **.NET 10.0 (Long Term Support)**. This upgrade affects 53 projects across a 7-level dependency hierarchy.

### Scope
- **Projects Affected**: 53 projects (52 require framework changes; 1 docker-compose project unchanged)
- **Current State**: 
  - Net6.0 projects: 4 projects (Retriever components, TransformerControler, TransporterController)
  - Net8.0 projects: 48 projects (majority of solution)
- **Target State**: All applicable projects upgraded to net10.0

### Selected Strategy: Bottom-Up (Dependency-First)
Given the solution's size (53 projects) and depth (7 levels), a **Bottom-Up incremental migration** approach is required. Projects will be upgraded tier-by-tier starting from leaf nodes (Level 0) and progressing through dependency levels to top-level applications (Level 7).

**Rationale:**
- **Scale**: 53 projects necessitate phased approach to manage risk
- **Depth**: 7-level hierarchy requires strict dependency ordering
- **Stability**: Each tier builds on validated, upgraded foundations
- **Risk mitigation**: High-risk projects (Authentication, Primary, Support) isolated and validated before dependent projects

### Complexity Assessment

**Classification**: **COMPLEX**

**Metrics:**
- **Project Count**: 53 projects (threshold: >15)
- **Dependency Depth**: 7 levels (threshold: >4)
- **Total Issues**: 226 issues (99 mandatory, 115 potential, 12 optional)
- **High-Risk Projects**: 3 (Authentication, Primary, Support with API incompatibilities)
- **Security Issues**: 1 package vulnerability (RestSharp)
- **Incompatible Packages**: 4 packages requiring replacement

### Critical Issues

**API Incompatibilities:**
1. **Authentication.csproj** - Binary + source incompatibilities (23 mandatory issues)
2. **Primary.csproj** - Binary + source incompatibilities (18 mandatory issues)
3. **Support.csproj** - Binary incompatibilities (5 mandatory issues)

**Incompatible NuGet Packages:**
1. **Fhir4Collector** - Incompatible package needs replacement
2. **Fhir5Collector** - Incompatible package needs replacement
3. **Hl7CDACollector** - Incompatible package needs replacement
4. **Hl7v2Collector** - Incompatible package needs replacement

**Security Vulnerabilities:**
- **RestSharp** (current: 110.2.0) - Security vulnerability; upgrade to 114.0.0

**Deprecated Packages:**
- Microsoft.AspNetCore (used in Lists, IntegrationTests, UnitTests)
- Microsoft.AspNetCore.Identity (used in Lists)
- Npgsql.EntityFrameworkCore.PostgreSQL.Design (used in multiple projects)
- xunit 2.9.3 (used in UnitTests)

### Recommended Approach
**Incremental Bottom-Up Migration** across 8 tiers (Levels 0-7), with approximately 14-16 detail iterations:
- **Phase 1**: Foundation tier (Level 0) - 19 independent projects in 2 batches
- **Phase 2**: Core infrastructure (Level 1) - Support.csproj (high-risk, separate iteration)
- **Phase 3**: Mid-tier services (Level 2) - 6 projects in 2 batches (Authentication & Primary separate due to high-risk)
- **Phase 4**: Transporters (Level 3) - 8 projects in 1 batch
- **Phase 5**: Collectors & Transformers (Level 4) - 3 projects in 1 batch
- **Phase 6**: Specialized collectors (Level 5) - 11 projects in 2 batches
- **Phase 7**: Factory (Level 6) - 1 project
- **Phase 8**: Top-level controllers (Level 7) - 2 projects in 1 batch

### Iteration Strategy
Using **complexity-based incremental** approach:
- High-risk projects (Authentication, Primary, Support): Individual iterations with detailed analysis
- Medium-complexity projects: Batches of 3-5 projects
- Low-complexity projects: Batches of 5-10 projects
- Expected total iterations: ~15 (3 mandatory setup + ~12 detail iterations)

---

## Migration Strategy

### Approach Selection: Incremental Bottom-Up Migration

**Selected Approach**: **Incremental Tier-by-Tier Migration**

**Justification:**
- **Solution Scale**: 53 projects exceed threshold for all-at-once approach
- **Dependency Complexity**: 7-level hierarchy requires careful sequencing
- **Risk Distribution**: API incompatibilities in 3 projects require isolation
- **Validation Requirements**: Each tier needs testing before dependents upgrade
- **Team Capacity**: Staged approach enables parallel work within tiers while maintaining stability

### Bottom-Up Strategy Rationale

**Why Bottom-Up:**
1. **Dependency Safety**: Each project upgrades only after its dependencies are stable on net10.0
2. **No Multi-Targeting**: Avoid complexity of maintaining multiple framework versions simultaneously
3. **Clear Validation**: Each tier provides stable foundation before next tier begins
4. **Risk Isolation**: Issues contained within tier boundaries
5. **Learning Curve**: Lessons from early tiers (e.g., PalisaidMeta, Support) inform later work

**Strategy Trade-offs:**
- ✅ **Lower Risk**: Incremental validation at each tier
- ✅ **Easier Debugging**: Issues isolated to current tier
- ✅ **Progressive Benefits**: Foundation improvements available early
- ⚠️ **Longer Timeline**: 8 phases vs single big-bang
- ⚠️ **Late Application Benefits**: User-facing apps upgraded in final phases

### Dependency-Based Ordering Principles

**Tier Sequencing Rules:**
1. **Tier 0 first**: Projects with no internal dependencies (leaf nodes)
2. **Tier N+1 next**: Projects depending only on Tiers 0 through N
3. **Strict ordering**: Cannot skip tiers or upgrade out of sequence
4. **No assumptions**: Even "simple" projects respect dependency order

**Within-Tier Parallelization:**
- Projects in same tier CAN be upgraded in parallel (no inter-dependencies)
- Batching decisions based on complexity, not dependencies
- All projects in tier must complete before next tier begins

### Execution Sequencing

#### Phase-by-Phase Approach

**Phase 1: Foundation Tier (Level 0)**
- **Scope**: 19 projects with zero internal dependencies
- **Approach**: Two batches based on criticality
  - Batch 1A: PalisaidMeta (critical bottleneck - 13 dependents)
  - Batch 1B: Remaining 18 leaf projects (can parallelize)
- **Validation**: Each project builds independently; PalisaidMeta tested by dependents in Phase 2

**Phase 2: Core Infrastructure (Level 1)**
- **Scope**: Support.csproj (high-risk)
- **Approach**: Single project, dedicated iteration
- **Validation**: Build with PalisaidMeta; test by 20 dependent projects before Phase 3

**Phase 3: Application Layer (Level 2)**
- **Scope**: 7 projects (2 high-risk, 5 standard)
- **Approach**: Three batches
  - Batch 3A: Authentication (high-risk, 40 issues)
  - Batch 3B: Primary (high-risk, 37 issues)
  - Batch 3C: Administration, Lists, IntegrationTests, TransformerFactory, Transporter (5 projects)
- **Validation**: Integration testing between Authentication/Primary and Support; end-to-end scenarios

**Phase 4: Transporter Layer (Level 3)**
- **Scope**: 8 transporter implementations
- **Approach**: Single batch (homogeneous group, low complexity)
- **Validation**: Protocol-specific tests; integration with Transporter base

**Phase 5: Processing Core (Level 4)**
- **Scope**: 3 projects (Collector, CollectorSupport, TransporterFactory)
- **Approach**: Two batches
  - Batch 5A: CollectorSupport (security vulnerability)
  - Batch 5B: Collector, TransporterFactory
- **Validation**: Security scan post-RestSharp update; collector base functionality tests

**Phase 6: Specialized Collectors (Level 5)**
- **Scope**: 11 collector implementations (4 with incompatible packages)
- **Approach**: Two batches
  - Batch 6A: Fhir4Collector, Fhir5Collector, Hl7CDACollector, Hl7v2Collector (incompatible packages)
  - Batch 6B: Fhir2Collector, Fhir3Collector, Fhir4bCollector, Hl7v3Collector, X12Collector, PopulationHealth, DevTests
- **Validation**: Format-specific data collection tests; integration with Collector base

**Phase 7: Factory Orchestration (Level 6)**
- **Scope**: CollectorFactory (depends on all 11 Level 5 collectors)
- **Approach**: Single project
- **Validation**: Factory instantiation tests for all collector types

**Phase 8: Top-Level Controllers (Level 7)**
- **Scope**: CollectorsController, CollectorTests
- **Approach**: Single batch
- **Validation**: End-to-end integration tests; full solution validation

### Parallel vs Sequential Execution

**Within-Tier Parallelization Opportunities:**
- **Tier 1**: 18 leaf projects (excluding PalisaidMeta) can upgrade in parallel
- **Tier 4**: All 8 transporters can upgrade in parallel
- **Tier 6, Batch B**: 7 standard collectors can upgrade in parallel

**Sequential Requirements:**
- **Cross-Tier**: Strict sequential (Tier N must complete before Tier N+1)
- **High-Risk Projects**: Authentication, Primary, Support upgraded sequentially (individual validation)
- **Incompatible Packages**: 4 collectors in Batch 6A (package research may reveal dependencies)

**Recommendation:**
- Use parallelization within tiers to reduce calendar time
- Maintain sequential execution for high-risk projects
- Validate entire tier before proceeding to next tier

---

## Detailed Dependency Analysis

### Dependency Graph Structure

The solution has a clear 8-tier hierarchy (Levels 0-7) with 53 projects:

```
Level 7: [CollectorsController] [CollectorTests]
           ↓
Level 6: [CollectorFactory]
           ↓
Level 5: [Fhir2-5 Collectors] [Hl7 Collectors] [X12Collector] [PopulationHealth] [DevTests]
           ↓
Level 4: [Collector] [CollectorSupport] [TransporterFactory]
           ↓
Level 3: [Transporters: Fhir2-5, MLLP, REST, TCPIP]
           ↓
Level 2: [Administration] [Authentication] [Primary] [Lists] [IntegrationTests] [TransformerFactory] [Transporter]
           ↓
Level 1: [Support]
           ↓
Level 0: [PalisaidMeta] [19 Independent Leaf Projects]
```

### Tier Breakdown by Bottom-Up Strategy

#### **Tier 1: Foundation (Level 0)** - 19 projects
**No internal dependencies**

**Core Foundation:**
- **PalisaidMeta** (26 issues, 1 mandatory) - Critical foundation library used by 13 projects

**Independent Leaf Projects:**
- Calendar, Codes, Eligibility, HL7FhirTransformer, HL7v2Transformer
- ICD, LOINC, NDC, Reports, Retriever, RetrieverController, RetrieverFactory, SNOMED
- TransformerControler, TransporterController, UnitTests, X12Transformer
- docker-compose (no changes needed)

**Why This Tier:**
- Zero internal project dependencies
- Enables all higher tiers once upgraded
- PalisaidMeta is foundational - used extensively across solution

**Complexity:**
- **Low**: 18 projects (simple framework update only)
- **Medium**: 1 project (PalisaidMeta - 26 issues including deprecated packages)

---

#### **Tier 2: Core Infrastructure (Level 1)** - 1 project
**Depends only on Tier 1**

**Projects:**
- **Support** (10 issues, 5 mandatory) - **HIGH RISK**

**Why This Tier:**
- Depends only on PalisaidMeta (Level 0)
- Critical infrastructure used by 20 downstream projects
- Contains binary incompatibilities requiring careful handling

**Complexity:**
- **High**: Binary incompatible APIs, widely used dependency

**Breaking Change Exposure:**
- **High**: Binary incompatibilities affect all 20 consuming projects

---

#### **Tier 3: Application & Service Layer (Level 2)** - 7 projects
**Depends on Tiers 1-2**

**High-Risk Projects:**
- **Authentication** (40 issues, 23 mandatory) - Binary + source incompatibilities
- **Primary** (37 issues, 18 mandatory) - Binary + source incompatibilities

**Medium-Risk Projects:**
- Administration (5 issues)
- Lists (5 issues, includes deprecated packages)
- IntegrationTests (11 issues, includes deprecated packages)

**Low-Risk Projects:**
- TransformerFactory (4 issues)
- Transporter (1 issue)

**Why This Tier:**
- Depend on Support (Level 1) and/or PalisaidMeta (Level 0)
- Application-level services and controllers
- Authentication & Primary are critical entry points with significant API changes

**Complexity:**
- **High**: 2 projects (Authentication, Primary)
- **Medium**: 3 projects (Administration, Lists, IntegrationTests)
- **Low**: 2 projects (TransformerFactory, Transporter)

**Breaking Change Exposure:**
- **High**: Authentication & Primary have extensive API incompatibilities
- **Medium**: Lists has deprecated packages requiring replacement

---

#### **Tier 4: Transporter Layer (Level 3)** - 8 projects
**Depends on Tiers 1-3**

**Projects:**
- Fhir2Transporter (4 issues)
- Fhir3Transporter (1 issue)
- Fhir4Transporter (2 issues, behavioral changes)
- Fhir4bTransporter (1 issue)
- Fhir5Transporter (1 issue)
- MLLPTransporter (1 issue)
- RESTTransporter (1 issue)
- TCPIPTransporter (1 issue)

**Why This Tier:**
- All depend on Transporter (Level 2) and Support (Level 1)
- Protocol-specific implementations
- Homogeneous group - similar complexity

**Complexity:**
- **Low**: 7 projects (simple framework updates)
- **Medium**: 1 project (Fhir2Transporter - 4 issues)

**Breaking Change Exposure:**
- **Low**: Mostly framework updates, one behavioral change

---

#### **Tier 5: Processing Core (Level 4)** - 3 projects
**Depends on Tiers 1-4**

**Projects:**
- Collector (4 issues) - Core collector base
- CollectorSupport (12 issues, includes security vulnerability) - **MEDIUM RISK**
- TransporterFactory (1 issue)

**Why This Tier:**
- Collector depends on PalisaidMeta, TransformerFactory, Transporter, Transformer
- CollectorSupport depends on PalisaidMeta, Transformer, Support
- TransporterFactory depends on multiple Level 3 transporters

**Complexity:**
- **Medium**: 1 project (CollectorSupport - security vulnerability)
- **Low**: 2 projects (Collector, TransporterFactory)

**Breaking Change Exposure:**
- **Medium**: CollectorSupport has RestSharp security vulnerability

---

#### **Tier 6: Specialized Collectors (Level 5)** - 11 projects
**Depends on Tiers 1-5**

**Projects with Incompatible Packages:**
- **Fhir4Collector** (3 issues, 2 mandatory) - Incompatible package
- **Fhir5Collector** (3 issues, 2 mandatory) - Incompatible package
- **Hl7CDACollector** (3 issues, 2 mandatory) - Incompatible package
- **Hl7v2Collector** (3 issues, 2 mandatory) - Incompatible package

**Standard Collectors:**
- Fhir2Collector (2 issues)
- Fhir3Collector (2 issues)
- Fhir4bCollector (2 issues)
- Hl7v3Collector (2 issues)
- X12Collector (2 issues)
- PopulationHealth (2 issues)
- DevTests (4 issues, behavioral changes)

**Why This Tier:**
- All depend on Collector (Level 4)
- Protocol/format-specific implementations
- 4 projects have incompatible packages needing replacement

**Complexity:**
- **Medium**: 4 projects (incompatible packages)
- **Low**: 7 projects (standard framework/package updates)

**Breaking Change Exposure:**
- **High**: 4 projects need package replacements
- **Low**: 7 projects standard updates

---

#### **Tier 7: Collector Factory (Level 6)** - 1 project
**Depends on Tiers 1-6**

**Projects:**
- **CollectorFactory** (2 issues, 1 mandatory)

**Why This Tier:**
- Depends on 11 Level 5 collectors
- Orchestrates all collector implementations
- Must be upgraded after all concrete collectors

**Complexity:**
- **Low**: Simple framework update

---

#### **Tier 8: Top Controllers & Tests (Level 7)** - 2 projects
**Depends on Tiers 1-7**

**Projects:**
- **CollectorsController** (2 issues, 1 mandatory)
- **CollectorTests** (1 issue, 1 mandatory)

**Why This Tier:**
- Top of dependency hierarchy
- Depend on CollectorFactory (Level 6)
- Final validation point for entire upgrade

**Complexity:**
- **Low**: Both projects have minimal changes

---

### Critical Path

**Longest dependency chain** (8 levels):
```
PalisaidMeta (L0) → Support (L1) → Transporter (L2) → MLLPTransporter (L3) 
  → Collector (L4) → Hl7v2Collector (L5) → CollectorFactory (L6) → CollectorsController (L7)
```

**Bottleneck Projects** (block multiple downstream projects):
1. **PalisaidMeta** (Level 0) - Blocks 13 direct dependents
2. **Support** (Level 1) - Blocks 20 direct dependents
3. **Collector** (Level 4) - Blocks 12 direct dependents

### Circular Dependencies
**None detected** - Clean hierarchical structure suitable for bottom-up migration.

### Migration Phase Groupings

Based on Bottom-Up strategy and dependency tiers:

| Phase | Tiers | Projects | Rationale |
|-------|-------|----------|-----------|
| **Phase 1** | Tier 1 (L0) | 19 projects | Foundation - no dependencies, can be parallelized |
| **Phase 2** | Tier 2 (L1) | 1 project (Support) | Critical infrastructure - high risk, needs isolation |
| **Phase 3** | Tier 3 (L2) | 7 projects | Applications & services - contains 2 high-risk projects |
| **Phase 4** | Tier 4 (L3) | 8 projects | Transporters - homogeneous group |
| **Phase 5** | Tier 5 (L4) | 3 projects | Processing core - includes security vulnerability |
| **Phase 6** | Tier 6 (L5) | 11 projects | Specialized collectors - 4 with incompatible packages |
| **Phase 7** | Tier 7 (L6) | 1 project | Factory orchestrator |
| **Phase 8** | Tier 8 (L7) | 2 projects | Final controllers & tests |

---

## Project-by-Project Plans

### Tier 1: Foundation (Level 0) - 19 Projects

#### PalisaidMeta (src\PalisaidMeta\PalisaidMeta.csproj)
**Current State**: net8.0, ClassLibrary, 26 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Medium

##### Current State Details
- **Framework**: net8.0
- **Project Type**: ClassLibrary (SDK-style)
- **Files**: 57 total files
- **Dependencies**: None (Level 0 - leaf node)
- **Dependents**: 13 projects (IntegrationTests, DevTests, Lists, Administration, Authentication, CollectorSupport, CollectorsController, Support, Fhir2Collector, Primary, Collector, Transformer, TransformerFactory)
- **Package Count**: 6 explicit packages
- **Issues**: 26 total (1 mandatory, 24 potential, 1 optional)

##### Migration Steps

**1. Prerequisites**
- Tier 1 (Level 0) has no prerequisites
- Validate .NET 10 SDK installed
- Ensure all dependents are on net8.0 or earlier (confirmed)

**2. Framework Update**
Update `src\PalisaidMeta\PalisaidMeta.csproj`:
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package Name | Current Version | Target Version | Update Reason |
|--------------|----------------|----------------|---------------|
| Microsoft.Extensions.Configuration | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.Extensions.Configuration.Json | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.EntityFrameworkCore | 8.0.0 | 10.0.4 | Recommended for net10.0 (implicit via compatibility) |
| Microsoft.EntityFrameworkCore.Design | 8.0.0 | 10.0.4 | Recommended for net10.0 (implicit via compatibility) |
| Npgsql.EntityFrameworkCore.PostgreSQL | 8.0.0 | *(check compatibility)* | Validate compatibility with EF Core 10 |

**Deprecated Package Handling:**
- **Npgsql.EntityFrameworkCore.PostgreSQL.Design** (1.1.0): Remove if unused; marked deprecated but assessment shows compatible

**4. Expected Breaking Changes**

**Behavioral Changes** (20 occurrences - Potential severity):
- API behavioral changes in .NET 10 that may affect runtime behavior
- Most likely in:
  - Configuration loading patterns
  - EF Core query translation
  - PostgreSQL provider behaviors

⚠️ **Requires Investigation**: Query specific API behavioral changes during execution phase.

**5. Code Modifications**

**Anticipated Changes:**
- Review configuration loading patterns for behavioral changes
- Validate EF Core DbContext initialization
- Check for obsolete API usage (if any flagged during compilation)
- Update any hardcoded framework version checks

**Configuration Updates:**
- No appsettings.json expected (ClassLibrary)
- Verify any EF Core migrations compatibility

**6. Testing Strategy**

**Unit Tests:**
- Run all tests in projects that reference PalisaidMeta
- Validate database operations (EF Core with PostgreSQL)
- Test configuration loading scenarios

**Integration Tests:**
- Build all 13 dependent projects (on their current frameworks) against upgraded PalisaidMeta
- Verify no binary incompatibilities introduced
- Smoke test critical paths

**Performance Tests:**
- Baseline EF Core query performance
- Compare before/after for any significant regressions

**7. Validation Checklist**
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] All 13 dependent projects still compile against upgraded PalisaidMeta
- [ ] EF Core migrations still apply successfully
- [ ] PostgreSQL database operations function correctly
- [ ] Configuration loading works as expected
- [ ] No new package vulnerabilities introduced
- [ ] Behavioral changes documented and validated

**Critical Success Factor**: PalisaidMeta is foundational - any issues here will cascade to 13 dependent projects. Thorough validation required before Phase 2.

---

#### Calendar (src\Calendar\Calendar.csproj)
**Current State**: net8.0, AspNetCore, 2 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Details**: [To be filled]

---

#### Codes (src\Codes\Codes.csproj)
**Current State**: net8.0, AspNetCore, 4 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Details**: [To be filled]

---

#### Reports (src\Reports\Reports.csproj)
**Current State**: net8.0, AspNetCore, 2 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Details**: [To be filled]

---

#### UnitTests (test\UnitTests\UnitTests.csproj)
**Current State**: net8.0, DotNetCoreApp, 2 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Details**: [To be filled]

---

#### Calendar (src\Calendar\Calendar.csproj)
**Current State**: net8.0, AspNetCore, 2 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low

##### Current State Details
- **Framework**: net8.0
- **Project Type**: AspNetCore (SDK-style)
- **Files**: 5 files
- **Dependencies**: None (Level 0 - leaf node)
- **Dependents**: None (standalone application)
- **Package Count**: 2 packages
- **Issues**: 2 total (1 mandatory, 1 potential)

##### Migration Steps

**1. Prerequisites**
- .NET 10 SDK installed
- No dependency prerequisites

**2. Framework Update**
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package Name | Current Version | Target Version | Update Reason |
|--------------|----------------|----------------|---------------|
| Microsoft.AspNetCore.OpenApi | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Swashbuckle.AspNetCore | 6.5.0 | *(compatible)* | No update required |

**4. Expected Breaking Changes**
- None anticipated (simple AspNetCore application)
- Behavioral change flagged but likely minimal impact

**5. Code Modifications**
- No code changes expected
- Validate OpenApi configuration if behavioral changes occur

**6. Testing Strategy**
- Build project successfully
- Run application smoke test
- Verify OpenAPI/Swagger endpoint generation

**7. Validation Checklist**
- [ ] Builds without errors
- [ ] Builds without warnings
- [ ] Application starts successfully
- [ ] OpenAPI documentation generates correctly
- [ ] No package vulnerabilities

---

#### Codes (src\Codes\Codes.csproj)
**Current State**: net8.0, AspNetCore, 4 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low

##### Current State Details
- **Framework**: net8.0
- **Project Type**: AspNetCore (SDK-style)
- **Files**: 5 files
- **Dependencies**: None (Level 0)
- **Dependents**: None (standalone application)
- **Package Count**: 4 packages
- **Issues**: 4 total (1 mandatory, 2 potential, 1 optional)

##### Migration Steps

**1. Prerequisites**
- .NET 10 SDK installed

**2. Framework Update**
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package Name | Current Version | Target Version | Update Reason |
|--------------|----------------|----------------|---------------|
| Microsoft.EntityFrameworkCore.Tools | 8.0.1 | 10.0.4 | Recommended for net10.0 |
| Microsoft.AspNetCore.OpenApi | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Npgsql.EntityFrameworkCore.PostgreSQL | *(implicit)* | *(check compatibility)* | Validate with EF Core 10 |

**Deprecated Package:**
- **Npgsql.EntityFrameworkCore.PostgreSQL.Design** (1.1.0): Monitor during build; remove if causing issues

**4. Expected Breaking Changes**
- EF Core Tools behavioral changes (minor)
- OpenApi configuration changes (unlikely)

**5. Code Modifications**
- Validate EF Core migrations apply successfully
- Check for any OpenAPI configuration updates

**6. Testing Strategy**
- Build and run application
- Verify database connectivity
- Test EF Core migrations
- Validate API documentation generation

**7. Validation Checklist**
- [ ] Builds without errors/warnings
- [ ] EF migrations apply successfully
- [ ] Database operations work
- [ ] OpenAPI documentation correct
- [ ] No deprecated package warnings

---

#### Tier 1 Remaining Leaf Projects (16 projects)

**Projects in this batch:**
- Reports (src\Reports\Reports.csproj)
- UnitTests (test\UnitTests\UnitTests.csproj)
- Retriever (src\Retrievers\Retriever\Retriever.csproj)
- RetrieverController (src\Retrievers\RetrieverController\RetrieverController.csproj)
- RetrieverFactory (src\Retrievers\RetrieverFactory\RetrieverFactory.csproj)
- Eligibility (src\Retrievers\Eligibility\Eligibility.csproj)
- ICD (src\Retrievers\ICD\ICD.csproj)
- LOINC (src\Retrievers\LOINC\LOINC.csproj)
- NDC (src\Retrievers\NDC\NDC.csproj)
- SNOMED (src\Retrievers\SNOMED\SNOMED.csproj)
- HL7FhirTransformer (src\Transformers\HL7FhirTransformer\HL7FhirTransformer.csproj)
- HL7v2Transformer (src\Transformers\HL7v2Transformer\HL7v2Transformer.csproj)
- X12Transformer (src\Transformers\X12Transformer\X12Transformer.csproj)
- TransformerControler (src\Transformers\TransformerControler\TransformerControler.csproj)
- TransporterController (src\Transporters\TransporterController\TransporterController.csproj)
- docker-compose (docker-compose.dcproj)

##### Shared Characteristics
- **Framework**: Mixed (net6.0: 3 projects; net8.0: 13 projects)
- **Issues**: 1-2 issues each (all simple framework updates)
- **Dependencies**: None (Level 0)
- **Dependents**: Most have none (leaf/standalone)
- **Complexity**: Low (minimal changes)

##### Common Migration Steps

**1. Framework Update**
All projects: Update `<TargetFramework>` to `net10.0`

**2. Package Updates**
Most projects have no package updates required (compatible packages or no packages).

**Special Cases:**
- **Reports**: OpenApi 8.0.0 → 10.0.4 (recommended)
- **UnitTests**: Deprecated xunit package (monitor during build)

**3. Expected Breaking Changes**
Minimal - primarily behavioral changes from .NET 6 → 10 or .NET 8 → 10.

**NET 6 Projects (larger jump):**
- Retriever, RetrieverController, RetrieverFactory, TransformerControler, TransporterController
- More potential behavioral changes due to 4-version jump
- Review: API changes in .NET 7, 8, 9, 10

**NET 8 Projects:**
- Standard 8 → 10 upgrade
- Fewer breaking changes expected

**4. Code Modifications**
- Review for obsolete API usage during compilation
- Update any framework version checks
- Validate project-specific functionality (retrievers, transformers, controllers)

**5. Testing Strategy**
- Build each project independently
- Run smoke tests for applications
- Validate retriever/transformer/transporter functionality
- For docker-compose: Verify build configuration remains valid

**6. Batch Validation Checklist**
- [ ] All 16 projects build successfully
- [ ] No errors or warnings
- [ ] .NET 6 projects (5 total) validated for multi-version jump
- [ ] Application projects start successfully
- [ ] Test project (UnitTests) executes tests
- [ ] docker-compose builds (if applicable)

**Notes:**
- These projects can be upgraded in parallel (no inter-dependencies)
- Simple updates with minimal risk
- Focus validation on .NET 6 → 10 projects (larger jump)

---

### Tier 2: Core Infrastructure (Level 1) - 1 Project

#### Support (src\Support\Support.csproj) - **HIGH RISK**
**Current State**: net8.0, ClassLibrary, 10 issues (5 mandatory)
**Target State**: net10.0
**Risk Level**: High

##### Current State Details
- **Framework**: net8.0
- **Project Type**: ClassLibrary (SDK-style)
- **Files**: 21 files
- **Dependencies**: PalisaidMeta (Level 0)
- **Dependents**: 20 projects across all tiers (critical infrastructure)
- **Package Count**: 5 explicit packages
- **Issues**: 10 total (5 mandatory, 4 potential, 1 optional)
- **Feature Usage**: IdentityModel & Claims-based Security (4 issues)

##### Critical Issues

**Binary Incompatibilities (4 mandatory):**
1. **JwtSecurityToken** class - Binary incompatible
   - File: `src\Support\Model\GetTenantId.cs`, Line 14
   - Usage: `var jwtToken = new JwtSecurityToken(jwt);`
   - Impact: Constructor signature changed

2. **JwtSecurityToken.Payload** property - Binary incompatible
   - File: `src\Support\Model\GetTenantId.cs`, Line 16
   - Usage: `foreach (var i in jwtToken.Payload)`
   - Impact: Property type or accessibility changed

3. **JwtPayload** type - Binary incompatible
   - File: `src\Support\Model\GetTenantId.cs`, Line 16
   - Impact: Type definition changed

**Behavioral Changes (2 potential):**
- **SslStream.AuthenticateAsClient** - Behavioral changes in .NET 10
  - Files: `src\Support\Model\TCPClient.cs`, Lines 160, 177
  - Impact: Authentication behavior may differ

##### Migration Steps

**1. Prerequisites**
- PalisaidMeta upgraded to net10.0 (Phase 1 complete)
- .NET 10 SDK installed
- Review JWT/Identity breaking changes documentation

**2. Framework Update**
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package Name | Current Version | Target Version | Update Reason |
|--------------|----------------|----------------|---------------|
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.AspNetCore.Identity | 2.2.0 | **Remove** | **Deprecated** - functionality included in framework |
| EasyNetQ | 7.8.0 | *(compatible)* | No update needed |
| NLog | 5.2.7 | *(compatible)* | No update needed |

**Deprecated Package Action:**
- **Microsoft.AspNetCore.Identity** (2.2.0): Remove `<PackageReference>` entirely; verify Identity functionality through AspNetCore.Identity.EntityFrameworkCore

**4. Expected Breaking Changes**

**Critical - JWT Token Handling:**
- **JwtSecurityToken API changes**: Constructor and Payload property have binary incompatibilities
- **File**: `src\Support\Model\GetTenantId.cs`
- **Current Code Pattern**:
  ```csharp
  var jwtToken = new JwtSecurityToken(jwt);
  foreach (var i in jwtToken.Payload)
  {
      if (i.Key.ToLower().Contains("primarysid"))
      {
          return Guid.Parse(i.Value.ToString() ?? throw new InvalidDataException("JwtToken Incomplete"));
      }
  }
  ```
- **Potential New Pattern** (requires validation):
  ```csharp
  // May need to use JwtSecurityTokenHandler or updated Payload access pattern
  // Exact replacement depends on .NET 10 API changes
  ```

**SSL/TLS Authentication:**
- **SslStream.AuthenticateAsClient** has behavioral changes
- **Files**: `src\Support\Model\TCPClient.cs` (Lines 160, 177)
- **Current Usage**: `stream.AuthenticateAsClient(stream.TargetHostName);` and `stream.AuthenticateAsClient("localhost");`
- **Impact**: Authentication process or certificate validation may behave differently
- **Mitigation**: Test SSL connections thoroughly; review TLS version requirements

**5. Code Modifications**

**Mandatory Changes:**
1. **GetTenantId.cs** - Update JWT token handling pattern
   - Replace JwtSecurityToken constructor usage
   - Update Payload property access (may now return different type)
   - Validate parsing logic still extracts "primarysid" claim correctly

2. **TCPClient.cs** - Validate SSL authentication behavior
   - Test both authentication calls (with hostname and "localhost")
   - Verify certificate validation still works
   - Check for any new required parameters or options

**Potential Changes:**
- Review all 20 files for behavioral change impacts
- Update any Identity API usage patterns
- Validate EasyNetQ messaging patterns (behavioral changes possible)

**Configuration Updates:**
- No configuration files expected (ClassLibrary)
- May need to update JWT configuration in consuming applications

**6. Testing Strategy**

**Critical - JWT Functionality:**
- [ ] JWT token parsing successful
- [ ] "primarysid" claim extraction works
- [ ] Guid parsing from claim value succeeds
- [ ] Invalid token handling behaves correctly

**SSL/TLS Functionality:**
- [ ] SSL authentication with target hostname succeeds
- [ ] SSL authentication with "localhost" succeeds
- [ ] Certificate validation works as expected
- [ ] TLS version negotiation correct

**Package Integration:**
- [ ] JwtBearer authentication middleware works
- [ ] Identity EntityFrameworkCore integration intact
- [ ] EasyNetQ messaging functions correctly
- [ ] NLog logging unaffected

**Dependent Project Validation:**
- [ ] Build sample from each dependent tier:
  - Tier 3: Administration (simple)
  - Tier 3: Authentication (high-risk, should fail gracefully if Support issues)
  - Tier 4: Fhir2Transporter (simple transporter)
  - Tier 5: Collector (core component)

**7. Validation Checklist**
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] JWT token parsing unit tests pass
- [ ] SSL connection integration tests pass
- [ ] All 20 dependent projects still compile (on their current frameworks)
- [ ] No regressions in PalisaidMeta (Tier 1)
- [ ] Deprecated package removed without errors
- [ ] Binary incompatibilities resolved
- [ ] Security not compromised

**Post-Upgrade Actions:**
- Document exact JWT API changes made
- Update any shared documentation about JWT token handling
- Notify teams about SSL behavioral changes
- Validate in integration environment before Phase 3

**⚠️ CRITICAL**: Support is used by 20 projects. Any issues here will cascade widely. Allow extra validation time before proceeding to Phase 3.

---

### Tier 3: Application & Service Layer (Level 2) - 7 Projects

#### Authentication (src\Authentication\Authentication.csproj) - **HIGH RISK**
**Current State**: net8.0, AspNetCore, 40 issues (23 mandatory)
**Target State**: net10.0
**Risk Level**: High

##### Current State Details
- **Framework**: net8.0
- **Project Type**: AspNetCore (SDK-style)
- **Files**: 8 files
- **Dependencies**: Support (Level 1), PalisaidMeta (Level 0)
- **Dependents**: None (top-level application)
- **Package Count**: 6 explicit packages
- **Issues**: 40 total (23 mandatory, 16 potential, 1 optional)
- **Feature Usage**: IdentityModel & Claims-based Security (22 issues)

##### Critical Issues

**Binary Incompatibilities (22 mandatory):**
All concentrated in `src\Authentication\Controllers\AuthenticateController.cs`, affecting:

1. **JwtSecurityTokenHandler** class (5 occurrences)
   - Constructor: `new JwtSecurityTokenHandler()`
   - Methods: `ValidateToken()`, `WriteToken()`
   - Lines: 285, 378, 379

2. **JwtSecurityToken** class (6 occurrences)
   - Constructor: `new JwtSecurityToken(...)`
   - Properties: `ValidTo`, `ValidFrom`, `Header`
   - Lines: 266, 285, 338, 348, 381

3. **JwtHeader** class and properties (3 occurrences)
   - Type: `JwtHeader`
   - Property: `Header.Alg`
   - Line: 381

**Source Incompatibilities (13 potential):**
- Similar JWT-related APIs may require code adjustments during compilation

**Key Code Patterns Affected:**
- **Token Creation** (Lines 338-356)
- **Token Writing** (Line 285)
- **Token Validation** (Lines 378-381)
- **Token Header Validation** (Line 381)
- **Refresh Token Handling** (Lines 266-270)

##### Migration Steps

**1. Prerequisites**
- Support upgraded to net10.0 (Phase 2 complete)
- PalisaidMeta upgraded to net10.0 (Phase 1 complete)
- Review Microsoft's JWT breaking changes documentation for .NET 10
- Backup authentication database

**2. Framework Update**
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package Name | Current Version | Target Version | Update Reason |
|--------------|----------------|----------------|---------------|
| Microsoft.AspNetCore.OpenApi | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.EntityFrameworkCore | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.EntityFrameworkCore.Design | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Npgsql.EntityFrameworkCore.PostgreSQL | 8.0.0 | *(check compatibility)* | Validate with EF Core 10 |
| Swashbuckle.AspNetCore | 6.5.0 | *(compatible)* | No update needed |

**Deprecated Package Removal:**
- **Npgsql.EntityFrameworkCore.PostgreSQL.Design** (1.1.0): Remove completely (not needed in newer provider versions)

**4. Expected Breaking Changes**

**CRITICAL - JWT Token Management:**

**A. Token Creation Pattern (Lines 338-356):**
```csharp
// CURRENT CODE (net8.0):
private JwtSecurityToken CreateToken(List<Claim> authClaims)
{
    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
    var validTo = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:TokenValidityInMinutes"]));

    var token = new JwtSecurityToken(
        issuer: _configuration["JWT:ValidIssuer"],
        audience: _configuration["JWT:ValidAudience"],
        expires: validTo,
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
    );
    return token;
}
```
**REPLACEMENT STRATEGY** (net10.0):
- May need to use `JsonWebTokenHandler` or updated `JwtSecurityTokenHandler` API
- Constructor parameters might have changed
- Validate signing credentials handling

**B. Token Writing Pattern (Line 285):**
```csharp
// CURRENT CODE:
accesstoken = new JwtSecurityTokenHandler().WriteToken(token)
```
**REPLACEMENT STRATEGY**:
- Check if `WriteToken` method signature changed
- May need to use async variant or different token handler

**C. Token Validation Pattern (Lines 378-381):**
```csharp
// CURRENT CODE:
var tokenHandler = new JwtSecurityTokenHandler();
var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

if (securityToken is not JwtSecurityToken jwtSecurityToken || 
    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
{
    throw new SecurityTokenException("Invalid token");
}
```
**REPLACEMENT STRATEGY**:
- `ValidateToken` may be deprecated in favor of `ValidateTokenAsync`
- `Header.Alg` property access may change
- Algorithm validation pattern may need update

**D. Token Properties (Lines 266-285):**
```csharp
// CURRENT CODE:
user.RefreshTokenExpiryTime = token.ValidFrom;
validTo = token.ValidTo
```
**REPLACEMENT STRATEGY**:
- `ValidFrom` and `ValidTo` properties may be renamed or moved
- Check for new date/time handling patterns

**5. Code Modifications**

**Mandatory File Changes:**
- **AuthenticateController.cs** - Primary file with all 22 binary incompatibilities
  - Update `CreateToken` method (Lines 338-356)
  - Update token writing logic (Line 285)
  - Update `ValidateToken` method (Lines 378-381)
  - Update header validation logic (Line 381)
  - Update refresh token handling (Lines 266-270)

**Step-by-Step Approach:**
1. Attempt compilation - review specific errors
2. Consult Microsoft JWT migration guide for .NET 10
3. Update JWT creation to new API
4. Update JWT validation to new API
5. Update JWT header access pattern
6. Test each authentication flow individually

**Configuration Updates:**
- **appsettings.json**: Validate JWT configuration keys still correct
- May need to add new JWT settings for .NET 10

**6. Testing Strategy**

**CRITICAL - Security Testing Required:**

**Unit Tests:**
- [ ] Token creation succeeds
- [ ] Token contains correct claims
- [ ] Token has correct issuer/audience
- [ ] Token expiration set correctly
- [ ] Token signing works

**Authentication Flow Tests:**
- [ ] User login successful
- [ ] JWT token generated correctly
- [ ] Token validation succeeds
- [ ] Refresh token flow works
- [ ] Token expiry handled correctly
- [ ] Invalid token rejected
- [ ] Tampered token rejected
- [ ] Algorithm validation (HMAC SHA256) works

**Security Validation:**
- [ ] Token signature verification works
- [ ] Algorithm header validation prevents attacks
- [ ] Refresh token expiry enforced
- [ ] Claims extraction correct (especially for dependent services)
- [ ] No security downgrade from breaking changes

**Integration Tests:**
- [ ] Protected endpoints require valid token
- [ ] Token works across service boundaries
- [ ] OpenAPI authentication testing
- [ ] EF Core database operations succeed

**7. Validation Checklist**
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] All 22 binary incompatibilities resolved
- [ ] All authentication flows tested
- [ ] Security review passed
- [ ] JWT tokens validated by dependent services
- [ ] Refresh token mechanism works
- [ ] Token expiry behaves correctly
- [ ] Database operations successful
- [ ] OpenAPI documentation correct
- [ ] No security vulnerabilities introduced

**⚠️ CRITICAL SECURITY NOTICE**: 
- Authentication is security-critical infrastructure
- All 22 JWT incompatibilities must be resolved correctly
- Thorough security testing mandatory before production
- Consider security code review by senior developer
- Document all JWT API changes for team awareness
- Test with actual production-like token scenarios

**Post-Upgrade Actions:**
- Document JWT API migration patterns
- Update authentication documentation
- Notify dependent services of any token format changes
- Plan rollback strategy if issues discovered post-deployment

---

#### Primary (src\PrimaryControllers\Primary.csproj) - **HIGH RISK**
**Current State**: net8.0, AspNetCore, 37 issues (18 mandatory)
**Target State**: net10.0
**Risk Level**: High

##### Current State Details
- **Framework**: net8.0
- **Project Type**: AspNetCore (SDK-style)
- **Files**: 21 files
- **Dependencies**: Support (Level 1), PalisaidMeta (Level 0)
- **Dependents**: None (top-level application - primary controller)
- **Package Count**: 8 explicit packages
- **Issues**: 37 total (18 mandatory, 18 potential, 1 optional)
- **Feature Usage**: IdentityModel & Claims-based Security (17 issues)

##### Critical Issues

**Binary Incompatibilities (17 mandatory):**
- Similar JWT token handling issues as Authentication project
- Affect IdentityModel and Claims-based Security APIs

**Source Incompatibilities (13 potential):**
- API method signatures may have changed
- Require code modifications during compilation

**Behavioral Changes (1 potential):**
- Runtime behavior change in one API

##### Migration Steps

**1. Prerequisites**
- Support upgraded to net10.0 (Phase 2 complete)
- PalisaidMeta upgraded to net10.0 (Phase 1 complete)
- **Authentication upgraded to net10.0** (Phase 3 prior batch)
- Leverage JWT patterns from Authentication upgrade

**2. Framework Update**
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package Name | Current Version | Target Version | Update Reason |
|--------------|----------------|----------------|---------------|
| Microsoft.AspNetCore.OpenApi | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.EntityFrameworkCore | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.EntityFrameworkCore.Design | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Microsoft.EntityFrameworkCore.Tools | 8.0.0 | 10.0.4 | Recommended for net10.0 |
| Npgsql.EntityFrameworkCore.PostgreSQL | 8.0.0 | *(check compatibility)* | Validate with EF Core 10 |
| LinqKit | 1.2.5 | *(compatible)* | No update needed |
| Swashbuckle.AspNetCore | 6.5.0 | *(compatible)* | No update needed |

**Deprecated Package Removal:**
- **Npgsql.EntityFrameworkCore.PostgreSQL.Design** (1.1.0): Remove completely

**4. Expected Breaking Changes**

**JWT Token Handling (17 incompatibilities):**
- **Same patterns as Authentication project**
- Likely affects multiple controllers in 21 files
- Use Authentication JWT migration patterns as template

**Areas Requiring Investigation:**
1. JWT token validation in controllers
2. Claims extraction from tokens
3. Identity/authorization attribute usage
4. Token-based authentication middleware configuration

**Source Incompatibilities:**
- Method signature changes requiring code updates
- Possibly in LinqKit integration or EF Core queries
- Parameter types or return types may have changed

**5. Code Modifications**

**Strategy:**
- Apply JWT fixes learned from Authentication project
- Review all 21 files for JWT/identity API usage
- Update claims-based authorization patterns
- Validate controller-level authentication attributes

**Expected Change Areas:**
- Controllers with `[Authorize]` attributes
- JWT token validation logic
- Claims extraction code
- Identity/user context access
- EF Core query patterns (source incompatibilities)

**6. Testing Strategy**

**Controller-Level Testing:**
- [ ] All API endpoints accessible with valid JWT
- [ ] Unauthorized access properly rejected
- [ ] Claims extraction correct
- [ ] User context populated correctly

**Integration Testing:**
- [ ] End-to-end API flows work
- [ ] Authentication integration with Authentication service
- [ ] Database operations succeed
- [ ] LinqKit query extensions work

**EF Core Validation:**
- [ ] Database migrations apply
- [ ] Complex queries execute correctly
- [ ] No performance regressions

**7. Validation Checklist**
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] All 17 binary incompatibilities resolved
- [ ] All 13 source incompatibilities resolved
- [ ] JWT token validation works
- [ ] All controller endpoints tested
- [ ] OpenAPI documentation correct
- [ ] Database connectivity validated
- [ ] No security issues introduced

**Leverage Authentication Upgrade:**
- Reuse JWT migration patterns from Authentication
- Apply same security validation approach
- Follow documented JWT API changes
- Use same testing checklist

**Post-Upgrade Actions:**
- Validate with Authentication service integration
- Test all primary controller endpoints
- Document any Primary-specific JWT patterns
- Update API documentation if needed

---

#### Tier 3 Remaining Projects - Standard Applications (5 projects)

##### Administration (src\Administration\Administration.csproj)
**Current State**: net8.0, AspNetCore, 5 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Dependencies**: Support, PalisaidMeta
**Migration**: Standard framework + package updates; no special issues

##### Lists (src\Vectors\Lists.csproj)
**Current State**: net8.0, AspNetCore, 5 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Medium
**Dependencies**: Support, PalisaidMeta
**Special Handling**: Deprecated packages (Microsoft.AspNetCore, Microsoft.AspNetCore.Identity) - remove references

##### IntegrationTests (test\IntegrationTests\IntegrationTests.csproj)
**Current State**: net8.0, DotNetCoreApp, 11 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Dependencies**: Support, PalisaidMeta
**Special Handling**: Deprecated packages; behavioral changes; validate all integration tests pass

##### TransformerFactory (src\Transformers\TransformerFactory\TransformerFactory.csproj)
**Current State**: net8.0, ClassLibrary, 4 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Dependencies**: PalisaidMeta, Support
**Dependents**: Collector, Transformer
**Special Handling**: Behavioral changes (1 potential)

##### Transporter (src\Transporters\Transporter\Transporter.csproj)
**Current State**: net8.0, ClassLibrary, 1 issue (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Dependencies**: Support
**Dependents**: 10 transporter implementations (Level 3)

**Shared Migration Steps:**
1. Update framework to net10.0
2. Update Microsoft packages to 10.0.4 (AspNetCore.OpenApi, EF Core packages)
3. Remove deprecated Npgsql.EntityFrameworkCore.PostgreSQL.Design
4. For Lists: Remove Microsoft.AspNetCore and Microsoft.AspNetCore.Identity references
5. Build and test each project
6. Validate dependents still compile

**Batch Validation:**
- [ ] All 5 projects build successfully
- [ ] Deprecated packages removed without errors
- [ ] IntegrationTests execute and pass
- [ ] TransformerFactory and Transporter validated by dependents

---

#### Lists (src\Vectors\Lists.csproj)
**Current State**: net8.0, AspNetCore, 5 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Medium
**Details**: [To be filled]

---

#### IntegrationTests (test\IntegrationTests\IntegrationTests.csproj)
**Current State**: net8.0, DotNetCoreApp, 11 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Details**: [To be filled]

---

#### TransformerFactory (src\Transformers\TransformerFactory\TransformerFactory.csproj)
**Current State**: net8.0, ClassLibrary, 4 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Details**: [To be filled]

---

#### Transporter (src\Transporters\Transporter\Transporter.csproj)
**Current State**: net8.0, ClassLibrary, 1 issue (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Details**: [To be filled]

---

### Tier 4: Transporter Layer (Level 3) - 8 Projects

**All projects in this tier:**
- Fhir2Transporter, Fhir3Transporter, Fhir4Transporter, Fhir4bTransporter, Fhir5Transporter
- MLLPTransporter, RESTTransporter, TCPIPTransporter

**Shared Characteristics:**
- **Current State**: net8.0, ClassLibrary
- **Target State**: net10.0
- **Dependencies**: Transporter (Level 2), Support (Level 1)
- **Issues**: 1-4 issues per project (all  mandatory for framework update)
- **Risk Level**: Low (Fhir2Transporter: Medium due to 4 issues)

**Common Migration Steps:**
1. Update framework to net10.0
2. No package updates required (all packages compatible)
3. Validate behavioral changes (Fhir2Transporter, Fhir4Transporter)
4. Test protocol-specific functionality
5. Validate integration with Transporter base class

**Behavioral Changes:**
- Fhir2Transporter: 1 behavioral change (requires validation)
- Fhir4Transporter: 1 behavioral change (requires validation)

**Validation:**
- [ ] All 8 transporters build successfully
- [ ] Protocol-specific tests pass (FHIR R2-R5, MLLP, REST, TCP/IP)
- [ ] Integration with Transporter base validated
- [ ] Behavioral changes tested and documented

---

### Tier 5: Processing Core (Level 4) - 3 Projects

#### CollectorSupport (src\\Collectors\\CollectorSupport\\CollectorSupport.csproj) - **SECURITY RISK**
**Current State**: net8.0, AspNetCore, 12 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Medium
**Dependencies**: PalisaidMeta, Transformer, Support
**Special Handling**: **RestSharp security vulnerability** (110.2.0 → 114.0.0)

**Migration Priority:**
1. Update framework to net10.0
2. **Upgrade RestSharp to 114.0.0** (security vulnerability fix)
3. Test HTTP client behavior post-upgrade
4. Validate API interactions
5. Verify authentication handling

**Security Validation:**
- [ ] RestSharp 114.0.0 installed
- [ ] Security vulnerability resolved
- [ ] HTTP requests function correctly
- [ ] Authentication/authorization preserved

---

#### Collector & TransporterFactory (2 projects)
**Projects:**
- Collector (src\\Collectors\\Collector\\Collector.csproj) - 4 issues
- TransporterFactory (src\\Transporters\\TransporterFactory\\TransporterFactory.csproj) - 1 issue

**Shared Migration:**
- Update framework to net10.0
- Standard package updates
- Validate dependencies on Level 3 components
- Test with Level 5 consumers (collectors)

---

### Tier 6: Specialized Collectors (Level 5) - 11 Projects

#### Batch A: Incompatible Package Collectors (4 projects) - **MEDIUM RISK**

**Projects:**
- Fhir4Collector (src\\Collectors\\Fhir4Collector\\Fhir4Collector.csproj)
- Fhir5Collector (src\\Collectors\\Fhir5Collector\\Fhir5Collector.csproj)
- Hl7CDACollector (src\\Collectors\\Hl7c-CDACollector\\Hl7CDACollector.csproj)
- Hl7v2Collector (src\\Collectors\\Hl7v2Collector\\Hl7v2Collector.csproj)

**Current State**: net8.0, AspNetCore, 3 issues each (2 mandatory per project)
**Target State**: net10.0
**Risk Level**: Medium

**Critical Issue**: Each project has 1 incompatible NuGet package requiring replacement.

**Migration Steps:**
1. Query assessment for specific incompatible package names
2. Research net10.0-compatible replacement packages
3. Update framework to net10.0
4. Replace incompatible packages with compatible alternatives
5. Update code for new package APIs (if necessary)
6. Validate format-specific functionality:
   - Fhir4Collector: FHIR R4 spec compliance
   - Fhir5Collector: FHIR R5 spec compliance
   - Hl7CDACollector: CDA document parsing
   - Hl7v2Collector: HL7 v2.x message parsing

**Validation:**
- [ ] Replacement packages identified
- [ ] Packages installed successfully
- [ ] Format-specific data collection works
- [ ] Integration with Collector base validated

---

#### Batch B: Standard Collectors (7 projects)

**Projects:**
- Fhir2Collector, Fhir3Collector, Fhir4bCollector, Hl7v3Collector, X12Collector
- PopulationHealth, DevTests

**Shared Migration:**
- Update framework to net10.0
- Standard package updates (mostly compatible)
- Test format-specific data collection
- Validate integration with Collector base

**Special Handling:**
- DevTests: Behavioral changes (4 issues); validate test execution

---

### Tier 7: Collector Factory (Level 6) - 1 Project

#### CollectorFactory (src\\Collectors\\CollectorFactory\\CollectorFactory.csproj)
**Current State**: net8.0, ClassLibrary, 2 issues (1 mandatory)
**Target State**: net10.0
**Risk Level**: Low
**Dependencies**: All 11 Level 5 collectors
**Dependents**: CollectorsController, CollectorTests

**Migration:**
1. Update framework to net10.0
2. Standard package updates
3. **Critical**: Validate factory can instantiate all 11 collector types
4. Test routing logic for each collector

**Validation:**
- [ ] Factory builds successfully
- [ ] All 11 collector types instantiate correctly
- [ ] Routing/selection logic works

---

### Tier 8: Top Controllers & Tests (Level 7) - 2 Projects

#### CollectorsController (src\\Collectors\\CollectorsController\\CollectorsController.csproj)
**Current State**: net8.0, AspNetCore, 2 issues (1 mandatory)
**Target State**: net10.0
**Dependencies**: PalisaidMeta, Transformer, Support, CollectorFactory
**Risk Level**: Low

**Migration:**
- Update framework to net10.0
- Standard package updates
- Validate all API endpoints
- Test integration with CollectorFactory

---

#### CollectorTests (test\\CollectorTests\\CollectorTests\\CollectorTests.csproj)
**Current State**: net8.0, DotNetCoreApp, 1 issue (1 mandatory)
**Target State**: net10.0
**Dependencies**: CollectorFactory
**Risk Level**: Low

**Migration:**
- Update framework to net10.0
- Validate all tests execute and pass
- Final validation point for entire collector infrastructure

**Final Validation:**
- [ ] CollectorsController all endpoints work
- [ ] CollectorTests 100% pass rate
- [ ] End-to-end collector scenarios validated
- [ ] Full solution build successful

---

---

## Risk Management

### High-Risk Changes

| Project | Risk Level | Description | Mitigation Strategy |
|---------|-----------|-------------|---------------------|
| **Authentication** | **High** | 40 issues (23 mandatory); binary + source incompatibilities in identity/claims APIs | Dedicated iteration; extensive testing of auth flows; validate JWT token handling; isolate from other Phase 3 projects |
| **Primary** | **High** | 37 issues (18 mandatory); binary + source incompatibilities in core controller logic | Dedicated iteration; comprehensive integration tests; validate all API endpoints; deploy after Authentication validated |
| **Support** | **High** | 10 issues (5 mandatory); binary incompatibilities; used by 20 projects | Upgrade early (Phase 2); extensive testing before Phase 3; validate all 20 consumers still function |
| **Fhir4Collector** | **Medium** | Incompatible NuGet package; requires package replacement | Research replacement package; validate FHIR R4 spec compliance; test data collection |
| **Fhir5Collector** | **Medium** | Incompatible NuGet package; requires package replacement | Research replacement package; validate FHIR R5 spec compliance; test data collection |
| **Hl7CDACollector** | **Medium** | Incompatible NuGet package; requires package replacement | Research replacement package; validate CDA parsing; test document ingestion |
| **Hl7v2Collector** | **Medium** | Incompatible NuGet package; requires package replacement | Research replacement package; validate HL7 v2.x parsing; test message handling |
| **CollectorSupport** | **Medium** | Security vulnerability in RestSharp (110.2.0 → 114.0.0) | Priority security update; validate HTTP client behavior; test API interactions |

### Security Vulnerabilities

| Package | Current Version | Target Version | CVE/Advisory | Projects Affected | Remediation |
|---------|----------------|----------------|--------------|-------------------|-------------|
| **RestSharp** | 110.2.0 | 114.0.0 | Security vulnerability identified | CollectorSupport | Upgrade to 114.0.0; validate HTTP client behavior; test all REST API calls; verify authentication handling |

**Remediation Timeline**: Address in Phase 5, Batch 5A (CollectorSupport upgrade)

### Incompatible Packages

| Package | Current Version | Projects Affected | Issue | Resolution Strategy |
|---------|----------------|-------------------|-------|---------------------|
| Unknown (Fhir4) | TBD | Fhir4Collector | Incompatible with net10.0 | Query assessment for specific package; research compatible alternatives; validate FHIR R4 compliance |
| Unknown (Fhir5) | TBD | Fhir5Collector | Incompatible with net10.0 | Query assessment for specific package; research compatible alternatives; validate FHIR R5 compliance |
| Unknown (CDA) | TBD | Hl7CDACollector | Incompatible with net10.0 | Query assessment for specific package; research compatible alternatives; validate CDA parsing |
| Unknown (HL7v2) | TBD | Hl7v2Collector | Incompatible with net10.0 | Query assessment for specific package; research compatible alternatives; validate HL7 v2.x parsing |

⚠️ **Requires Investigation**: Specific incompatible package names will be queried from assessment during detail iterations.

### Deprecated Packages

| Package | Projects Affected | Replacement Strategy |
|---------|-------------------|----------------------|
| **Microsoft.AspNetCore** | Lists, IntegrationTests, UnitTests | Remove explicit package reference (included in framework for AspNetCore projects); validate builds succeed |
| **Microsoft.AspNetCore.Identity** | Lists | Remove explicit package reference (included in framework); validate identity functionality |
| **Npgsql.EntityFrameworkCore.PostgreSQL.Design** | Multiple projects | Remove package (design-time only, deprecated); use EF Core tools instead |
| **xunit** (2.9.3) | UnitTests | Update to latest stable version or keep if compatible; verify test execution |

### Contingency Plans

#### Scenario: Incompatible Package Has No Replacement
**Likelihood**: Low
**Impact**: High (blocks collector functionality)
**Mitigation**: 
- Research alternative FHIR/HL7 libraries
- Consider forking/updating package if open-source
- Evaluate custom implementation for critical functionality
- Escalate to architecture team for decision

#### Scenario: API Breaking Changes Block Compilation
**Likelihood**: Medium (3 projects have source incompatibilities)
**Impact**: Medium (delays tier completion)
**Mitigation**:
- Consult Microsoft breaking changes documentation
- Use IDE quick fixes for common patterns
- Implement compatibility shims for complex cases
- Consider temporary code duplication if needed

#### Scenario: Authentication Changes Break Security
**Likelihood**: Medium (23 mandatory issues in Authentication)
**Impact**: Critical (security compromise)
**Mitigation**:
- Extensive security testing post-upgrade
- Validate JWT token generation/validation
- Test all authentication flows (login, SSO, API keys)
- Security code review before Phase 3 completion
- Rollback plan ready

#### Scenario: Performance Regression Post-Upgrade
**Likelihood**: Low
**Impact**: Medium
**Mitigation**:
- Baseline performance metrics before upgrade
- Performance testing after each high-risk tier
- Profile hot paths in Production-like environment
- Document any behavioral changes affecting performance

#### Scenario: Tests Fail After Framework Upgrade
**Likelihood**: Medium
**Impact**: Medium (delays validation)
**Mitigation**:
- Update test frameworks first (xunit, coverlet)
- Fix test infrastructure before application tests
- Accept behavioral changes if documented by Microsoft
- Update test assertions to match new behavior

### Tier-Specific Risks

#### Tier 1 Risks
- **PalisaidMeta failure blocks 13 projects**: Validate thoroughly before Phase 2
- **Multiple .NET 6 projects**: May have more breaking changes (6→10 vs 8→10)

#### Tier 2 Risks
- **Support binary incompatibilities**: Affects 20 downstream projects; extensive validation required

#### Tier 3 Risks
- **Authentication security**: API changes may affect security posture; mandatory security review
- **Primary as main controller**: Downtime risk if issues occur; plan maintenance window

#### Tier 6 Risks
- **4 incompatible packages**: May delay entire tier if replacements difficult to find; start research early

---

## Testing & Validation Strategy

### Phase-by-Phase Testing Requirements

#### Phase 1: Foundation Tier (Level 0) - 19 Projects
**Smoke Tests:**
- [ ] Each project builds independently without errors/warnings
- [ ] PalisaidMeta: Unit tests pass; database operations work
- [ ] Application projects start successfully

**Tier Validation:**
- [ ] All 19 projects validated
- [ ] No regressions introduced
- [ ] Ready for Phase 2 (Support depends on PalisaidMeta)

---

#### Phase 2: Core Infrastructure (Level 1) - Support
**Critical Tests:**
- [ ] JWT token parsing works correctly
- [ ] SSL/TLS authentication succeeds
- [ ] Binary incompatibilities fully resolved
- [ ] JwtBearer middleware functions
- [ ] Identity EntityFrameworkCore integration intact

**Dependent Validation:**
- [ ] Sample projects from Phase 3 compile against upgraded Support (Administration, Authentication)
- [ ] No API breaking changes affecting consumers

**Security Verification:**
- [ ] JWT token handling secure
- [ ] SSL certificate validation correct
- [ ] No security downgrade

---

#### Phase 3: Application Layer (Level 2) - 7 Projects
**High-Risk Project Tests (Authentication, Primary):**
- [ ] All JWT token flows work (creation, validation, refresh)
- [ ] Authentication endpoints function correctly
- [ ] Authorization attributes work
- [ ] Token expiry handled correctly
- [ ] Security testing passed
- [ ] Integration with Support validated

**Standard Project Tests:**
- [ ] Administration, Lists, IntegrationTests build and run
- [ ] Deprecated packages removed successfully
- [ ] TransformerFactory and Transporter validated by Level 3 dependents

**Tier Validation:**
- [ ] All authentication flows end-to-end tested
- [ ] Integration tests pass
- [ ] Ready for Phase 4 (transporters)

---

#### Phase 4: Transporter Layer (Level 3) - 8 Projects
**Protocol Tests:**
- [ ] Each transporter implementation validated
- [ ] FHIR transporters: R2, R3, R4, R4B, R5 protocols work
- [ ] MLLP, REST, TCP/IP transporters function
- [ ] Behavioral changes validated

**Integration:**
- [ ] All transporters work with Transporter base class
- [ ] Protocol-specific data transmission validated

---

#### Phase 5: Processing Core (Level 4) - 3 Projects
**Security Priority:**
- [ ] CollectorSupport: RestSharp 114.0.0 installed and functioning
- [ ] Security vulnerability resolved
- [ ] HTTP client behavior validated
- [ ] API interactions work

**Core Functionality:**
- [ ] Collector base class validated
- [ ] TransporterFactory instantiation works

---

#### Phase 6: Specialized Collectors (Level 5) - 11 Projects
**Incompatible Package Projects (Priority):**
- [ ] Fhir4Collector: Replacement package identified, installed, validated
- [ ] Fhir5Collector: Replacement package identified, installed, validated
- [ ] Hl7CDACollector: Replacement package identified, installed, validated
- [ ] Hl7v2Collector: Replacement package identified, installed, validated
- [ ] Format-specific data collection validated

**Standard Collectors:**
- [ ] Fhir2, Fhir3, Fhir4b, Hl7v3, X12 collectors validated
- [ ] PopulationHealth and DevTests functional
- [ ] All collectors integrate with Collector base

---

#### Phase 7: Factory Orchestration (Level 6) - CollectorFactory
**Factory Tests:**
- [ ] Instantiation of all 11 collector types works
- [ ] Factory routing logic correct
- [ ] All collectors accessible via factory

---

#### Phase 8: Top Controllers (Level 7) - 2 Projects
**End-to-End Validation:**
- [ ] CollectorsController: All endpoints functional
- [ ] CollectorTests: All tests pass
- [ ] Full solution integration validated

---

### Comprehensive Validation (Before Completion)

**Full Solution Build:**
- [ ] Clean build of entire solution succeeds
- [ ] Zero errors, zero warnings
- [ ] All 53 projects on net10.0

**All Tests Pass:**
- [ ] Unit tests: 100% pass rate
- [ ] Integration tests: 100% pass rate
- [ ] Collector tests: 100% pass rate

**Performance Validation:**
- [ ] No significant performance regressions
- [ ] EF Core query performance acceptable
- [ ] API response times within tolerance

**Security Scan:**
- [ ] No package vulnerabilities (RestSharp upgraded)
- [ ] Security-critical projects validated (Authentication, Support)
- [ ] JWT token handling secure

**Smoke Tests:**
- [ ] Each application project starts successfully
- [ ] Key user scenarios work end-to-end
- [ ] Database connectivity validated
- [ ] Message queue integration works (EasyNetQ)

**Documentation:**
- [ ] Breaking changes documented
- [ ] JWT API migration patterns documented
- [ ] Incompatible package replacements documented
- [ ] Team notified of changes

---

## Complexity & Effort Assessment

### Per-Project Complexity Ratings

| Tier | Project | Complexity | Mandatory Issues | Total Issues | Dependencies | Risk Factors |
|------|---------|-----------|------------------|--------------|--------------|--------------|
| **Tier 1 (L0)** | PalisaidMeta | **Medium** | 1 | 26 | 0 | Deprecated packages; used by 13 projects |
| Tier 1 | Calendar | Low | 1 | 2 | 0 | Simple framework update |
| Tier 1 | Codes | Low | 1 | 4 | 0 | Standard package updates |
| Tier 1 | Reports | Low | 1 | 2 | 0 | Simple framework update |
| Tier 1 | UnitTests | Low | 1 | 2 | 0 | Deprecated package |
| Tier 1 | Retriever components | Low | 1 each | 1 each | 0 | 8 simple projects |
| Tier 1 | Transformer components | Low | 1 each | 1 each | 0 | 4 simple projects |
| Tier 1 | TransporterController | Low | 1 | 1 | 0 | Simple framework update |
| **Tier 2 (L1)** | **Support** | **High** | **5** | **10** | 1 | **Binary incompatibilities; 20 dependents** |
| **Tier 3 (L2)** | **Authentication** | **High** | **23** | **40** | 2 | **Binary + source incompatibilities** |
| **Tier 3 (L2)** | **Primary** | **High** | **18** | **37** | 2 | **Binary + source incompatibilities** |
| Tier 3 | Administration | Low | 1 | 5 | 2 | Standard updates |
| Tier 3 | Lists | Medium | 1 | 5 | 2 | Deprecated packages |
| Tier 3 | IntegrationTests | Low | 1 | 11 | 2 | Deprecated packages; behavioral changes |
| Tier 3 | TransformerFactory | Low | 1 | 4 | 2 | Behavioral changes |
| Tier 3 | Transporter | Low | 1 | 1 | 1 | Simple framework update |
| **Tier 4 (L3)** | Fhir2Transporter | Low | 1 | 4 | 2 | Behavioral changes |
| Tier 4 | Fhir3Transporter | Low | 1 | 1 | 2 | Simple framework update |
| Tier 4 | Fhir4Transporter | Low | 1 | 2 | 2 | Behavioral changes |
| Tier 4 | Fhir4bTransporter | Low | 1 | 1 | 2 | Simple framework update |
| Tier 4 | Fhir5Transporter | Low | 1 | 1 | 2 | Simple framework update |
| Tier 4 | MLLPTransporter | Low | 1 | 1 | 2 | Simple framework update |
| Tier 4 | RESTTransporter | Low | 1 | 1 | 2 | Simple framework update |
| Tier 4 | TCPIPTransporter | Low | 1 | 1 | 2 | Simple framework update |
| **Tier 5 (L4)** | **CollectorSupport** | **Medium** | **1** | **12** | 3 | **Security vulnerability (RestSharp)** |
| Tier 5 | Collector | Low | 1 | 4 | 4 | Used by 12 projects |
| Tier 5 | TransporterFactory | Low | 1 | 1 | 4 | Simple framework update |
| **Tier 6 (L5)** | **Fhir4Collector** | **Medium** | **2** | **3** | 3 | **Incompatible package** |
| **Tier 6** | **Fhir5Collector** | **Medium** | **2** | **3** | 1 | **Incompatible package** |
| **Tier 6** | **Hl7CDACollector** | **Medium** | **2** | **3** | 1 | **Incompatible package** |
| **Tier 6** | **Hl7v2Collector** | **Medium** | **2** | **3** | 2 | **Incompatible package** |
| Tier 6 | Fhir2Collector | Low | 1 | 2 | 4 | Standard updates |
| Tier 6 | Fhir3Collector | Low | 1 | 2 | 1 | Standard updates |
| Tier 6 | Fhir4bCollector | Low | 1 | 2 | 1 | Standard updates |
| Tier 6 | Hl7v3Collector | Low | 1 | 2 | 1 | Standard updates |
| Tier 6 | X12Collector | Low | 1 | 2 | 1 | Standard updates |
| Tier 6 | PopulationHealth | Low | 1 | 2 | 1 | Standard updates |
| Tier 6 | DevTests | Low | 1 | 4 | 6 | Behavioral changes |
| **Tier 7 (L6)** | CollectorFactory | Low | 1 | 2 | 11 | Depends on all collectors |
| **Tier 8 (L7)** | CollectorsController | Low | 1 | 2 | 4 | Top-level controller |
| **Tier 8** | CollectorTests | Low | 1 | 1 | 1 | Final validation |

### Phase Complexity Assessment

| Phase | Tiers | Project Count | Complexity | Key Challenges | Estimated Relative Effort |
|-------|-------|--------------|-----------|----------------|---------------------------|
| **Phase 1** | Tier 1 (L0) | 19 | **Low-Medium** | PalisaidMeta with deprecated packages; 4 net6.0 projects | **Medium** (foundational, but mostly simple) |
| **Phase 2** | Tier 2 (L1) | 1 | **High** | Support binary incompatibilities; 20 downstream dependents | **High** (critical path bottleneck) |
| **Phase 3** | Tier 3 (L2) | 7 | **High** | Authentication & Primary with extensive API incompatibilities | **Very High** (2 high-risk + 5 standard) |
| **Phase 4** | Tier 4 (L3) | 8 | **Low** | Homogeneous transporter group; mostly simple updates | **Low** (parallelizable, simple) |
| **Phase 5** | Tier 5 (L4) | 3 | **Medium** | CollectorSupport security vulnerability | **Medium** (security priority) |
| **Phase 6** | Tier 6 (L5) | 11 | **Medium-High** | 4 incompatible packages requiring replacement | **High** (package research + testing) |
| **Phase 7** | Tier 7 (L6) | 1 | **Low** | Depends on all collectors; orchestration testing | **Low** (simple update, complex testing) |
| **Phase 8** | Tier 8 (L7) | 2 | **Low** | Final integration validation | **Low** (validation-focused) |

### Overall Complexity: **High**

**Factors Contributing to Complexity:**
- Large project count (53 projects)
- Deep dependency hierarchy (7 levels)
- High-risk API incompatibilities in 3 critical projects
- 4 packages requiring research and replacement
- Security vulnerability requiring immediate attention
- Mix of .NET 6 and .NET 8 projects (varying breaking change exposure)

### Resource Requirements

**Skill Levels Needed:**
- **.NET Migration Expertise**: Essential for Phases 2-3 (binary/source incompatibilities)
- **Security Knowledge**: Required for Phase 5 (RestSharp vulnerability)
- **FHIR/HL7 Domain Knowledge**: Required for Phase 6 (package replacements)
- **Testing Expertise**: Required across all phases (validation)

**Parallel Work Capacity:**
- **Phase 1**: Up to 18 projects can be upgraded in parallel (excluding PalisaidMeta)
- **Phase 4**: Up to 8 transporters in parallel
- **Phase 6**: Up to 7 standard collectors in parallel (after incompatible package collectors resolved)
- **Critical Path**: Phases 2-3 (Support → Authentication → Primary) must be sequential

**Time Considerations:**
- **Foundation phases** (1-2): Must be thorough to avoid downstream rework
- **High-risk phases** (2-3): Allow buffer time for issue resolution
- **Validation phases** (7-8): Comprehensive testing before completion

---

## Source Control Strategy

### Branching Strategy

**Main Branch**: `master`
- Current production code
- Starting point for upgrade

**Upgrade Branch**: `upgrade-to-NET10`
- Created from `master`
- Contains all .NET 10 upgrade changes
- Organized by phases/tiers

**Branch Structure:**
```
master (production)
  └── upgrade-to-NET10 (upgrade work)
```

**Rationale:**
- Single long-lived feature branch for entire upgrade
- Enables incremental commits while maintaining stable master
- Facilitates testing and validation before merge
- Allows rollback to master if critical issues arise

### Commit Strategy

**Commit Frequency**: After each tier completion

**Commit Granularity:**
- **Per-Tier Commits**: Commit after validating entire tier
- **High-Risk Projects**: Individual commits for Authentication, Primary, Support
- **Security Fixes**: Separate commit for RestSharp vulnerability fix

**Commit Message Format:**
```
[.NET 10 Upgrade] <Phase/Tier>: <Brief Description>

- Detailed changes
- Issues resolved
- Validation performed

Tier: <Tier Name and Level>
Projects: <Project List>
Issues Resolved: <Count>
```

**Example Commits:**
```
[.NET 10 Upgrade] Phase 1: Foundation Tier (Level 0) - PalisaidMeta

- Updated TargetFramework to net10.0
- Upgraded Microsoft.Extensions.Configuration packages to 10.0.4
- Resolved 26 issues (1 mandatory, 24 potential, 1 optional)
- Validated by 13 dependent projects

Tier: Tier 1 (Level 0)
Projects: PalisaidMeta
Issues Resolved: 26
```

```
[.NET 10 Upgrade] Phase 2: Core Infrastructure - Support (HIGH RISK)

- Updated framework to net10.0
- Resolved 4 JWT binary incompatibilities in GetTenantId.cs
- Updated JwtSecurityToken API usage patterns
- Validated SSL/TLS authentication behavior
- Upgraded JwtBearer and Identity packages to 10.0.4
- Removed deprecated Microsoft.AspNetCore.Identity package

Tier: Tier 2 (Level 1)
Projects: Support
Issues Resolved: 10 (5 mandatory)
Risk: HIGH - Used by 20 dependent projects
```

### Checkpoints

**Mandatory Checkpoints** (no new work until validated):
1. **After Phase 1**: PalisaidMeta validated; 13 dependents compile
2. **After Phase 2**: Support validated; 20 dependents compile; JWT working
3. **After Phase 3**: Authentication & Primary security tested and approved
4. **After Phase 5**: RestSharp security vulnerability resolved
5. **After Phase 6**: All incompatible packages replaced
6. **Before Merge**: Full solution validation complete

**Checkpoint Criteria:**
- All projects in phase build without errors/warnings
- All tests pass
- Security validation (where applicable)
- Dependent projects validated
- Documented in commit message

### Review and Merge Process

**Pull Request Requirements:**
- **Title**: `.NET 10 Upgrade - Full Solution (53 Projects)`
- **Description**: Summary of all 8 phases, key changes, testing performed
- **Reviewers**: Minimum 2 senior developers
- **Special Review**: Security team review for Authentication, Primary, Support changes

**PR Checklist:**
- [ ] All 53 projects upgraded to net10.0
- [ ] All 226 issues resolved
- [ ] Full solution builds successfully (zero errors, zero warnings)
- [ ] All unit tests pass
- [ ] All integration tests pass
- [ ] Security vulnerabilities resolved (RestSharp)
- [ ] Incompatible packages replaced (4 collectors)
- [ ] JWT breaking changes documented
- [ ] Performance validated (no significant regressions)
- [ ] Breaking changes documented for team
- [ ] Deployment plan ready

**Merge Criteria:**
- All PR checklist items complete
- At least 2 approvals from senior developers
- Security team approval (for Authentication/Support changes)
- Successful deployment to staging environment
- Smoke tests passed in staging
- Rollback plan documented

**Post-Merge Actions:**
- Tag release: `v{version}-net10`
- Update documentation
- Notify team of upgrade completion
- Schedule production deployment
- Monitor production for issues

### Rollback Plan

**If critical issues discovered:**
1. **Before Merge**: Simply abandon `upgrade-to-NET10` branch; continue work on `master`
2. **After Merge**: Revert merge commit; restore `master` to pre-upgrade state
3. **After Deployment**: Rollback deployment; investigate; apply fixes; redeploy

**Rollback Triggers:**
- Security vulnerability introduced
- Critical functionality broken
- Significant performance degradation
- Data corruption or loss

---

## Success Criteria

### Technical Criteria

**Framework Upgrade:**
- [ ] All 53 projects (52 requiring changes + 1 docker-compose) processed
- [ ] All applicable projects target net10.0
- [ ] No projects remain on net6.0 or net8.0 (except docker-compose if not applicable)

**Package Updates:**
- [ ] All recommended package updates applied (45 packages reviewed)
- [ ] Microsoft.AspNetCore packages: 8.0.0 → 10.0.4
- [ ] Microsoft.EntityFrameworkCore packages: 8.0.0 → 10.0.4
- [ ] Microsoft.Extensions packages: 8.0.0 → 10.0.4
- [ ] RestSharp: 110.2.0 → 114.0.0 (security fix)
- [ ] All incompatible packages replaced (4 collectors)
- [ ] All deprecated packages removed (5 packages)

**Build Success:**
- [ ] Clean build of entire solution succeeds
- [ ] Zero build errors across all 53 projects
- [ ] Zero build warnings across all 53 projects
- [ ] All projects compile with net10.0 SDK

**Test Success:**
- [ ] All unit tests pass (100% pass rate)
- [ ] All integration tests pass (100% pass rate)
- [ ] CollectorTests pass (100% pass rate)
- [ ] No test regressions introduced

**Issue Resolution:**
- [ ] All 226 issues addressed:
  - [ ] 99 mandatory issues resolved
  - [ ] 115 potential issues resolved
  - [ ] 12 optional issues resolved
- [ ] All 22 binary incompatibilities resolved (Authentication, Primary, Support)
- [ ] All 13 source incompatibilities resolved (Authentication, Primary)
- [ ] All behavioral changes validated and documented

**Security:**
- [ ] No package vulnerabilities remain
- [ ] RestSharp security vulnerability resolved
- [ ] JWT token handling secure (Authentication, Primary, Support)
- [ ] SSL/TLS authentication validated (Support)
- [ ] Security code review passed

### Quality Criteria

**Code Quality:**
- [ ] No compiler warnings
- [ ] No obsolete API usage (or documented exceptions)
- [ ] Code follows .NET 10 best practices
- [ ] Breaking changes properly handled

**Test Coverage:**
- [ ] Test coverage maintained or improved
- [ ] High-risk projects have comprehensive test coverage (Authentication, Primary, Support)
- [ ] Security-critical flows tested

**Documentation:**
- [ ] All breaking changes documented
- [ ] JWT API migration patterns documented
- [ ] Incompatible package replacements documented
- [ ] Behavioral changes documented
- [ ] Team notified of all changes

**Performance:**
- [ ] No significant performance regressions
- [ ] EF Core query performance acceptable
- [ ] API response times within tolerance
- [ ] Memory usage within acceptable limits

### Process Criteria

**Bottom-Up Strategy Adherence:**
- [ ] All projects upgraded in correct tier order (Tiers 1-8)
- [ ] No tier started before previous tier validated
- [ ] Dependency constraints respected throughout
- [ ] High-risk projects (Support, Authentication, Primary) upgraded with extra validation

**Tier Completion:**
- [ ] **Tier 1 (Level 0)**: 19 projects ✓
- [ ] **Tier 2 (Level 1)**: 1 project (Support) ✓
- [ ] **Tier 3 (Level 2)**: 7 projects (including Authentication, Primary) ✓
- [ ] **Tier 4 (Level 3)**: 8 transporters ✓
- [ ] **Tier 5 (Level 4)**: 3 projects (including CollectorSupport security fix) ✓
- [ ] **Tier 6 (Level 5)**: 11 collectors (4 with incompatible packages) ✓
- [ ] **Tier 7 (Level 6)**: 1 project (CollectorFactory) ✓
- [ ] **Tier 8 (Level 7)**: 2 projects (CollectorsController, CollectorTests) ✓

**Source Control:**
- [ ] All changes committed to `upgrade-to-NET10` branch
- [ ] Commits organized by tier/phase
- [ ] Commit messages follow format
- [ ] All checkpoints validated
- [ ] PR created and reviewed
- [ ] Security team approval obtained

**Validation Points:**
- [ ] PalisaidMeta validated before Phase 2
- [ ] Support validated before Phase 3
- [ ] Authentication security approved before moving to Primary
- [ ] RestSharp security fix validated
- [ ] All incompatible packages replaced and tested
- [ ] Full solution validation before merge

### Deployment Readiness

**Pre-Deployment:**
- [ ] Staging environment deployment successful
- [ ] Smoke tests passed in staging
- [ ] Performance validated in staging environment
- [ ] Security scan passed
- [ ] Rollback plan documented and tested

**Deployment:**
- [ ] Production deployment plan approved
- [ ] Maintenance window scheduled
- [ ] Team notified of deployment
- [ ] Monitoring plan in place

**Post-Deployment:**
- [ ] Production smoke tests passed
- [ ] No critical errors in logs
- [ ] Performance metrics within acceptable range
- [ ] User-facing scenarios validated
- [ ] Team aware of changes

### Definition of Done

**The .NET 10 upgrade is complete when:**
1. ✅ All 53 projects successfully target net10.0
2. ✅ All 226 issues resolved (99 mandatory, 115 potential, 12 optional)
3. ✅ All package updates applied and incompatible packages replaced
4. ✅ Security vulnerability (RestSharp) resolved
5. ✅ All binary/source incompatibilities fixed (Authentication, Primary, Support)
6. ✅ Zero build errors, zero build warnings
7. ✅ 100% test pass rate (unit + integration + collector tests)
8. ✅ Security validation passed
9. ✅ Performance validation passed
10. ✅ Bottom-Up strategy successfully executed across all 8 tiers
11. ✅ Source control process followed (commits, PR, reviews)
12. ✅ Documentation updated
13. ✅ Staging deployment successful
14. ✅ Production deployment successful
15. ✅ Post-deployment validation complete

**Final Sign-Off Required From:**
- Development Team Lead
- Security Team (for Authentication/Support changes)
- QA Team (for test validation)
- DevOps Team (for deployment validation)
