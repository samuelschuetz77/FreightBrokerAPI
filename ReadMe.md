# FreightBrokerAPI

This is a practice ASP.NET Core minimal API project that models a **freight brokerage dashboard**.  
It is inspired by real-world brokerage operations and uses **Entity Framework Core** (EF Core) for data access.

---

## What is a Freight Broker?

A **freight broker** is a middleman between two sides of the trucking industry:

- **Shippers** → Companies that need freight moved (farmers, food processors, manufacturers, retailers, warehouses).
- **Carriers** → Trucking companies or owner-operators who physically move the freight.

A broker does **not** own trucks and usually does **not** own the freight. Instead, brokers:
1. Find freight posted by shippers.
2. Find a carrier to haul it.
3. Negotiate rates with both sides.
4. Earn the difference (margin) between what the shipper pays and what the carrier is paid.

---

## Common Terms & Entities

- **Shipper**  
  The party that needs freight moved. Could be:
  - A farmer selling potatoes.
  - A food processor shipping frozen French fries.
  - A Walmart distribution center moving goods to stores.

- **Consignee (Receiver)**  
  The party that receives the freight at the destination.
  - Example: A Sysco foodservice warehouse in Denver receiving frozen potatoes.

- **Carrier**  
  A trucking company or independent owner-operator who has DOT/MC authority to haul freight.  
  - Each carrier may have multiple trucks and drivers.  
  - Carriers provide equipment like dry vans, refrigerated trailers ("reefers"), or flatbeds.

- **Driver**  
  The individual behind the wheel.  
  - In this practice project, we don’t model drivers separately, but in real systems, they are tied to trucks.

- **Load**  
  A shipment to be moved from point A (**Shipper/Origin**) to point B (**Consignee/Destination**).  
  - Attributes include origin, destination, weight, rate, and status.  
  - Status flow: `Open → Covered → InTransit → Delivered` (or `Canceled`).

- **Coverage**  
  The assignment of a load to a specific carrier.  
  - Includes the rate the carrier is paid.  
  - Links a load and a carrier together.

- **Margin**  
  The broker’s profit on a load.  
  - Formula: **ShipperRate − CarrierRate**.  
  - Example: Broker charges shipper $1,850, pays carrier $1,500 → Margin = $350.

- **Deadhead**  
  Miles a truck drives empty to reach the pickup. Brokers and carriers try to minimize deadhead.

- **Soft Delete**  
  Instead of deleting a load record permanently, we flip an `IsDeleted` flag.  
  - Useful when a shipper cancels a load but we still want the record in the database for auditing.

---

## How Brokers Work (End-to-End)

1. **Shipper posts a load**  
   Example: *Simplot Frozen Foods, Idaho Falls, ID → Sysco DC, Denver, CO. Commodity: Frozen potatoes, 42,000 lbs. Shipper pays $1,850.*

2. **Broker sees the load** in a dashboard like FreightBrokerAPI. The load shows up with `Status = Open`.

3. **Broker finds a carrier**  
   Carrier A has a truck empty in Grand Junction, CO. Broker offers the load for $1,500. Carrier accepts.

4. **Coverage created**  
   Broker records the match: Load 221 → Carrier A, CarrierRate = $1,500.  
   Load status changes to `Covered`.

5. **Driver dispatched**  
   Carrier’s driver heads to Idaho Falls, picks up potatoes, and drives to Denver.

6. **Delivery**  
   Sysco DC in Denver signs the bill of lading. Load status changes to `Delivered`.

7. **Broker margin recorded**  
   ShipperRate ($1,850) − CarrierRate ($1,500) = $350 margin.

---

## Data Model in FreightBrokerAPI

- **Shipper** (1) → (many) **Loads**  
- **Carrier** (1) → (many) **Coverages**  
- **Load** (1) → (0 or 1) **Coverage**  

Tables created by EF Core migrations:
- `Shippers`
- `Carriers`
- `Loads`
- `Coverages`
- `__EFMigrationsHistory` (internal EF Core tracking)

---

## Why Build This Project?

- To practice **Chapter 12** concepts (EF Core: entities, DbContext, migrations, CRUD).
- To simulate real-world freight brokerage logic in a **minimal, clean way**.
- To extend later into a full broker dashboard:  
  - Load board scraping (shippers post freight elsewhere).  
  - Carrier availability updates.  
  - Margin & KPI reporting.  
  - Eventually, a marketplace platform.

---
