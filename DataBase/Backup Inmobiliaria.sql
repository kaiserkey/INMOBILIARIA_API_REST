CREATE DATABASE  IF NOT EXISTS `inmobiliaria` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_bin */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `inmobiliaria`;
-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: localhost    Database: inmobiliaria
-- ------------------------------------------------------
-- Server version	8.1.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `contrato`
--

LOCK TABLES `contrato` WRITE;
/*!40000 ALTER TABLE `contrato` DISABLE KEYS */;
INSERT INTO `contrato` VALUES (3,2,4,'2023-04-01 00:00:00','2023-04-25 11:36:09',0,50000),(4,2,5,'2023-05-01 00:00:00','2023-06-30 00:00:00',0,60000),(5,3,6,'2023-07-01 00:00:00','2023-08-31 00:00:00',0,80000),(6,4,7,'2023-09-01 00:00:00','2023-10-31 00:00:00',0,50000),(7,5,8,'2023-11-01 00:00:00','2023-12-31 00:00:00',0,30000),(18,1,44,'2023-11-01 22:33:00','2023-12-31 22:33:00',1,70000),(19,2,12,'2023-11-01 22:39:00','2023-12-31 22:39:00',1,150000);
/*!40000 ALTER TABLE `contrato` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `inmueble`
--

LOCK TABLES `inmueble` WRITE;
/*!40000 ALTER TABLE `inmueble` DISABLE KEYS */;
INSERT INTO `inmueble` VALUES (4,'departamento','10.4567,-65.4321',200000,3,'residencial',1,5,'direccion',NULL),(5,'apartamento','12.5678,-67.9012',100000,2,'residencial',1,6,'direccion',NULL),(6,'casa','10.4567,-65.4321',200000,3,'residencial',1,4,'direccion',NULL),(7,'apartamento','12.5678,-67.9012',100000,2,'residencial',1,5,'direccion',NULL),(8,'local comercial','13.2468,-68.1357',800000,2,'comercial',1,4,'direccion',NULL),(11,'casa','54.8442,-63.0834',50000,2,'comercial',1,2,'Direccion Casa','/Uploads/casa3.png'),(12,'casa','54.8442,-63.0834',150000,4,'residencial',1,2,'Direccion Casa','/Uploads/casa2.png'),(44,'Casa','13.7567,-63.4321',70000,2,'residencial',1,2,'Direccion Casa','/Uploads/casa1.png'),(52,'casa',NULL,450000,2,'residencial',0,2,'Crea Inmueble','/Uploads/foto_defecto_52.jpg'),(53,'casa',NULL,450000,2,'residencial',0,2,'Pruebas','/Uploads/foto_53.jpg');
/*!40000 ALTER TABLE `inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `inquilino`
--

LOCK TABLES `inquilino` WRITE;
/*!40000 ALTER TABLE `inquilino` DISABLE KEYS */;
INSERT INTO `inquilino` VALUES (1,'Juan','P?rez','juanperez@mail.com','12345678A','654321987','1990-03-21 05:02:00'),(2,'Mar','Gonz?lez','mariagonzalez@mail.com','87654321B','789456123','1985-11-25 00:00:00'),(3,'Carlos','Ruiz','carlosruiz@mail.com','56789012C','123789456','1995-05-02 00:00:00'),(4,'Ana','Mart?nez','anamartinez@mail.com','34567890D','456123789','1988-07-17 00:00:00'),(5,'Pedro','L?pez','pedrolopez@mail.com','90123456E','987654321','1992-09-29 00:00:00');
/*!40000 ALTER TABLE `inquilino` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `multa`
--

LOCK TABLES `multa` WRITE;
/*!40000 ALTER TABLE `multa` DISABLE KEYS */;
INSERT INTO `multa` VALUES (5,30000,2);
/*!40000 ALTER TABLE `multa` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
INSERT INTO `pago` VALUES (3,'2022-03-01 00:00:00',3,90000,5),(5,'2022-02-01 00:00:00',2,80000,5),(46,'2023-04-12 13:43:00',234,30000,3),(47,'2023-04-12 13:59:00',14,30000,6),(236,'2023-04-12 10:30:00',222,10000,3),(238,'2023-04-25 00:32:00',234,80000,5),(239,'2023-11-07 22:35:00',1,70000,18),(240,'2023-11-07 22:40:00',1,150000,19);
/*!40000 ALTER TABLE `pago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `propietario`
--

LOCK TABLES `propietario` WRITE;
/*!40000 ALTER TABLE `propietario` DISABLE KEYS */;
INSERT INTO `propietario` VALUES (2,'Laura','Garc','Avenida Europa 245','654987126','1234567891','lauragarcia@mail.com','/Uploads/avatar_propietario_2.jpg','O1KF6KBcYCwEkODmtZ8AyLuROk8TBp13UCxSZoCY5BA='),(3,'Jorge','Mart?nez','Avenida Europa 15','789456123','8765432191','jorgemartinez@mail.com','/Uploads/avatar_4.jpg','O1KF6KBcYCwEkODmtZ8AyLuROk8TBp13UCxSZoCY5BA='),(4,'Ana','Fern?ndez','Calle Real 8','456789123','5678901291','anafernandez@mail.com','/Uploads/avatar_4.jpg','O1KF6KBcYCwEkODmtZ8AyLuROk8TBp13UCxSZoCY5BA='),(5,'Carlos','L?pez','Plaza Mayor 1','123456789','3456789091','carloslopez@mail.com','/Uploads/avatar_4.jpg','O1KF6KBcYCwEkODmtZ8AyLuROk8TBp13UCxSZoCY5BA='),(6,'Mar','S?nchez','Calle Libertad 3','987654321','9012345691','mariasanchez@mail.com','/Uploads/avatar_4.jpg','O1KF6KBcYCwEkODmtZ8AyLuROk8TBp13UCxSZoCY5BA=');
/*!40000 ALTER TABLE `propietario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (3,'Fernando Daniel','Gonzalez','/Uploads/avatar_3.jpeg','yr1ZB3YxgrIXmv1YCasNyqxwF9MXCI/9anVYVHn4T5w=','kaiserkey2@gmail.com',1,'123456789','2657534231'),(4,'Jose','Vizcay','/Uploads/avatar_4.jpg','yr1ZB3YxgrIXmv1YCasNyqxwF9MXCI/9anVYVHn4T5w=','josevizcay@gmail.com',2,'123456783','2657544323');
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-11-11 12:55:26
