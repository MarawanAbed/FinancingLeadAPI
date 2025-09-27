# FinancingLeadAPI
### Entity Properties & Constraints
The `FinancingLead` entity contains the following key properties:

- `Id` (GUID): Primary key.
- `Contact.Name` (string): Max length = 100
- `Contact.Email` (string): Max length = 254 (standard email max length)
- `Contact.PhoneE164` (string): Max length = 20 (supports international formats)
- `AmountRequested` (decimal): Precision = 18, Scale = 2 — to avoid rounding issues
- `CreatedAt` / `UpdatedAt` (DateTime): Stored as UTC (see below)
- `ReviewStatus` (enum): Values = Pending, Accepted, Rejected
- `ReviewedAt` (DateTime?): Nullable — only set after review
- `ReviewReason` (string?): Optional, Max length = 500

### Indexes

The following indexes are defined to improve query performance:

- `IX_FinancingLeads_CreatedAt_DESC`: Speeds up default sorting by most recent leads.
- `IX_FinancingLeads_Contact_PhoneE164`: Supports exact phone lookups and phoneStartsWith filtering.
- `IX_FinancingLeads_Contact_Email`: Ensures email uniqueness and quick lookup.
- Composite Index: `(Contact.Name, Contact.Email, Contact.PhoneE164)` to optimize full-text search over contact fields.


### UTC Time Handling

All `DateTime` values (e.g., `CreatedAt`, `ReviewedAt`) are stored in **UTC** to ensure consistency across time zones.

- UTC is enforced at the entity level — `CreatedAt` is set in the entity constructor using `DateTime.UtcNow`.
- No conversion is done on save; we assume clients use UTC or handle conversion on the frontend.
- Database timestamps are stored in UTC and formatted using ISO 8601 (`"O"` format) when serialized.
