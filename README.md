# Payment-System
Designing a Payment System using Modular Monolith

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
