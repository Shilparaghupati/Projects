SELECT * FROM "Admins";

SELECT * FROM "Customers";


SELECT * FROM "FoodItems";

SELECT * FROM "OrderItems";

SELECT * FROM "Orders";

SELECT * FROM "Payments";

SELECT EXISTS (
    SELECT 1
    FROM "Customers"
    WHERE "Email" = 'shilparagupati28@gmail.com'
);

SELECT current_database();

SELECT current_user;

SELECT "CustomerId", "Name", "Email"
FROM "Customers";

SELECT current_database(), current_user;

SELECT * 
FROM "Customers"
WHERE "Email" ILIKE '%shilparaghupati%';

SELECT "CustomerId", "Name", "Email", "Phone", "Address", "CreatedAt"
FROM "Customers"
ORDER BY "CustomerId";
