-- Delete database data query

-- Delete all data in the correct order to respect foreign key constraints
-- Start with dependent tables first, then parent tables

-- 1. Delete all cart items first
DELETE FROM CartItems;

-- 2. Delete all order items 
DELETE FROM OrderItems;

-- 3. Delete all library entries
DELETE FROM LibraryEntries;

-- 4. Delete all game images
DELETE FROM GameImages;

-- 5. Delete all shopping carts
DELETE FROM ShoppingCarts;

-- 6. Delete all orders
DELETE FROM Orders;

-- 7. Delete game-genre relationships (if you have a junction table)
-- DELETE FROM GameGenres; -- Uncomment if this table exists

-- 8. Delete game-platform relationships (if you have a junction table)  
-- DELETE FROM GamePlatforms; -- Uncomment if this table exists

-- 9. Delete game-developer relationships (if you have a junction table)
-- DELETE FROM GameDevelopers; -- Uncomment if this table exists

-- 10. Delete all games
DELETE FROM Games;

-- 11. Delete all users (except maybe keep admin)
DELETE FROM Users WHERE Username != 'admin'; -- Keep admin user
-- OR DELETE FROM Users; -- Delete all users including admin

-- 12. Delete all companies
DELETE FROM Companies;

-- 13. Delete all genres
DELETE FROM Genres;

-- 14. Delete all platforms
DELETE FROM Platforms;

-- 15. Delete all statuses
DELETE FROM Statuses;

-- 16. Delete all roles (be careful - you might want to keep these)
-- DELETE FROM Roles; -- Only if you want to completely reset

-- Reset identity columns back to 1 (optional)
DBCC CHECKIDENT('CartItems', RESEED, 0);
DBCC CHECKIDENT('OrderItems', RESEED, 0);
DBCC CHECKIDENT('LibraryEntries', RESEED, 0);
DBCC CHECKIDENT('GameImages', RESEED, 0);
DBCC CHECKIDENT('ShoppingCarts', RESEED, 0);
DBCC CHECKIDENT('Orders', RESEED, 0);
DBCC CHECKIDENT('Games', RESEED, 0);
DBCC CHECKIDENT('Users', RESEED, 0);
DBCC CHECKIDENT('Companies', RESEED, 0);
DBCC CHECKIDENT('Genres', RESEED, 0);
DBCC CHECKIDENT('Platforms', RESEED, 0);
DBCC CHECKIDENT('Statuses', RESEED, 0);
DBCC CHECKIDENT('Roles', RESEED, 0);