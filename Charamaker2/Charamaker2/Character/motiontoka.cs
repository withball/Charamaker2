﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charamaker2.Character
{
    [Serializable]
    public class motion
    {
        public List<moveman> moves = new List<moveman>();
        protected int idx = 0, sidx = 0;
        public float sp = 1;
        public bool loop = false;
        public bool owari { get { return moves.Count <= sidx; } }
        public motion() { }
        public motion(moveman mv) { addmoves(mv); }
        public motion(motion m)
        {
            idx = m.idx;
            sidx = m.sidx;
            loop = m.loop;
            foreach (var a in m.moves)
            {
                var t = a.GetType();
                moves.Add((moveman)Activator.CreateInstance(t, a));
            }
        }

        public void start(character c)
        {

            idx = 0;
            sidx = 0;
            if (!owari) moves[idx].start(c);
        }
        public void frame(character c,float cl=1)
        {
            if (sp <= 0)
            {
                sidx = moves.Count();
                return;
            }
            if (!owari)
            {

                for (int i = sidx; i <= idx && i < moves.Count; i++)
                {


                    if (!moves[i].owari)
                    {
                        moves[i].frame(c, sp*cl);
                    }
                    else if (sidx == i)
                    {
                        sidx++;

                    }
                    if (!moves[i].STOP && i == idx)
                    {
                        idx++;
                        if (idx < moves.Count()) moves[idx].start(c);
                    }
                }
            }
            if (owari && loop) start(c);
        }
        public void addmoves(moveman m)
        {
            moves.Add(m);

        }
        public void addmovesikkini(motion m, int kai = 1)
        {
            for (int i = 0; i < kai; i++)
                foreach (var a in m.moves)
                {
                    var t = a.GetType();
                    moves.Add((moveman)Activator.CreateInstance(t, a));
                }
        }
        public void removemoves(Type t)
        {
            for (int i = moves.Count() - 1; i >= 0; i--)
            {
                if (moves[i].GetType() == t || moves[i].GetType().IsSubclassOf(t)) moves.RemoveAt(i);
            }
        }

    }
    [Serializable]
    public class moveman
    {
        public float time;
        protected float timer;
        public bool st;
        public bool owari { get { return timer >= time; } }
        public bool STOP { get { return st && !owari; } }

        public float getnokotime(float sp, float from = -1)
        {
            if (from < 0) from = time;
            var noko = from - timer;
            if (noko < 0) noko = 0;
            if (noko < sp) return noko;
            return sp;
        }


        public moveman(float t, bool stop = false)
        {
            time = t;
            timer = 0;
            st = stop;
        }
        public moveman(moveman m)
        {
            time = m.time;
            timer = m.timer;
            st = m.st;

        }
        public moveman() { }


        virtual public void start(character c)
        {
            timer = 0;
        }
        virtual public void frame(character c, float cl)
        {

            timer += cl;


        }
    }
    [Serializable]
    public class idouman : moveman
    {
        public float vx;
        public float vy;
        public double vrad;
        public List<setu> tag = new List<setu>();

        public idouman(float t, float vvx, float vvy, double vvsita = 0, bool stop = false) : base(t, stop)
        {
            vx = vvx;
            vy = vvy;
            vrad = Math.PI * vvsita / 180;
        }
        public idouman(idouman i) : base(i)
        {
            vx = i.vx;
            vy = i.vy;
            vrad = i.vrad;
            tag = new List<setu>(i.tag);
        }
        public idouman() { }
        public override void start(character c)
        {
            base.start(c);
            tag = c.core.getallsetu();
        }
        public override void frame(character c, float cl)
        {
            float timen = getnokotime(cl);
            c.x += vx * timen;
            c.y += vy * timen;
            c.RAD += vrad * timen;
            foreach (var a in tag)
            {

                a.p.RAD += vrad * timen;

            }

            base.frame(c, cl);
        }

    }
    [Serializable]
    public class zureman : moveman
    {
        public float vx;
        public float vy;


        public zureman(float t, float wariaiw, float wariaih, bool stop = false) : base(t, stop)
        {
            vx = wariaiw;
            vy = wariaih;

        }
        public zureman(zureman i) : base(i)
        {
            vx = i.vx;
            vy = i.vy;

        }
        public zureman() { }
        public override void start(character c)
        {
            base.start(c);

        }
        public override void frame(character c, float cl)
        {
            var timen = getnokotime(cl);

            c.wowidouxy(c.w * vx * timen, c.h * vy * timen);

            base.frame(c, cl);
        }

    }
    [Serializable]
    public class zahyosetman : moveman
    {
        public float x;
        public float y;


        public zahyosetman(float t, float xx, float yy, bool stop = false) : base(t, stop)
        {
            x = xx;
            y = yy;

        }
        public zahyosetman(zahyosetman i) : base(i)
        {
            x = i.x;
            y = i.y;

        }
        public zahyosetman() { }
        public override void start(character c)
        {
            base.start(c);

        }
        public override void frame(character c, float cl)
        {
            base.frame(c, cl);
            c.settxy(x, y);

        }

    }
    [Serializable]
    public class setuidouman : moveman
    {
        public string nm;
        public double vrad;
        public float vdx;
        public float vdy;
        protected List<setu> tag = new List<setu>();
        public setuidouman(float t, string name, double vvsita, float vvdx = 0, float vvdy = 0, bool stop = false) : base(t, stop)
        {
            nm = name;
            vrad = Math.PI * vvsita / 180;
            vdx = vvdx;
            vdy = vvdy;
        }
        public setuidouman(setuidouman s) : base(s)
        {
            nm = s.nm;
            vrad = s.vrad;
            vdx = s.vdx;
            vdy = s.vdy;
            tag = new List<setu>(s.tag);

        }
        public setuidouman() { }
        public override void start(character c)
        {
            base.start(c);
            var temp = c.core.GetSetu(nm);
            if (temp != null)
            {
                tag = temp.getallsetu();
            }
            else
            {
                tag = new List<setu>();
            }

        }
        public override void frame(character c, float cl)
        {

            var t = getnokotime(cl);
            foreach (var a in tag)
            {
                a.p.RAD += vrad * t;

                if (a == c.core.GetSetu(nm))
                {
                    a.dx += vdx * t;
                    a.dy += vdy * t;
                }

            }
            base.frame(c, cl);

        }



    }
    [Serializable]
    public class setumageman : moveman
    {
        public string nm;
        public double radto;
        public double radsp;
        public bool sai;
        protected setu tag;
        protected setu pretag;
        protected List<setu> tags = new List<setu>();
        public setumageman(float t, string name, double sitato, double sitasp, bool saitan = true, bool stop = false) : base(t, stop)
        {
            radto = Math.PI * sitato / 180;
            radsp = Math.PI * sitasp / 180;
            nm = name;
            sai = saitan;
        }
        public setumageman(setumageman s) : base(s)
        {

            radto = s.radto;
            radsp = s.radsp;
            nm = s.nm;
            sai = s.sai;
            tag = s.tag;
            pretag = s.pretag;
            tags = new List<setu>(s.tags);
        }
        public setumageman() { }
        public override void start(character c)
        {

            base.start(c);
            tag = c.core.GetSetu(nm);
            pretag = c.core.Getrootsetu(nm);
            if (tag != null)
            {
                tags = tag.getallsetu();
            }
            else
            {
                tags = new List<setu>();
            }

        }
        public override void frame(character c, float cl)
        {

            var t = getnokotime(cl);

            if (tag != null)
            {
                double rkaku;
                if (pretag != null)
                {
                    rkaku = pretag.p.RAD;
                }
                else
                {
                    rkaku = c.RAD;
                }

                {

                    if (Math.Abs(Math.Atan2(Math.Sin(tag.p.RAD - rkaku - radto), Math.Cos(tag.p.RAD - rkaku - radto))) <= Math.Abs(radsp * 1.1 * t) /*&& (sai||radsp*(tag.p.RAD - rkaku - radto)<=Math.Abs(radsp)*0.01f)*/)
                    {
                        double sp = tag.p.RAD - rkaku - radto;

                        foreach (var a in tags)
                        {
                            a.p.RAD -= sp;

                        }
                    }
                    else
                    {
                        if (sai)
                        {

                            if (Math.Atan2(Math.Sin(tag.p.RAD - rkaku - radto), Math.Cos(tag.p.RAD - rkaku - radto)) < 0)
                            {
                                radsp = Math.Abs(radsp);


                            }
                            else
                            {
                                radsp = -Math.Abs(radsp);



                            }
                        }
                        foreach (var a in tags)
                        {
                            a.p.RAD += radsp * t;

                        }
                    }


                }
            }
            base.frame(c, cl);

        }



    }
    [Serializable]
    public class radtoman : moveman
    {
        public double radto;
        public double radsp;
        public string nm;
        protected setu tag;
        protected List<setu> tags;
        public bool sai;
        public radtoman(float t, string name, double sitato, double sitasp, bool saitan = true, bool stop = false) : base(t, stop)
        {
            radto = Math.PI * sitato / 180;
            radsp = Math.PI * sitasp / 180;
            sai = saitan;
            nm = name;
        }
        public radtoman(radtoman r) : base(r)
        {
            radto = r.radto;
            radsp = r.radsp;
            sai = r.sai;
            nm = r.nm;
            tag = r.tag;
            tags = r.tags;

        }
        public radtoman() { }

        public override void start(character c)
        {
            base.start(c);
            tag = c.core.GetSetu(nm);
            if (tag != null)
            {
                tags = tag.getallsetu();
            }
            else
            {
                tags = c.core.getallsetu();
            }

        }
        public override void frame(character c, float cl)
        {

            var t = getnokotime(cl);

            if (tag != null)
            {

                {
                    if (Math.Abs(Math.Atan2(Math.Sin(tag.p.RAD - radto), Math.Cos(tag.p.RAD - radto))) <= Math.Abs(radsp * 1.1 * t) /*&& (sai || radsp * (tag.p.RAD - radto) <= Math.Abs(radsp) * 0.01f)*/)
                    {
                        double sp = tag.p.RAD - radto;

                        foreach (var a in tags)
                        {
                            a.p.RAD -= sp;

                        }
                    }
                    else
                    {
                        if (sai)
                        {
                            if (Math.Atan2(Math.Sin(tag.p.RAD - radto), Math.Cos(tag.p.RAD - radto)) < 0)
                            {
                                radsp = Math.Abs(radsp);


                            }
                            else
                            {
                                radsp = -Math.Abs(radsp);


                            }
                        }
                        foreach (var a in tags)
                        {
                            a.p.RAD += radsp * t;

                        }
                    }


                }


            }
            else if (nm == "")
            {


                if (Math.Abs(Math.Atan2(Math.Sin(c.RAD - radto), Math.Cos(c.RAD - radto))) <= Math.Abs(radsp * 1.1 * t))
                {

                    double sp = c.RAD - radto;
                    c.RAD -= sp;
                    foreach (var a in tags)
                    {
                        a.p.RAD -= sp;

                    }
                }
                else
                {
                    if (sai)
                    {
                        if (Math.Atan2(Math.Sin(c.RAD - radto), Math.Cos(c.RAD - radto)) < 0)
                        {
                            radsp = Math.Abs(radsp);

                        }
                        else
                        {
                            radsp = -Math.Abs(radsp);

                        }

                    }
                    c.RAD += radsp * t;
                    foreach (var a in tags)
                    {
                        a.p.RAD += radsp * t;

                    }

                }





            }
            base.frame(c, cl);
        }

    }

    [Serializable]
    public class texpropman : moveman
    {
        public string nm;
        public int how;
        public float opa;
        public float opasp;
        public bool kzk;
        protected List<setu> tag = new List<setu>();
        public texpropman(float t, string name, int mirhow = 471, float toopa = -1, bool kozokumo = true, bool stop = false) : base(t, stop)
        {
            nm = name;
            how = mirhow;
            opa = toopa;
            kzk = kozokumo;

        }
        public texpropman(texpropman t) : base(t)
        {
            nm = t.nm;
            how = t.how;
            opa = t.opa;
            opasp = t.opasp;
            kzk = t.kzk;
            tag = new List<setu>(t.tag);
        }
        public texpropman() { }

        public override void start(character c)
        {
            base.start(c);
            var t = c.core.GetSetu(nm);

            if (t != null)
            {
                if (kzk)
                {
                    tag = t.getallsetu();
                }
                else
                {
                    tag = new List<setu>();
                    tag.Add(t);
                }
            }
            else if (nm == "" && kzk)
            {
                tag = c.core.getallsetu();
            }
            else
            {
                tag = new List<setu>();
            }
            if (opa >= 0 && tag.Count() > 0)
            {
                if (opa > 1) opa = 1;
                if (t != null) opasp = (opa - t.p.OPA) / time;
                else opasp = (opa - c.core.p.OPA) / time;
            }
            else
            {
                opasp = 0;
            }
            if (how != 471)
            {
                if (tag.Count == 0)
                {
                    if (how == 0)
                    {
                        c.mirror = !c.mirror;
                    }
                    else if (how > 0)
                    {
                        c.mirror = true;
                    }
                    else
                    {
                        c.mirror = false;
                    }
                }
                else
                {
                    foreach (var a in tag)
                    {
                        if (how == 0)
                        {
                            a.p.mir = !a.p.mir;
                        }
                        else if (how > 0)
                        {
                            a.p.mir = false;
                        }
                        else
                        {
                            a.p.mir = true;
                        }

                    }
                }
            }
        }
        public override void frame(character c, float cl)
        {


            var t = getnokotime(cl);
            foreach (var a in tag)
            {
                a.p.OPA += opasp * t;
            }
            base.frame(c, cl);
        }

    }
    [Serializable]
    public class texchangeman : moveman
    {
        public string nm;
        public string tex;
        public texchangeman(string name, string texname) : base(0, false)
        {
            nm = name;
            tex = texname;
        }
        public texchangeman(texchangeman t) : base(t)
        {
            nm = t.nm;
            tex = t.tex;
        }
        public texchangeman() { }
        public override void start(character c)
        {
            base.start(c);
            var t = c.core.GetSetu(nm);
            if (t != null)
            {
                t.p.texname = tex;

            }
        }

    }
    [Serializable]
    public class zchangeman : moveman
    {
        public string nm;
        public float vz;
        public bool kzk;
        protected List<setu> tag = new List<setu>();

        public zchangeman(float t, string name, float ddz, bool kouzokumo = true, bool stop = false) : base(t, stop)
        {
            nm = name;
            vz = ddz;
            kzk = kouzokumo;
        }
        public zchangeman(zchangeman z) : base(z)
        {
            nm = z.nm;
            vz = z.vz;
            kzk = z.kzk;
            tag = new List<setu>(z.tag);
        }
        public zchangeman() { }
        public override void start(character c)
        {
            base.start(c);

            var t = c.core.GetSetu(nm);

            if (t != null && nm != "")
            {
                if (kzk)
                {
                    tag = t.getallsetu();
                }
                else
                {
                    tag = new List<setu>();
                    tag.Add(t);
                }
            }
            else
            {
                if (kzk)
                {
                    tag = c.core.getallsetu();
                }
                else
                {
                    tag = new List<setu>();
                }
            }
            for (int i = 0; i < tag.Count(); i++)
            {

                tag[i].p.z += vz;
            }
            //hyojiman.resetpics();
        }
        public override void frame(character c, float cl)
        {
            base.frame(c, cl);
        }

    }
    [Serializable]
    public class Kzchangeman : moveman
    {
        public string nm,tonm;
        public float vz;
        public bool kzk;
        protected List<setu> tag = new List<setu>();

        public Kzchangeman(string name, string toname,float dz, bool kouzokumo = true) : base(1, false)
        {
            nm = name;
            tonm = toname;
            vz = dz;
            kzk = kouzokumo;
        }
        public Kzchangeman(Kzchangeman z) : base(z)
        {
            nm = z.nm;
            tonm = z.tonm;
            vz = z.vz;
            kzk = z.kzk;
            tag = new List<setu>(z.tag);
        }
        public Kzchangeman() { }
        public override void start(character c)
        {
            base.start(c);

            var t = c.core.GetSetu(nm);

            if (t != null && nm != "")
            {
                float ddd = 0;
                var cn = c.getkijyun();
                var s2 = cn.core.GetSetu(tonm);
                if (s2 != null)
                {
                    ddd = s2.p.z - t.p.z;
                }
                if (kzk)
                {
                    tag = t.getallsetu();
                }
                else
                {
                    tag = new List<setu>();
                    tag.Add(t);
                }
                for (int i = 0; i < tag.Count(); i++)
                {

                    tag[i].p.z += ddd+vz;
                }
            }
            else
            {
              

                
                if (kzk)
                {
                    tag = c.core.getallsetu();
                }
                else
                {
                    tag = new List<setu>();
                }
                for (int i = 0; i < tag.Count(); i++)
                {

                    tag[i].p.z += vz;
                }
                for (int i = 0; i < tag.Count(); i++)
                {
                
                    tag[i].p.z += vz;
                }
            }
           
            //hyojiman.resetpics();
        }

    }
    [Serializable]
    public class sizetokaman : moveman
    {
        public string nm;
        public float w;
        public float h;
        public float tx;
        public float ty;
        protected setu tag;
        public sizetokaman(float t, string name, float vtx, float vty, float vw = 0, float vh = 0, bool stop = false) : base(t, stop)
        {
            nm = name;
            w = vw;
            h = vh;
            tx = vtx;
            ty = vty;
        }
        public sizetokaman(sizetokaman s) : base(s)
        {
            nm = s.nm;
            w = s.w;
            h = s.h;
            tx = s.tx;
            ty = s.ty;
            tag = s.tag;
        }
        public sizetokaman() { }
        public override void start(character c)
        {
            base.start(c);
            tag = c.core.GetSetu(nm);
        }

        public override void frame(character c, float cl)
        {
            var t = getnokotime(cl);
            base.frame(c, cl);
            if (tag != null)
            {

                tag.p.w += w * t;
                tag.p.h += h * t;
                tag.p.tx += tx * t;
                tag.p.ty += ty * t;
            }
            else if (nm == "")
            {
                c.w += w * t;
                c.h += h * t;
                c.tx += tx * t;
                c.ty += ty * t;
            }
        }
    }
    [Serializable]
    public class scalechangeman : moveman
    {
        public float middle;
        public bool kzk;
        public float scalex;
        public float scaley;
        public string nm;
        protected List<float> spw = new List<float>();
        protected List<float> sph = new List<float>();
        protected List<float> sptx = new List<float>();
        protected List<float> spty = new List<float>();
        protected List<float> spdx = new List<float>();
        protected List<float> spdy = new List<float>();


        protected setu tag;
        protected float count;
        protected List<setu> tags = new List<setu>();
        public float kakun { get { return (time - middle) / 2; } }
        public float middlen { get { return (time - middle) / 2 + middle; } }
        public bool kakudai { get { return kakun >= timer; } }
        public bool syukusyo { get { return (middlen < timer) && middle >= 0; } }
        public scalechangeman(float t, float middletime, string name, float changescalex, float changescaley, bool kouzokumo = true, bool stop = false) : base(t, stop)
        {
            middle = middletime;
            time += t;
            if (middle >= 0) time += middle;
            kzk = kouzokumo;
            nm = name;
            scalex = changescalex - 1;
            scaley = changescaley - 1;
        }
        public scalechangeman(scalechangeman s) : base(s)
        {
            middle = s.middle;
            kzk = s.kzk;
            nm = s.nm;
            scalex = s.scalex;
            scaley = s.scaley;
            spw = new List<float>(s.spw);
            sph = new List<float>(s.sph);
            sptx = new List<float>(s.sptx);
            spty = new List<float>(s.spty);
            spdx = new List<float>(s.spdx);
            spdy = new List<float>(s.spdy);

            count = s.count;
            tag = s.tag;
            tags = new List<setu>(s.tags);
        }
        public scalechangeman() { }

        public override void start(character c)
        {
            base.start(c);
            tag = c.core.GetSetu(nm);
            spw = new List<float>();
            sph = new List<float>();
            sptx = new List<float>();
            spty = new List<float>();
            spdx = new List<float>();
            spdy = new List<float>();

            if (tag != null)
            {
                if (kzk)
                {
                    tags = tag.getallsetu();
                    tags.Remove(tag);
                    tags.Insert(0, tag);
                }
                else
                {
                    tags = new List<setu>();
                    tags.Add(tag);
                }
                for (int i = 0; i < tags.Count(); i++)
                {
                    if (middle > 0)
                    {
                        spw.Add(tags[i].p.w * scalex / ((time - middle) / 2));
                        sph.Add(tags[i].p.h * scaley / ((time - middle) / 2));
                        sptx.Add(tags[i].p.tx * scalex / ((time - middle) / 2));
                        spty.Add(tags[i].p.ty * scaley / ((time - middle) / 2));
                        spdx.Add(tags[i].dx * scalex / ((time - middle) / 2));
                        spdy.Add(tags[i].dy * scaley / ((time - middle) / 2));
                    }
                    else
                    {
                        spw.Add(tags[i].p.w * scalex / ((time) / 2));
                        sph.Add(tags[i].p.h * scaley / ((time) / 2));
                        sptx.Add(tags[i].p.tx * scalex / ((time) / 2));
                        spty.Add(tags[i].p.ty * scaley / ((time) / 2));
                        spdx.Add(tags[i].dx * scalex / ((time) / 2));
                        spdy.Add(tags[i].dy * scaley / ((time) / 2));
                    }

                }
            }
            else if (nm == "")
            {
                if (kzk)
                {
                    tags = c.core.getallsetu();
                }
                else
                {
                    tags = new List<setu>();
                }
                if (middle > 0)
                {
                    spw.Add(c.w * scalex / ((time - middle) / 2));
                    sph.Add(c.h * scaley / ((time - middle) / 2));
                    sptx.Add(c.tx * scalex / ((time - middle) / 2));
                    spty.Add(c.ty * scaley / ((time - middle) / 2));
                    spdx.Add(c.x * scalex / ((time - middle) / 2));
                    spdy.Add(c.y * scaley / ((time - middle) / 2));
                }
                else
                {
                    spw.Add(c.w * scalex / ((time) / 2));
                    sph.Add(c.h * scaley / ((time) / 2));
                    sptx.Add(c.tx * scalex / ((time) / 2));
                    spty.Add(c.ty * scaley / ((time) / 2));
                    spdx.Add(c.x * scalex / ((time) / 2));
                    spdy.Add(c.y * scaley / ((time) / 2));
                }

                for (int i = 0; i < tags.Count(); i++)
                {
                    if (middle > 0)
                    {
                        spw.Add(tags[i].p.w * scalex / ((time - middle) / 2));
                        sph.Add(tags[i].p.h * scaley / ((time - middle) / 2));
                        sptx.Add(tags[i].p.tx * scalex / ((time - middle) / 2));
                        spty.Add(tags[i].p.ty * scaley / ((time - middle) / 2));
                        spdx.Add(tags[i].dx * scalex / ((time - middle) / 2));
                        spdy.Add(tags[i].dy * scaley / ((time - middle) / 2));
                    }
                    else
                    {
                        spw.Add(tags[i].p.w * scalex / ((time) / 2));
                        sph.Add(tags[i].p.h * scaley / ((time) / 2));
                        sptx.Add(tags[i].p.tx * scalex / ((time) / 2));
                        spty.Add(tags[i].p.ty * scaley / ((time) / 2));
                        spdx.Add(tags[i].dx * scalex / ((time) / 2));
                        spdy.Add(tags[i].dy * scaley / ((time) / 2));
                    }

                }
            }
            count = 0;
        }
        public override void frame(character c, float cl)
        {
            while (cl > 0)
            {

                var p = getnokotime(cl, kakun);
                if (p <= 0) p = getnokotime(cl, middlen);
                if (p <= 0) p = getnokotime(cl);
                cl -= p;
                base.frame(c, p);
                if (owari || (p <= 0 && syukusyo)) cl = 0;

                if (kakudai)
                {

                    count += p;
                    if (tag != null)
                    {
                        tags[0].dx -= spdx[0] * p;
                        tags[0].dy -= spdy[0] * p;
                        for (int i = 0; i < tags.Count(); i++)
                        {
                            tags[i].p.w += spw[i] * p;
                            tags[i].p.h += sph[i] * p;
                            tags[i].p.tx += sptx[i] * p;
                            tags[i].p.ty += spty[i] * p;
                            tags[i].dx += spdx[i] * p;
                            tags[i].dy += spdy[i] * p;

                        }
                    }
                    else if (nm == "")
                    {
                        c.w += spw[0] * p;
                        c.h += sph[0] * p;
                        c.wowidouxy(-sptx[0] * p, -spty[0] * p);
                        c.tx += sptx[0] * p;
                        c.ty += spty[0] * p;
                        for (int i = 0; i < tags.Count(); i++)
                        {
                            tags[i].p.w += spw[i + 1] * p;
                            tags[i].p.h += sph[i + 1] * p;
                            tags[i].p.tx += sptx[i + 1] * p;
                            tags[i].p.ty += spty[i + 1] * p;
                            tags[i].dx += spdx[i + 1] * p;
                            tags[i].dy += spdy[i + 1] * p;
                        }
                    }
                }
                else if (syukusyo)
                {
                    count -= p;
                    if (tag != null)
                    {
                        tags[0].dx += spdx[0] * p;
                        tags[0].dy += spdy[0] * p;
                        for (int i = 0; i < tags.Count(); i++)
                        {
                            tags[i].p.w -= spw[i] * p;
                            tags[i].p.h -= sph[i] * p;
                            tags[i].p.tx -= sptx[i] * p;
                            tags[i].p.ty -= spty[i] * p;
                            tags[i].dx -= spdx[i] * p;
                            tags[i].dy -= spdy[i] * p;

                        }
                    }
                    else if (nm == "")
                    {
                        c.w -= spw[0] * p;
                        c.h -= sph[0] * p;
                        c.wowidouxy(sptx[0] * p, spty[0] * p);
                        c.tx -= sptx[0] * p;
                        c.ty -= spty[0] * p;
                        for (int i = 0; i < tags.Count(); i++)
                        {
                            tags[i].p.w -= spw[i + 1] * p;
                            tags[i].p.h -= sph[i + 1] * p;
                            tags[i].p.tx -= sptx[i + 1] * p;
                            tags[i].p.ty -= spty[i + 1] * p;
                            tags[i].dx -= spdx[i + 1] * p;
                            tags[i].dy -= spdy[i + 1] * p;

                        }
                    }
                }
                if (owari && middle >= 0)
                {

                    if (tag != null)
                    {
                        tags[0].dx -= spdx[0] * count;
                        tags[0].dy -= spdy[0] * count;
                        for (int i = 0; i < tags.Count(); i++)
                        {
                            tags[i].p.w += spw[i] * count;
                            tags[i].p.h += sph[i] * count;
                            tags[i].p.tx += sptx[i] * count;
                            tags[i].p.ty += spty[i] * count;
                            tags[i].dx += spdx[i] * count;
                            tags[i].dy += spdy[i] * count;

                        }
                    }
                    else if (nm == "")
                    {
                        c.w += spw[0] * count;
                        c.h += sph[0] * count;
                        c.wowidouxy(-sptx[0] * count, -spty[0] * count);
                        c.tx += sptx[0] * count;
                        c.ty += spty[0] * count;
                        for (int i = 0; i < tags.Count(); i++)
                        {
                            tags[i].p.w += spw[i + 1] * count;
                            tags[i].p.h += sph[i + 1] * count;
                            tags[i].p.tx += sptx[i + 1] * count;
                            tags[i].p.ty += spty[i + 1] * count;
                            tags[i].dx += spdx[i + 1] * count;
                            tags[i].dy += spdy[i + 1] * count;
                        }
                    }
                }



            }
        }
    }


    [Serializable]
    public class tyusinchangeman : moveman
    {
        public float middle;
        public float tyusinx;
        public bool Y;
        public string nm;
        protected float sptx;
        protected float spty;
        public bool add;

        protected setu tag;
        protected float count;
        public float kakun { get { return (time - middle) / 2; } }
        public float middlen { get { return (time - middle) / 2 + middle; } }

        public bool kakudai { get { return (time - middle) / 2 >= timer; } }
        public bool syukusyo { get { return ((time - middle) / 2 + middle < timer) && middle >= 0; } }
        public tyusinchangeman(float t, float middletime, string name, float totyusinx, bool Ynisuru = false, bool addsitei = false, bool stop = false) : base(t, stop)
        {
            middle = middletime;
            time += t;
            if (middle >= 0) time += middle;
            nm = name;
            add = addsitei;
            tyusinx = totyusinx;
            Y = Ynisuru;
        }
        public tyusinchangeman(tyusinchangeman s) : base(s)
        {
            middle = s.middle;
            add = s.add;
            nm = s.nm;
            tyusinx = s.tyusinx;
            Y = s.Y;

            sptx = s.sptx;
            spty = s.spty;
            tag = s.tag;
            count = s.count;
        }
        public tyusinchangeman() { }

        public override void start(character c)
        {
            base.start(c);
            tag = c.core.GetSetu(nm);




            if (tag != null)
            {
                if (!add)
                {
                    if (!Y)
                    {
                        sptx = (tag.p.w * tyusinx - tag.p.tx) / ((time - middle) / 2);
                        spty = 0;
                    }
                    else
                    {
                        spty = (tag.p.h * tyusinx - tag.p.ty) / ((time - middle) / 2);
                        sptx = 0;
                    }
                }
                else
                {
                    if (!Y)
                    {
                        sptx = (tag.p.w * tyusinx) / ((time - middle) / 2);
                        spty = 0;
                    }
                    else
                    {
                        spty = (tag.p.h * tyusinx) / ((time - middle) / 2);
                        sptx = 0;
                    }
                }
            }
            else if (nm == "")
            {
                if (!add)
                {
                    if (!Y)
                    {
                        sptx = (c.w * tyusinx - c.tx) / ((time - middle) / 2);
                        spty = 0;
                    }
                    else
                    {
                        spty = (c.h * tyusinx - c.ty) / ((time - middle) / 2);
                        sptx = 0;
                    }
                }
                else
                {
                    if (!Y)
                    {
                        sptx = c.w * tyusinx / ((time - middle) / 2);
                        spty = 0;
                    }
                    else
                    {
                        spty = c.h * tyusinx / ((time - middle) / 2);
                        sptx = 0;
                    }
                }
            }
            count = 0;
        }
        public override void frame(character c, float cl)
        {
            while (cl > 0)
            {

                var p = getnokotime(cl, kakun);
                if (p <= 0) p = getnokotime(cl, middlen);
                if (p <= 0) p = getnokotime(cl);
                cl -= p;
                base.frame(c, p);
                if (owari || (p <= 0 && syukusyo)) cl = 0;
                if (kakudai)
                {
                    count += p;
                    if (tag != null)
                    {
                        tag.p.tx += sptx * p;
                        tag.p.ty += spty * p;
                    }
                    else if (nm == "")
                    {
                        c.tx += sptx * p;
                        c.ty += spty * p;
                    }
                }
                else if (syukusyo && middle >= 0)
                {
                    count -= p;
                    if (tag != null)
                    {
                        tag.p.tx -= sptx * p;
                        tag.p.ty -= spty * p;
                    }
                    else if (nm == "")
                    {
                        c.tx -= sptx * p;
                        c.ty -= spty * p;
                    }
                }
                if (owari && middle >= 0)
                {

                    if (tag != null)
                    {
                        tag.p.tx += sptx * count;
                        tag.p.ty += spty * count;
                    }
                    else if (nm == "")
                    {
                        c.tx += sptx * count;
                        c.ty += spty * count;
                    }

                }
            }
        }

    }
    /*
             var yuren = new motion();
    yuren.addmoves(new yureman(3, 60, 90, 20, "core"));
          yuren.addmoves(new yureman(2, 30, 90, 10, "core"));
          yuren.addmoves(new yureman(1, 10, 90, 5, "core"));
          humen.camera.addmotion(yuren);

         var  yuren = new motion();
                yuren.addmoves(new yureman(6, 100, 90, 10, "core"));
                yuren.addmoves(new yureman(3, 50, 90, 10, "core"));
                yuren.addmoves(new yureman(2, 20, 90, 10, "core"));
                yuren.addmoves(new yureman(1, 10, 90, 10, "core"));
                humen.camera.addmotion(yuren);
       */

    [Serializable]
    public class dxchangeman : moveman
    {
        public float middle;
        public float dx;
        public bool Y;
        public string nm;
        public float spdx;
        public float spdy;
        public bool add;

        protected setu tag;
        protected float count;
        public float kakun { get { return (time - middle) / 2; } }
        public float middlen { get { return (time - middle) / 2 + middle; } }
        public bool kakudai { get { return (time - middle) / 2 >= timer; } }
        public bool syukusyo { get { return ((time - middle) / 2 + middle < timer) && middle >= 0; } }
        public dxchangeman(float t, float middletime, string name, float todx, bool Ynisuru = false, bool addsitei = false, bool stop = false) : base(t, stop)
        {
            middle = middletime;
            time += t;
            if (middle >= 0) time += middle;
            nm = name;
            add = addsitei;
            dx = todx;
            Y = Ynisuru;
        }
        public dxchangeman(dxchangeman s) : base(s)
        {
            middle = s.middle;
            add = s.add;
            nm = s.nm;
            dx = s.dx;
            Y = s.Y;

            spdx = s.spdx;
            spdy = s.spdy;
            tag = s.tag;
            count = s.count;
        }
        public dxchangeman() { }

        public override void start(character c)
        {
            base.start(c);
            tag = c.core.GetSetu(nm);
            var pre = c.core.Getrootsetu(nm);



            if (tag != null)
            {
                if (pre != null)
                {
                    if (!add)
                    {
                        if (!Y)
                        {

                            spdx = (pre.p.w * dx - tag.dx) / ((time - middle) / 2);
                            spdy = 0;


                        }
                        else
                        {

                            spdy = (pre.p.h * dx - tag.dy) / ((time - middle) / 2);
                            spdx = 0;


                        }
                    }
                    else
                    {
                        if (!Y)
                        {
                            spdx = (pre.p.w * dx) / ((time - middle) / 2);
                            spdy = 0;
                        }
                        else
                        {
                            spdy = (pre.p.h * dx) / ((time - middle) / 2);
                            spdx = 0;
                        }
                    }
                }
                else
                {
                    if (!add)
                    {
                        if (!Y)
                        {
                            spdx = (c.w * dx - tag.dx - c.tx) / ((time - middle) / 2);
                            spdy = 0;
                        }
                        else
                        {
                            spdy = (c.h * dx - tag.dy - c.ty) / ((time - middle) / 2);
                            spdx = 0;
                        }
                    }
                    else
                    {
                        if (!Y)
                        {
                            spdx = (c.w * dx) / ((time - middle) / 2);
                            spdy = 0;
                        }
                        else
                        {
                            spdy = (c.h * dx) / ((time - middle) / 2);
                            spdx = 0;
                        }
                    }
                }
            }
            else
            {
                timer = 0;
            }
            count = 0;
        }
        public override void frame(character c, float cl)
        {
            while (cl > 0)
            {

                var p = getnokotime(cl, kakun);
                if (p <= 0) p = getnokotime(cl, middlen);
                if (p <= 0) p = getnokotime(cl);
                cl -= p;
                base.frame(c, p);
                if (owari || (p <= 0 && syukusyo)) cl = 0;
                if (kakudai)
                {
                    count += p;
                    if (tag != null)
                    {
                        tag.dx += spdx * p;
                        tag.dy += spdy * p;
                    }
                    else if (nm == "")
                    {

                    }
                }
                else if (syukusyo && middle >= 0)
                {
                    count -= p;
                    if (tag != null)
                    {
                        tag.dx -= spdx * p;
                        tag.dy -= spdy * p;
                    }
                    else if (nm == "")
                    {

                    }
                }
                if (owari && middle >= 0)
                {

                    if (tag != null)
                    {
                        tag.dx += spdx * count;
                        tag.dy += spdy * count;
                    }
                    else if (nm == "")
                    {

                    }

                }
            }
        }

    }


    [Serializable]
    public class Kscalechangeman : moveman
    {
        public bool kzk;
        public float scalex;
        public float scaley;
        public string nm;
        protected List<float> spw = new List<float>();
        protected List<float> sph = new List<float>();
        protected List<float> sptx = new List<float>();
        protected List<float> spty = new List<float>();
        protected List<float> spdx = new List<float>();
        protected List<float> spdy = new List<float>();


        protected setu tag;

        protected List<setu> tags = new List<setu>();
        int md = 0;
        public Kscalechangeman(float t, string name, float changescalex, float changescaley, int mode = 0, bool kouzokumo = true, bool stop = false) : base(t, stop)
        {
            md = mode;

            kzk = kouzokumo;
            nm = name;
            scalex = changescalex;
            scaley = changescaley;
        }
        public Kscalechangeman(Kscalechangeman s) : base(s)
        {

            kzk = s.kzk;
            nm = s.nm;
            scalex = s.scalex;
            scaley = s.scaley;
            spw = new List<float>(s.spw);
            sph = new List<float>(s.sph);
            sptx = new List<float>(s.sptx);
            spty = new List<float>(s.spty);
            spdx = new List<float>(s.spdx);
            spdy = new List<float>(s.spdy);


            tag = s.tag;
            tags = new List<setu>(s.tags);
            md = s.md;

        }
        public Kscalechangeman() { }

        public override void start(character c)
        {

            base.start(c);
            tag = c.core.GetSetu(nm);
            spw = new List<float>();
            sph = new List<float>();
            sptx = new List<float>();
            spty = new List<float>();
            spdx = new List<float>();
            spdy = new List<float>();

            if (tag != null)
            {
                if (kzk)
                {
                    tags = tag.getallsetu();
                    tags.Remove(tag);
                    tags.Insert(0, tag);
                }
                else
                {
                    tags = new List<setu>();
                    tags.Add(tag);
                }

                for (int i = 0; i < tags.Count(); i++)
                {
                    var ttt = c.getkijyun().core.GetSetu(tags[i].nm);

                    {
                        var scx = (ttt.p.w * scalex - tags[i].p.w);
                        var scy = (ttt.p.h * scaley - tags[i].p.h);
                        if (md == 1) scy = 0;
                        if (md == -1) scx = 0;
                        spw.Add(scx / time);
                        sph.Add(scy / time);

                        scx = (ttt.p.tx * scalex - tags[i].p.tx);
                        scy = (ttt.p.ty * scaley - tags[i].p.ty);
                        if (md == 1) scy = 0;
                        if (md == -1) scx = 0;
                        sptx.Add(scx / time);
                        spty.Add(scy / time);

                        scx = (ttt.dx * scalex - tags[i].dx);
                        scy = (ttt.dy * scaley - tags[i].dy);
                        if (md == 1) scy = 0;
                        if (md == -1) scx = 0;
                        spdx.Add(scx / time);
                        spdy.Add(scy / time);
                    }

                }
            }
            else if (nm == "")
            {
                if (kzk)
                {
                    tags = c.core.getallsetu();
                }
                else
                {
                    tags = new List<setu>();
                }

                {
                    var ttt = c.getkijyun();
                    var scx = (ttt.w * scalex - c.w);
                    var scy = (ttt.h * scaley - c.h);
                    if (md == 1) scy = 0;
                    if (md == -1) scx = 0;
                    spw.Add(scx / time);
                    sph.Add(scy / time);


                    scx = (ttt.tx * scalex - c.tx);
                    scy = (ttt.ty * scaley - c.ty);
                    if (md == 1) scy = 0;
                    if (md == -1) scx = 0;
                    sptx.Add(scx / time);
                    spty.Add(scy / time);

                    //ここはつかわないあたいだけどいれろく
                    scx = (ttt.w * scalex - c.w);
                    scy = (ttt.h * scaley - c.h);
                    if (md == 1) scy = 0;
                    if (md == -1) scx = 0;
                    spdx.Add(scx / time);
                    spdy.Add(scy / time);
                }

                for (int i = 0; i < tags.Count(); i++)
                {
                    var ttt = c.getkijyun().core.GetSetu(tags[i].nm);

                    {

                        var scx = (ttt.p.w * scalex - tags[i].p.w);
                        var scy = (ttt.p.h * scaley - tags[i].p.h);
                        if (md == 1) scy = 0;
                        if (md == -1) scx = 0;
                        spw.Add(scx / time);
                        sph.Add(scy / time);

                        scx = (ttt.p.tx * scalex - tags[i].p.tx);
                        scy = (ttt.p.ty * scaley - tags[i].p.ty);
                        if (md == 1) scy = 0;
                        if (md == -1) scx = 0;
                        sptx.Add(scx / time);
                        spty.Add(scy / time);

                        scx = (ttt.dx * scalex - tags[i].dx);
                        scy = (ttt.dy * scaley - tags[i].dy);
                        if (md == 1) scy = 0;
                        if (md == -1) scx = 0;
                        spdx.Add(scx / time);
                        spdy.Add(scy / time);
                    }

                }
            }

        }
        public override void frame(character c, float cl)
        {


            var p = getnokotime(cl);

            base.frame(c, cl);

            if (tag != null)
            {
                tags[0].dx -= spdx[0] * p;
                tags[0].dy -= spdy[0] * p;
                for (int i = 0; i < tags.Count(); i++)
                {
                    tags[i].p.w += spw[i] * p;
                    tags[i].p.h += sph[i] * p;
                    tags[i].p.tx += sptx[i] * p;
                    tags[i].p.ty += spty[i] * p;
                    tags[i].dx += spdx[i] * p;
                    tags[i].dy += spdy[i] * p;

                }
            }
            else if (nm == "")
            {
                c.w += spw[0] * p;
                c.h += sph[0] * p;
                c.wowidouxy(-sptx[0] * p, -spty[0] * p);
                c.tx += sptx[0] * p;
                c.ty += spty[0] * p;
                for (int i = 0; i < tags.Count(); i++)
                {
                    tags[i].p.w += spw[i + 1] * p;
                    tags[i].p.h += sph[i + 1] * p;
                    tags[i].p.tx += sptx[i + 1] * p;
                    tags[i].p.ty += spty[i + 1] * p;
                    tags[i].dx += spdx[i + 1] * p;
                    tags[i].dy += spdy[i + 1] * p;
                }
            }

        }
    }


    [Serializable]
    public class Ktyusinchangeman : moveman
    {

        public float scalex;
        public float scaley;
        public string nm;
        protected float sptx = 0;
        protected float spty = 0;
        int md = 0;

        protected setu tag;


        bool add = false;
        public Ktyusinchangeman(float t, string name, float changescalex, float changescaley, int mode = 0, bool addin = false, bool stop = false) : base(t, stop)
        {


            nm = name;
            scalex = changescalex;
            scaley = changescaley;
            md = mode;
            add = addin;
        }
        public Ktyusinchangeman(Ktyusinchangeman s) : base(s)
        {


            nm = s.nm;
            scalex = s.scalex;
            scaley = s.scaley;

            sptx = (s.sptx);
            spty = (s.spty);
            add = s.add;

            tag = s.tag;

            md = s.md;
        }
        public Ktyusinchangeman() { }

        public override void start(character c)
        {

            base.start(c);
            tag = c.core.GetSetu(nm);


            if (tag != null)
            {

                var ttt = c.getkijyun().core.GetSetu(tag.nm);

                {
                    var scx = (ttt.p.w * scalex - tag.p.tx);
                    var scy = (ttt.p.h * scaley - tag.p.ty);
                    if (add)
                    {
                        scx += (ttt.p.tx);
                        scy += (ttt.p.ty);
                    }
                    if (md == 1) scy = 0;
                    if (md == -1) scx = 0;
                    sptx = ((scx) / time);
                    spty = ((scy) / time);
                }


            }
            else if (nm == "")
            {


                var ttt = c.getkijyun();

                {
                    var scx = (ttt.w * scalex - c.tx);
                    var scy = (ttt.h * scaley - c.ty);
                    if (add)
                    {
                        scx += (ttt.tx);
                        scy += (ttt.ty);
                    }
                    if (md == 1) scy = 0;
                    if (md == -1) scx = 0;
                    sptx = ((scx) / time);
                    spty = ((scy) / time);
                }
            }

        }
        public override void frame(character c, float cl)
        {


            var p = getnokotime(cl);

            base.frame(c, cl);

            if (tag != null)
            {
                tag.p.tx += sptx * p;
                tag.p.ty += spty * p;
            }
            else if (nm == "")
            {
                c.tx += sptx * p;
                c.ty += spty * p;
            }

        }
    }

    [Serializable]
    public class Kdxychangeman : moveman
    {

        public float scalex;
        public float scaley;
        public string nm;
        protected float sptx = 0;
        protected float spty = 0;
        int md = 0;

        protected setu tag;
        bool add = false;


        public Kdxychangeman(float t, string name, float changescalex, float changescaley, int mode = 0, bool addin = false, bool stop = false) : base(t, stop)
        {

            add = addin;
            nm = name;
            scalex = changescalex;
            scaley = changescaley;
            md = mode;
        }
        public Kdxychangeman(Kdxychangeman s) : base(s)
        {

            add = s.add;
            nm = s.nm;
            scalex = s.scalex;
            scaley = s.scaley;

            sptx = (s.sptx);
            spty = (s.spty);


            tag = s.tag;

            md = s.md;
        }
        public Kdxychangeman() { }

        public override void start(character c)
        {

            base.start(c);

            tag = c.core.GetSetu(nm);
            var pre = c.core.Getrootsetu(nm);
            ////
            if (tag != null)
            {
                if (pre != null)
                {

                    {
                        var ttt = c.getkijyun().core.GetSetu(pre.nm);
                        var ttt2 = c.getkijyun().core.GetSetu(tag.nm);
                        var scx = (ttt.p.w * scalex - tag.dx);
                        var scy = (ttt.p.h * scaley - tag.dy);
                        if (add)
                        {
                            scx += (ttt2.dx);
                            scy += (ttt2.dy);
                        }
                        if (md == 1) scy = 0;
                        if (md == -1) scx = 0;
                        sptx = ((scx) / time);
                        spty = ((scy) / time);

                    }

                }
                else
                {

                    {
                        var ttt = c.getkijyun();
                        var ttt2 = c.getkijyun().core.GetSetu(tag.nm);
                        var scx = (ttt.w * scalex - tag.dx);
                        var scy = (ttt.h * scaley - tag.dy);
                        if (add)
                        {
                            scx += (ttt2.dx);
                            scy += (ttt2.dy);
                        }
                        if (md == 1) scy = 0;
                        if (md == -1) scx = 0;
                        sptx = ((scx) / time);
                        spty = ((scy) / time);

                    }

                }
            }
            else
            {
                timer = 0;
            }
            ////



        }
        public override void frame(character c, float cl)
        {


            var p = getnokotime(cl);

            base.frame(c, cl);

            if (tag != null)
            {
                tag.dx += sptx * p;
                tag.dy += spty * p;
            }

        }
    }
    /*
             var yuren = new motion();
    yuren.addmoves(new yureman(3, 60, 90, 20, "core"));
          yuren.addmoves(new yureman(2, 30, 90, 10, "core"));
          yuren.addmoves(new yureman(1, 10, 90, 5, "core"));
          humen.camera.addmotion(yuren);

         var  yuren = new motion();
                yuren.addmoves(new yureman(6, 100, 90, 10, "core"));
                yuren.addmoves(new yureman(3, 50, 90, 10, "core"));
                yuren.addmoves(new yureman(2, 20, 90, 10, "core"));
                yuren.addmoves(new yureman(1, 10, 90, 10, "core"));
                humen.camera.addmotion(yuren);
       */
    [Serializable]
    public class yureman : moveman
    {
        public int cou;
        public float haba, sp, kaku, sinhaba;
        public double sinkaku;

        protected setu tag;
        public string nm;
        protected double now = 0;
        public bool izonn = false;
        public bool soutai = false;
        public yureman(int kaisuu, float speed, float kakudo, float haban, string tai, bool izonnu = false, bool soutaikaku = false, bool stop = false) : base(360 * kaisuu / speed, stop)
        {
            cou = kaisuu;
            haba = haban;
            sp = speed;
            kaku = kakudo;
            nm = tai;
            izonn = izonnu;
            soutai = soutaikaku;
        }
        public yureman(yureman y) : base(y)
        {
            cou = y.cou;
            haba = y.haba;
            sp = y.sp;
            kaku = y.kaku;

            tag = y.tag;
            nm = y.nm;

            now = y.now;

            izonn = y.izonn;
            soutai = y.soutai;
            sinhaba = y.sinhaba;
            sinkaku = y.sinkaku;
        }
        public yureman() { }
        public override void start(character c)
        {
            base.start(c);
            tag = c.core.GetSetu(nm);

            now = 0;

            if (soutai)
            {
                if (tag != null)
                    sinkaku = tag.p.RAD + kaku / 180 * Math.PI;
                else
                    sinkaku = c.RAD + kaku / 180 * Math.PI;
            }
            else sinkaku = kaku / 180 * Math.PI;

            if (izonn)
            {
                if (tag != null)
                    sinhaba = haba * (float)Math.Abs(tag.p.w * Math.Cos(sinkaku) + tag.p.h * Math.Sin(sinkaku));
                else
                    sinhaba = haba * (float)Math.Abs(c.w * Math.Cos(sinkaku) + c.h * Math.Sin(sinkaku));
            }
            else sinhaba = haba;
        }
        public override void frame(character c, float cl)
        {
            var p = getnokotime(cl);
            base.frame(c, cl);
            if (tag != null)
            {

                tag.dx -= sinhaba * (float)(Math.Sin(now) * Math.Cos(sinkaku));
                tag.dy -= sinhaba * (float)(Math.Sin(now) * Math.Sin(sinkaku));
                if (!owari)
                {

                    now += sp / 180 * Math.PI * p;
                    if (soutai)
                    {
                        if (tag != null)
                            sinkaku = tag.p.RAD + kaku / 180 * Math.PI;
                        else
                            sinkaku = c.RAD + kaku / 180 * Math.PI;
                    }
                    else sinkaku = kaku / 180 * Math.PI;

                    if (izonn)
                    {
                        if (tag != null)
                            sinhaba = haba * (float)Math.Abs(tag.p.w * Math.Cos(sinkaku - tag.p.RAD) + tag.p.h * Math.Sin(sinkaku - tag.p.RAD));
                        else
                            sinhaba = haba * (float)Math.Abs(c.w * Math.Cos(sinkaku - c.RAD) + c.h * Math.Sin(sinkaku - c.RAD));
                    }
                    else sinhaba = haba;
                    tag.dx += sinhaba * (float)(Math.Sin(now) * Math.Cos(sinkaku));
                    tag.dy += sinhaba * (float)(Math.Sin(now) * Math.Sin(sinkaku));
                }
            }
            else
            {

                c.x -= sinhaba * (float)(Math.Sin(now) * Math.Cos(sinkaku));
                c.y -= sinhaba * (float)(Math.Sin(now) * Math.Sin(sinkaku));
                if (!owari)
                {
                    now += sp / 180 * Math.PI;
                    c.x += sinhaba * (float)(Math.Sin(now) * Math.Cos(sinkaku));
                    c.y += sinhaba * (float)(Math.Sin(now) * Math.Sin(sinkaku));
                }
            }
        }
    }

    [Serializable]
    public class zkaitenman : moveman
    {
        int md;
        double sta, end, byosoku;
        string nm;
        setu tag;
        List<setu> tags = new List<setu>();
        character moto;
        double now;
        bool kzk;
        public zkaitenman(float time, string name, float strsita, float endsita, int mode = 1, bool kouzoku = false, bool stop = false) : base(time, stop)
        {
            nm = name;
            sta = strsita / 180 * Math.PI;
            end = endsita / 180 * Math.PI;
            kzk = kouzoku;
            md = mode;
        }
        public zkaitenman(zkaitenman z) : base(z)
        {
            nm = z.nm;
            sta = z.sta;
            end = z.end;
            byosoku = z.byosoku;
            md = z.md;
            tag = z.tag;
            tags = new List<setu>(z.tags);
            kzk = z.kzk;
        }
        public zkaitenman() : base()
        {

        }
        public override void start(character c)
        {
            base.start(c);
            if (time > 0)
            {
                tag = c.core.GetSetu(nm);
                moto = new character(c, true);
                var ttt = moto.getkijyun();
                if (tag != null || nm == "")
                {
                    if (kzk)
                    {
                        if (tag != null)
                        {
                            tags = tag.getallsetu();
                        }
                        else
                        {
                            tags = c.core.getallsetu();
                        }
                    }
                    else
                    {
                        if (tag != null)
                        {
                            tags = new List<setu> { tag };
                        }
                        else
                        {
                            tags = new List<setu>();
                        }
                    }
                    var cos = Math.Cos(sta);
                    if (cos != 0)
                    {

                        moto.w = (float)(moto.w / cos);
                        moto.tx = (float)(moto.tx / cos);
                        moto.h = (float)(moto.h / cos);
                        moto.ty = (float)(moto.ty / cos);

                    }
                    else
                    {
                        moto.w = ttt.w;
                        moto.h = ttt.h;
                        moto.tx = ttt.tx;
                        moto.ty = ttt.ty;
                    }
                    foreach (var a in moto.core.getallsetu())
                    {
                        if (cos != 0)
                        {
                            a.dx = (float)(a.dx / cos);
                            a.dy = (float)(a.dy / cos);
                            a.p.w = (float)(a.p.w / cos);
                            a.p.h = (float)(a.p.h / cos);
                            a.p.tx = (float)(a.p.tx / cos);
                            a.p.ty = (float)(a.p.ty / cos);
                        }
                        else
                        {
                            var s = ttt.core.GetSetu(a.nm);
                            if (s != null)
                            {
                                a.dx = s.dx;
                                a.dy = s.dy;
                                a.p.w = s.p.w;
                                a.p.h = s.p.h;
                                a.p.tx = s.p.tx;
                                a.p.ty = s.p.ty;
                            }

                        }
                    }

                }
                else { timer = time; }

                byosoku = (end - sta) / time;
                now = sta;
            }
        }
        public override void frame(character c, float cl)
        {
            var p = getnokotime(cl);

            now = now + byosoku * p;
            base.frame(c, cl);
            var cos = Math.Cos(now);
            if (tag != null)
            {
                foreach (var a in tags)
                {
                    var s = moto.core.GetSetu(a.nm);
                    if (s != null)
                    {
                        if (tag != a)
                        {
                            if (md != -1)
                            {
                                a.dx = (float)(s.dx * cos);
                            }
                            if (md != 1)
                            {
                                a.dy = (float)(s.dy * cos);
                            }
                        }
                        if (md != -1)
                        {
                            a.p.tx = (float)(s.p.tx * cos);
                            a.p.w = (float)(s.p.w * cos);
                        }
                        if (md != 1)
                        {
                            a.p.h = (float)(s.p.h * cos);
                            a.p.ty = (float)(s.p.ty * cos);
                        }

                    }
                }
                foreach (var a in tag.sts)
                {
                    var s = moto.core.GetSetu(a.nm);
                    if (s != null)
                    {
                        if (tag != a)
                        {
                            if (md != -1)
                            {
                                a.dx = (float)(s.dx * cos);
                            }
                            if (md != 1)
                            {
                                a.dy = (float)(s.dy * cos);
                            }
                        }

                    }
                }
            }
            else if (nm == "")
            {
                var ttx = c.gettx();
                var tty = c.getty();
                if (md != -1)
                {
                    c.w = (float)(moto.w * cos);
                    c.tx = (float)(moto.tx * cos);
                }
                if (md != 1)
                {
                    c.h = (float)(moto.h * cos);
                    c.ty = (float)(moto.ty * cos);
                }
                c.settxy(ttx, tty);

                foreach (var a in tags)
                {
                    var s = moto.core.GetSetu(a.nm);
                    if (s != null)
                    {
                        if (md != -1)
                        {
                            a.dx = (float)(s.dx * cos);
                            a.p.tx = (float)(s.p.tx * cos);
                            a.p.w = (float)(s.p.w * cos);
                        }
                        if (md != 1)
                        {
                            a.dy = (float)(s.dx * cos);
                            a.p.h = (float)(s.p.h * cos);
                            a.p.ty = (float)(s.p.ty * cos);
                        }

                    }
                }
            }





        }
    }
    [Serializable]
    public class hantenkaitenman : moveman
    {
        string nm;
        setu tag;
        List<setu> tags = new List<setu>();
        double sp;
        int md;
        public hantenkaitenman(float time, string name, int mode = 0, bool stop = false) : base(time, stop)
        {
            md = mode;
            nm = name;
        }
        public hantenkaitenman(hantenkaitenman h) : base(h)
        {
            md = h.md;
            sp = h.sp;
            nm = h.nm;
            tags = new List<setu>(h.tags);
            tag = h.tag;
        }
        public hantenkaitenman() : base() { }
        public override void start(character c)
        {
            base.start(c);
            if (time < 0)
            {
                timer = time;
                return;
            }
            var setu = c.core.GetSetu(nm);
            tag = setu;
            if (setu != null)
            {
                tags = setu.getallsetu();
                var to = setu.p.RAD;
                var kijyun = c.getkijyun();
                {

                    var b = kijyun.core.GetSetu(setu.nm);
                    if (b != null)
                    {
                        var ki = setu.p.RAD - b.p.RAD;
                        to = b.p.RAD - ki;
                    }

                }
                var dd = (to - setu.p.RAD);
                dd = Math.Atan2(Math.Sin(dd), Math.Cos(dd));
                if (md == 1)
                {
                    if (dd < 0) dd = Math.PI * 2 + dd;

                }
                else if (md == -1)
                {
                    if (dd > 0) dd = -Math.PI * 2 + dd;
                }
                sp = dd / time;
            }
            else if (nm == "")
            {
                tags = c.core.getallsetu();
                var to = c.RAD;
                var kijyun = c.getkijyun();
                {

                    var ki = c.RAD - kijyun.RAD;
                    to = kijyun.RAD - ki;

                }
                var dd = (to - c.RAD);
                dd = Math.Atan2(Math.Sin(dd), Math.Cos(dd));
                if (md == 1)
                {
                    if (dd < 0) dd = Math.PI * 2 + dd;

                }
                else if (md == -1)
                {
                    if (dd > 0) dd = -Math.PI * 2 + dd;
                }
                sp = dd / time;
            }
        }
        public override void frame(character c, float cl)
        {
            var p = getnokotime(cl);
            base.frame(c, cl);
            if (tag == null && nm == "") c.RAD += sp * p;
            foreach (var a in tags)
            {
                a.p.RAD += sp * p;
            }
        }
    }

    [Serializable]
    public class stopaman : moveman
    {
        //これは直接扱わぬ
        List<double> rads = new List<double>();
        public stopaman(float t) : base(t, false) { }
        public stopaman(stopaman s) : base(s)
        {
            rads = new List<double>(s.rads);
        }

        public stopaman() : base() { }
        public override void start(character c)
        {
            base.start(c);
            rads.Clear();
            rads.Add(c.RAD);
            var a = c.core.getallsetu();
            for (int i = 0; i < a.Count(); i++)
            {
                rads.Add(a[i].p.RAD);
            }
        }
        public override void frame(character c, float cl)
        {
            base.frame(c, cl);
            var a = c.core.getallsetu();
            c.RAD = rads[0];
            for (int i = 0; i < a.Count(); i++)
            {
                a[i].p.RAD = rads[i + 1];
            }
        }
    }
    [Serializable]
    public class stopaaddman : moveman
    {


        public stopaaddman(float t, bool stop = false) : base(t, stop) { }
        public stopaaddman(stopaaddman s) : base(s)
        {

        }

        public stopaaddman() : base() { }
        public override void start(character c)
        {
            base.start(c);
            c.addmotion(new motion(new stopaman(time)), true);
        }

    }
    [Serializable]
    public class playotoman:moveman
    {
       protected string oto;
        protected float vol;
        protected bool bgmn;
        public playotoman(string name, float volume = 1,bool BGM=false) : base(1,false) 
        {
            oto = name;
            vol = volume;
            bgmn = BGM;
        }
        public playotoman(playotoman s) : base(s)
        {
            oto = s.oto;
            vol = s.vol;
            bgmn = s.bgmn;
        }

        public playotoman() : base() { }
        public override void start(character c)
        {
            base.start(c);
            if (!bgmn) {
                fileman.playoto(oto,vol);
            }
            else { bool butu = vol < 0; fileman.playbgm(oto,butu); }
        }
    }
    [Serializable]
    public class pplayotoman : moveman
    {
        protected string oto;
        protected float vol;
        protected bool bgmn;
        [NonSerialized]
        protected hyojiman hyo;
        public pplayotoman(hyojiman h,string name, float volume = 1, bool BGM = false) : base(1, false)
        {
            hyo = h;
            oto = name;
            vol = volume;
            bgmn = BGM;
        }
        public pplayotoman(pplayotoman s) : base(s)
        {
            hyo = s.hyo;
            oto = s.oto;
            vol = s.vol;
            bgmn = s.bgmn;
        }

        public pplayotoman() : base() { }
        public override void start(character c)
        {
            base.start(c);
            if(hyo!=null)
            if (!bgmn)
            {
                
                hyo.playoto(oto, vol);
            }
            else { bool butu = vol < 0; hyo.playbgm(oto, butu); }
        }
    }

}
