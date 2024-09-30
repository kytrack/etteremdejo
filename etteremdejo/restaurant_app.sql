-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2024. Sze 30. 09:51
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `restaurant_app`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `foods`
--

CREATE TABLE `foods` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` text DEFAULT NULL,
  `price` decimal(8,2) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `image_url` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `foods`
--

INSERT INTO `foods` (`id`, `name`, `description`, `price`, `created_at`, `image_url`) VALUES
(1, 'Pizza Margherita', 'Classic Italian pizza with tomato, mozzarella, and basil.', 8.99, '2024-09-26 08:25:03', 'https://i.imgur.com/LZt0Q7O.jpeg'),
(2, 'Caesar Salad', 'Fresh romaine lettuce with Caesar dressing, croutons, and Parmesan.', 5.49, '2024-09-26 08:25:03', 'https://i.imgur.com/9zFAK1Z.jpeg'),
(3, 'Cheeseburger', 'Grilled beef patty with cheese, lettuce, tomato, and onion.', 7.99, '2024-09-26 08:25:03', 'https://magazin.klarstein.hu/wp-content/uploads/2022/03/KS_Magazine_0322_Cheeseburger_1300x1300px.jpg'),
(4, 'Spaghetti Bolognese', 'Pasta with a rich tomato and beef sauce.', 10.50, '2024-09-26 08:25:03', 'https://www.slimmingeats.com/blog/wp-content/uploads/2010/04/spaghetti-bolognese-36.jpg'),
(5, 'Sushi Platter', 'Assorted sushi rolls with soy sauce and wasabi.', 15.00, '2024-09-26 08:25:03', 'https://img.freepik.com/premium-photo/mouthwatering-sushi-platter-with-assortment-nigiri-maki-sashimi_953733-823.jpg'),
(6, 'Grilled Chicken Sandwich', 'Grilled chicken breast with lettuce, tomato, and mayo.', 6.75, '2024-09-26 08:25:03', 'https://www.licious.in/blog/wp-content/uploads/2020/12/Grilled-Chicken-Sandwich.jpg'),
(7, 'Chocolate Cake', 'Moist chocolate cake with layers of fudge frosting.', 4.25, '2024-09-26 08:25:03', 'https://www.allrecipes.com/thmb/ZENL7sE25q2AFXdqw7wfPLHCzNQ=/0x512/filters:no_upscale():max_bytes(150000):strip_icc()/708879-One-Bowl-Chocolate-Cake-III-Dianne-4x3-0b686cb5d1d647cabefd86545b1bccdf.jpg'),
(8, 'Lobster Bisque', 'Creamy lobster soup with a hint of sherry.', 12.99, '2024-09-26 08:25:03', 'https://www.howsweeteats.com/wp-content/uploads/2018/11/lobster-bisque-I-howsweeteats.com-6.jpg'),
(9, 'French Fries', 'Crispy golden fries served with ketchup.', 2.99, '2024-09-26 08:25:03', 'https://static.toiimg.com/thumb/54659021.cms?imgsize=275086&width=800&height=800'),
(10, 'Tacos al Pastor', 'Marinated pork tacos with pineapple, cilantro, and onion.', 9.25, '2024-09-26 08:25:03', 'https://www.seriouseats.com/thmb/4kbwN13BlZnZ3EywrtG2AzCKuYs=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/20210712-tacos-al-pastor-melissa-hom-seriouseats-37-f72cdd02c9574bceb1eef1c8a23b76ed.jpg');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `food_ingredients`
--

CREATE TABLE `food_ingredients` (
  `food_id` int(11) NOT NULL,
  `ingredient_id` int(11) NOT NULL,
  `quantity_required` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `food_ingredients`
--

INSERT INTO `food_ingredients` (`food_id`, `ingredient_id`, `quantity_required`) VALUES
(1, 1, 0.50),
(1, 2, 0.30),
(1, 3, 0.10),
(2, 4, 0.50),
(2, 5, 0.25),
(2, 6, 0.10),
(2, 7, 0.10),
(3, 8, 1.00),
(3, 9, 0.20),
(3, 10, 0.10),
(3, 11, 0.05),
(4, 12, 1.00),
(4, 13, 0.50),
(5, 14, 0.20),
(5, 15, 0.05),
(6, 16, 0.25),
(6, 17, 0.05),
(7, 18, 0.30),
(7, 19, 0.10),
(8, 20, 0.50),
(8, 21, 0.10),
(9, 22, 0.25),
(9, 23, 0.05),
(10, 24, 0.25),
(10, 25, 0.10),
(10, 26, 0.05);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `ingredients`
--

CREATE TABLE `ingredients` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `quantity_in_stock` decimal(10,2) NOT NULL,
  `unit` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `ingredients`
--

INSERT INTO `ingredients` (`id`, `name`, `quantity_in_stock`, `unit`) VALUES
(1, 'Tomato', 100.00, 'kg'),
(2, 'Mozzarella Cheese', 50.00, 'kg'),
(3, 'Basil', 20.00, 'kg'),
(4, 'Romaine Lettuce', 30.00, 'kg'),
(5, 'Caesar Dressing', 15.00, 'liters'),
(6, 'Croutons', 25.00, 'kg'),
(7, 'Parmesan Cheese', 10.00, 'kg'),
(8, 'Beef Patty', 60.00, 'pieces'),
(9, 'Lettuce', 40.00, 'kg'),
(10, 'Tomato Slices', 30.00, 'pieces'),
(11, 'Onion', 25.00, 'kg'),
(12, 'Pasta', 50.00, 'kg'),
(13, 'Ground Beef', 70.00, 'kg'),
(14, 'Soy Sauce', 20.00, 'liters'),
(15, 'Wasabi', 10.00, 'kg'),
(16, 'Chicken Breast', 55.00, 'kg'),
(17, 'Mayo', 10.00, 'liters'),
(18, 'Chocolate', 15.00, 'kg'),
(19, 'Fudge Frosting', 12.00, 'liters'),
(20, 'Lobster', 20.00, 'kg'),
(21, 'Sherry', 5.00, 'liters'),
(22, 'Potato', 100.00, 'kg'),
(23, 'Ketchup', 10.00, 'liters'),
(24, 'Pork', 50.00, 'kg'),
(25, 'Pineapple', 25.00, 'kg'),
(26, 'Cilantro', 15.00, 'kg'),
(27, 'Taco Shells', 40.00, 'pieces');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `orders`
--

CREATE TABLE `orders` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `order_date` timestamp NOT NULL DEFAULT current_timestamp(),
  `status` enum('pending','completed','cancelled') DEFAULT 'pending'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `orders`
--

INSERT INTO `orders` (`id`, `user_id`, `order_date`, `status`) VALUES
(1, 3, '2024-09-30 00:55:15', 'pending'),
(2, 2, '2024-09-30 06:05:32', 'pending'),
(3, 3, '2024-09-30 07:45:12', 'pending');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `order_items`
--

CREATE TABLE `order_items` (
  `id` int(11) NOT NULL,
  `order_id` int(11) NOT NULL,
  `food_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `total_price` decimal(8,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `order_items`
--

INSERT INTO `order_items` (`id`, `order_id`, `food_id`, `quantity`, `total_price`) VALUES
(1, 1, 1, 1, 8.99),
(2, 2, 1, 3, 26.97),
(3, 2, 1, 3, 26.97),
(4, 2, 1, 3, 26.97),
(5, 2, 2, 1, 5.49),
(6, 3, 1, 1, 8.99);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `role` enum('user','admin') DEFAULT 'user',
  `registration_date` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `users`
--

INSERT INTO `users` (`id`, `username`, `email`, `password_hash`, `role`, `registration_date`) VALUES
(1, 'Imre Krsztián', 'asdasd', '94bdae107d66f43c5b7a58fd2b9b71f9185b992e6a9ef14956349d4ab790c993', 'user', '2024-09-20 09:48:06'),
(2, 'Buzi', 'Rudi@gmail.com', '688787d8ff144c502c7f5cffaafe2cc588d86079f9de88304c26b0cb99ce91c6', 'admin', '2024-09-26 07:51:33'),
(3, 'a', 'a@g.com', 'ca978112ca1bbdcafac231b39a23dc4da786eff8147c4e72b9807785afee48bb', 'user', '2024-09-26 09:49:01');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `foods`
--
ALTER TABLE `foods`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `food_ingredients`
--
ALTER TABLE `food_ingredients`
  ADD PRIMARY KEY (`food_id`,`ingredient_id`),
  ADD KEY `ingredient_id` (`ingredient_id`);

--
-- A tábla indexei `ingredients`
--
ALTER TABLE `ingredients`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`);

--
-- A tábla indexei `order_items`
--
ALTER TABLE `order_items`
  ADD PRIMARY KEY (`id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `food_id` (`food_id`);

--
-- A tábla indexei `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `email` (`email`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `foods`
--
ALTER TABLE `foods`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT a táblához `ingredients`
--
ALTER TABLE `ingredients`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=41;

--
-- AUTO_INCREMENT a táblához `orders`
--
ALTER TABLE `orders`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `order_items`
--
ALTER TABLE `order_items`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `food_ingredients`
--
ALTER TABLE `food_ingredients`
  ADD CONSTRAINT `food_ingredients_ibfk_1` FOREIGN KEY (`food_id`) REFERENCES `foods` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `food_ingredients_ibfk_2` FOREIGN KEY (`ingredient_id`) REFERENCES `ingredients` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `order_items`
--
ALTER TABLE `order_items`
  ADD CONSTRAINT `order_items_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `order_items_ibfk_2` FOREIGN KEY (`food_id`) REFERENCES `foods` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
