-- MySQL dump 10.13  Distrib 8.0.31, for macos12 (x86_64)
--
-- Host: localhost    Database: kompaniondb
-- ------------------------------------------------------
-- Server version	8.0.31

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
-- Table structure for table `UserBasic`
--

DROP TABLE IF EXISTS `UserBasic`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserBasic` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(45) NOT NULL,
  `CreateDate` datetime NOT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  `Status` int NOT NULL,
  `UserType` int NOT NULL DEFAULT '2',
  `PasswordHash` varbinary(64) NOT NULL,
  `PasswordSalt` varbinary(128) NOT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `UserID_UNIQUE` (`UserId`),
  UNIQUE KEY `Username_UNIQUE` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserBasic`
--

LOCK TABLES `UserBasic` WRITE;
/*!40000 ALTER TABLE `UserBasic` DISABLE KEYS */;
INSERT INTO `UserBasic` VALUES (1,'Admin','2022-12-04 15:11:41',NULL,1,2,_binary '\�j���·)\�~�W;�s&N�����[ŀ���\Z�p�\�b:S�\�\��\�\�\�^�,%Zn�\�w\�',_binary '\�\�wRj\"\�Й@C�4�|ԒCeΠ5�L�&׽���\�\Z�$\�f�S��b\�s9q�sR�c�\�t���R;:��ޠO�\�w\�n�uo\�n��q\��\���f� �\�5=\�T�\�W;A\�-�q�}�\�µK�y\�'),(2,'Baycosinus','2022-12-04 18:23:37',NULL,1,2,_binary 'ܦ\�\'��\�E�>�%`\"�\�]s�\�G�\\\�}\�6��2����\�\�=��h,��\��-ŷ�\��UO�',_binary 'm�\�[\"a�uϋ��\��\�%���@�)^\�\�Lꄵu\�%\0ZYLoD=��\�^�\�\�\�G�LcWakd�x�t����\� \rl�K�ː�\�o\�y3\\)�\�\�y\�R_�>�\0YŦSVE�\��z�+Xd\�+�\'f'),(3,'Baycosinus2','2022-12-04 18:24:28',NULL,1,2,_binary 'i�%ō:�\n\���\�\r\�n~vN��\�g^\�\�\�*�TsD�ą^�� 7g�ڹ0��\\^�+m�\�Rz\�\��',_binary 'HÎҏ\�\�$t\Z}�zoz9\r>7\�\�\�\�g\�y�.�,�P\�X���J\�\']\�*�.�9�\���=ҏ�)�L˿�_K񱿵�8\�\�9SR�<j^c�x\ru�I�ӭ��\r�q \�\�w/\�園Z,l�\�\�,�U�\�');
/*!40000 ALTER TABLE `UserBasic` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-12-05 21:06:41
