| | |
|---|---|
| **Document** | 02 — Requirements & Acceptance Criteria |
| **Product** | AssetHub |
| **Owner** | Andrew Strelke |
| **Version** | 0.02 |
| **Date Created** | 2026-09-08 |
| **Date Updated** | 2026-10-20 |
---

## 1. How to read this

Each requirement has an id, a short statement and acceptance criteria (AC). This is an early draft: anything marked *TBD* is not decided yet and will be filled in as the application is built.

| Prefix | Area |
|---|---|
| `AUTH` | Accounts, sessions and access |
| `ASSET` | The asset register and asset lifecycle |
| `CUST` | Custody: check-out and check-in |
| `MAINT` | Maintenance work orders |
| `REF` | Reference data: categories, locations, manufacturers |
| `AUDIT` | The audit trail |
| `DASH` | Dashboard |
| `USER` | User administration |
| `NFR` | Cross-cutting requirements |

---

## 2. Users and roles

Every user signs in and holds one or more roles. The roles are Administrator, AssetManager, Technician and Viewer.

The Administration section shows Reference data and Audit trail to Administrators and AssetManagers, and Users to Administrators only. Hiding a link is a convenience; the API enforces the same rules.

What each role can do beyond this is *TBD*.

---

## 3. Functional requirements

### 3.1 Accounts and access — `AUTH`

**AUTH-01 — Sign in with email and password.**

- **AC** A wrong password, an unknown email and a deactivated account all show exactly the same message: "Email or password is incorrect." The screen must not reveal which one it was.
- **AC** After 5 failed attempts the account is locked for 15 minutes. Signing in while locked shows "This account is temporarily locked. Try again later." as a warning. The client recognizes this case by the code `auth_account_locked`, not by parsing the text.
- **AC** A malformed email is a validation failure shown under the field, not an authentication failure.
- **AC** No password strength rules are applied at sign-in.
- **AC** A signed-out visitor who opens a page is sent to sign-in, and the page they asked for is kept as `returnUrl`.
- **AC** After signing in, the user goes to `returnUrl` if it is a relative path, otherwise to the dashboard. An absolute URL is ignored so a crafted link cannot send the user to another site.

**AUTH-02 — Stay signed in.**

- **AC** A session is carried by a refresh token. Refresh tokens are stored only as a SHA-256 hash.
- **AC** Token lifetimes and rotation rules are *TBD*.

**AUTH-03 — Sign out.**

- **AC** Sign out is available from the user menu and from the header.

**AUTH-04 — Change password.**

- **AC** Changing a password needs the current password, the new password and a confirmation.
- **AC** The new password must be 12 to 128 characters with an uppercase letter, a lowercase letter, a digit and a symbol. The form shows a checklist of these rules as the user types; the server remains the authority.
- **AC** A wrong current password, or a new password identical to the current one, comes back from the server and is shown under the relevant field.
- **AC** A successful change revokes every refresh token for the user. The client then signs in again silently with the new password, so the current tab keeps working, and confirms: "Password changed. Other devices have been signed out."

**AUTH-05 — See who I am.**

- **AC** The user menu shows the signed-in user's name, email and role.

**AUTH-06 — Pages I can't open.**

- **AC** A signed-in user who opens a page their roles don't allow sees "Your account does not have permission to view this page."
- **AC** An address that matches no page shows "Page not found" with a link back to the dashboard.

### 3.2 Asset register and lifecycle — `ASSET`

**ASSET-01 — Register an asset.**

- **AC** An asset records a tag, name, serial number, model, category, location, manufacturer, condition, purchase date, purchase cost, warranty expiry and notes. Which fields are required is *TBD*.
- **AC** The asset tag is unique and cannot be changed after the asset is created.
- **AC** A new asset starts `Available`. Its condition defaults to `Good`.

**ASSET-02 — Edit an asset.**

- **AC** A disposed asset cannot be edited.
- **AC** Two people editing the same asset must not silently overwrite each other. The asset carries a row version for this; the exact behavior is *TBD*.

**ASSET-03 — Find assets.** *TBD.*

**ASSET-04 — Dispose of an asset.**

- **AC** Disposal records a date and a reason.
- **AC** An asset can be disposed of when it is Available, Lost, or UnderMaintenance with no open work order. Disposed is terminal: no further transition and no edit.

**ASSET-05 — Report an asset lost.**

- **AC** An Available, Assigned or UnderMaintenance asset can be reported lost.
- **AC** Reporting an Assigned asset lost closes its open assignment.
- **AC** Reporting an asset lost while its work is in progress cancels that work.

**ASSET-06 — Report a lost asset found.**

- **AC** A found asset returns to `Available`, or to `UnderMaintenance` if it was found `Unserviceable`.

### 3.3 Custody — `CUST`

**CUST-01 — Check an asset out.**

- **AC** Checking out an Available asset opens an assignment and moves the asset to `Assigned`.
- **AC** An asset has at most one open assignment.
- **AC** Overdue is derived, never stored: an open assignment is overdue from the day after its due-back date. A closed assignment is never overdue, and one with no due-back date never is.

**CUST-02 — Check an asset in.**

- **AC** Checking in closes the assignment with the closure reason `Returned` and the condition the asset came back in.
- **AC** An asset returned in any condition other than `Unserviceable` becomes `Available`. An asset returned `Unserviceable` becomes `UnderMaintenance` without a work order.
- **AC** A closed assignment is never reopened and never deleted.

### 3.4 Maintenance — `MAINT`

**MAINT-01 — Schedule work.**

- **AC** A work order has a type, title, description, scheduled date and vendor. It starts `Scheduled`.
- **AC** An asset has at most one open work order (Scheduled or In progress).
- **AC** Rescheduling is done by canceling the order and raising a new one, so the missed date stays visible.

**MAINT-02 — Start work.**

- **AC** Starting moves the asset to `UnderMaintenance`. It is refused while the asset is Assigned or Lost.

**MAINT-03 — Complete work.**

- **AC** Completing records a resolution and moves the asset to `Available`, or leaves it `UnderMaintenance` if the resulting condition is `Unserviceable`.

**MAINT-04 — Cancel work.**

- **AC** Canceling unstarted work never changes the asset's status.
- **AC** Canceling started work returns the asset to service, except that it stays `UnderMaintenance` if it is still `Unserviceable`, and it becomes `Lost` when the cancel comes from reporting the asset lost.

### 3.5 Reference data — `REF`

**REF-01 — Maintain categories, locations and manufacturers.**

- **AC** Each has a name that is unique within its type.
- **AC** A category has a description and a default useful life in months. A location has a building, floor and room. A manufacturer has a support phone, support email and website URL.
- **AC** An entry can be deactivated instead of deleted. *TBD:* what deactivation does.

### 3.6 Audit trail — `AUDIT`

**AUDIT-01 — Changes are recorded.**

- **AC** Each audit entry records the entity, the action, a summary, the changes, who did it (id and name), the correlation id and when it happened.
- **AC** The audit trail is append-only. It has no foreign key to users, so entries survive any change to the user record.

### 3.7 Dashboard — `DASH`

**DASH-01 — Dashboard.** The dashboard is the landing page. Its content is *TBD*.

### 3.8 User administration — `USER`

**USER-01 — Administrators manage users.**

- **AC** A user has an email, full name, department, active flag and one or more roles.
- **AC** Creating, editing and deactivating users is *TBD*.

---

## 4. Field rules

| Field | Rule |
|---|---|
| Email (sign-in) | A valid email address |
| Password (sign-in) | Required; no strength rules |
| New password | 12–128 characters, with an uppercase letter, a lowercase letter, a digit and a symbol |
| Confirm new password | Must match the new password |
| Asset tag | Unique; cannot be changed |
| Reference data name | Unique within its type |
| Everything else | *TBD* |

---

## 5. Cross-cutting requirements — `NFR`

**NFR-01 — Concurrency.**

- **AC** A rule that must hold when two requests arrive at once is backed by the database (a
  constraint or unique index), not only by an application check. For example, the asset tag is checked first so the error can name it, but the unique index is what guarantees it. See `rule-validation-tiers-diagram.png`.

Everything else is *TBD*.

---

## 6. Error contract

*TBD.* Known so far: a locked account is reported with the code `auth_account_locked`.

---

## 7. Traceability

*TBD.* Tests will be listed here as they are written.

---

## 8. Open questions

1. What can each non-Administrator role do?
2. What does the dashboard show?
3. Which asset fields are required, and what are their length limits?
4. What does deactivating reference data change for existing assets?
5. What is the error response format for the API?
