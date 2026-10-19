| | |
|---|---|
| **Document** | 01 — Vision & Scope |
| **Product** | AssetHub |
| **Owner** | Andrew Strelke |
| **Version** | 0.01  |
| **Date Created** | 2026-08-31 |
| **Date Updated** | 2026-10-12 |
---

## 1. Problem

Organizations that issue equipment to people lose track of it. A spreadsheet can say what we own, but not who has it right now, whether it works, or who changed the record and when.

## 2. Vision

**A registry that is the source of truth for tracked equipment: where each asset is, who has it, and whether it is serviceable.**

An asset's status is not edited directly. It moves because something happened to the asset: it was checked out, checked in, sent for repair, reported lost or disposed of.

## 3. Objectives

| Objective | How we'll know |
|---|---|
| Custody is unambiguous | An asset has at most one open assignment |
| Serviceability has one answer | An asset has at most one open work order |
| Changes are traceable | Changes are recorded in an append-only audit trail |
| Concurrent requests can't corrupt data | Rules that must hold under concurrency are backed by the database |

## 4. Users

Users sign in with an email and password. Each user holds one or more roles.

| Role | Primary job |
|---|---|
| **Administrator** | Runs the system, including user administration |
| *Other roles* | To be defined |

## 5. In scope

- **Asset register.** Asset tag, name, serial number, model, category, location, manufacturer, condition, purchase date and cost, warranty expiry and notes.
- **Custody.** Check an asset out to a person and check it back in.
- **Lifecycle.** Five statuses: Available, Assigned, UnderMaintenance, Lost and Disposed. A lost asset can be reported found; Disposed is terminal. See `asset-transitions-diagram.png`.
- **Maintenance.** Work orders that are scheduled, started, then completed or canceled.
- **Reference data.** Categories, locations and manufacturers, which can be deactivated.
- **Audit trail.** An append-only record of changes.
- **Dashboard.** Content to be decided.
- **Accounts and access.** Sign-in, sign-out, change password, roles and user administration.

## 6. Out of scope

To be decided.

## 7. Constraints and assumptions

- Built on .NET 9: a Blazor WebAssembly client and a separate HTTP API, backed by a relational
  database.
- One organization.

### Decision: Blazor WebAssembly as the hosting model

The client is a standalone Blazor WebAssembly app that runs entirely in the browser and talks to
the API over HTTP. Blazor Server and the server-rendered Blazor Web App were not chosen.

| Reason | What it means for AssetHub |
|---|---|
| The API is the only gate | Every rule and permission is enforced by the API. The client can hide a button for convenience, but the same request sent directly to the API gets the same answer, so a clean HTTP boundary keeps that honest. |
| The API is a product in its own right | The API contract is a design document. Building the client against the same HTTP endpoints anyone else would call keeps that contract real and testable. |
| The client sees contracts, not internals | The client references only the shared contract types (DTOs and enums). It cannot reach the database or business logic directly. |
| No server connection per user | Blazor Server keeps a live connection and UI state on the server for every open tab. WebAssembly needs none, so a dropped connection doesn't lose the user's page. |
| Simple to host | The built client is static files, served from any web server or CDN, separately from the API. |

**Trade-offs accepted:** a larger first download (the .NET runtime is fetched once, then cached),
and code in the browser can be inspected, so it must never hold secrets. Both are acceptable for an
internal tool used by employees.
- Every user is an employee with an account; there is no anonymous access.

## 8. Principal risks

| Risk | Response |
|---|---|
| Two people are recorded as holding the same asset | Enforce at the database, not only in application code |
| Two requests create the same asset tag | A unique index on the asset tag decides the race |

## 9. Definition of done

To be decided.

## 10. Downstream documents

02 Requirements & acceptance criteria → 03 Domain model & lifecycle specification → 04 Data model and 05 API contract → 06 UI design.

## 11. Open questions

- What can roles other than Administrator do?
- What does the dashboard show?
- What is deliberately out of scope for a first version?
