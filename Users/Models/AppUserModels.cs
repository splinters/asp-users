using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Users.Models
{

    public enum Cities
    {
        [Display(Name = "Лондон")]
        LONDON,

        [Display(Name = "Париж")]
        PARIS,

        [Display(Name = "Київ")]
        KYIV
    }

    public enum Status
    {
        [Display(Name = "Админ")]
        admin,

        [Display(Name = "Перегляд")]
        view,

        [Display(Name = "Редагування")]
        edit
    }


       public enum Countries
    {
        [Display(Name = "Не указано")]
        NONE,

        [Display(Name = "Англия")]
        ENG,

        [Display(Name = "Франция")]
        FRA,

        [Display(Name = "Україна")]
        UA
    }

    public class AppUser : IdentityUser
    {
        // Здесь будут указываться дополнительные свойства
        public Cities City { get; set; }
        public Status Status { get; set; }
        public Countries Country { get; set; }

        public void SetCountryFromCity(Cities city)
        {
            switch (city)
            {
                case Cities.LONDON:
                    Country = Countries.ENG;
                    break;
                case Cities.PARIS:
                    Country = Countries.FRA;
                    break;
                case Cities.KYIV:
                    Country = Countries.UA;
                    break;
                default:
                    Country = Countries.NONE;
                    break;
            }
        }
    }
}