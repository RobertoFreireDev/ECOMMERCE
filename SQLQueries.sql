SELECT
    o.Id                 AS OrderId,
    o.CustomerId,
    o.ShippingAddressId,
    o.BillingAddressId,
    o.PaymentMethod,
    o.CouponCode,

    i.ProductId,
    i.Name               AS ProductName,
    i.UnitPrice,
    i.Quantity
FROM Orders o
LEFT JOIN OrderItems i
    ON o.Id = i.OrderId
WHERE o.Id = '87521c0a-521e-4192-b9d9-e6ea16cf3b72';