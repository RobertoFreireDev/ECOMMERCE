# Payment-System
Designing a Payment System using Modular Monolith

## Using Chain of Responsibility for Transaction Validation

Question:
Can the Chain of Responsibility pattern be used to validate a transaction?

Answer:
Yes. It is one of the most elegant ways to organize fraud prevention and billing/payment validation logic in backend systems.

What the Chain of Responsibility Pattern Solves

Decoupling
Adding a new validation rule (e.g., CPF block, daily transfer limit, fraud score check) does not require modifying existing validators.
This follows the Open/Closed Principle (SOLID).

Clear Orchestration
The exact order of validations is explicitly defined.
If any validation fails, the chain can be short-circuited, preventing unnecessary processing.

Maintainability
Each validator or microservice remains small, focused, and single-responsibility, whether validating boletos or digital payments.

Practical Fintech Example

A transaction validation chain might look like:

Balance Validator

Daily Limit Validator

Risk / Fraud Validator (calling an external service)

Transaction Execution

## SOLID Principles in Fintech – Beyond “Clean Code”

A senior engineer does not say SOLID exists just to “make code clean.”
That answer shows memorization, not understanding.

In fintech interviews, SOLID is evaluated by how it reduces risk, improves scalability, and increases resilience in real systems.

The Statement That Disqualifies a Senior Candidate

“The ‘S’ (Single Responsibility) means my class should do only one thing.”

This is oversimplified and misleading.

How a Software Architecture Specialist Understands SOLID
S – Single Responsibility Principle

A class should have only one reason to change.

If a digital payments class changes both because of business rules and infrastructure concerns, it violates SRP.
This increases CI/CD risk and makes deployments more fragile.

O – Open/Closed Principle

This is a resilience principle.

Code should be open for extension (e.g., adding new fraud-prevention rules) but closed for modification.
This minimizes regressions and stabilizes production systems (e.g., Java/Spring Boot services).

D – Dependency Inversion Principle

This is a core pillar of software architecture.

High-level modules must depend on abstractions (interfaces), not concrete implementations.
This allows swapping infrastructure components—such as databases (PostgreSQL ↔ Oracle) or Kafka clients—without rewriting business logic.

ISP - Interface Segregation Principle

A “fat” interface forces client classes to depend on methods they do not need.

In a digital payments system, this can mean a Billing Payments service becomes coupled to fraud prevention logic, even if it only needs two payment-related methods.

More coupling directly leads to lower resilience, harder testing, and riskier deployments.

Apply the Interface Segregation Principle (ISP) by splitting large interfaces into smaller, cohesive contracts, such as:

PaymentProcessing

FraudEvaluation

BalanceQuery

This approach:

Improves test-driven development (TDD)

Enables safe implementation swapping

Preserves Liskov Substitution by keeping interfaces focused and predictable

Architectural Impact

ISP is not just about cleaner code—it is about software architecture and CI/CD.

Segregated interfaces:

Reinforce Single Responsibility across microservices

Enable independent deployments

Reduce blast radius of changes

Improve system performance and scalability

LSP - Liskov Substitution Principle 

“Give a practical example in a digital payments system where you would apply the Liskov Substitution Principle (LSP), and explain why.”

Many candidates freeze or give a purely theoretical answer.

The Answer That Stands Out: LSP Connected to DDD and SOLID
Common Problem

You have a Billing Payments service that processes multiple payment methods:

Boleto

TED

PIX

Object-Oriented / LSP Solution

Define a common abstraction, such as an interface or abstract class:

PaymentMethod

BoletoPayment

TEDPayment

PIXPayment

The client system interacts only with PaymentMethod.
Any concrete implementation can be substituted without changing the client code, and the process() behavior is always correct and predictable.

The Architectural Insight

This is not just an OOP exercise—it is software architecture.

Guarantees resilience and scalability

Enables safe extension when new payment methods are introduced

Supports the Open/Closed Principle: new payment types are added without breaking existing code

If a new payment method is introduced by the Central Bank tomorrow, you simply add a new implementation—no refactoring of existing logic is required.

## Database Sharding for Digital Accounts – Architecture and Risks

Interview question:
“If your database (PostgreSQL or Oracle) is reaching its scalability limits in a fintech system, how would you implement sharding for digital accounts—and what could go wrong?”

What the Interviewer Is Really Evaluating

The interviewer does not want to hear “I’ll just split the database.”
They want to know whether you understand software architecture, data access patterns, and real operational risks.

The Senior Software Engineer Answer

The biggest mistake is not technical—it is business-related: choosing the wrong sharding key.

Bad Sharding Key (The Disaster Scenario)

Using Transaction ID as the sharding key.

Queries that need all transactions for a single customer (billing, statements) must query all shards (fan-out queries).

This destroys performance and severely impacts resilience.

Correct Sharding Key (The Foundation)

Using Customer ID / Account ID as the sharding key.

All critical data for a customer (balance, transaction history, fraud signals) lives on the same shard.

This enables fast, localized single-shard queries, which are essential for digital payments.

The Load-Balancing Challenge

Customer-based sharding introduces the risk of hot shards, where a small number of high-volume customers generate most of the traffic.

A senior engineer must address:

Hash-based sharding strategies

Periodic rebalancing or resharding plans

Capacity planning for uneven workloads

## Why Use Kafka Instead of Traditional Queues (SQS, RabbitMQ)?

Interview question:
“Why would you use Kafka instead of a traditional queue like SQS or RabbitMQ?”

The Answer That Kills Your Chances

“Because Kafka is faster.”

This is the worst possible answer.
Technical interviews are not about raw speed—they are about architectural understanding.

What a Mid-Level or Senior Engineer Should Explain

Ordering Is Non-Negotiable in Financial Systems
In digital payments, digital accounts, and any balance-related workflow, event ordering is critical.
Kafka, as a distributed event log, guarantees ordering within a partition, preventing subtle and expensive inconsistencies.

Replayability and Real Resilience
When a microservice fails—or when events must be reprocessed for auditing, reconciliation, or bug fixes—Kafka allows consumers to re-read historical events.
In contrast, SQS removes messages once they are consumed.
In fintech, event replay is essential for fraud prevention and billing reconciliation.

Scalability with Backpressure Control
Kafka supports multiple independent consumers reading the same topic at their own pace.
This enables true horizontal scalability and controlled backpressure in high-throughput cloud environments (AWS, Kubernetes, etc.).

## Fan-Out Messaging, Failure Isolation, and DLQs

Interview question:
“An Order service uses a Fan-Out pattern (e.g., RabbitMQ exchange) to notify five microservices (Inventory, Billing, Logistics, Email, Analytics) about a new order. If one consumer (Inventory) is down at the time of notification, what happens to the message and to the other four services?”

The Failing Answer (90% of Candidates)

“The entire order fails and the message is lost.”

This answer reveals monolithic thinking and a lack of understanding of distributed systems and resilience.

What a Senior Backend Engineer Should Answer
Failure Isolation Is the Key

In a Fan-Out (broadcast) pattern, the failure of one consumer is isolated.
Once the message is successfully published to the RabbitMQ exchange, each bound queue behaves independently.

The four services that are healthy and correctly bound will receive and process the message normally, regardless of the Inventory service being down.

What Happens to the ‘Lost’ Message

The message intended for the Inventory service is not lost.
It should be routed to the Dead Letter Queue (DLQ) associated with the Inventory consumer.

The DLQ acts as a controlled failure buffer, ensuring that critical messages (e.g., inventory decrement or reservation) can be reprocessed later.

Guaranteeing Consistency

Senior responsibility goes beyond the message broker.
You must ensure that eventual consistency is achieved:

When the Inventory service comes back online, it must reprocess its DLQ to correctly apply the stock update or confirm the reservation, preserving inventory integrity.

## Preventing Overselling and Double Spending in Distributed Systems

Interview question:
“Two users try to buy the last ticket or withdraw the same balance at the same time. How do you prevent selling or spending what you don’t have?”

The Beginner Answer (Critical Mistake)

“I use synchronized on the Java method.”

In a microservices architecture with multiple instances, this is a fatal error.
synchronized only locks the local JVM thread—it does nothing to prevent concurrency issues across instances. Race conditions will still occur.

The Expert Engineering Answer

Concurrency control must be enforced at the database level (PostgreSQL / Oracle), where the data actually lives.

1. Optimistic Locking (@Version)

Best for: Low-conflict scenarios (e.g., profile updates).

How it works:
You assume collisions are rare. If they occur, the update fails and the application handles the exception.

Benefits:
Lightweight, scalable, and high-throughput.

2. Pessimistic Locking (SELECT … FOR UPDATE)

Mandatory for: Fintech and critical payment flows.

How it works:
When updating a balance, the database row is locked. No other transaction can read or write it until the current transaction completes.

Benefits:
Guarantees absolute consistency (ACID), even at the cost of some performance.

## Normalization vs. Denormalization in Real-Time Fraud Prevention

Interview question:
“Your fraud prevention system needs high performance to check a customer’s history in real time. Do you use normalization or denormalization?”

The Engineering Answer That Stands Out
Normalization (e.g., 3NF)

Primary Focus: Data integrity and minimal redundancy (ACID guarantees).

Where to Use in Fintech:
Digital accounts and core payment systems (balances, critical transactions).
This layer is the source of truth.

Pros:

Optimized for writes

Minimizes inconsistency risk

Technologies:
PostgreSQL, Oracle (transactional systems)

Denormalization (e.g., Event or History Tables)

Primary Focus: Read performance and scalability.

Where to Use in Fintech:
Transaction history, event logs, reporting, and fraud detection systems.

Pros:

Optimized queries

Fast access to historical data

Technologies:
NoSQL databases or optimized PostgreSQL tables (analytical systems)

Specialist Insight

In fintech, the correct answer is not “either one.”
You use normalization for the source of truth (current monetary state) and denormalization for fast access to historical data used in fraud prevention.

This is the fusion of transactional and analytical architectures.

## Observability in Fintech Microservices – Beyond Tools

Anyone who has debugged a microservice running on Kubernetes in AWS knows that logs alone are not enough.

A common interview mistake is treating observability as a tooling problem (Prometheus, Grafana, Datadog).
In senior-level fintech interviews, what matters is the software architecture behind observability.

The Game-Changing Design Pattern: The Observability Pattern

Observability is not about collecting data—it is about instrumenting code so the system can proactively answer three critical questions:

1. Tracing

Question: How did a digital payment transaction flow from the gateway to the database?

Approach:
Use Correlation IDs and distributed tracing (Jaeger, Zipkin) to track a PIX payment end-to-end across:

Multiple microservices

Kafka topics and consumers

This enables full visibility into cross-service transaction lifecycles.

2. Metrics

Question: Where is the performance or scalability bottleneck?

Approach:
Monitor business-level metrics, not just infrastructure metrics:

Boleto issuance success rate

Fraud prevention API latency

Payment authorization error rates

Infrastructure metrics (CPU/RAM) are necessary but insufficient on their own.

3. Logging

Question: What exactly happened at the point of failure?

Approach:

Standardize logs in structured formats (JSON)

Ensure every log entry includes the Correlation ID

This makes logs searchable, traceable, and suitable for automated analysis.

## Circuit Breaker in Fintech Systems – Beyond the Definition

If a backend engineer says “a Circuit Breaker just stops calling a failing service”, the answer is technically correct—but architecturally shallow.

In fintech and mission-critical systems, interviewers expect a deeper understanding of why the pattern exists and what it actually protects.

What a Senior Fintech Engineer Must Understand

Resource Protection (Most Overlooked Point)
The Circuit Breaker does not exist to protect the failing service.
Its primary purpose is to protect your own service from exhausting threads, connections, and throughput.
This is how cascading failures are prevented in high-criticality systems.

Realistic Fallback Strategies
In payments and banking, fallback is not “return cached data” or fabricate alternative responses.
Valid strategies include:

Fail fast with clear errors

Controlled degradation

Asynchronous processing

In regulated environments, consistency outweighs availability tricks.

Non-Negotiable Observability
Circuit state changes (open, half-open, closed) must generate metrics and alerts via tools like CloudWatch, Prometheus, or Grafana.
Without telemetry, a Circuit Breaker becomes an invisible failure mechanism—and invisible failures bring systems down.

A Circuit Breaker Alone Is Insufficient
The pattern only works effectively when combined with:

Well-defined timeouts

Bulkheads (resource isolation)

Rate limiting

Resilience is a system of patterns, not a single one.

## Stop Reconciling. A Ledger Is the Only Architecture That Eliminates Financial Chaos

The biggest engineering mistake in digital payment systems (PIX, boletos, wire transfers) is relying on traditional relational models to represent financial events.

When transactions, statements, and balances must be reconstructed through complex joins, the database becomes fragile, slow, and prone to inconsistencies.

In high-scale, microservices environments, this is not just inefficient—it is a direct risk to account consistency, reconciliation correctness, and fraud prevention.

The Senior Backend Solution: An Accounting Ledger

A ledger is not merely an accounting concern.
It is a resilience layer that acts as the single source of truth, recording every financial movement as an immutable event.

Conceptually, it behaves like a journaling API with:

Guaranteed ordering

Explicit idempotency

Append-only writes

Three Critical Engineering Advantages of a Ledger

Structural Atomicity (Double-Entry Bookkeeping)
The only reliable way to ensure money is never “lost” is to enforce symmetry between debit and credit.
If one side fails, the other is not applied.
This is the practical foundation for handling distributed atomicity in digital accounts and avoiding dangerous intermediate states.

Consistent Performance (Non-Degrading APIs)
Systems without a ledger often recompute balances by aggregating thousands of records.
With a ledger, balances are maintained as known snapshots, updated on each event.
The result is lightweight and predictable balance/statement APIs, even under extreme traffic spikes.

Automatic Reconciliation and Native Auditability
Reconciliation is no longer a nightly batch job—it becomes an inherent property of the immutable ledger structure.
This provides mathematical proof of internal consistency, reduces false positives in antifraud systems, and simplifies external audits.

## Using CQRS in Digital Account Systems

Common interview question:
“Would you use CQRS (Command Query Responsibility Segregation) in a Digital Accounts system? Why?”

The failing answer:
“Yes, to separate read and write databases and improve performance.”

That response is superficial. In fintech, interviewers want to assess whether you understand the architectural impact and financial risk involved.

What a Senior Software Engineer Should Explain

Eventual Consistency and Financial Risk
In financial systems with separate read and write models, the critical challenge is preventing the read side from becoming so outdated that it creates risk.
Explain how events are propagated via Kafka or RabbitMQ, how delays can cause users to temporarily see inconsistent balances, and how the system tolerates and mitigates these states.
This is fundamentally about resilience, fraud prevention, and controlled inconsistency.

The Link to Event Sourcing
CQRS reaches its full potential when combined with Event Sourcing.
Commands do not directly mutate state; instead, they emit immutable events such as “TransferCompleted”.
The read database becomes a projection of these events.
This approach enables state reconstruction, simplifies audits, and reduces reconciliation complexity—critical in billing and payment systems.

Context-Driven Application with DDD
CQRS is not a global application pattern. It should be applied selectively where the domain requires different read and write models.
In microservices with asymmetric load:

A Statement/Transaction History service with massive read volume benefits greatly from CQRS.

An Account Registration service with simple writes and limited reads does not justify the added complexity.

## Idempotency in Distributed Payment Systems

In distributed systems, network failures and timeouts are expected.
In fintech, the risk is critical: processing a debit event twice means real financial loss for the customer.

This is why idempotency is a core pillar of resilient payment systems.

The Problem (A Common Trap)

By default, Kafka provides “At-Least-Once” delivery.
If a consumer fails to commit its offset, the same message may be re-delivered.

Without idempotent business logic, a single debit of $50 can become $100, breaking the integrity of digital accounts.

The Expert Solution: Idempotent Processing

Idempotency means executing the same operation multiple times while producing the same final result.

How to implement it in the backend:

Unique Idempotency Key
Extract a globally unique identifier from the Kafka event or API header
(e.g., paymentId, transferId).

Processing Record
Before executing any critical logic (e.g., balance debit, invoice or boleto issuance),
check a dedicated store (PostgreSQL table or Redis cache) to see if the idempotency key has already been successfully processed.

Conditional Execution

If the key already exists → ignore the message

If it does not exist → execute the business logic, persist the key, and commit the Kafka offset

## Duplicate Payment Reconciliation – Performance Considerations

Classic Problem in Digital Payments

Problem:
To reconcile duplicate payments, a naïve algorithm compares each new transaction against all previous transactions to check for duplication.

Complexity:
This approach has O(n²) time complexity.
In high-volume Billing Payments systems, this quickly leads to increased latency, timeouts, and reconciliation failures.

Engineering-Grade Solutions (O(n) or O(log n))

Data Structure Optimization
Do not iterate over all records.
Generate a unique transaction hash (e.g., combination of taxpayer ID, amount, and date) and store it in a fast lookup structure:

HashSet (in-memory, single instance)

Redis (distributed cache)

This reduces duplicate checks to O(1) on average.

Proper Use of Database Indexes
When using relational databases such as PostgreSQL or Oracle, define composite indexes on the fields that determine uniqueness.
This avoids full table scans and reduces lookups to approximately O(log n).

Queues and Asynchronous Processing
Use Kafka (or similar) to process reconciliation events asynchronously.
The queue decouples duplicate validation from transaction ingestion, preventing bottlenecks and improving resilience and throughput.

## Webhook Handling in Payment Systems – Key Engineering Principles

Webhooks are not just “reverse API calls.” In large-scale digital payment systems, they must be designed to handle failures, retries, and malicious scenarios safely.

When receiving payment webhooks (e.g., PIX notifications or billing updates), focus on these three core principles:

Idempotency Is Mandatory
Payment gateways will resend the same event multiple times.
Your backend must detect whether an event ID has already been processed to avoid double credits or duplicate charges.
Use database-level guarantees (e.g., PostgreSQL unique constraints, locks, or a dedicated processing table).

Availability vs. Processing
Never execute heavy business logic synchronously when receiving a webhook.
Acknowledge the request immediately with 200 OK, then enqueue the event (Kafka, AWS SQS, etc.) for asynchronous processing.
This improves resilience when databases or downstream services are slow or temporarily unavailable.

Security Beyond Basic JSON Validation
Do not trust the payload by default.
Validate that the webhook truly comes from the payment provider by verifying a signature (e.g., HMAC) sent in request headers.

## References:

https://www.systemdesignhandbook.com/guides/design-a-payment-system/

https://bytebytego.com/guides/payment-system/?utm_source=chatgpt.com

https://newsletter.pragmaticengineer.com/p/designing-a-payment-system

https://www.geeksforgeeks.org/system-design/what-is-a-modular-monolith/

https://www.linkedin.com/in/daniellimafintech
