using Charamaker2.Character;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// ムービーのための名前空間
/// </summary>
namespace Charamaker2.movie
{
    /// <summary>
    /// 使わないでくれー！見ないでくれー！
    /// </summary>
    public  class Movie
    {
        hyojiman hyojiman,moto;

       
        public float tcamx, tcamy;
        public float timer = -1;

       public  Dictionary<float ,string> section = new Dictionary< float,string> { {0,"end" } };
        public Dictionary<string, character> charas = new Dictionary<string, character>();
        public Dictionary<string, effectchara> effects = new Dictionary<string, effectchara>();
        public bool ended = true;
        public string nowsec;
        public bool changed;
        Delegate dell;
        public Movie(Delegate deligate) 
        {
            dell = deligate;

        }
        public void frame() 
        {
            timer += 1;
            foreach (var a in charas) a.Value.frame();
            hyojiman.hyoji();
            getsection();

            //Console.WriteLine(nowsec + " sad");

            var L = (ScriptRunner<object>)dell;
            L(globals: this);

           // foreach(var a in sousas){ Console.WriteLine(a.Value.GetType().ToString() + " xy:" + a.Value.se.c.x + " " + a.Value.se.c.y); }
          
            if (nowsec == "end") end();
        }
        public void start(hyojiman h) 
        {
            hyojiman = new hyojiman(h);
            moto = h;


            hyojiman.reset();
           
           
            timer = -1;
            frame();
            ended = false;

        }
        public void end() 
        {
            if (ended == false&&timer>0)
            {
                ended = true;
                var temtempic = new List<drawings>();
                foreach (var a in hyojiman.pics)
                    temtempic.Add(a);
                foreach (var a in temtempic)
                    hyojiman.removepicture(a);

         
                /////
                var temtemhaikeipic = new List<haikeidraws>();
                foreach (var a in hyojiman.haikeipics)
                    temtemhaikeipic.Add(a);
                foreach (var a in temtemhaikeipic)
                    hyojiman.removehaikeipicture(a);

                

             

                hyojiman.camx = tcamx;
                hyojiman.camy = tcamy;
            }
        }
        public void picsdakekaesu() 
        {
            foreach (var a in moto.pics)
                hyojiman.addpicture(a);
            foreach (var a in moto.effects) 
            {
                a.resethyoji(hyojiman);
            }

        }
        public void haikeipicsdakekaesu()
        {
            foreach (var a in moto.haikeipics)
                hyojiman.addhaikeipicture(a);
            

        }
        public void getsection() 
        {
            float temp = -1;
            string res = null;
            foreach (var a in section) 
            {
                if (a.Key < timer&&a.Key>temp) 
                {
                    temp = a.Key;
                    res = a.Value;
                }
            }
            changed=nowsec != res;
            nowsec = res;
            
        }
        public bool SCT(string section) 
        {
            return nowsec == section && changed;
        }
        public bool SSS(string section) 
        {
            return nowsec == section && !changed;
        }
        public bool SS(string section) 
        {
            return nowsec == section;
        }
        public float waittimer = 0;
        public void wait(float t) 
        {
            if (waittimer < t)
            {
                timer -= 1;
                waittimer += 1;
            }
        }
      
    }
  

}
