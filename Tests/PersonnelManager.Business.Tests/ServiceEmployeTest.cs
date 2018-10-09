using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonnelManager.Business.Exceptions;
using PersonnelManager.Business.Services;
using PersonnelManager.Dal.Data;
using PersonnelManager.Dal.Entites;

namespace PersonnelManager.Business.Tests
{
    [TestClass]
    public class ServiceEmployeTest
    {
        [TestMethod]
        public void ValiderNomEtPrenomRequis()
        {
            Assert.IsTrue(
                TestsHelper.HasAttribute<Employe, RequiredAttribute>(x => x.Nom));
            Assert.IsTrue(
                TestsHelper.HasAttribute<Employe, RequiredAttribute>(x => x.Prenom));
        }

        [TestMethod]
        public void DateEmbaucheOuvrierPosterieureA1920()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var ouvrier = new Ouvrier
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(1920, 12, 31),
                TauxHoraire = 12
            };

            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });

            Assert.AreEqual("La date d'embauche doit être > 1920", 
                exception.Message);
        }

        [TestMethod]
        public void DateEmbaucheCadrePosterieureA1920()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerCadre(It.IsAny<Cadre>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var cadre = new Cadre
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(1920, 12, 31),
                SalaireMensuel = 1500
            };

            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerCadre(cadre);
            });

            Assert.AreEqual("La date d'embauche doit être > 1920",
                exception.Message);

            
        }

        [TestMethod]
        public void DateEmbaucheCadreAnterieureAujourdhuiPlus3Mois()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerCadre(It.IsAny<Cadre>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var cadre = new Cadre
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(2020, 12, 31),
                SalaireMensuel = 1500
            };

            var exception3Mois = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerCadre(cadre);
            });

            Assert.AreEqual("La date d'embauche doit être anterieur à aujourd'hui plus 3 mois",
                exception3Mois.Message);
        }

        [TestMethod]
        public void DateEmbaucheOuvrierAnterieureAujourdhuiPlus3Mois()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var ouvrier = new Ouvrier
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(2220, 12, 31),
                TauxHoraire = 12
            };

            var exception3Mois = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });

            Assert.AreEqual("La date d'embauche doit être anterieur à aujourd'hui plus 3 mois",
                exception3Mois.Message);
        }

        [TestMethod]
        public void SalaireCadrePositif()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var cadre = new Cadre
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(2017, 12, 31),
                SalaireMensuel = -1500
            };

            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerCadre(cadre);
            });

            Assert.AreEqual("Le salaire doit etre positif et > à 0",
                exception.Message);
        }

        [TestMethod]
        public void TauxHoraireOuvrierPositif()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var ouvrier = new Ouvrier
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(1920, 12, 31),
                TauxHoraire = -12
            };

            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });

            Assert.AreEqual("Le Taux Horaire doit etre positif et > à 0",
                exception.Message);
        }

        [TestMethod]
        public void InterdireCaracteresSpeciauxDansNomEtPrenomCadre()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var cadre = new Cadre
            {
                Nom = "Du&ont",
                Prenom = "Gér@rd",
                DateEmbauche = new DateTime(2017, 12, 31),
                SalaireMensuel = 1500
            };

            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerCadre(cadre);
            });

            Assert.AreEqual("Le Nom et Prenom ne peuvent pas contenir de Caracteres Speciaux ou de chiffre",
                exception.Message);
        }

        [TestMethod]
        public void InterdireCaracteresSpeciauxDansNomEtPrenomOuvrier()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var ouvrier = new Ouvrier
            {
                Nom = "Du-ont",
                Prenom = "Gér-r@d",
                DateEmbauche = new DateTime(2017, 12, 31),
                TauxHoraire = 15
            };

            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });

            Assert.AreEqual("Le Nom et Prenom ne peuvent pas contenir de Caracteres Speciaux ou de chiffre",
                exception.Message);
        }

        [TestMethod]
        public void OuvrierEstNonNull()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            Assert.ThrowsException<InvalidOperationException>(
                () => serviceEmploye.EnregistrerOuvrier(null));
        }

        [TestMethod]
        public void CadreEstNonNull()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            Assert.ThrowsException<InvalidOperationException>(
                () => serviceEmploye.EnregistrerCadre(null));
        }
    }
}
