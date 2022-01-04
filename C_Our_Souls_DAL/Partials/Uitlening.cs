using C_Our_Souls_DAL.BasisModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Uitlening
    {
        public double BoeteBedrag(DateTime date = new DateTime())
        {
            if (date.Year == 1)
            {
                date = DateTime.Now;
            }
            //get
            //{
                int dagenTeveel = 0;
                int wekenTeveel = 0;
                double boete = 0;

                if (((UitgeleendOp != null && UitlenenTot != null && Binnengebracht == null && UitlenenTot < date) || Binnengebracht > UitlenenTot) && BoeteBetaald==null)
                {//Er is een boete
                 //Hoeveel dagen te laat
                    if (Binnengebracht == null)
                    {
                        dagenTeveel = (int)(date - (DateTime)UitlenenTot).TotalDays;
                    }
                    else
                    {
                        dagenTeveel = (int)((DateTime)Binnengebracht.Value - (DateTime)UitlenenTot).TotalDays;
                    }

                    //Berekent aantal weken ==> decimaal getal mogelijk
                    Double D = dagenTeveel / 7;
                    //Geeft het geheel getal voor de comma terug
                    wekenTeveel = (int)Math.Truncate(D);

                    //Voor elke week extra, na de eerste week, komt er 50 cent bij de boete
                    for (int i = 0; i < wekenTeveel; i++)
                    {
                        if (i == 0)
                        {
                            boete += 1;
                        }
                        else
                        {
                            boete += 0.50;
                        }
                    }
                }
                return boete;
            //}
        }

        public double Boete
        {
            get
            {
                return BoeteBedrag();
            }
        }

        public DateTime? UitlenenTot
        {
            get
            {
                int week = 7;//week is 7 dagen
                int member = 4 * week;//leden mogen 4 weken ontlenen
                int nonMember = 3 * week;//niet-leden mogen 3 weken ontlenen

                if (UitgeleendOp != null)
                {
                    if (Gebruiker.IsLid)
                    {//Gebruiker is momenteel lid
                        return UitgeleendOp.Value.AddDays(member);
                    }
                    else
                    {
                        return UitgeleendOp.Value.AddDays(nonMember);
                    }
                }
                return null;
            }
        }
    }
}