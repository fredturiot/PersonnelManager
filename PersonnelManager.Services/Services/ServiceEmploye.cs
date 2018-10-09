using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PersonnelManager.Business.Exceptions;
using PersonnelManager.Dal.Data;
using PersonnelManager.Dal.Entites;

namespace PersonnelManager.Business.Services
{
    public class ServiceEmploye
    {
        private readonly IDataEmploye dataEmploye;

        public ServiceEmploye(IDataEmploye dataEmploye)
        {
            this.dataEmploye = dataEmploye;
        }

        public Ouvrier GetOuvrier(string nom)
        {
            return this.dataEmploye.GetListeOuvriers().FirstOrDefault(x => x.Nom == nom);
        }

        public Ouvrier GetOuvrier(int idOuvrier)
        {
            return this.dataEmploye.GetOuvrier(idOuvrier);
        }

        public Cadre GetCadre(int idCadre)
        {
            return this.dataEmploye.GetCadre(idCadre);
        }

        public void EnregistrerCadre(Cadre cadre)
        {
            if (cadre == null)
            {
                throw new InvalidOperationException();
            }

            if (cadre.DateEmbauche.Year <= 1920)
            {
                throw new BusinessException("La date d'embauche doit être > 1920");
            }
          
            if (cadre.DateEmbauche > DateTime.Today.AddMonths(3))
            {
                throw new BusinessException("La date d'embauche doit être anterieur à aujourd'hui plus 3 mois");
            }

            if (cadre.SalaireMensuel <= 0)
            {
                throw new BusinessException("Le salaire doit etre positif et > à 0");
            }

            Regex special = new Regex(@"[^a-zA-ZâêôûéÇàèùîØàáâãäåæçèéêëìíîïðñòóôõöøùúûüýÿœ\-\s]");
            int badCharCheckNom = special.Match(cadre.Nom).Length;
            int badCharCheckPrenom = special.Match(cadre.Prenom).Length;
            if (badCharCheckNom > 0 || badCharCheckPrenom > 0)
            {
                throw new BusinessException("Le Nom et Prenom ne peuvent pas contenir de Caracteres Speciaux ou de chiffre");
            }

            this.dataEmploye.EnregistrerCadre(cadre);
        }

        public void EnregistrerOuvrier(Ouvrier ouvrier)
        {
            if (ouvrier == null)
            {
                throw new InvalidOperationException();
            }

            if (ouvrier.TauxHoraire <= 0)
            {
                throw new BusinessException("Le Taux Horaire doit etre positif et > à 0");
            }

            if (ouvrier.DateEmbauche.Year <= 1920)
            {
                throw new BusinessException("La date d'embauche doit être > 1920");
            }

            if (ouvrier.DateEmbauche > DateTime.Today.AddMonths(3))
            {
                throw new BusinessException("La date d'embauche doit être anterieur à aujourd'hui plus 3 mois");
            }

            Regex special = new Regex(@"[^a-zA-ZâêôûéÇàèùîØàáâãäåæçèéêëìíîïðñòóôõöøùúûüýÿœ\-\s]");
            int badCharCheckNom = special.Match(ouvrier.Nom).Length;
            int badCharCheckPrenom = special.Match(ouvrier.Prenom).Length;
            if (badCharCheckNom > 0 || badCharCheckPrenom > 0)
            {
                throw new BusinessException("Le Nom et Prenom ne peuvent pas contenir de Caracteres Speciaux ou de chiffre");
            }

            this.dataEmploye.EnregistrerOuvrier(ouvrier);
        }

        public IEnumerable<Employe> GetListeEmployes()
        {
            var listeEmployes = new List<Employe>();
            listeEmployes.AddRange(this.dataEmploye.GetListeOuvriers());
            listeEmployes.AddRange(this.dataEmploye.GetListeCadres());

            return listeEmployes.OrderBy(x => x.Nom).ThenBy(x => x.Prenom);
        }

        public IEnumerable<SalaireOuvrier> GetSalaireOuvrier(int idOuvrier, DateTime mois)
        {
            return null;
        }

        public IEnumerable<Salaire> GetSalaireCadre(int idCadre, DateTime mois)
        {
            return null;
        }
    }
}
