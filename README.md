# Payment-System
Designing a Payment System using Modular Monolith

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
