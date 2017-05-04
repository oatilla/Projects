using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{    
    public class UstMenu
    {
        AnnaSarahEntities newEntity = new AnnaSarahEntities();

        public string getNut()
        {
            List<tblCategory> getNuts = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == 177 select c).ToList();
            if (getNuts.Count > 0)
            {
                tblCategory getNut = getNuts.First();
                return getNut.Image;
            }
            else
            {
                return "";
            }
        }


        public string getDriedFruits()
        {
            List<tblCategory> getDriedFruits = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == 178 select c).ToList();
            if (getDriedFruits.Count > 0)
            {
                tblCategory getDriedFruit = getDriedFruits.First();
                return getDriedFruit.Image;
            }
            else
            {
                return "";
            }
        }


        public string getSeeds_Grains()
        {
            List<tblCategory> getSeeds_Grains = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == 179 select c).ToList();
            if (getSeeds_Grains.Count > 0)
            {
                tblCategory getSeeds_Grain = getSeeds_Grains.First();
                return getSeeds_Grain.Image;
            }
            else
            {
                return "";
            }
        }


        public string getSnacks()
        {
            List<tblCategory> getSnacks = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == 180 select c).ToList();
            if (getSnacks.Count > 0)
            {
                tblCategory getSnack = getSnacks.First();
                return getSnack.Image;
            }
            else
            {
                return "";
            }
        }


        public string getGifts()
        {
            List<tblCategory> getGifts = (List<tblCategory>)(from c in newEntity.tblCategories where c.Cid == 181 select c).ToList();
            if (getGifts.Count > 0)
            {
                tblCategory getGift = getGifts.First();
                return getGift.Image;
            }
            else
            {
                return "";
            }
        }





   
        //List<tblCategory> getSeeds_Grains = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 179 select c).ToList();
        //            if (getSeeds_Grains.Count > 0)
        //            {
        //                tblCategory getSeeds_Grain = getSeeds_Grains.First();
        //ViewBag.Seeds1 = getSeeds_Grain.Image;
        //            }
        //            List<tblCategory> getSnacks = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 180 select c).ToList();
        //            if (getSnacks.Count > 0)
        //            {
        //                tblCategory getSnack = getSnacks.First();
        //ViewBag.Snacks1 = getSnack.Image;
        //            }
        //            List<tblCategory> getGifts = (List<tblCategory>)(from c in getCategoryAllList where c.Cid == 181 select c).ToList();
        //            if (getGifts.Count > 0)
        //            {
        //                tblCategory getGift = getGifts.First();
        //ViewBag.Gifts1 = getGift.Image;
        //            }
    }
}